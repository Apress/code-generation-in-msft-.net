<?xml version="1.0" encoding="UTF-8" ?>
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

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
Option Explicit On
Option Strict On

Imports System
Imports cslaMiddleTierTest

Public Class <xsl:value-of select="@Name"/>Select
	Inherits gen<xsl:value-of select="@Name"/>Select

End Class
</xsl:template> 

</xsl:stylesheet>

  