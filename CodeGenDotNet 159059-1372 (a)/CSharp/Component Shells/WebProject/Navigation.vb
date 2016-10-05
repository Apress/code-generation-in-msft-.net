' ***^^^***|||***^^^***' ' ' ' %%%###%%%df413fe46cf828f31dcb6958e85c58ed%%%###%%%' ***^^^***|||***^^^***
Public Class NavigateURL
   Inherits genNavigateURL
End Class

Public Class Navigate
   Inherits genNavigate
End Class

Public Class ServerTransfer
   Inherits genServerTransfer

   Public Overloads Shared Sub Login(ByVal app As HttpApplication)
      app.Server.Transfer(NavigateURL.Login)
   End Sub

End Class
