' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Contains some general use functions 

Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics

Public MustInherit Class SourceBase
   Implements IDisposable

   Public Enum ItemStatus
      CheckedOut = 1
      CheckedOutToMe
      NotCheckedOut
      DoesntExist
   End Enum

   Public MustOverride Function CheckOut( _
                     ByVal file As String, _
                     ByVal basePath As String, _
                     ByVal baseProject As String) _
                     As SourceBase.ItemStatus

   Public MustOverride Function CheckIn( _
                     ByVal file As String, _
                     ByVal basePath As String, _
                     ByVal baseProject As String) _
                     As Utility.SourceBase.ItemStatus

   Public MustOverride Sub AddFile( _
                     ByVal file As String, _
                     ByVal basePath As String, _
                     ByVal baseProject As String)

   Public MustOverride Sub CleanUp()

   Public Overridable Sub Dispose() _
                     Implements System.IDisposable.Dispose
      CleanUp()
      GC.SuppressFinalize(Me)
   End Sub

   Protected Overrides Sub Finalize()
      CleanUp()
   End Sub

   Public Overridable Function BuildSSPath( _
                     ByVal file As String, _
                     ByVal basePath As String, _
                     ByVal baseProject As String) _
                     As String
      'file = IO.Path.GetFullPath(file)
      'file = file.Replace(basePath, "")
      'If file.StartsWith("/") Or file.StartsWith("\") Then
      '   file = file.Substring(1)
      'End If
      file = IO.Path.GetFileName(file)
      Return IO.Path.Combine(baseProject, file).Replace("\", "/")
   End Function

End Class

Public Class SourceControlException
   Inherits System.ApplicationException

   Private mFile As String

   Public Sub New(ByVal message As String, _
                     ByVal file As String)
      MyBase.New(message)
      mFile = file
   End Sub

   Public Sub New(ByVal message As String, _
                     ByVal innerException As System.Exception, _
                     ByVal file As String)
      MyBase.New(message, innerException)
      mFile = file
   End Sub

   Public Property File() As String
      Get
         Return mFile
      End Get
      Set(ByVal Value As String)
         mFile = Value
      End Set
   End Property

End Class
