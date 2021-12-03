<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="SELECT name, n_izd FROM pmib8307.j"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="Select * from pmib8307.p"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource3" runat="server"></asp:SqlDataSource>

        </div>
        <table width="100%" cellspacing="0" cellpadding="5">
        <tr> 
            <td width="50%" valign="top">
                 Задание 1.
                Получить информацию о рекомендованной цене на указанное изделие на заданную дату.
         <p>
             Выберете изделие:
            <asp:DropDownList ID="DropDownList1" runat="server">
            </asp:DropDownList>
         </p>
                 <p>
             Выберете дату:<asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Рекомендованная цена" />
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
         </p>
            </td>
            <td valign="top">
                Задание 2. Для изделий, в состав которых входит заданная деталь, сдвинуть на месяц назад дату начала действия последней рекомендованной цены.
        <p>
            Выберете деталь:
            <asp:DropDownList ID="DropDownList3" runat="server" OnSelectedIndexChanged="DropDownList3_SelectedIndexChanged">
            </asp:DropDownList>
        </p>
                <p>
            <asp:Button ID="Button2" runat="server" OnClick="DropDownList3_SelectedIndexChanged" Text="Показать текущие данные" />
        </p>
                <p>
            <asp:GridView ID="GridView1" runat="server">
                 
            </asp:GridView>
        </p>
                <p>
                    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Изменить даты" />
        </p>
                <p>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        </p>
            </td>
        </tr>
        </table>
        
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        
    </form>
</body>
</html>
