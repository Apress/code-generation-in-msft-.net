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
  Summary: Provides supporting templates for web generation  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template name="GetRuntimeChildDesc">
   <xsl:param name="childname" />
      <xsl:choose>
         <xsl:when test="//orm:Object[@CollectionName=$childname]//orm:Property[@UseForDesc='true']">
            <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Property[@UseForDesc='true']" >
      desc &amp;= obj.<xsl:value-of select="@Name" />.ToString<xsl:text/>
               <xsl:if test="position()!=last()">
                  <xsl:text/> &amp; ", "<xsl:text/>
               </xsl:if>
            </xsl:for-each>
         </xsl:when>
         <xsl:otherwise>
            <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Property[@IsPrimaryKey='true']" >
      desc &amp;= obj.<xsl:value-of select="@Name" />.ToString<xsl:text/>
               <xsl:if test="position()!=last()">
                  <xsl:text/> &amp; ", "<xsl:text/>
               </xsl:if>
            </xsl:for-each>
         </xsl:otherwise>
      </xsl:choose>
</xsl:template>

<xsl:template name="AssignValue">
	<xsl:param name="value"  />
	<xsl:param name="type"  />
	<xsl:choose>
	   <xsl:when test="$type='System.Guid'">
	      <xsl:text/>New System.Guid(<xsl:value-of select="$value"/>.ToString)<xsl:text/>
	   </xsl:when>
	   <xsl:when test="$type='System.String'">
	      <xsl:value-of select="$value"/>.ToString<xsl:text/>
	   </xsl:when>
	   <xsl:otherwise>
	      <xsl:text/>CType(<xsl:value-of select="$value"/>, <xsl:text/>
	            <xsl:value-of select="$type"/>)
	   </xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="PrimaryKeyGridValues">
   <xsl:param name="cols"/>
   <xsl:param name="prefix" />
   <xsl:param name="dg" select="'dg'" />
   <xsl:for-each select="$cols[@IsPrimaryKey='true']">
      <xsl:choose>
         <xsl:when test="@NETType='System.Guid'">New Guid(Me.<xsl:value-of select="$prefix"/><xsl:value-of select="$dg"/>.SelectedItem.Cells(<xsl:value-of select="$dg"/>Columns.<xsl:value-of select="@Name"/>).Text)</xsl:when>
         <xsl:when test="@NETType='System.Int32'">CInt(Me.<xsl:value-of select="$prefix"/><xsl:value-of select="$dg"/>.SelectedItem.Cells(<xsl:value-of select="$dg"/>Columns.<xsl:value-of select="@Name"/>).Text)</xsl:when>
         <xsl:otherwise>Me.<xsl:value-of select="$prefix"/><xsl:value-of select="$dg"/>.SelectedItem.Cells(<xsl:value-of select="$dg"/>Columns.<xsl:value-of select="@Name"/>).Text</xsl:otherwise>
      </xsl:choose>
      <xsl:if test="position()!=last()">, _
               </xsl:if>
   </xsl:for-each>
</xsl:template>

<xsl:template name="FormControls">
   <xsl:param name="prefix"/>
   Protected WithEvents <xsl:value-of select="$prefix"/>btnNew As System.Web.UI.WebControls.LinkButton
   Protected WithEvents <xsl:value-of select="$prefix"/>Label2 As System.Web.UI.WebControls.Label
   Protected WithEvents <xsl:value-of select="$prefix"/>hplHome As System.Web.UI.WebControls.HyperLink
   <xsl:for-each select="orm:Property">
   Protected WithEvents <xsl:value-of select="$prefix"/><xsl:value-of select="@ControlName" /> As System.Web.UI.WebControls.TextBox
   </xsl:for-each>
   Protected WithEvents <xsl:value-of select="$prefix"/>hplList As System.Web.UI.WebControls.HyperLink
   'Protected WithEvents <xsl:value-of select="$prefix"/>rqdName As System.Web.UI.WebControls.RequiredFieldValidator
   Protected WithEvents <xsl:value-of select="$prefix"/>btnSave As System.Web.UI.WebControls.Button
   Protected WithEvents <xsl:value-of select="$prefix"/>btnCancel As System.Web.UI.WebControls.Button
   <xsl:for-each select="orm:ChildCollection">
   Protected WithEvents <xsl:value-of select="$prefix"/>dg<xsl:value-of select="@Name"/> As System.Web.UI.WebControls.DataGrid
   Protected WithEvents <xsl:value-of select="$prefix"/>btnAdd<xsl:value-of select="@Name"/> As System.Web.UI.WebControls.Button
   </xsl:for-each>
</xsl:template>

<xsl:template name="AssignFormControls">
   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      mbtnNew = btnNew
      mLabel2 = Label2
      mhplHome = hplHome
      <xsl:for-each select="orm:Property">
      m<xsl:value-of select="@ControlName" /> = <xsl:value-of select="@ControlName" />
      </xsl:for-each>
      mhplList = hplList
      'mrqdName = rqdName
      mbtnSave = btnSave
      mbtnCancel = btnCancel
      <xsl:for-each select="orm:ChildCollection">
      mdg<xsl:value-of select="@Name"/> = dg<xsl:value-of select="@Name"/> 
      mbtnAdd<xsl:value-of select="@Name"/> = btnAdd<xsl:value-of select="@Name"/> 
      </xsl:for-each>
      Mybase.OnLoad(e)
   End Sub
</xsl:template>

</xsl:stylesheet> 
  