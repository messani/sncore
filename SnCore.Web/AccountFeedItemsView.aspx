<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemsView.aspx.cs"
 Inherits="AccountFeedItemsView" Title="Blog Posts" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AutoScroll" Src="AutoScrollControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleBlogPosts" Text="Blog Posts" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         Do you have time to read two hundred blogs? These posts come from 
         <a href="AccountFeedsView.aspx">syndicated blogs</a>, newest posts first. It's a convenient way to 
         keep up with all this information. Blogs are updated several times a day.
         We also extract and publish <a href="AccountFeedItemImgsView.aspx">pictures</a>, 
         <a href="AccountFeedItemMediasView.aspx">podcasts and videos</a> from all these posts.
        </div>
        <div class="sncore_title_paragraph">
         Do you have a blog? You can <a href="AccountFeedWizard.aspx">syndicate yours here</a>. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountFeedsView.aspx">&#187; Blog Directory</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
       <a href="AccountFeedItemMediasView.aspx">&#187; Podcasts &amp; Videos</a>
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All Posts" />
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; All Local Posts" />
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
       <asp:Hyperlink id="linkPermalink" NavigateUrl="AccountFeedItemsView.aspx" runat="server" Text="&#187; Permalink" />
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountFeedItemsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelSearch" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server" EnableViewState="true">
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       search:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
      </td>
     </tr>
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
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
        OnClick="search_Click" EnableViewState="false" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCore:Cities id="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="5"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
    ShowHeader="false" OnDataBinding="gridManage_DataBinding" BorderWidth="0" BorderColor="White">
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
      <itemtemplate>
       <table width="100%" border="0" cellspacing="0" cellpadding="0" bordercolor="white">
        <tr>
         <td align="left" valign="top" width="*" class="sncore_message_left">
          <div class="sncore_message_header">
           <div class="sncore_message_subject">
            <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
             <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
            </a>
           </div>
           <div class="sncore_description">
            <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
             &#187; <%# GetComments((int) Eval("CommentCount"))%>
            </a>
            <a target="_blank" href='<%# base.Render(Eval("Link")) %>'>
             &#187; x-posted
            </a>
            in
            <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
             <%# base.Render(Eval("AccountName")) %>
            </a>'s
            <a href='AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>'>
             <%# base.Render(GetValue(Eval("AccountFeedName"), "Untitled")) %>
            </a>
            <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Created")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
             &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>            
            </span>
           </div>
          </div>
          <div class="sncore_message_body">
           <%# base.GetSummary((string) Eval("Description"), (string) Eval("AccountFeedLinkUrl")) %>
          </div>
         </td>
        </tr>
       </table>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AutoScroll runat="server" ID="autoScrollToForm" />
</asp:Content>
