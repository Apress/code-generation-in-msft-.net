Option Strict On
Option Explicit On 

Public Class Login
   Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

   'This call is required by the Web Form Designer.
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

   End Sub
   Protected WithEvents Label1 As System.Web.UI.WebControls.Label
   Protected WithEvents Image1 As System.Web.UI.WebControls.Image
   Protected WithEvents txtUser As System.Web.UI.WebControls.TextBox
   Protected WithEvents txtPwd As System.Web.UI.WebControls.TextBox
   Protected WithEvents btnLogin As System.Web.UI.WebControls.Button
   Protected WithEvents rqdUser As System.Web.UI.WebControls.RequiredFieldValidator
   Protected WithEvents rqdPwd As System.Web.UI.WebControls.RequiredFieldValidator

   'NOTE: The following placeholder declaration is required by the Web Form Designer.
   'Do not delete or move it.
   Private designerPlaceholderDeclaration As System.Object

   Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
      'CODEGEN: This method call is required by the Web Form Designer
      'Do not modify it using the code editor.
      InitializeComponent()
   End Sub

#End Region

   Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
   End Sub

   Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
      Dim userName As String = Me.txtUser.Text
      Dim password As String = Me.txtPwd.Text
      Dim principal As System.Security.Principal.IPrincipal
      Session.Clear()
      CSLA.Security.BusinessPrincipal.Login(userName, password)
      If System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated Then
         principal = System.Threading.Thread.CurrentPrincipal
         Me.Session("CSLA-Principal") = principal
         System.Web.HttpContext.Current.User = principal
         System.Web.Security.FormsAuthentication.RedirectFromLoginPage(userName, False)
      End If
   End Sub
End Class
