<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="SystemAccountPropertyGroupsManage.aspx.cs"
 Inherits="SystemAccountPropertyGroupsManage" Title="Accounts | Property Groups" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:Label CssClass="sncore_navigate_item" ID="linkSystem" Text="Accounts" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkSection" Text="Property Groups" runat="server" />
 </div>
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Account Property Groups
    </div>
    <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemAccountPropertyGroupEdit.aspx"
     runat="server" />
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15"
     AllowPaging="true" OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false"
     CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn>
       <itemtemplate>
     <img src="images/Item.gif" />
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
      <asp:TemplateColumn>
       <itemtemplate>
        <a href='SystemAccountPropertyGroupEdit.aspx?id=<%# Eval("Id") %>'>
         Edit
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>