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
  Summary:  One of the steps of the ORM transformation process. -->

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

<xsl:template match="/ | @* | node()">
	<xsl:choose>
		<xsl:when test="name()='orm:Assembly'" /> <!-- Handled Below -->
		<xsl:otherwise>
			<xsl:copy>
				<xsl:apply-templates select="@*"  />
				<xsl:apply-templates select="node()"/>
	            <xsl:if test="name()='orm:MappingRoot'">
				      <xsl:call-template name="Assembly" />
	            </xsl:if>
			</xsl:copy>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="Assembly">
	<xsl:for-each select="//orm:Assembly">
		<xsl:copy>
			<xsl:apply-templates select="@*" />
   		<xsl:apply-templates select="node()"/>
   		<xsl:for-each select=".//orm:Object">
      		<xsl:apply-templates select=".//orm:ChildCollection" mode="MissingChildren" />
   		</xsl:for-each>
		</xsl:copy>
	</xsl:for-each>
</xsl:template>

<xsl:template match="orm:ChildCollection" mode="MissingChildren">
   <xsl:variable name="assembly" select="ancestor::orm:Assembly"/>
   <xsl:variable name="dsname" select="ancestor::*[@MapDataStructure][1]/@MapDataStructure"/>
   <xsl:variable name="name" select="@Name" />
   <xsl:variable name="singularname" select="net:GetSingular(@Name)" />
   <xsl:variable name="pos" select="position()"/>
   <xsl:variable name="tablename" select="@ChildTableName" />
   <xsl:if test="count($assembly//orm:Object[@Name=$singularname])=0">
      <!-- Need to add it -->
      <xsl:variable name="childof" select="net:GetSingular(ancestor::orm:Object/@TableName)"/>
      <xsl:variable name="inherits" select="net:GetSingular(@ChildTableName)"/>
      <xsl:for-each select="//dbs:DataStructure[$dsname=@Name]//dbs:Table[@Name=$tablename]">
         <xsl:call-template name="AddObject" >
            <xsl:with-param name="childof" select="$childof" />
            <xsl:with-param name="inherits" select="$inherits" />
            <xsl:with-param name="pos" select="$pos"/>
            <xsl:with-param name="name" select="$name" />
         </xsl:call-template>
      </xsl:for-each>
   </xsl:if>
</xsl:template>

</xsl:stylesheet> 
  