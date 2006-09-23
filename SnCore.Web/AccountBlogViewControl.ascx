<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogViewControl.ascx.cs"
 Inherits="AccountBlogViewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<div class="sncore_h2" runat="server">
 <a href='AccountBlogView.aspx?id=<% Response.Write(BlogId); %>'>
  <% Response.Write(BlogName); %>
  <img src="images/site/right.gif" border="0" />
 </a>
</div>
<asp:Panel ID="panelLinks" runat="server" CssClass="sncore_createnew">
 <div id="divLinks" class="sncore_link" runat="server">
  <span id="spanLinkViewBlog" runat="server">
   <a href='AccountBlogView.aspx?id=<% Response.Write(BlogId); %>'>
    &#187; view blog
   </a>
  </span>
 </div>
</asp:Panel>
<table class="sncore_half_table">
 <tr>
  <td class="sncore_table_tr_td">
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
    RepeatRows="3" ShowHeader="false" AllowCustomPaging="true">
    <ItemTemplate>
     <table cellpadding="0" cellspacing="0" width="100%">
      <tr>
       <td valign="top">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
          <%# base.GetImage((string) Eval("Body")) %>
         </a>
       </td>
       <td valign="top">
        <div class="sncore_title">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
          <%# base.GetTitle((string) Eval("Title")) %>
         </a>
        </div>
        <div style="font-size: smaller;">
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>&#187; read</a>
         <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>&#187;
          <%# GetComments((int) Eval("CommentCount"))%>
         </a>
        </div>
        <div style="margin-top: 10px;">
         <%# base.GetDescription((string) Eval("Body")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </td>
 </tr>
</table>
