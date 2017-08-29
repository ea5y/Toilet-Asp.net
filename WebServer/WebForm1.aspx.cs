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
            int columns = 4;
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

                    var canSpanRow = this.CanSpanRow(cell, i, j, ref index);
                    if(canSpanRow)
                    {
                        this.SpanRow(cell, i, j, index);
                    }

                    this.SetCellWidthNew(cell, j);
                    this.SetFirstRowNew(cell, i, j);
                    this.SetFloor(cell, i, j, index, canSpanRow);
                    this.SetRoomNameNew(cell, i, j, index, canSpanRow);
                    this.SetStatusNew(cell, i, j, index, canSpanRow);

                    this.AddCell(row, cell, i, j, canSpanRow);
                }
                table.Controls.Add(row);
            }
        }

        private void AddCell(HtmlTableRow trow, HtmlTableCell cell, int row, int column, bool canSpanRow)
        {

            if (!canSpanRow && row != 0 && column == 3)
            {
            }
            else
            {
                trow.Cells.Add(cell);
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

        private void SetCellWidthNew(HtmlTableCell cell, int column)
        {
            cell.Height = "35px";
            if(column == 0)
            {
                cell.Width = "35px";
            }
            else if(column == 1)
            {
                cell.Width = "20px";
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

        private void SetFirstRowNew(HtmlTableCell cell, int row, int column)
        {
            if(row == 0)
            {
                switch(column)
                {
                    case 0:
                        cell.InnerHtml = "";
                        cell.Style.Value = "border-left: 0px;border-right: 0px;border-top: 0px;";
                        break;
                    case 1:
                        cell.InnerHtml = "";
                        cell.Style.Value = "border-left: 0px;border-top: 0px;";
                        break;
                    case 2:
                        cell.InnerHtml = "使用可";
                        cell.BgColor = "#352323";
                        cell.Style.Value = "color: #ffffff";
                        break;
                    case 3:
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
                //index++;
                cell.InnerHtml = Global.RoomDataList[index].RoomName;
            }
        }

        private void SetRoomNameNew(HtmlTableCell cell, int row, int column, int index, bool canSpanRow)
        {
            if(canSpanRow)
            {
                if(row != 0 && column == 1)
                {
                    cell.InnerHtml = Global.RoomDataList[index].RoomName;
                }
            }
            else
            {
                if (row != 0 && column == 0)
                {
                    cell.InnerHtml = Global.RoomDataList[index].RoomName;
                }
            }
        }

        private void SetFloor(HtmlTableCell cell, int row, int column, int index, bool canSpanRow)
        {
            if(canSpanRow)
            {
                if (row != 0 && column == 0)
                {
                    cell.InnerHtml = Global.RoomDataList[index].FloorName;
                    this.SetFloorColor(cell, index);
                }
            }
        }

        private void SetFloorColor(HtmlTableCell cell, int index)
        {
            int i = int.Parse(Global.RoomDataList[index].Index);
            if( i % 2 == 0)
            {
                cell.BgColor = "#39c729";
                cell.Style.Value = "color: #ffffff";
            }
            else
            {
                cell.BgColor = "#d8ffb6";
            }
        }

        private void SpanRow(HtmlTableCell cell, int row, int colum, int index)
        {
            if(row != 0 && colum == 0)
            {
                int i = int.Parse(Global.RoomDataList[index].Index);
                cell.Attributes.Add("rowspan", Global.FlashAirConfigList[i].RoomCount);
            }
        }

        private bool CanSpanRow(HtmlTableCell cell, int row, int column, ref int index)
        {
            if(row != 0 && column == 0)
                index++;
            if (index == -1)
                return false;
            if(index == 0)
                return true;
            if(Global.RoomDataList[index].FloorName != Global.RoomDataList[index - 1].FloorName)
            {
                return true;
            }
            return false;
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

        private void SetStatusNew(HtmlTableCell cell, int row, int column, int index, bool canSpanRow)
        {
            //if(row != 0 && column != 0 && column != 1)
            if(row != 0)
            {
                if(canSpanRow)
                {
                    this.SetCellColor(cell, column, index, 2, 3);
                }
                else
                {
                    this.SetCellColor(cell, column, index, 1, 2);
                }
            }
        }

        private void SetCellColor(HtmlTableCell cell, int column, int index, int canUseColumn, int cannotUseColumn)
        {
            if (column == canUseColumn)
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
            if (column == cannotUseColumn)
            {
                if (Global.RoomDataList[index].IsUsing)
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