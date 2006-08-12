<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemAttributesManage.aspx.cs"
 Inherits="SystemAttributesManage" Title="Attributes" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Attributes
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemAttributeEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
     AllowPaging="true" OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false"
     CssClass="sncore_account_table">
     <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
     <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
        <a href='SystemAttributeEdit.aspx?id=<%# Eval("Id") %>'><img 
         style='<%# (bool) Eval("HasBitmap") ? "" : "display:none" %>' border="0" src='SystemAttribute.aspx?id=<%# Eval("Id") %>&CacheDuration=0' /></a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
       <itemtemplate>
        <b><%# base.Render(Eval("Name")) %></b>
        <div class="sncore_description">
         <%# base.Render(Eval("Description")) %>
        </div>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
        <a href='SystemAttributeEdit.aspx?id=<%# Eval("Id") %>'>
         Edit
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete"></asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
