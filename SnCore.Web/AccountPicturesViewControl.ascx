<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountPicturesViewControl.ascx.cs"
 Inherits="AccountPicturesViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="noPictures" runat="server" Visible="false">
 <img border="0" src="AccountPictureThumbnail.aspx" />
</asp:Panel>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="panelPictures">
 <ContentTemplate>
  <SnCoreWebControls:PagedList runat="server" ID="picturesView" RepeatColumns="1" RepeatRows="5"
   AllowCustomPaging="true">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="&#187;"
    PrevPageText="&#171;" HorizontalAlign="Center" PageButtonCount="5" />
   <ItemTemplate>
    <a href='<%# string.Format("AccountPictureView.aspx?id={0}", Eval("Id")) %>'>
     <img border="0" src='<%# string.Format("AccountPictureThumbnail.aspx?id={0}", Eval("Id")) %>'
      alt='<%# base.Render(Eval("Name")) %>' />
     <div style="font-size: smaller;">
      <%# GetCommentCount((int) Eval("CommentCount")) %>
     </div>
    </a>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
