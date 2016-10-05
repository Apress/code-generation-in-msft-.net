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
  Summary: Generates the editable class for the windows selection  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
Imports System.Threading
Imports <xsl:value-of select="$bonamespace"/>

Public Class <xsl:value-of select="$Name"/>
<xsl:choose>
	<xsl:when test="@Inherits">
	Inherits <xsl:value-of select="@Inherits"/>
	</xsl:when>
	<xsl:otherwise>
	Inherits gen<xsl:value-of select="$Name"/>
	</xsl:otherwise>
</xsl:choose>
  <xsl:call-template name="WindowsFormDesigner"/>
  <xsl:call-template name="RestOfForm"/>
End Class
</xsl:template>

<xsl:template name="WindowsFormDesigner">
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

  &lt;System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.SuspendLayout()
    '
    ' <xsl:value-of select="@Name"/>Select
    '
    Me.Name = "<xsl:value-of select="$Name"/>"
    Me.Text = "<xsl:value-of select="$Name"/>"
    Me.ResumeLayout(False)

  End Sub

#End Region
</xsl:template>

<xsl:template name="RestOfForm">
  Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
	 MyBase.OnLoad(e)
  End Sub
</xsl:template>

</xsl:stylesheet> 
  