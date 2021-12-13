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

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="SELECT n_izd, name || ' - ' || town as text 
FROM pmib8307.j
"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="Select pmib8307.p.n_det, pmib8307.p.name || ' - ' || pmib8307.p.cvet || ' - ' ||  pmib8307.p.town as text
 from pmib8307.p"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="SELECT pmib8307.j.name || ' - ' || pmib8307.j.town as &quot;Изделие&quot;,
              t.last_date as &quot;Дата&quot;,
              pmib8307.v.cost as &quot;Цена&quot;, 
              pmib8307.p.name || ' - ' || pmib8307.p.cvet  || ' - ' || pmib8307.p.town as &quot;Выбранная деталь&quot;
from pmib8307.j 
join pmib8307.q on pmib8307.q.n_izd = pmib8307.j.n_izd 
join pmib8307.p on pmib8307.p.n_det = pmib8307.q.n_det 
join (select pmib8307.v.n_izd, max(pmib8307.v.date_begin) as last_date 
         from pmib8307.v group by pmib8307.v.n_izd
        ) t on t.n_izd = pmib8307.j.n_izd join pmib8307.v 
        on t.n_izd = pmib8307.v.n_izd and pmib8307.v.date_begin = t.last_date 
where pmib8307.p.n_det = ? ">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList3" DefaultValue="'P1'" Name="?" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:studentsConnectionString4 %>" ProviderName="<%$ ConnectionStrings:studentsConnectionString4.ProviderName %>" SelectCommand="SELECT cost 
from pmib8307.v 
where n_izd = ? and  date_begin &lt; ? 
order by date_begin desc 
limit(1)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownList1" DefaultValue="'J1'" Name="?" PropertyName="SelectedValue" />
                    <asp:ControlParameter ControlID="Calendar1" DbType="DateTime" DefaultValue="" Name="?" PropertyName="SelectedDate" />
                </SelectParameters>
            </asp:SqlDataSource>

        </div>
        <table width="100%" cellspacing="0" cellpadding="5">
        <tr> 
            <td width="50%" valign="top">
                 Задание 1.
                Получить информацию о рекомендованной цене на указанное изделие на заданную дату.
         <p>
             Выберете изделие:
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="text" DataValueField="n_izd" OnSelectedIndexChanged="UpdateForm" OnTextChanged="UpdateForm">
            </asp:DropDownList>
         </p>
                 <p>
             Выберете дату:<asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        <br />
                     <asp:Button ID="Button4" runat="server" OnClick="UpdateForm" Text="Обновить цену" />
         </p>
                 <p>
                     Рекомендованная цена:
                     <asp:GridView ID="GridView2" runat="server" BorderStyle="None" BorderWidth="0px" DataSourceID="SqlDataSource4" GridLines="None" ShowHeader="False">
                         <EmptyDataTemplate>
                             нет данных
                         </EmptyDataTemplate>
                     </asp:GridView>
         </p>
            </td>
            <td valign="top">
                Задание 2. Для изделий, в состав которых входит заданная деталь, сдвинуть на месяц назад дату начала действия последней рекомендованной цены.
        <p>
            Выберете деталь:
            <asp:DropDownList ID="DropDownList3" runat="server" OnSelectedIndexChanged="UpdateForm" DataSourceID="SqlDataSource2" DataTextField="text" DataValueField="n_det" OnTextChanged="UpdateForm">
            </asp:DropDownList>
        </p>
                <p>
            <asp:Button ID="Button2" runat="server" OnClick="UpdateForm" Text="Обновить таблицу" />
        </p>
                <p>
            <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource3" EmptyDataText="нет данных" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Изделие" HeaderText="Изделие" ReadOnly="True" SortExpression="Изделие" />
                    <asp:BoundField DataField="Дата" DataFormatString="{0:d}" HeaderText="Дата" ReadOnly="True" SortExpression="Дата" />
                    <asp:BoundField DataField="Цена" HeaderText="Цена" SortExpression="Цена" />
                    <asp:BoundField DataField="Выбранная деталь" HeaderText="Выбранная деталь" ReadOnly="True" SortExpression="Выбранная деталь" />
                </Columns>
                <EmptyDataTemplate>
                    &nbsp;Нет данных&nbsp;
                </EmptyDataTemplate>
                 
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
