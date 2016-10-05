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
  Summary: Generates the plumbing class for selecting -->

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

<xsl:variable name="columns" select="//orm:Object[@Name=$objectname]//orm:SetSelectSP//orm:SPRecordSet//orm:Column[@IsPrimaryKey='true' or @UseForDesc='true']"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
Option Strict On
Option Explicit On 

Imports System

Public Class gen<xsl:value-of select="@Name"/>Select
   Inherits WebSelectBase

   Private Enum dgColumns
   <xsl:for-each select="$columns">
      <xsl:if test="position()!=1"><xsl:text>&#x0d;&#x0a;   </xsl:text></xsl:if>
      <xsl:text>   </xsl:text><xsl:value-of select="@Name"/>
   </xsl:for-each>
      Remove
   End Enum

#Region " Web Form Designer Generated Code "

   'This call is required by the Web Form Designer.
   &lt;System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

   End Sub
   Protected WithEvents Label2 As System.Web.UI.WebControls.Label
   Protected WithEvents hlHome As System.Web.UI.WebControls.HyperLink
   Protected WithEvents btnNew As System.Web.UI.WebControls.LinkButton
   Protected WithEvents dg As System.Web.UI.WebControls.DataGrid

   'NOTE: The following placeholder declaration is required by the Web Form Designer.
   'Do not delete or move it.
   Private designerPlaceholderDeclaration As System.Object

   Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
      'CODEGEN: This method call is required by the Web Form Designer
      'Do not modify it using the code editor.
      InitializeComponent()
   End Sub

#End Region

   Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      Me.dg.DataSource = cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>List.Get<xsl:text/>
               <xsl:value-of select="$objectname"/>List
         Me.DataBind()
         Me.btnNew.Visible = cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>.CanCreate
         Me.dg.Columns(dgColumns.Remove).Visible = cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>.CanDelete
   End Sub

   Private Sub dg_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dg.SelectedIndexChanged
      Me.Session("<xsl:value-of select="$objectname"/>") = <xsl:text/>
               <xsl:text/>cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>.Get<xsl:text/>
               <xsl:value-of select="$objectname"/>( _
               <xsl:call-template name="PrimaryKeyGridValues">
                  <xsl:with-param name="cols" select="$columns"/>
               </xsl:call-template>)
      Navigate.<xsl:value-of select="@Name"/>Edit(Me) 
   End Sub

   Private Sub dg_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dg.DeleteCommand
      Navigate.UnderConstruction(Me)
      <!-- TODO: Work out delete 
      cslaMiddleTierTest.<xsl:value-of select="$objectname"/>.Delete<xsl:value-of select="$objectname"/>( _
               <xsl:call-template name="PrimaryKeyGridValues">
                  <xsl:with-param name="cols" select="$columns"/>
               </xsl:call-template>)
      Me.dg.DataSource = cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>List.Get<xsl:text/>
               <xsl:value-of select="$objectname"/>List
      DataBind() -->
   End Sub

   Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
      Navigate.UnderConstruction(Me)
      <!-- TODO: Work out insert
      Me.Session("<xsl:value-of select="$objectname"/>") = <xsl:text/>
               <xsl:text/>cslaMiddleTierTest.<xsl:text/>
               <xsl:value-of select="$objectname"/>.New<xsl:text/>
               <xsl:value-of select="$objectname"/>
      Navigate.<xsl:value-of select="$objectname"/>Edit(Me) -->
   End Sub

End Class
</xsl:template>

</xsl:stylesheet>

  