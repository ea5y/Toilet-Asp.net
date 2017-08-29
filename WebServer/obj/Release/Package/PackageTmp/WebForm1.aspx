<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebServer.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Toilet</title>
    <script runat="server">
        protected void Timer1_Tick(object sender, EventArgs e)
        {
        }
    </script>
    <style>
        .divcss table{
            border-collapse: collapse;
            border: none;
            padding: 2px;
            text-align: center;
        }
        .divcss td{
            border: solid #aaaaaa 2px;
        }
    </style>
</head>
<body style="width: 696px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="1000" />
        <asp:UpdatePanel ID="StockPricePanel" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" />

            </Triggers>
            <ContentTemplate>
                <div class="divcss" id="TableContainer" runat="server" style="position:absolute; left:10px; top:30px;">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <img src="img.png" width="400px"; height="600px"; style="position:absolute; left:500px; top:0px;" />

    </form>
</body>
</html>
