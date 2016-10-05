<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="WebPTracker._Default" EnableSessionState="True" enableViewState="False" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
   <HEAD>
      <title>Default</title>
      <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
      <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
      <meta content="JavaScript" name="vs_defaultClientScript">
      <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
   </HEAD>
   <body>
      <form id="Form1" method="post" runat="server">
         <TABLE id="Table2" cellSpacing="0" cellPadding="0" width="300" border="0">
            <TR>
               <TD>
                  <P><asp:image id="Image1" runat="server" ImageUrl="Images\PTracker.WMF" Width="160px" Height="126px"></asp:image></P>
               </TD>
               <TD>
                  <P><asp:label id="Label1" runat="server" Width="224px" CssClass="H1" Font-Size="XX-Large" Font-Names="Bauhaus 93">Project Tracker</asp:label></P>
               </TD>
            </TR>
            <tr>
               <TD vAlign="top">
                  <P>Welcome
                     <asp:label id="lblName" runat="server">User Identity</asp:label></P>
               </TD>
            </tr>
            <TR>
               <td>
               <TD>
                  <!-- TODO: Replace these with calls to NavigateURL methods -->
                  <P>
                     <asp:hyperlink id="HyperLink1" runat="server" 
                              NavigateUrl="~/ASPX/WebSelect/ProjectSelect.aspx">
                        Projects
                     </asp:hyperlink>
                  </P>
                  <P>
                     <asp:hyperlink id="HyperLink2" runat="server" 
                              NavigateUrl="~/ASPX/WebSelect/ResourceSelect.aspx">
                        Resources
                     </asp:hyperlink>
                  </P>
                  <!-- <P>
                     <asp:hyperlink id="Hyperlink3" runat="server" NavigateUrl="<%# WebPTracker.NavigateURL.ProjectSelect %>">
                        Projects
                     </asp:hyperlink>
                  </P>
                  <P>
                     <asp:hyperlink id="Hyperlink4" runat="server" NavigateUrl="ab<%# WebPTracker.NavigateURL.ResourceSelect %>">
                        Resources
                     </asp:hyperlink>
                  </P> -->
               </TD>
            </TR>
         </TABLE>
      </form>
   </body>
</HTML>
