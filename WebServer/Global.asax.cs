using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using WebServer.Src;
using System.Configuration;

namespace WebServer
{
    public class FlashAirData
    {
        public string BinaryData = "xxxxxxxx";
    }

    public class FlashAirConfig
    {
        public string Index;
        public string UnitId;
        public string ModelName;
        public string RoomCount;
        public string FlashAirUrl;
    }

    public class RoomData
    {
        public string RoomName;

        public bool IsUsing { get; set; }

        public DateTime TimeStart;
        public DateTime TimeEnd;

        public RoomData()
        {
            this.TimeStart = DateTime.Now;
            this.TimeEnd = DateTime.Now;
        }

        public void UpdateTimeStart(DateTime timeEnd)
        {
            this.TimeStart = timeEnd;
        }

        public void UpdateTimeEnd(DateTime timeEnd)
        {
            this.TimeEnd = timeEnd;
        }
    }

    public class Global : System.Web.HttpApplication
    {
        public static List<FlashAirConfig> FlashAirConfigList = new List<FlashAirConfig>();
        public static List<FlashAirData> FlashAirDataList;

        public static List<RoomData> RoomDataList = new List<RoomData>();
        private Dictionary<int, string> RoomLetterCodeDic = new Dictionary<int, string>()
        {
            { 0, "A"},
            { 1, "B"},
            { 2, "C"},
            { 3, "D"},
            { 4, "E"},
            { 5, "F"},
        };

        protected void Application_Start(object sender, EventArgs e)
        {
            Debug.Print("Application Start...");

            this.ReadConfig();
            this.InitRoomList();
            this.InitFlashAirDataList();
            this.Run(FlashAirConfigList);
        }

        private void ReadConfig()
        {
            FlashAirComplexSection complex = ConfigurationManager.GetSection("flashaircomplexsection") as FlashAirComplexSection;
            foreach (ChildSection item in complex.Children)
            {
                FlashAirConfig config = new FlashAirConfig();
                config.Index = item.Index;
                config.UnitId = item.UnitId;
                config.ModelName = item.ModelName;
                config.RoomCount = item.RoomCount;
                config.FlashAirUrl = item.FlashAirUrl;

                FlashAirConfigList.Add(config);
                Debug.Print("FlashAisrSection: indexs={0} unitid={1} modelname={2} roomCount={3} flashAirUrl={4}", item.Index, item.UnitId, item.ModelName, item.RoomCount, item.FlashAirUrl);
            }
        }

        private void InitRoomList()
        {
            for(int i = 0; i < FlashAirConfigList.Count; i++)
            {
                for(int j = 0; j < int.Parse(FlashAirConfigList[i].RoomCount); j++)
                {
                    RoomData roomData = new RoomData();
                    roomData.RoomName = string.Format("{0}{1}", i + 1, this.GetRoomLetterCode(j));
                    roomData.IsUsing = false;
                    RoomDataList.Add(roomData);
                }
            }
        }

        private string GetRoomLetterCode(int index)
        {
            return this.RoomLetterCodeDic[index];
        }

        private void InitFlashAirDataList()
        {
            FlashAirDataList = new List<FlashAirData>();
            for(int i = 0; i < FlashAirConfigList.Count; i++)
            {
                FlashAirData data = new FlashAirData();
                FlashAirDataList.Add(data);
            }
        }

        private void UpdateRoomDataList()
        {
            int index = 0;
            for(int i = 0; i < FlashAirDataList.Count; i++)
            {
                var roomCount = int.Parse(FlashAirConfigList[i].RoomCount);
                var binaryData = FlashAirDataList[i].BinaryData;
                var need = binaryData.Substring(binaryData.Length - roomCount, roomCount);
                var len = need.Length;

                for(int j = 0; j < roomCount; j++)
                {
                    var value = need[--len].ToString();
                    if(this.DataChangesTakeEffect(index))
                    {
                        RoomDataList[index].IsUsing = value == "1";
                    }
                    index++;
                }
            }
        }

        private bool DataChangesTakeEffect(int index)
        {
            var data = RoomDataList[index];
            data.TimeEnd = DateTime.Now;
            Debug.Print("TimeStart:{0}, TimeEnd:{1}", data.TimeStart, data.TimeEnd);

            if(this.DateDiff(data.TimeStart, data.TimeEnd) >= 3000)
            {
                data.TimeStart = data.TimeEnd;
                Debug.Print("===>TakeEffect\n TimeStart:{0}, TimeEnd:{1}", data.TimeStart, data.TimeEnd);
                return true;
            }
            return false;
        }

        private int DateDiff(DateTime start, DateTime end)
        {
            TimeSpan ts1 = new TimeSpan(start.Ticks);
            TimeSpan ts2 = new TimeSpan(end.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return (int)ts3.TotalMilliseconds;
        }

        private void Run(List<FlashAirConfig> flashAirConfiglist)
        {
            foreach(var item in flashAirConfiglist)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(RefreshThread));
                thread.Start(item);
            }
        }

        private void RefreshThread(object flashAir)
        {
            var _flashAir = flashAir as FlashAirConfig;

            var faData = FlashAirDataList[int.Parse(_flashAir.Index)];

            string binaryData;
            while(true)
            {
                //Request flashAir
                HttpHelper.GetDataFromFlashAir(_flashAir.FlashAirUrl, (res) =>
                {
                    binaryData = Convert.ToString(Convert.ToInt32(res.DATA, 16), 2).PadLeft(8, '0');

                    faData.BinaryData = binaryData;
                    var str = string.Format("Data: {0}", binaryData);

                    this.UpdateRoomDataList();
                    Debug.Print(str);
                });
                //===>Test
                /*
                    binaryData = Convert.ToString(Convert.ToInt32("0x1c", 16), 2).PadLeft(8, '0');

                    faData.BinaryData = binaryData;
                    var str = string.Format("Data: {0}", binaryData);
                    Debug.Print(str);

                    this.UpdateRoomDataList();
                    */

                Thread.Sleep(100);
            }
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            Debug.Print("Sesstion Start...");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            Debug.Print("Application_BeginRequest Start...");
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            Debug.Print("Application_AuthenticateRequest Start...");
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Debug.Print("Sesstion_End Start...");

        }

        protected void Application_End(object sender, EventArgs e)
        {
        
            Debug.Print("Application_End Start...");
        }
    }
}