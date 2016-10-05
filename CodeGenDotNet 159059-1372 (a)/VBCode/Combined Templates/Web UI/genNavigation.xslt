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
  Summary: Generates the code for navigation -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" 
			xmlns:fil="http://kadgen/filelist.xsd">
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
	Public Class genNavigateURL
		<xsl:apply-templates select="//fil:File[contains(@Name,'.aspx') and 
		                              not(contains(@Name,'.vb')) and 
		                              not(contains(@Name,'.resx')) ]" 
									mode="NavigateURL"/>
	End Class

	Public Class genNavigate
		<xsl:apply-templates select="//fil:File[contains(@Name,'.aspx') and 
		                              not(contains(@Name,'.vb')) and 
		                              not(contains(@Name,'.resx')) ]" 
									mode="Navigate"/>
	End Class

   Public Class genServerTransfer
		<xsl:apply-templates select="//fil:File[contains(@Name,'.aspx') and 
		                              not(contains(@Name,'.vb')) and 
		                              not(contains(@Name,'.resx')) ]" 
									mode="ServerTransfer"/>
   End Class
</xsl:template>

<xsl:template match="fil:File" mode="NavigateURL">
   <xsl:variable name="pagename">
      <xsl:call-template name="GetPageName" />
   </xsl:variable>
      Public Const <xsl:value-of select="$pagename"/> As String = <xsl:text/>
                  <xsl:text/>“~/<xsl:value-of select="$filename"/>”
</xsl:template>

<xsl:template match="fil:File" mode="Navigate">
   <xsl:variable name="pagename">
      <xsl:call-template name="GetPageName" />
   </xsl:variable>
      Public Shared Sub <xsl:value-of select="$pagename"/>(page as Web.UI.Page)
         page.Response.Redirect(genNavigateURL.<xsl:value-of select="$pagename"/>)
      End Sub
</xsl:template>

<xsl:template match="fil:File" mode="ServerTransfer">
   <xsl:variable name="filename">
      <xsl:call-template name="GetFileName"/>
   </xsl:variable>
   <xsl:variable name="pagename">
      <xsl:call-template name="GetPageName" />
   </xsl:variable>
      Public Shared Sub <xsl:value-of select="$pagename"/>(page as Web.UI.Page)
         page.Server.Transfer(genNavigateURL.<xsl:value-of select="$pagename"/>)
      End Sub
</xsl:template>

<xsl:template name="GetFileName">
   <xsl:variable name="isgrandparentdir">
      <xsl:for-each select="../..">
         <xsl:if test="name()='fil:Dir'">true</xsl:if>
      </xsl:for-each>
   </xsl:variable>
   <xsl:if test="$isgrandparentdir='true'">
      <xsl:apply-templates select=".." mode="DirTree"/>/<xsl:text/>
   </xsl:if>
   <xsl:value-of select="@Name"/>
</xsl:template>

<xsl:template name="GetPageName">
   <xsl:choose>
      <xsl:when test="substring-before(@Name,'.')='Default'">Home</xsl:when>
      <xsl:otherwise>
         <xsl:value-of select="substring-before(@Name,'.')"/>
      </xsl:otherwise>
   </xsl:choose>
      
</xsl:template>

<xsl:template match="*" mode="DirTree">
   <xsl:variable name="name" select="@Name"/>
   <xsl:for-each select=".." >
      <xsl:variable name="hasparentdir">
         <xsl:for-each select="..">
            <xsl:if test="name()='fil:Dir'">true</xsl:if>
         </xsl:for-each>
      </xsl:variable>
      <xsl:choose>
         <xsl:when test="$hasparentdir='true'">
            <xsl:variable name="parentdirs">
               <xsl:apply-templates select="." mode="DirTree" />
            </xsl:variable>
            <xsl:value-of select="concat($parentdirs,'/',$name)" />
         </xsl:when>
         <xsl:otherwise>
            <xsl:value-of select="$name" />
         </xsl:otherwise>
      </xsl:choose>
   </xsl:for-each>
</xsl:template>


</xsl:stylesheet> 

  