<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="MarketingCampaignsManage.aspx.cs" Inherits="MarketingCampaignsManage" Title="Marketing Campaigns" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Marketing Campaigns
 </div>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="MarketingCampaignEdit.aspx"
  runat="server" />
 <asp:UpdatePanel id="panelManageUpdate" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
    OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" AllowCustomPaging="true"
    CssClass="sncore_account_table">
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="center" />
    <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="center" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src="images/Item.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
      <itemtemplate>
       <a href='<%# base.Render(Eval("Url")) %>' target="_blank">
        <%# base.Render(Eval("Name")) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='MarketingCampaignAccountRecepientsManage.aspx?id=<%# Eval("Id") %>'>Recepients</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href='MarketingCampaignEdit.aspx?id=<%# Eval("Id") %>'>Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
