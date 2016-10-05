' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Search User Control
'  NOTE: THis code is preliminary

Public Class SearchUserControl
   Inherits System.Windows.Forms.UserControl

#Region "Class Declarations"
   Private mList As CSLA.ReadOnlyCollectionBase
#End Region

#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call

   End Sub

   'UserControl overrides dispose to clean up the component list.
   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing Then
         If Not (components Is Nothing) Then
            components.Dispose()
         End If
      End If
      MyBase.Dispose(disposing)
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   Friend WithEvents lblSearch As System.Windows.Forms.Label
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.lblSearch = New System.Windows.Forms.Label
      Me.SuspendLayout()
      '
      'lblSearch
      '
      Me.lblSearch.AutoSize = True
      Me.lblSearch.Location = New System.Drawing.Point(0, 0)
      Me.lblSearch.Name = "lblSearch"
      Me.lblSearch.Size = New System.Drawing.Size(40, 16)
      Me.lblSearch.TabIndex = 0
      Me.lblSearch.Text = "Search"
      '
      'SearchUserControl
      '
      Me.Controls.Add(Me.lblSearch)
      Me.Name = "SearchUserControl"
      Me.Size = New System.Drawing.Size(304, 24)
      Me.ResumeLayout(False)

   End Sub

#End Region

   Public Property BusinessObject() As CSLA.ReadOnlyCollectionBase
      Get
         Return mList
      End Get
      Set(ByVal Value As CSLA.ReadOnlyCollectionBase)
         mList = Value
         SetupControl()
      End Set
   End Property

   Private Sub SetupControl()

   End Sub

End Class
