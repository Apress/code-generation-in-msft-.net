<?xml version="1.0" encoding="UTF-8" ?>
<!--
  ====================================================================
   Copyright ©2004, Kathleen Dollard, All Rights Reserved.
  ====================================================================
   I'm distributing this code so you'll be able to use it to see code
   generation in action and I hope it will be useful and you'll enjoy 
   using it. This code is provided "AS IS" without warranty, either 
   expressed or implied, including implied warranties of merchantability 
   and/or fitness for a particular purpose. 
  ====================================================================
  Summary: Generates the plumbing class for edting -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="webSupport.xslt"/>
<xsl:import href="../../Chapter 8/BusinessObjects/CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="formname" select="$Name" />

<xsl:template match="/">
   '<xsl:value-of select="$BusinessObject"/>
   '<xsl:value-of select="$objectname"/>
   '<xsl:value-of select="$formname"/>
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
Option Strict On
Option Explicit On 

Imports System

Public Class gen<xsl:value-of select="$formname"/>
   Inherits Web.UI.Page
   
   Private Const PageName As String = "<xsl:value-of select="$formname"/>.aspx"
   <xsl:for-each select="orm:ChildCollection">
   <xsl:variable name="childname" select="@Name"/>
   Private Enum dg<xsl:value-of select="@Name"/>Columns
      <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Property" >
         <xsl:value-of select="@Name"/><xsl:text>
      </xsl:text>
      </xsl:for-each>
      Remove
      Details
   End Enum
   </xsl:for-each>


   Protected m<xsl:value-of select="$objectname"/> As cslaMiddleTierTest.<xsl:value-of select="$objectname"/>

#Region " Web Form Designer Generated Code "

   'This call is required by the Web Form Designer.
   &lt;System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

   End Sub
   <xsl:call-template name="FormControls">
      <xsl:with-param name="prefix" select="'m'"/>
   </xsl:call-template>

   'NOTE: The following placeholder declaration is required by the Web Form Designer.
   'Do not delete or move it.
   Private designerPlaceholderDeclaration As System.Object

   Private Sub Page_Init( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles MyBase.Init
      'CODEGEN: This method call is required by the Web Form Designer
      'Do not modify it using the code editor.
      InitializeComponent()
   End Sub

#End Region

   Private Sub Page_Load( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles MyBase.Load
      GetSession()
      If m<xsl:value-of select="$objectname"/> Is Nothing Then
         If <xsl:text/>
         <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
            <xsl:text/>m<xsl:value-of select="@ControlName"/><xsl:text/>
            <xsl:text/>.Text.Length = 0<xsl:text/>
            <xsl:if test="position()!=last()"> Or </xsl:if>
         </xsl:for-each> Then
            m<xsl:value-of select="$objectname"/> = cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.New<xsl:value-of select="$objectname"/>
         Else
            m<xsl:value-of select="$objectname"/> 
               <xsl:text/>= cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>.Get<xsl:text/>
               <xsl:value-of select="$objectname"/>( _
                  <xsl:text/>
               <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
                  <xsl:call-template name="GatherOne">
                     <xsl:with-param name="prefix" select="'Me.m'"/>
                  </xsl:call-template> 
                  <xsl:if test="position()!=last()">, _
                  </xsl:if>
               </xsl:for-each>)
         End If
      End If
      StoreSession
      <xsl:for-each select="orm:ChildCollection">
      If Not m<xsl:value-of select="$objectname"/> Is Nothing Then
         Me.mdg<xsl:value-of select="@Name"/>.DataSource = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@Name"/>
      Else
         Me.mdg<xsl:value-of select="@Name"/>.DataSource = Nothing
      End If
      </xsl:for-each>
      If Not m<xsl:value-of select="$objectname"/> Is Nothing Then
         If IsPostBack Then<xsl:text/>
         <xsl:for-each select="orm:ChildCollection">
            Me.mdg<xsl:value-of select="@Name"/>.DataBind()<xsl:text/>
         </xsl:for-each>
         Else
            Me.DataBind()
         End If
      End If

      Me.mbtnNew.Visible = cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.CanCreate
      Me.mbtnSave.Visible = cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.CanUpdate
      <xsl:for-each select="orm:ChildCollection">
      Me.mbtnAdd<xsl:value-of select="@Name"/>.Visible = cslaMiddleTierTest.<xsl:value-of select="@ObjectName"/>.CanCreate
      Me.mdg<xsl:value-of select="@Name"/>.Columns(Me.dg<xsl:value-of select="@Name"/>Columns.Remove).Visible = cslaMiddleTierTest.<xsl:value-of select="@ObjectName"/>.CanDelete
      </xsl:for-each>
   End Sub

   Private Sub btnSave_Click( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles mbtnSave.Click
      Me.SaveFormToObject()
      m<xsl:value-of select="$objectname"/> = CType(m<xsl:value-of select="$objectname"/>.Save, cslaMiddleTierTest.<xsl:value-of select="$objectname"/>)
      ClearSession()
      SendUserBack()
      ' NOTE: binding is not reset because we're leaving the page
   End Sub

   Private Sub btnCancel_Click( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles mbtnCancel.Click
      ClearSession()
      SendUserBack()
   End Sub

   Private Sub btnNew_Click( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles mbtnNew.Click
      Navigate.UnderConstruction(Me)
      'm<xsl:value-of select="$objectname"/> = cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.New<xsl:value-of select="$objectname"/>
      'StoreSession()
      'Navigate.ProjectEdit(Me)
   End Sub
   
#Region "Child Collection Response"
   <xsl:for-each select="orm:ChildCollection">
   <xsl:variable name="childname" select="@Name" />
   <xsl:variable name="childobject" select="//orm:Object[@CollectionName=$childname]" />
   <xsl:variable name="useclass">
      <xsl:choose>
         <xsl:when test="$childobject[@Inherits]"><xsl:value-of select="$childobject/@Inherits"/></xsl:when>
         <xsl:otherwise><xsl:value-of select="$childname"/></xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   Private Sub btnAdd<xsl:value-of select="@Name"/>_Click( _
               ByVal sender As System.Object, _
               ByVal e As System.EventArgs) _
               Handles mbtnAdd<xsl:value-of select="@Name"/>.Click
      <!--Me.SaveFormToObject()
      'Navigate.<xsl:value-of select="@Name"/>(Me) -->
      Navigate.UnderConstruction(Me)
   End Sub

   Private Sub dg<xsl:value-of select="@Name"/>_SelectedIndexChanged( _
               ByVal sender As Object, _
               ByVal e As System.EventArgs) _
               Handles mdg<xsl:value-of select="@Name"/>.SelectedIndexChanged
     <!-- If cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.CanUpdate Then
         Me.SaveFormToObject()
         m<xsl:value-of select="$objectname"/>.Save()
      End If
      Me.Session.Remove("<xsl:value-of select="$objectname"/>")
      Me.Session("<xsl:value-of select="@Name"/>") = cslaMiddleTierTest.<xsl:value-of select="$useclass"/>.Get<xsl:value-of select="$useclass"/>( _
               <xsl:call-template name="PrimaryKeyGridValues">
                  <xsl:with-param name="cols" select="$childobject/orm:Property[@IsPrimaryKey='true']"/>
                  <xsl:with-param name="prefix" select="'m'"/>
                  <xsl:with-param name="dg" select="concat('dg',@Name)"/>
               </xsl:call-template>)
      Navigate.<xsl:value-of select="@Name"/>Edit(Me) -->
      Navigate.UnderConstruction(Me)
   End Sub

   Private Sub dg<xsl:value-of select="@Name"/>_ItemCommand( _
               ByVal source As Object, _
               ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
               Handles mdg<xsl:value-of select="@Name"/>.ItemCommand
<!--      If e.CommandName = "SelectRole" Then
         Me.Session("<xsl:value-of select="@Name"/>") = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@Name"/>.GetItem( _
               <xsl:call-template name="PrimaryKeyGridValues">
                  <xsl:with-param name="cols" select="$childobject/orm:Property[@IsPrimaryKey='true']"/>
                  <xsl:with-param name="prefix" select="'m'"/>
                  <xsl:with-param name="dg" select="concat('dg',@Name)"/>
               </xsl:call-template>)
         Me.Session("Source") = PageName
         Me.Response.Redirect("ChooseRole.aspx")
      End If-->
      Navigate.UnderConstruction(Me)
   End Sub

   Private Sub dg<xsl:value-of select="@Name"/>_DeleteCommand( _
               ByVal source As Object, _
               ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
               Handles mdg<xsl:value-of select="@Name"/>.DeleteCommand
      m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@Name"/>.Remove(m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@Name"/>.GetItem( _
               <xsl:call-template name="PrimaryKeyGridValues">
                  <xsl:with-param name="cols" select="$childobject/orm:Property[@IsPrimaryKey='true']"/>
                  <xsl:with-param name="prefix" select="'m'"/>
                  <xsl:with-param name="dg" select="concat('dg',@Name)"/>
               </xsl:call-template>))
      Me.mdg<xsl:value-of select="@Name"/>.DataSource = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@Name"/>
      Me.mdg<xsl:value-of select="@Name"/>.DataBind()
   End Sub
   </xsl:for-each>
#End Region

#Region "Private methods and properties"
   Private Sub SaveFormToObject()
      With m<xsl:value-of select="$objectname"/>
      <xsl:for-each select="orm:Property">
         <xsl:choose>
         <xsl:when test="@IsPrimaryKey='true'"/>
         <xsl:otherwise>
         .<xsl:value-of select="@Name"/> = <xsl:text/>
         <xsl:call-template name="GatherOne">
            <xsl:with-param name="prefix" select="'Me.m'"/>
         </xsl:call-template>  
         </xsl:otherwise>
         </xsl:choose>
      </xsl:for-each>
      End With
   End Sub
   
   Private Sub SendUserBack()
      Navigate.<xsl:value-of select="$objectname"/>Select(Me)
   End Sub
   
   Private Sub GetSession()
      If TypeOf Me.Session("<xsl:value-of select="$objectname"/>") Is cslaMiddleTierTest.<xsl:value-of select="$objectname"/> Then
         m<xsl:value-of select="$objectname"/> = CType(Me.Session("<xsl:value-of select="$objectname"/>"), cslaMiddleTierTest.<xsl:value-of select="$objectname"/>)
      End If
   End Sub

   Private Sub StoreSession()
      Me.Session("<xsl:value-of select="$objectname"/>") = m<xsl:value-of select="$objectname"/>
   End Sub

   Private Sub ClearSession()
      Me.Session.Remove("<xsl:value-of select="$objectname"/>")
   End Sub

#End Region

End Class

</xsl:template>


<xsl:template name="GatherOne">
   <xsl:param name="prefix"/>
   <xsl:choose>
      <xsl:when test="@NETType='System.Int32'">CInt(</xsl:when>
      <xsl:when test="@NETType='System.Guid'">New System.Guid(</xsl:when>
      <xsl:when test="@NETType='System.Double'">CDbl(</xsl:when>
      <xsl:otherwise>CStr(</xsl:otherwise>
   </xsl:choose>
   <xsl:value-of select="$prefix"/><xsl:value-of select="@ControlName"/>.Text)<xsl:text/>
</xsl:template>

</xsl:stylesheet>

  