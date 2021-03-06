<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacePropertyGroupsViewControl.ascx.cs"
 Inherits="PlacePropertyGroupsViewControl" %>
 
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:DataGrid runat="server" ID="groups" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false"
 OnItemCreated="groups_ItemCreated" BorderColor="White">
 <ItemStyle HorizontalAlign="Left" />
 <Columns>
  <asp:TemplateColumn>
   <ItemTemplate>
    <div class="sncore_h2" runat="server" id="title">
     <%# Renderer.Render(Eval("Name")) %>
    </div>
    <asp:DataGrid runat="server" ID="values" BorderWidth="0" ShowHeader="false" AutoGenerateColumns="false"
     CssClass="sncore_account_table">
     <ItemStyle HorizontalAlign="Left" CssClass="sncore_table_tr_td" />
     <Columns>
      <asp:TemplateColumn>
       <ItemTemplate>
        <div class="sncore_h3">
         <%# Render(Eval("PlacePropertyName")) %>
        </div>
        <div class="sncore_h2sub">
         <%# base.RenderValue(
          (string) Eval("PlacePropertyTypeName"), 
          (string) Eval("PlacePropertyGroupName"), 
          (string) Eval("PlacePropertyName"), 
          (string) Eval("Value")) %>
        </div>
       </ItemTemplate>
      </asp:TemplateColumn>
     </Columns>
    </asp:DataGrid>    
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
