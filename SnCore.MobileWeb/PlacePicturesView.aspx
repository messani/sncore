<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlacePicturesView.aspx.cs"
 Inherits="PlacePicturesView" Title="Place | Pictures" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <h3>
  <asp:Label ID="placeName" runat="server" />
 </h3>
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" RepeatColumns="1" RepeatRows="5" RepeatDirection="Vertical">
  <pagerstyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <ItemTemplate>
   <div>
    <a href="PlacePictureView.aspx?id=<%# Eval("Id") %>">
     <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("Id") %>"
      alt="<%# Renderer.Render(Eval("Name")) %>" border="0" />
    </a>
   </div>
   <div class="sncore_description">
    <%# Renderer.Render(Eval("Description")) %>
   </div>
   <div class="sncore_description">
    <a href="PlacePictureView.aspx?id=<%# Eval("Id") %>">
     <%# GetCommentCount((int)Eval("CommentCount")) %>
    </a>
   </div>
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
