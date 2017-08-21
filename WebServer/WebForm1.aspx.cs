using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YduCs;
using System.Diagnostics;
using System.Threading;
using System.Web.UI.HtmlControls;

namespace WebServer
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.Print("===>PageLoad");
            //this.CreateTable();
            this.CreateTableNew();
        }

        private void CreateTableNew()
        {
            int columns = 3;
            int rows = GetRows();

            HtmlTable table = new HtmlTable();
            table.Border = 0;

            RefreshTableNew(rows, columns, table);

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

        private void RefreshTableNew(int rows, int columns, HtmlTable table)
        {
            for(int i = 0; i < rows; i++)
            {
                HtmlTableRow row = new HtmlTableRow();
                for(int j = 0; j < columns; j++)
                {
                    HtmlTableCell cell = new HtmlTableCell();
                    /*
                    cell.Controls.Add(new LiteralControl("Room " + i.ToString()));
                    if(j == 0)
                    {
                        cell.Width = "15px";
                    }
                    else
                    {
                        cell.Width = "150px";
                    }
                    */
                    this.SetCellWidth(cell, j);
                    this.SetFirstRow(cell, i, j);
                    row.Cells.Add(cell);
                }
                table.Controls.Add(row);
            }
        }

        private void SetCellWidth(HtmlTableCell cell, int column)
        {
            if (column == 0)
            {
                cell.Width = "15px";
            }
            else
            {
                cell.Width = "150px";
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
                        break;
                    case 2:
                        cell.InnerHtml = "使用中";
                        break;
                }
            }
        }

        private void SetRoomName(HtmlTableCell cell, int row, int column)
        {
            if(row != 0 && column == 0)
            {
                
            }
        }

        private void SetStatus(HtmlTableCell cell, int row, int column)
        {

        }

        private void CreateTable()
        {
            for (int i = 0; i < Global.FlashAirConfigList.Count; i++)
            {
                HtmlTable table = new HtmlTable();
                table.Border = 0;
                
                int numrows = Convert.ToInt32("2");
                int numcells = int.Parse(Global.FlashAirConfigList[i].RoomCount);

                var status = this.InitTable(numrows, numcells, table, i);
                //this.RefreshTable(numrows, numcells, table, i);

                Label label = new Label();
                label.Text =  string.Format("Floor_{0} Status:{1}", i, status);
                //TableContainer.Controls.Add(label);
                TableContainer.Controls.Add(table);
            }
        }

        private void RefreshTable(int numrows, int numcells, HtmlTable table, int index)
        {
            for (int j = 0; j < numrows; j++)
            {
                if (j % 2 == 1)
                {
                    var binaryData = Global.FlashAirDataList[index].BinaryData;
                    int roomCount = int.Parse(Global.FlashAirConfigList[index].RoomCount);
                    var need = binaryData.Substring(binaryData.Length - roomCount, roomCount);
                    var len = need.Length;
                    for (int i = 0; i < numcells; i++)
                    {
                        var tr = table.Rows[j].Cells[i];
                        var value = need[(--len)].ToString();
                        tr.InnerHtml = value;
                    }
                }
            }
        }

        private string InitTable(int numrows, int numcells, HtmlTable table, int index)
        {
            string status = string.Empty;
            for (int j = 0; j < numrows; j++)
            {
                HtmlTableRow row = new HtmlTableRow();

                /*
                // Provide a different background color for alternating rows.
                if (j % 2 == 0)
                    row.BgColor = "Gray";

                // Iterate through the cells of a row.
                for (int i = 0; i < numcells; i++)
                {
                    HtmlTableCell cell = new HtmlTableCell();
                    cell.Controls.Add(new LiteralControl("Room " + i.ToString()));
                    row.Cells.Add(cell);

                }
                */
                if (j % 2 == 0)
                {
                    //row.BgColor = "Gray";
                    for (int i = 0; i < numcells; i++)
                    {
                        HtmlTableCell cell = new HtmlTableCell();
                        cell.Controls.Add(new LiteralControl("Room " + i.ToString()));
                        row.Cells.Add(cell);
                    }
                }
                else
                {
                    status = Global.FlashAirDataList[index].BinaryData;
                    int roomCount = int.Parse(Global.FlashAirConfigList[index].RoomCount);
                    var need = status.Substring(status.Length - roomCount, roomCount);
                    var len = need.Length;
                    for (int i = 0; i < numcells; i++)
                    {
                        HtmlTableCell cell = new HtmlTableCell();
                        var value = need[(--len)].ToString();
                        cell.InnerHtml = value;
                        cell.Attributes.Add("style", "");

                        row.Cells.Add(cell);
                    }
                }

                table.Rows.Add(row);
            }
            return status;
        }
    }
}