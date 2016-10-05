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

<xsl:template match="orm:ChildCollection">
   <!-- Check for correct number of BuildRecordsets -->
   <xsl:if test="not net:IsPlural(@Name)">
      WARNING: The ChildCollection element of <xsl:value-of select="ancestor::orm:Object/@Name"/> appears to contain a singular form of the name (<xsl:value-of select="@Name"/>)
   </xsl:if>
</xsl:template>

<xsl:template match="orm:Object">
   <!-- Check for correct number of BuildRecordsets -->
   <xsl:if test="@NoGen='true'">
      WARNING: You have specified NoGen on an orm:Object (<xsl:value-of select="@Name"/>) which is an unusual occurrence for a middle tier object.
   </xsl:if>
</xsl:template>

<xsl:template match="orm:Object">
   <xsl:variable name="name" select="@Name" />
   <xsl:if test="count(//dbs:Table[@SingularName=$name])=0">
      ERROR:  You have specified an Object name with no corresponding Table (<xsl:value-of select="$name"/>)
   </xsl:if>
</xsl:template>

<xsl:template match="orm:ChildCollection">
   <xsl:variable name="name" select="@Name" />
   <xsl:if test="count(//dbs:Table[@Plural=$name])=0">
      ERROR:  You have specified an Child Collection name with no corresponding Table (Object=<xsl:value-of select="ancestor::orm:Object/@Name"/> - <xsl:value-of select="$name"/>)
   </xsl:if>
</xsl:template>

<xsl:template match="orm:BuildTable">
   <xsl:variable name="name" select="@Name" />
   <xsl:if test="count(//dbs:Table[@Name=$name])=0">
      ERROR:  You have specified an Build Table name with no corresponding database Table (Object=<xsl:value-of select="ancestor::orm:Object/@Name"/> - <xsl:value-of select="$name"/>)
   </xsl:if>
</xsl:template>

<xsl:template match="orm:Join//orm:Left | orm:Join//orm:Right">
   <xsl:choose>
      <xsl:when test="@JoinTable">
         <xsl:variable name="name" select="@JoinTable" />
         <xsl:if test="count(//dbs:Table[@Name=$name])=0">
            ERROR:  You have specified a Join Table name with no corresponding database Table (Object=<xsl:value-of select="ancestor::orm:Object/@Name"/>: BuildRecordset=<xsl:value-of select="ancestor::orm:BuildRecordset/@Name"/> - <xsl:value-of select="$name"/>)
         </xsl:if>
      </xsl:when>
      <xsl:when test="@Join">
         <xsl:variable name="name" select="@Join" />
         <xsl:if test="count(ancestor::orm:BuildRecordset//orm:Join[@Name=$name]) = 0">
            ERROR:  You have specified a Join that is not defined (Object=<xsl:value-of select="ancestor::orm:Object/@Name"/>: BuildRecordset=<xsl:value-of select="ancestor::orm:BuildRecordset/@Name"/> - <xsl:value-of select="$name"/>)
         </xsl:if>
      </xsl:when>
      <xsl:otherwise>
         ERROR: Join has no JoinTable or Join (Object=<xsl:value-of select="ancestor::orm:Object/@Name"/>: BuildRecordset=<xsl:value-of select="ancestor::orm:BuildRecordset/@Name"/>)
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>

</xsl:stylesheet> 
  