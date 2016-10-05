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
  Summary:  One of the steps in providing a reality check on templates
  NOTE:     This is preliminary code. -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure"
         xmlns:orm="http://kadgen.com/KADORM.xsd" 
         xmlns:gen="http://kadgen.com/GenInput.xsd"
         xmlns:net="http://kadgen.com/NETTools">
<xsl:import href="ORMSupport2.xslt"/>
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/ | node()">
	<xsl:apply-templates select="node()"/>
</xsl:template>

<xsl:template match="orm:Object">
   <!-- Check for primary keys -->
   <xsl:if test="count(.//orm:Property[@IsPrimaryKey='true'])=0">
      ERROR: There are no primary keys for Object <xsl:value-of select="@Name"/>
   </xsl:if>
</xsl:template>

<xsl:template match="orm:Object">
   <!-- Check for properties -->
   <xsl:if test="count(.//orm:Property)=0">
      ERROR: There are no properties for Object <xsl:value-of select="@Name"/>
   </xsl:if>
</xsl:template>

</xsl:stylesheet> 
  