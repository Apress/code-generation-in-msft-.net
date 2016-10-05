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
  Summary:  Creates the Create Stored Procedure for TSQL-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure" 
         xmlns:orm="http://kadgen.com/KADORM.xsd" >
<xsl:import href="SPSupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>

<xsl:variable name="spname" >
  <xsl:call-template name="StripPathAndExtension">
	  <xsl:with-param name="fname" select="$filename" />
  </xsl:call-template>
</xsl:variable>

<xsl:template match="/">
	<xsl:apply-templates 
			select="//orm:SPSet[@Name=$Name]/orm:RunSP[@Task='Create']" 
			mode="RunSP" />
</xsl:template>
	
<xsl:template match="orm:RunSP" mode="RunSP">
	<xsl:variable name="tablename" select="ancestor::orm:SPSet/@TableName"/>
	<xsl:variable name="origtablename" select="//dbs:Table[@Name=$tablename]/@OriginalName"/>

	<xsl:call-template name="OpenSP"/>
	(
	<xsl:apply-templates select="orm:RunSPParam" mode="SPParameters"/>
	)
	AS
   INSERT INTO  [<xsl:value-of select="$origtablename"/>]
	(<xsl:apply-templates select="orm:RunSPParam[not(@IsAutoIncrement='true')]" mode="SPFields"/>
	)
	VALUES
	(<xsl:apply-templates select="orm:RunSPParam[not(@IsAutoIncrement='true')]" mode="SPValues"/>
	)
	
	<xsl:for-each select="orm:RunSPParam[@IsAutoIncrement='true']">
		SELECT @<xsl:value-of select="@Name"/> = @@IDENTITY
   </xsl:for-each>	
	
	RETURN (@@ERROR)
	
	<xsl:call-template name="CloseSP">
		<xsl:with-param name="privileges" select="ancestor::orm:SPSet//orm:Privilege[contains(@Rights,'C')]" />
	</xsl:call-template>

</xsl:template>

<xsl:template match="orm:RunSPParam" mode="SPFields">
	[<xsl:value-of select="@Column"/>]<xsl:text/>
	<xsl:if test="position()!=last()">, </xsl:if>
</xsl:template>

<xsl:template match="orm:RunSPParam" mode="SPValues">
	@<xsl:value-of select="@Name"/><xsl:text/>
	<xsl:if test="position()!=last()">, </xsl:if>
</xsl:template>


</xsl:stylesheet> 
  