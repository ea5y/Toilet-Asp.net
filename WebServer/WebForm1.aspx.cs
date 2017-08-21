using System;
using System.Diagnostics;
using System.Web.UI.HtmlControls;

namespace WebServer
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.Print("===>PageLoad");
            this.CreateTable();
        }

        private void CreateTable()
        {
            int columns = 3;
            int rows = GetRows();

            HtmlTable table = new HtmlTable();
            table.Border = 0;
            RefreshTable(rows, columns, table);
            TableContainer.Controls.Add(table);
        }

        private int GetRows()
        {
            int rows = 0;
            for(int i = 0; i < Global.FlashAirConfigList.Count; i++)
            {
                rows += int.Parse(Global.FlashAirConfigList[i].RoomCount);
            }
            return rows;
        }

        private void RefreshTable(int rows, int columns, HtmlTable table)
        {
            int index = -1;
            for(int i = 0; i < rows + 1; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                for(int j = 0; j < columns; j++)
                {
                    HtmlTableCell cell = new HtmlTableCell();

                    this.SetCellWidth(cell, j);
                    this.SetFirstRow(cell, i, j);
                    this.SetRoomName(cell, i, j, ref index);
                    this.SetStatus(cell, i, j, index);
                    row.Cells.Add(cell);
                }
                table.Controls.Add(row);
            }
        }

        private void SetCellWidth(HtmlTableCell cell, int column)
        {
            cell.Height = "35px";
            if (column == 0)
            {
                cell.Width = "35px";
            }
            else
            {
                cell.Width = "170px";
            }
        }

        private void SetFirstRow(HtmlTableCell cell, int row, int column)
        {
            if(row == 0)
            {
                switch(column)
                {
                    case 0:
                        cell.InnerHtml = "";
                        cell.Style.Value = "border-left: 0px;border-top: 0px;";
                        break;
                    case 1:
                        cell.InnerHtml = "使用可";
                        cell.BgColor = "#352323";
                        cell.Style.Value = "color: #ffffff";
                        break;
                    case 2:
                        cell.InnerHtml = "使用中";
                        cell.BgColor = "#352323";
                        cell.Style.Value = "color: #ffffff";
                        break;
                }
            }
        }

        private void SetRoomName(HtmlTableCell cell, int row, int column, ref int index)
        {
            if(row != 0 && column == 0)
            {
                index++;
                cell.InnerHtml = Global.RoomDataList[index].RoomName;
            }
        }

        private void SetStatus(HtmlTableCell cell, int row, int column, int index)
        {
            if(row != 0 && column != 0)
            {
                if (column == 1)
                {
                    if (Global.RoomDataList[index].IsUsing)
                    {
                        cell.BgColor = "White";
                    }
                    else
                    {
                        cell.BgColor = "Blue";
                    }
                }
                if (column == 2)
                {
                    if(Global.RoomDataList[index].IsUsing)
                    {
                        cell.BgColor = "Red";
                    }
                    else
                    {
                        cell.BgColor = "White";
                    }
                }
            }
        }
    }
}