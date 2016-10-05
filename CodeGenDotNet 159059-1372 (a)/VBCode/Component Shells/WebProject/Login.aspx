<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Login.aspx.vb" Inherits="WebPTracker.Login" enableViewState="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
   <HEAD>
      <title>Login</title>
      <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
      <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
      <meta content="JavaScript" name="vs_defaultClientScript">
      <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
   </HEAD>
   <body>
      <FORM id="Form1" method="post" runat="server">
         <TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0">
            <TR>
               <TD>
                  <P><asp:image id="Image1" runat="server" Height="94px" Width="120px" ImageUrl="Images\PTracker.WMF"></asp:image></P>
               </TD>
               <TD>
                  <P><asp:label id="Label1" runat="server" Width="168px" Font-Names="Bauhaus 93" Font-Size="X-Large"
                        CssClass="H1">Project Tracker</asp:label></P>
               </TD>
            </TR>
            <TR>
               <TD vAlign="top">
                  <P>User Name:</P>
               </TD>
               <TD>
                  <P><asp:textbox id="txtUser" runat="server"></asp:textbox><asp:requiredfieldvalidator id="rqdUser" runat="server" ErrorMessage="User Name is required" ControlToValidate="txtUser"></asp:requiredfieldvalidator></P>
               </TD>
            </TR>
            <tr>
               <TD>
                  <P>Password:</P>
               </TD>
               <TD>
                  <P><asp:textbox id="txtPwd" runat="server" Width="152px" TextMode="Password"></asp:textbox><asp:requiredfieldvalidator id="rqdPwd" runat="server" ErrorMessage="Password is required" ControlToValidate="txtPwd"></asp:requiredfieldvalidator></P>
               </TD>
            </tr>
         </TABLE>
         <asp:button id="btnLogin" runat="server" Width="272px" Text="Login"></asp:button></FORM>
   </body>
</HTML>
