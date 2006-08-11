<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacePropertyGroupsControl.ascx.cs"
 Inherits="PlacePropertyGroupsControl" %>
 
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:DataGrid runat="server" ID="groups" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false">
 <ItemStyle HorizontalAlign="Left" />
 <Columns>
  <asp:TemplateColumn>
   <ItemTemplate>
    <div class="sncore_h2">
     <%# Renderer.Render(Eval("Name")) %>
    </div>
    <div class="sncore_h2sub">
     <a href='PlacePropertyGroupEdit.aspx?id=<%# Eval("Id") %>&pid=<%# Request["id"] %>'>
      <%# Renderer.Render(Eval("Description")) %> &#187; Edit
     </a>
    </div>
   </ItemTemplate>
  </asp:TemplateColumn>
 </Columns>
</asp:DataGrid>
