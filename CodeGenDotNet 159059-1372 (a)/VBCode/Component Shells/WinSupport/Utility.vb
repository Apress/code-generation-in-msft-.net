' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
'  ====================================================================
'  Summary: Home for utility methods

Option Strict On
Option Explicit On 
Imports System


Public Class Utility

   Public Const border As Int32 = 6
   Public Const margin As Int32 = 4
   Public Const vbcrlf As String = Microsoft.VisualBasic.vbCrLf

   Public Shared Function SubstringBefore( _
                  ByVal s As String, _
                  ByVal sBefore As String) _
                  As String
      Dim iPos As Int32 = s.IndexOf(sBefore)
      If iPos = -1 Then
         Return s
      Else
         Return s.Substring(0, iPos)
      End If
   End Function

   Public Shared Function ParentForm( _
                  ByVal ctl As Windows.Forms.Control) _
                  As Windows.Forms.Form
      Do While Not TypeOf ctl Is Windows.Forms.Form
         ctl = ctl.Parent
      Loop
      If Not ctl Is Nothing Then
         Return CType(ctl, Windows.Forms.Form)
      End If
   End Function

   Public Shared Function ParentForm( _
                  ByVal ctl As Windows.Forms.MenuItem) _
                  As Windows.Forms.Form
      Return ctl.Parent.GetMainMenu.GetForm
   End Function

   Public Shared Function ParentForm( _
                  ByVal sender As Object) _
                  As Windows.Forms.Form
      If TypeOf sender Is Windows.Forms.MenuItem Then
         Return Utility.ParentForm(CType(sender, Windows.Forms.MenuItem))
      Else
         Return Utility.ParentForm(CType(sender, Windows.Forms.Control))
      End If
   End Function


   Public Shared Function Position( _
                  ByVal graphics As Drawing.Graphics, _
                  ByVal ctl As Windows.Forms.Control, _
                  ByVal iTop As Int32) _
                  As Int32
      Const c As Char = "K"c
      Dim sizef As Drawing.SizeF
      Dim sizefText As Drawing.SizeF
      Dim sizefSpace As Drawing.SizeF
      Dim sizeFNew As Drawing.SizeF
      Dim chars As Int32
      Dim cnt As Int32
      Dim noResize As Boolean
      Dim multiline As Boolean
      Dim iLines As Int32
      Dim iChars As Int32

      ctl.Top = iTop

      If TypeOf ctl Is Windows.Forms.DataGrid Then
         noResize = True
      ElseIf TypeOf ctl Is Windows.Forms.TextBox Then
         chars = CType(ctl, Windows.Forms.TextBox).MaxLength
         multiline = CType(ctl, Windows.Forms.TextBox).Multiline
      ElseIf TypeOf ctl Is Windows.Forms.ComboBox Then
         chars = CType(ctl, Windows.Forms.ComboBox).MaxLength
      End If

      If chars = 0 Then
         cnt = 1
      Else
         cnt = chars
      End If

      If Not noResize Then
         sizefText = graphics.MeasureString(ctl.Text, ctl.Font)
         sizef = graphics.MeasureString(New String(c, cnt), ctl.Font)
         sizefSpace = New Drawing.SizeF(ctl.Parent.Size.Width - ctl.Left - border, _
                        ctl.Parent.Size.Height - ctl.Top - border)

         If sizefText.Width > sizef.Width Then
            sizef.Width = sizefText.Width
         End If

         If chars > 32000 Then
            sizeFNew = New Drawing.SizeF(ctl.Parent.Width - ctl.Left, sizef.Height + 6)
         Else
            sizeFNew = sizef
         End If

         If multiline Then
            sizeFNew = New Drawing.SizeF(sizeFNew.Width, sizefSpace.Height)
         End If

         ctl.Size = New Drawing.Size(CInt(sizeFNew.Width), CInt(sizeFNew.Height))
      End If
      iTop = ctl.Bottom + margin
      Return iTop

   End Function

   Public Shared Function Position( _
                     ByVal graphics As Drawing.Graphics, _
                     ByVal lbl As Windows.Forms.Label, _
                     ByVal ctl As Windows.Forms.Control, _
                     ByVal labelWidth As Int32, _
                     ByVal iTop As Int32) _
                     As Int32
      Dim ret As Int32
      lbl.Width = labelWidth
      ctl.Left = lbl.Right + margin
      ret = Position(graphics, ctl, iTop)
      Position(graphics, lbl, iTop)
      Return ret
   End Function

   Public Shared Function IsEmpty(ByVal val As Object) As Boolean
      If TypeOf val Is Int16 Then
      ElseIf TypeOf val Is Int16 Then
         Return CType(val, Int16) = 0
      ElseIf TypeOf val Is Int32 Then
         Return CType(val, Int32) = 0
      ElseIf TypeOf val Is Int64 Then
         Return CType(val, Int64) = 0
      ElseIf TypeOf val Is Double Then
         Return CType(val, Double) = 0
      ElseIf TypeOf val Is Single Then
         Return CType(val, Single) = 0
      ElseIf TypeOf val Is String Then
         Return CType(val, String) = ""
      ElseIf TypeOf val Is Guid Then
         Return Guid.op_Equality(CType(val, Guid), Guid.Empty)
      ElseIf TypeOf val Is DateTime Then
         Return CType(val, DateTime) = #12:00:00 AM#
      ElseIf TypeOf val Is Byte Then
      End If
   End Function


   Public Shared Sub BindField( _
               ByVal control As Windows.forms.Control, _
               ByVal propertyName As String, _
               ByVal dataSource As Object, _
               ByVal dataMember As String)

      Dim bd As Windows.Forms.Binding
      Dim index As Integer

      For index = control.DataBindings.Count - 1 To 0 Step -1
         bd = control.DataBindings.Item(index)
         If bd.PropertyName = propertyName Then
            control.DataBindings.Remove(bd)
         End If
      Next
      control.DataBindings.Add(propertyName, dataSource, dataMember)

   End Sub

   Public Shared Sub BindEvents( _
               ByVal control As Windows.Forms.Control, _
               ByVal controlProperty As String, _
               ByVal source As Object, _
               ByVal sourceProperty As String, _
               ByVal delegFormat As Windows.Forms.ConvertEventHandler, _
               ByVal delegParse As Windows.Forms.ConvertEventHandler)

      Dim binding As Windows.Forms.Binding = control.DataBindings(controlProperty)

      If Not binding Is Nothing Then
         control.DataBindings.Remove(binding)
      End If

      binding = New Windows.Forms.Binding(controlProperty, source, sourceProperty)

      If Not delegFormat Is Nothing Then
         AddHandler binding.Format, delegFormat
      End If

      If Not delegParse Is Nothing Then
         AddHandler binding.Parse, delegParse
      End If

      control.DataBindings.Add(binding)

   End Sub

   Public Shared Function InvokeSharedMethod( _
            ByVal type As System.Type, _
            ByVal method As String, _
            ByVal ParamArray params() As Object) _
            As Object
      Do While type.GetMember(method).GetLength(0) = 0
         type = type.BaseType
         If type Is GetType(Object) Then
            type = Nothing
            Exit Do
         End If
      Loop
      Return type.InvokeMember(method, _
               Reflection.BindingFlags.InvokeMethod, _
               Nothing, Nothing, params)
   End Function

   Public Shared Function InvokeInstanceMethod( _
               ByVal type As System.Type, _
               ByVal method As String, _
               ByVal obj As Object, _
               ByVal ParamArray params() As Object) _
               As Object
      Return type.InvokeMember(method, _
               Reflection.BindingFlags.IgnoreCase Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.InvokeMethod, _
               Nothing, obj, params)
   End Function

   Public Shared Function InvokeSharedPropertyGet( _
               ByVal type As System.Type, _
               ByVal method As String, _
               ByVal ParamArray params() As Object) _
               As Object
      ' Static properties are a bit tricky to retrieve. 
      ' This is an explicit walk of the hierarchy looking 
      ' for the method, assuming if its found the signature
      ' is right. KAD 1/22/04
      Do While type.GetMember(method).GetLength(0) = 0
         type = type.BaseType
         If type Is GetType(Object) Then
            type = Nothing
            Exit Do
         End If
      Loop
      Return type.InvokeMember(method, _
               Reflection.BindingFlags.GetProperty, _
               Nothing, Nothing, params)
   End Function

   Public Shared Function CreateInstance( _
               ByVal type As System.Type, _
               ByVal ParamArray params() As Object) _
               As Object
      Return Activator.CreateInstance(type, params)
   End Function

   Public Shared Function GetWidth( _
               ByVal stringToMeasure As String, _
               ByVal font As Drawing.Font, _
               ByVal graphics As Drawing.Graphics) As Integer
      Return CInt(graphics.MeasureString(stringToMeasure, font).Width)
   End Function

End Class
