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
  Summary:  Incorporates ORM info identified by an INFO suffix
  Refactor: Replace this with a .NET function that will be faster -->


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

	
	<xsl:template match="orm:Assembly" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:apply-templates 
						select="//orm:AssemblyInfo[@Name=$name]" 
						mode="SetAttributes"/>
	</xsl:template>
	<xsl:template match="orm:Object" mode="SetUpAttributes">
		<xsl:variable name="assemblyname" select="ancestor::orm:Assembly/@Name"/>
		<xsl:variable name="name" select="@Name"/>
		<xsl:for-each select="//orm:Assembly[@Name=$assemblyname] | //orm:AssemblyInfo[@Name=$assemblyname]">
			<xsl:apply-templates 
						select=".//orm:ObjectInfo[@Name=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:Property" mode="SetUpAttributes">
		<xsl:variable name="assemblyname" select="ancestor::orm:Assembly/@Name"/>
		<xsl:variable name="objectname" select="ancestor::orm:Object/@Name"/>
		<xsl:variable name="name" select="@Name"/>
		<!-- Running against all these combinations in one select wasjust too ugly -->
		<xsl:for-each select="//orm:Assembly[@Name=$assemblyname] | //orm:AssemblyInfo[@Name=$assemblyname]">
			<xsl:for-each select=".//orm:Object[@Name=$objectname] | .//orm:ObjectInfo[@Name=$objectname]">
				<xsl:apply-templates 
						select=".//orm:PropertyInfo[@Name=$name]"
						mode="SetAttributes"/>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:ChildCollection" mode="SetUpAttributes">
		<xsl:variable name="assemblyname" select="ancestor::orm:Assembly/@Name"/>
		<xsl:variable name="objectname" select="ancestor::orm:Object/@Name"/>
		<xsl:variable name="name" select="@Name"/>
		<!-- Running against all these combinations in one select wasjust too ugly -->
		<xsl:for-each select="//orm:Assembly[@Name=$assemblyname] | //orm:AssemblyInfo[@Name=$assemblyname]">
			<xsl:for-each select=".//orm:Object[@Name=$objectname] | .//orm:ObjectInfo[@Name=$objectname]">
				<xsl:apply-templates 
						select=".//orm:ChildCollectionInfo[@Name=$name]"
						mode="SetAttributes"/>
			</xsl:for-each>
		</xsl:for-each>
		<xsl:value-of select="$assemblyname"/>*
		<xsl:value-of select="$objectname"/>*
		<xsl:value-of select="$name"/>*
	</xsl:template>
	<xsl:template match="orm:SPSet" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:apply-templates 
						select="//orm:SPSetInfo[@Name=$name]" 
						mode="SetAttributes"/>
	</xsl:template>
	<xsl:template match="orm:RetrieveParam" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
						select=".//orm:RetrieveParamInfo[@Name=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:WhereClause" mode="SetUpAttributes">
		<xsl:variable name="name" select="../@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
						select=".//orm:WhereClauseInfo[../@Name=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:SetSelect" mode="SetUpAttributes">
		<xsl:variable name="name" select="@TableName"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
						select=".//orm:SetSelectInfo[@TableName=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:SetSelectColumn" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="tablename" select="@TableName"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:for-each select=".//orm:SetSelect[@TableName=$tablename] | .//orm:SetSelectInfo[@TableName=$tablename]">
				<xsl:apply-templates 
						select=".//orm:SetSelectColumnInfo[@Name=$name]" 
						mode="SetAttributes"/>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:Privilege" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
						select=".//orm:PrivilegeInfo[@Name=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:BuildRecordset" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
						select=".//orm:BuildRecordsetInfo[@Name=$name]" 
						mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:BuildTable" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="buildname" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:for-each select=".//orm:BuildRecordset[@Name=$buildname] | .//orm:BuildRecordsetInfo[@Name=$buildname]">
				<xsl:apply-templates 
						select="//orm:BuildTableInfo[@Name=$name]" 
						mode="SetAttributes"/>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:BuildColumn" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="buildname" select="@Name"/>
		<xsl:variable name="buildtablename" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:for-each select=".//orm:BuildRecordset[@Name=$buildname] | .//orm:BuildRecordsetInfo[@Name=$buildname]">
				<xsl:for-each select=".//orm:BuildTable[@Name=$buildtablename] | .//orm:BuildTableInfo[@Name=$buildtablename]">
					<xsl:apply-templates 
						select=".//orm:BuildColumnInfo[@Name=$name]" 
						mode="SetAttributes"/>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="orm:RunSP" mode="SetUpAttributes">
		<xsl:variable name="name" select="@Name"/>
		<xsl:variable name="spname" select="ancestor::orm:SPSet/@Name"/>
		<xsl:for-each select="//orm:SPSet[@Name=$spname] | //orm:SPSetInfo[@Name=$spname]">
			<xsl:apply-templates 
					select=".//orm:RunSPInfo[@Name=$name]" 
					mode="SetAttributes"/>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet> 
  