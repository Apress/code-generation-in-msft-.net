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
  Summary:  Creates the Retrieve (select one) Stored Procedure for TSQL-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:dbs="http://kadgen/DatabaseStructure" xmlns:orm="http://kadgen.com/KADORM.xsd">
	<xsl:import href="SPSupport.xslt" />
	<xsl:output method="text" encoding="UTF-8" indent="yes" />
	<xsl:preserve-space elements="*" />
	<xsl:param name="Name" />
	<xsl:param name="filename" />
	<xsl:param name="database" />
	<xsl:param name="gendatetime" />
	<xsl:variable name="spname">
		<xsl:call-template name="StripPathAndExtension">
			<xsl:with-param name="fname" select="$filename" />
		</xsl:call-template>
	</xsl:variable>
	
<xsl:template match="/">
	<xsl:apply-templates select="//orm:SPSet[@Name=$Name]/orm:RunSP[@Task='Retrieve']" mode="RunSP" />
</xsl:template>
	
<xsl:template match="orm:RunSP" mode="RunSP">
	<xsl:call-template name="OpenSP" />
	(<xsl:apply-templates select="orm:RunSPParam" mode="SPParameters"/>
	)
	AS

<xsl:for-each select="ancestor::orm:SPSet//orm:BuildRecordset">
	SELECT <xsl:apply-templates select=".//orm:BuildColumn" mode="ColumnSPList" />   
	<xsl:call-template name="ORMJoinList" />
	WHERE <xsl:value-of  select="orm:WhereClause/@Clause"/>
	<xsl:call-template name="NewLine" />
	<xsl:call-template name="NewLine" />
</xsl:for-each>
	
	RETURN (@@ERROR)
	
<xsl:call-template name="CloseSP">
	<xsl:with-param name="privileges" select="ancestor::orm:SPSet//orm:Privilege[contains(@Rights,'R')]" />
</xsl:call-template>

</xsl:template>
</xsl:stylesheet>
