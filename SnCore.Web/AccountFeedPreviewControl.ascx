<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedPreviewControl.ascx.cs"
 Inherits="AccountFeedPreviewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="2"
 RepeatRows="3" ShowHeader="false" AllowCustomPaging="true" Width="95%" CssClass="sncore_inner_table"
 BorderWidth="0px" BorderColor="white">
 <PagerStyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemTemplate>
  <div>
   <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
    <%# base.GetTitle(Eval("Title")) %>
   </a>
  </div>
  <div class="sncore_link_description">
   &#187; x-posted
   <span class='<%# (DateTime.UtcNow.Subtract(((DateTime) Eval("Created"))).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
     <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
   </span>
  </div>
  <div style="font-size: smaller;">
   <%# base.GetDescription(Eval("Link"), Eval("Description")) %>
  </div>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
