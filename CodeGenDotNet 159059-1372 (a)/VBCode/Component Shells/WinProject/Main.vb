' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: The main MDI form for the application  

Option Strict On
Option Explicit On 

Imports System
Imports System.Configuration
Imports System.Security.Principal
'Imports CSLA.Security
Imports System.Threading
'Imports CSLA.BatchQueue
Imports KADGen.BusinessObjectSupport
Imports System.Windows.Forms

Public Class Main
   Inherits WinSupport.BaseMain
   Dim WithEvents cbo As Windows.Forms.ComboBox
   Dim WithEvents txt As Windows.Forms.TextBox
   Dim WithEvents btn As Windows.Forms.Button
   Dim WithEvents rdo As Windows.Forms.RadioButton
   Dim WithEvents grp As Windows.Forms.GroupBox


#Region " Windows Form Designer generated code "

   Public Sub New()
      MyBase.New()

      'This call is required by the Windows Form Designer.
      InitializeComponent()

      'Add any initialization after the InitializeComponent() call
   End Sub

   'Form overrides dispose to clean up the component list.
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
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.SuspendLayout()
      '
      'Main
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(720, 427)
      Me.IsMdiContainer = True
      Me.Name = "Main"
      Me.Text = "Project Tracker"
      Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
      Me.ResumeLayout(False)

   End Sub

#End Region

   Protected Overrides ReadOnly Property BusinessObjectAssemblyList() _
               As System.Reflection.Assembly()
      Get
         Return New System.Reflection.Assembly() {Singletons.GetBusinessObjectAssembly}
      End Get
   End Property

   Protected Overrides Sub BuildMenus()
      MyBase.BuildMenus()
      Me.mnuOtherTools.Visible = True
   End Sub

End Class
