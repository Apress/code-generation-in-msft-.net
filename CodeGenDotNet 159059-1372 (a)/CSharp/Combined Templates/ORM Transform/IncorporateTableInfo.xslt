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
  Summary:  Incorporate only table information identified in ORM file with an Info suffix. -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure"
         xmlns:orm="http://kadgen.com/KADORM.xsd">
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

	<xsl:template match="@* | node() ">
		<xsl:copy>
			<xsl:apply-templates select="./@*" mode="SetRootAttributes"	/>
			<xsl:apply-templates select="." mode="SetUpAttributes"/>
			<xsl:apply-templates select="node()"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="*" mode="SetAttributes" 	>
		<xsl:apply-templates select="./@*" mode="SetRootAttributes"	/>
	</xsl:template> 

	<xsl:template match="@*" mode="SetRootAttributes" >
		<xsl:copy/>
	</xsl:template>

	<xsl:template match="dbs:DataStructures" mode="SetUpAttributes">
	</xsl:template>

	<xsl:template match="dbs:Tables" mode="SetUpAttributes">
	</xsl:template>

	<xsl:template match="dbs:DataStructure" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:apply-templates 
			      select=".//dbs:DataStructureInfo[@Name=$name]"
					mode="SetAttributes"/>
	</xsl:template>
	
	<xsl:template match="dbs:Table" mode="SetUpAttributes">
		<xsl:variable name="structurename" select="ancestor::dbs:DataStructure/@Name"/>
		<xsl:variable name="name" select="@Name"/>
		<xsl:for-each 
						select="//dbs:DataStructure[@Name=$structurename] | //dbs:DataStructureInfo[@Name=$structurename]" >
			<xsl:apply-templates 
			         select=".//dbs:TableInfo[@Name=$name]"
						mode="SetAttributes"/>
	   </xsl:for-each>
	</xsl:template>

	<xsl:template match="dbs:TableColumn" mode="SetUpAttributes">
		<xsl:variable name="structurename" select="ancestor::dbs:DataStructure/@Name"/>
		<xsl:variable name="tablename" select="ancestor::dbs:Table/@Name"/>
		<xsl:variable name="name" select="@Name"/>
		<xsl:for-each 
						select="//dbs:DataStructure[@Name=$structurename] | //dbs:DataStructureInfo[@Name=$structurename]" >
		   <xsl:for-each 
						   select="//dbs:Table[@Name=$tablename] | //dbs:TableInfo[@Name=$tablename]" >
			   <xsl:apply-templates 
			            select=".//dbs:TableColumnInfo[@Name=$name]"
						   mode="SetAttributes"/>
	      </xsl:for-each>
	   </xsl:for-each>
	</xsl:template>

</xsl:stylesheet> 
  