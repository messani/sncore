<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountCreateInvitationWelcome.aspx.cs"
 Inherits="AccountCreateInvitationWelcome" Title="Welcome" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Welcome!
    </div>
    <div class="sncore_h2sub">
     <a href="Default.aspx">&#187; Return to HomePage</a>
     <a href="AccountPreferencesManage.aspx">&#187; Go to Me Me</a>
     <a href="AccountPicturesManage.aspx">&#187; Upload a Picture</a>
    </div>
    <table class="sncore_notice_info">
     <tr>
      <td class="sncore_notice_tr_td">
       <p>
        Dear <asp:Label ID="labelAccountName" runat="server" />. Your account has been created and you are now logged in.
       </p>
      </td>
     </tr>
    </table>
    <table class="sncore_account_table">
     <tr>
      <td>
       <p>
        It's now time to <a href="AccountPicturesManage.aspx">upload a picture</a> and 
        <a href="AccountPreferencesManage.aspx">adjust your preferences</a>.  
       </p>   
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
