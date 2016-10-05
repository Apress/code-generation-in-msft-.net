' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Encapsulates a log entry

Option Strict On
Option Explicit On 

Imports System

'! Class Summary: 

Public Class LogEntry
   Public Enum logLevel
      InfoOnly
      Warning
      SeriousError
      CriticalError
   End Enum

   Private m_Level As logLevel
   Private m_Message As String
   Private m_Source As String

   Public Sub New(ByVal level As logLevel, ByVal message As String, ByVal source As String)
      Me.Message = message
      Me.Level = level
      Me.Source = source
   End Sub

   Public Property Message() As String
      Get
         Return Me.m_Message
      End Get
      Set(ByVal value As String)
         Me.m_Message = value
      End Set
   End Property

   Public Property Source() As String
      Get
         Return Me.m_Source
      End Get
      Set(ByVal value As String)
         Me.m_Source = value
      End Set
   End Property

   Public Property Level() As logLevel
      Get
         Return Me.m_Level
      End Get
      Set(ByVal value As logLevel)
         Me.m_Level = value
      End Set
   End Property
End Class
