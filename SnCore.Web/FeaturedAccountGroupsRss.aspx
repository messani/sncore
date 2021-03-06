<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedAccountGroupsRss.aspx.cs" Inherits="FeaturedAccountGroupsRss" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description>all featured groups</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured Groups</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetAccountGroup((int) Eval("DataRowId")).Name) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountGroupView.aspx?id=<%# Eval("DataRowId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountGroupPictureThumbnail.aspx?id=<%# base.GetAccountGroup((int) Eval("DataRowId")).PictureId %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/AccountGroupView.aspx?id=<%# Eval("DataRowId") %>">
              <%# base.Render(base.GetAccountGroup((int) Eval("DataRowId")).Name) %>
             </a>
            </div>            
            <div style="color: silver">
             <%# base.Render(base.GetAccountGroup((int)Eval("DataRowId")).Description) %>
            </div>            
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountGroupView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountGroup/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
