<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="MarketingCampaignAccountRecepientsManage.aspx.cs" Inherits="MarketingCampaignAccountRecepientsManage" Title="Marketing Campaign Account Recepients" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="campaignName" runat="server" Text="Recepients" />
 </div>
 <asp:UpdatePanel ID="panelGrid" UpdateMode="Always" runat="server">
  <ContentTemplate>
   <div class="sncore_h2sub">
    <a href="MarketingCampaignsManage.aspx">&#187; Cancel</a>
    <asp:LinkButton ID="deleteAllRecepients" OnClientClick="return confirm('Are you sure you want to do this?')"
     OnClick="deleteAllRecepients_Click" runat="server" Text="&#187; Delete All" />
   </div>
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
       <img src='<%# ((bool) Eval("Sent")) ? "images/Item.gif" : "images/Question.gif" %>' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Account Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
      <itemtemplate>
       <a href='AccountView.aspx?id=<%# Eval("Account.Id") %>'>
        <%# base.Render(Eval("Account.Name")) %>
       </a>
       <div class="sncore_description" style="color: Red;">
        <%# base.Render(Eval("LastError")) %>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
   <div class="sncore_h2">
    Import
   </div>
   <div class="sncore_h3">
    All Users w/ Verified or Unverified E-Mails
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">       
     </td>
     <td class="sncore_form_value">
      <asp:CheckBox ID="importAllVerifiedEmails" Checked="True" CssClass="sncore_form_checkbox" Text="Verified E-Mails" runat="server" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">       
     </td>
     <td class="sncore_form_value">
      <asp:CheckBox ID="importAllUnverifiedEmails" CssClass="sncore_form_checkbox" Text="Unverified E-Mails" runat="server" />
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="importAllUsers" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="importAllUsers_Click" />
     </td>
    </tr>
   </table>
   <div class="sncore_h3">
    Single User
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      user id(s):
     </td>
     <td class="sncore_form_value">
      <asp:TextBox ID="importSingleUserIds" CssClass="sncore_form_textbox" runat="server" />
      <div class="sncore_description">
       separate by spaces
      </div>
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="Button1" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="importSingleUser_Click" />
     </td>
    </tr>
   </table>      
   <div class="sncore_h3">
    Users by Location
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      country and state:
     </td>
     <td class="sncore_form_value">
      <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
       <ContentTemplate>
        <asp:DropDownList CssClass="sncore_form_dropdown_small"
         ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
         runat="server" />
        <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState"
         AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" />
       </ContentTemplate>
      </asp:UpdatePanel>
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      city:
     </td>
     <td class="sncore_form_value">
      <asp:UpdatePanel runat="server" ID="panelCity" UpdateMode="Conditional">
       <ContentTemplate>
        <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
         DataValueField="Name" runat="server" AutoPostBack="true" />
       </ContentTemplate>
      </asp:UpdatePanel>
     </td>
    </tr>   
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="importUsersByLocation" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="importUsersByLocation_Click" />
     </td>
    </tr>
   </table>
   <div class="sncore_h3">
    Users w/ Account Property Value
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      group:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList ID="inputAccountPropertyGroup" runat="server" DataValueField="Id" DataTextField="Name" CssClass="sncore_form_dropdown" 
       OnSelectedIndexChanged="inputAccountPropertyGroup_SelectedIndexChanged" AutoPostBack="true" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      property:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList ID="inputAccountProperty" runat="server" DataValueField="Id" DataTextField="Name" CssClass="sncore_form_dropdown" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      value:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox ID="inputAccountPropertyValue" runat="server" CssClass="sncore_form_textbox" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
      <asp:CheckBox ID="inputAccountPropertyEmpty" runat="server" CssClass="sncore_form_checkbox" Checked="true" Text="include unset" />
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="importAccountProperty" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="importAccountProperty_Click" />
     </td>
    </tr>
   </table>      
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
