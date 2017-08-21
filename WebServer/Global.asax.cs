using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;
using YduCs;
using System.Threading;
using WebServer.Src;
using System.Text;
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
        public bool IsUsing;
    }
    
    public class Global : System.Web.HttpApplication
    {
        public static List<FlashAirConfig> FlashAirConfigList = new List<FlashAirConfig>();
        public static List<FlashAirData> FlashAirDataList;

        public static List<RoomData> RoomDataList = new List<RoomData>();

        public int IResult;
        public bool BResult;

        protected void Application_Start(object sender, EventArgs e)
        {
            Debug.Print("Application Start...");

            this.ReadConfig();
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

        private void InitFlashAirDataList()
        {
            FlashAirDataList = new List<FlashAirData>();
            for(int i = 0; i < FlashAirConfigList.Count; i++)
            {
                FlashAirData data = new FlashAirData();
                FlashAirDataList.Add(data);
            }
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

            /*
            IResult = Ydu.Open(ushort.Parse(_flashAir.UnitId), _flashAir.ModelName, Ydu.YDU_OPEN_NORMAL);

            if (IResult == Ydu.YDU_RESULT_SUCCESS)
            {
                var str = "YduOpen success";
                Debug.Print(str);
            }
            else
            {
                var str = string.Format("YduOpen error: 0x{0:X}", IResult);
                Debug.Print(str);
            }
            */

            string binaryData;
            while(true)
            {
                //Request flashAir
                HttpHelper.GetDataFromFlashAir(_flashAir.FlashAirUrl, (res) =>
                {
                    binaryData = Convert.ToString(Convert.ToInt32(res.DATA, 16), 2).PadLeft(8, '0');

                    faData.BinaryData = binaryData;
                    var str = string.Format("Data: {0}", binaryData);
                    Debug.Print(str);
                });
                /*
                    binaryData = Convert.ToString(Convert.ToInt32("0x1c", 16), 2).PadLeft(8, '0');

                    faData.BinaryData = binaryData;
                    var str = string.Format("Data: {0}", binaryData);
                    Debug.Print(str);
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