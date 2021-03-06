<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountGroupAccountsView.aspx.cs" Inherits="AccountGroupAccountsView" Title="Group Members" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel UpdateMode="Conditional" ID="panelLinks" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       <asp:Label id="labelName" runat="server" />
      </div>
      <div class="sncore_h2sub">
       <asp:HyperLink ID="linkAccountGroup" runat="server" Text="&#187; Back" />
       <a href="AccountGroupsView.aspx">&#187; All Groups</a>
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" AllowCustomPaging="true" RepeatColumns="4" RepeatRows="3" 
    RepeatDirection="Horizontal" CssClass="sncore_table" ShowHeader="false" OnItemCommand="gridManage_ItemCommand">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
     </a>
     <div>
      <asp:Image id="imgAdministrator" runat="server" ImageUrl="images/account/star.gif" Visible='<%# Eval("IsAdministrator") %>'
       Align="AbsMiddle" AlternateText="group administrator" />      
      <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
       <%# base.Render(Eval("AccountName")) %>
      </a>
     </div>
     <div>
      <asp:LinkButton id="linkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>'
       Text="&#187; Delete" OnClientClick='return confirm("Are you sure you want to delete this member?");' 
       Visible="<%# IsGroupAdministrator() %>"/>
     </div>
     <div>
      <asp:LinkButton id="linkPromote" runat="server" CommandName="Promote" CommandArgument='<%# Eval("Id") %>'
       Text="&#187; Promote" OnClientClick='return confirm("Are you sure you want to promote this member to group admin?");' 
       Visible='<%# IsGroupAdministrator() && ! (bool) Eval("IsAdministrator") %>'/>
     </div>
     <div>
      <asp:LinkButton id="linkDemote" runat="server" CommandName="Demote" CommandArgument='<%# Eval("Id") %>'
       Text="&#187; Demote" OnClientClick='return confirm("Are you sure you want to demote this member from group admin?");' 
       Visible='<%# IsGroupAdministrator() && (bool) Eval("IsAdministrator") %>'/>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList> 
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
