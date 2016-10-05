' ====================================================================
'  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
' ====================================================================
'   I'm distributing this code so you'll be able to use it to see code
'   generation in action and I hope it will be useful and you'll enjoy 
'   using it. This code is provided "AS IS" without warranty, either 
'   expressed or implied, including implied warranties of merchantability 
'   and/or fitness for a particular purpose. 
' ====================================================================
'  Summary: Tools to facilitate working with navigator approach to XML DOM
'  Refactor: Since Whidbey will prefer the navigator approach, this class needs updating for transfer into Whidbey.

Option Explicit On 
Option Strict On

Imports System

Public Class NodeItHelpers
   Public Shared Function GetNodeIt( _
               ByVal xnav As Xml.XPath.XPathNavigator, _
               ByVal xPath As String, _
               ByVal nsmgr As Xml.XmlNamespaceManager) _
               As Xml.XPath.XPathNodeIterator
      Dim xpathExpr As Xml.XPath.XPathExpression = xnav.Compile(xPath)
      If Not nsmgr Is Nothing Then
         xpathExpr.SetContext(nsmgr)
      End If
      Return xnav.Select(xpathExpr)
   End Function

   Public Shared Function GetSortedNodeIt( _
            ByVal xnav As Xml.XPath.XPathNavigator, _
            ByVal xPath As String, _
            ByVal nsmgr As Xml.XmlNamespaceManager, _
            ByVal sortXpath As String, _
            ByVal isAscending As Boolean, _
            ByVal isCaseSensitive As Boolean, _
            ByVal isNumber As Boolean) _
            As Xml.XPath.XPathNodeIterator
      Dim xpathExpr As Xml.XPath.XPathExpression = xnav.Compile(xPath)
      Dim sortOrder As Xml.XPath.XmlSortOrder
      Dim caseOrder As Xml.XPath.XmlCaseOrder
      Dim datatype As Xml.XPath.XmlDataType
      If isAscending Then
         sortOrder = Xml.XPath.XmlSortOrder.Ascending
      Else
         sortOrder = Xml.XPath.XmlSortOrder.Descending
      End If
      If isCaseSensitive Then
         caseOrder = Xml.XPath.XmlCaseOrder.UpperFirst
      Else
         caseOrder = Xml.XPath.XmlCaseOrder.None
      End If
      If isNumber Then
         datatype = Xml.XPath.XmlDataType.Number
      Else
         datatype = Xml.XPath.XmlDataType.Text
      End If
      xpathExpr.AddSort(sortXpath, sortOrder, caseOrder, "", datatype)
      If Not nsmgr Is Nothing Then
         xpathExpr.SetContext(nsmgr)
      End If
      Return xnav.Select(xpathExpr)
   End Function

   Public Shared Function GetChildAttribute( _
               ByVal xnav As Xml.XPath.XPathNavigator, _
               ByVal childName As String, _
               ByVal attributeName As String, _
               ByVal nsmgr As Xml.XmlNamespaceManager) _
               As String
      Dim nodeit As Xml.XPath.XPathNodeIterator = GetNodeIt(xnav, childName, nsmgr)
      nodeit.MoveNext()
      Return CStr(nodeit.Current.GetAttribute(attributeName, ""))
   End Function
End Class
