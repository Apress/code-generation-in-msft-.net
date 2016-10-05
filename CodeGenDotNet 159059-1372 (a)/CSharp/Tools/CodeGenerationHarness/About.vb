' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Displays information about the application 

Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class About
    Inherits System.Windows.Forms.Form

#Region "Class level declarations - empty"
#End Region

#Region " Windows Form Designer generated code"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization inside the InitForm method
	InitForm()

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
   Friend WithEvents pnlImage As System.Windows.Forms.Panel
   <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
      Me.pnlImage = New System.Windows.Forms.Panel
      Me.SuspendLayout()
      '
      'pnlImage
      '
      Me.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pnlImage.Location = New System.Drawing.Point(0, 0)
      Me.pnlImage.Name = "pnlImage"
      Me.pnlImage.Size = New System.Drawing.Size(292, 266)
      Me.pnlImage.TabIndex = 0
      '
      'About
      '
      Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
      Me.ClientSize = New System.Drawing.Size(292, 266)
      Me.Controls.Add(Me.pnlImage)
      Me.Name = "About"
      Me.Text = "About"
      Me.ResumeLayout(False)

   End Sub

#End Region

#Region "Event Handlers - empty"
#End Region

#Region "Public Methods and Properties - empty"
#End Region

#Region "Protected and Friend Methods and Properties - empty"
#End Region

#Region "Private Methods and Properties"
   Private Sub InitForm()
   End Sub
#End Region


   Private Sub pnlImage_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlImage.Paint
      Dim graphics As Drawing.Graphics = e.Graphics
      Dim pen As Drawing.Pen = Drawing.Pens.Wheat
      ' Dim fontFamily As new Drawing.Text.InstalledFontCollection(

      Dim font As Drawing.Font = New Drawing.Font("Papyrus", 30, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel, 1)
      Dim brush As New Drawing.Drawing2D.LinearGradientBrush(e.ClipRectangle, Drawing.Color.Indigo, Drawing.Color.Pink, Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal)
      graphics.FillRectangle(brush, e.ClipRectangle)
      brush.Dispose()
      brush = New Drawing.Drawing2D.LinearGradientBrush(e.ClipRectangle, Drawing.Color.Pink, Drawing.Color.AliceBlue, Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal)
      '      brush = New Drawing.Drawing2D.LinearGradientBrush(New Drawing.Rectangle(0, 0, e.ClipRectangle.Width, 150), Drawing.Color.AliceBlue, Drawing.Color.Pink, Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal)
      graphics.DrawString("Kathleen's", font, brush, 10, 10)
      graphics.DrawString("Code Generation", font, brush, 10, 50)
      graphics.DrawString("Harness", font, brush, 10, 90)
      font.Dispose()
      font = New Drawing.Font("Papyrus", 15, Drawing.FontStyle.Bold, Drawing.GraphicsUnit.Pixel, 1)
      graphics.DrawString("Version: " & Me.GetType.Assembly.GetName.Version.ToString, font, brush, 10, 140)
      brush.Dispose()


   End Sub
End Class
