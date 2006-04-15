<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectDateControl.ascx.cs"
 Inherits="SelectDateControl" %>
<asp:DropDownList ID="selectdateYear" runat="server" CssClass="sncore_form_dropdown_auto"
 OnSelectedIndexChanged="selectionChanged" AutoPostBack="true" />
<asp:DropDownList ID="selectdateMonth" runat="server" CssClass="sncore_form_dropdown_auto"
 OnSelectedIndexChanged="selectionChanged" AutoPostBack="true" />
<asp:DropDownList ID="selectdateDay" runat="server" CssClass="sncore_form_dropdown_auto"
 OnSelectedIndexChanged="selectionChanged" AutoPostBack="true" />
<asp:ImageButton ID="linkCalendar" CausesValidation="false" runat="server" OnClick="linkCalendar_Click"
 ImageUrl="images/calendar.gif" ImageAlign="AbsMiddle" />
<asp:RequiredFieldValidator ID="selectdateYearRequired" runat="server" ControlToValidate="selectdateYear"
 CssClass="sncore_form_validator" ErrorMessage="year is required" Display="None" />
<asp:RequiredFieldValidator ID="selectdateMonthRequired" runat="server" ControlToValidate="selectdateMonth"
 CssClass="sncore_form_validator" ErrorMessage="month is required" Display="None" />
<asp:RequiredFieldValidator ID="selectdateDayRequired" runat="server" ControlToValidate="selectdateDay"
 CssClass="sncore_form_validator" ErrorMessage="day is required" Display="None" />
<asp:Panel ID="panelCalender" runat="server" Visible="false" CssClass="sncore_inner_div">
 <asp:Calendar ID="selectDateCalendar" runat="server" BackColor="#FFFFCC" BorderColor="#FFCC66"
  BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
  ForeColor="#663399" Height="200px" ShowGridLines="True" Width="220px" NextMonthText="&#187;"
  OnSelectionChanged="selectDateCalendar_SelectionChanged" PrevMonthText="&#171;"
  SelectionMode="Day">
  <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
  <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
  <SelectorStyle BackColor="#FFCC66" />
  <OtherMonthDayStyle ForeColor="#CC9966" />
  <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
  <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
  <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
 </asp:Calendar>
</asp:Panel>
