Option Strict On
Option Explicit On 

Imports System
Imports System.Diagnostics

Public Class SourceSafe
   Inherits Utility.SourceBase

   Private mSSDatabase As New SourceSafeTypeLib.VSSDatabaseClass
   Private mRoot As SourceSafeTypeLib.VSSItem

   Public Sub New()
      mSSDatabase.Open("")
      mRoot = mSSDatabase.VSSItem("$/")
   End Sub

   Public Sub New(ByVal srcSafeINI As String)
      mSSDatabase.Open(srcSafeINI)
      mRoot = mSSDatabase.VSSItem("$/")
   End Sub

   Public Overrides Function CheckOut( _
                     ByVal file As String, _
                     ByVal baseLocalPath As String, _
                     ByVal baseProject As String) _
                     As Utility.SourceBase.ItemStatus
      file = BuildSSPath(file, baseLocalPath, baseProject)
      Dim item As SourceSafeTypeLib.VSSItem
      Dim status As Utility.SourceBase.ItemStatus
      item = Me.GetItem(file)
      status = Me.GetItemStatus(item)
      If status = Utility.SourceBase.ItemStatus.NotCheckedOut Then
         item.Checkout()
         status = Me.GetItemStatus(item)
      End If
      Return status
   End Function

   Public Overrides Function CheckIn( _
                     ByVal file As String, _
                     ByVal baseLocalPath As String, _
                     ByVal baseProject As String) _
                     As Utility.SourceBase.ItemStatus
      file = BuildSSPath(file, baseLocalPath, baseProject)
      Dim item As SourceSafeTypeLib.VSSItem
      Dim status As Utility.SourceBase.ItemStatus
      item = Me.GetItem(file)
      status = Me.GetItemStatus(item)
      Select Case status
         Case Utility.SourceBase.ItemStatus.DoesntExist
            ' Do nothing
         Case Utility.SourceBase.ItemStatus.CheckedOutToMe
            item.Checkin("Checked in via code generation")
            If Me.GetItemStatus(item) <> _
                     Utility.SourceBase.ItemStatus.NotCheckedOut Then
               Throw New Utility.SourceControlException("CheckIn Failed", file)
            End If
         Case Else
            Throw New Utility.SourceControlException( _
                     "Attempted to check in file you didn't have checked out", _
                     file)
      End Select
      Return status
   End Function

   Public Overrides Sub AddFile( _
                     ByVal file As String, _
                     ByVal baseLocalPath As String, _
                     ByVal baseProject As String)
      Dim dir As String
      Dim item As SourceSafeTypeLib.VSSItem
      Dim ssFile As String
      file = IO.Path.GetFullPath(file)
      ssFile = BuildSSPath(file, baseLocalPath, baseProject)
      dir = IO.Path.GetDirectoryName(ssFile).Replace("\", "/")
      If dir.IndexOf("/") > 0 Then
         dir = dir.Substring(dir.IndexOf("/") + 1)
      End If
      item = BuildProject(dir, mRoot)
      item.Add(file, "File added via automation " & DateTime.Now.ToString)
   End Sub

   Public Overrides Sub CleanUp()
      mRoot = Nothing
      ' TODO: Determine whether a marshal destroy call is appropriate.
   End Sub

   Protected Overridable Function BuildProject( _
                     ByVal file As String, _
                     ByVal item As SourceSafeTypeLib.VSSItem) _
                     As SourceSafeTypeLib.VSSItem
      Dim root As String
      Dim itemTest As SourceSafeTypeLib.VSSItem
      If file.IndexOf("/") >= 0 Then
         root = file.Substring(0, file.IndexOf("/"))
         file = file.Substring(file.IndexOf("/") + 1)
      Else
         root = file
         file = ""
      End If
      Try
         itemTest = item.Items.Item(root)
      Catch
      End Try
      If itemTest Is Nothing Then
         itemTest = item.NewSubproject(root, "Project added via automation " & DateTime.Now.ToString)
      End If
      file = file.Replace(root, "")
      If file.Trim.Length = 0 Then
         Return itemTest
      Else
         Return BuildProject(file, itemTest)
      End If
   End Function

   Private Function GetItem( _
                     ByVal file As String) _
                     As SourceSafeTypeLib.VSSItem
      Try
         Return mSSDatabase.VSSItem(file)
      Catch
      End Try
   End Function

   Private Function GetItemStatus( _
                     ByVal item As SourceSafeTypeLib.VSSItem) _
                     As Utility.SourceBase.ItemStatus
      If item Is Nothing Then
         Return Utility.SourceBase.ItemStatus.DoesntExist
      Else
         Select Case item.IsCheckedOut
            Case SourceSafeTypeLib.VSSFileStatus.VSSFILE_CHECKEDOUT
               Return Utility.SourceBase.ItemStatus.CheckedOut
            Case SourceSafeTypeLib.VSSFileStatus.VSSFILE_CHECKEDOUT_ME
               Return Utility.SourceBase.ItemStatus.CheckedOutToMe
            Case SourceSafeTypeLib.VSSFileStatus.VSSFILE_NOTCHECKEDOUT
               Return Utility.SourceBase.ItemStatus.NotCheckedOut
         End Select
      End If

   End Function
End Class
