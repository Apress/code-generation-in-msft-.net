<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure"
			xmlns:orm="http://kadgen.com/KADORM.xsd" >
<xsl:import href="..\..\XFiles\Support.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="*">
		<xsl:copy >
			<xsl:apply-templates select="./*" />
		</xsl:copy> 
</xsl:template>

<xsl:template match="'Fred'
	<xsl:choose>
	<xsl:when test="local-name()='MappingRoot'"></xsl:when>
	<xsl:when test="local-name()='MappingRoot'"></xsl:when>
	<xsl:when test="local-name()='MappingRoot'"></xsl:when>
	<xsl:when test="local-name()='MappingRoot'"></xsl:when>
	<xsl:when test="local-name()='MappingRoot'"></xsl:when>
	<xsl:when test=""></xsl:when>
	<xsl:otherwise>
		<xsl:copy >
			<xsl:apply-templates select="./*" />
		</xsl:copy> 
	</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="orm:Object" >
	&lt;xsl:apply-templates match="//dbs:DataStructure[@Name='']/dbs:TableColumns/dbs:Table[@Name='']>
	<xsl:call-template name="StoredProcs">
		<xsl:with-param name="task" select="Create"/>
	</xsl:call-template>
	<xsl:call-template name="StoredProcs">
		<xsl:with-param name="task" select="Retrieve"/>
	</xsl:call-template>
	<xsl:call-template name="StoredProcs">
		<xsl:with-param name="task" select="Update"/>
	</xsl:call-template>
	<xsl:call-template name="StoredProcs">
		<xsl:with-param name="task" select="Delete"/>
	</xsl:call-template>
	<xsl:call-template name="StoredProcs">
		<xsl:with-param name="task" select="Set"/>
	</xsl:call-template>
</xsl:template>

<xsl:template match="orm:AllProperties" >
	&lt;xsl:apply-templates select="//dbs:DataStructure[@Name='']/dbs:TableColumns/dbs:Table[@Name='']/TableColumns/TableColumn" />
</xsl:template>

<xsl:template match="orm:Property" >
	&lt;xsl:apply-templates select="//dbs:DataStructure[@Name='']/dbs:TableColumns/dbs:Table[@Name='']/TableColumns/TableColumn[@Name='']" />
</xsl:template>

<xsl:template match="orm:AllColumns" >
	&lt;xsl:apply-templates select="//dbs:DataStructure[@Name='']/dbs:TableColumns/dbs:Table[@Name='']/TableColumns/TableColumn" />
</xsl:template>

<xsl:template match="orm:Column" >
	&lt;xsl:apply-templates select="//dbs:DataStructure[@Name='']/dbs:TableColumns/dbs:Table[@Name='']/TableColumns/TableColumn[@Name='']" />
</xsl:template>

<!-- -------------------------------- -->
<xsl:template name="StoredProcs">
	<xsl:param name="task"/>
	<xsl:choose>
		<xsl:when test="contains(@GenerateProcs,substring($task,1)")>
		</xsl:when>
		<xsl:otherwise>
			<xsl:apply-templates select="orm:BuildInfo/orm:BuildInfoSP[Task=$task]" mode="xslSP" />
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="orm:BuildInfoSP" mode="xslCreateSP">
			&lt;xsl:call-template>
				&lt;xsl:with-param name="SP" select='<xsl:value-of select="@Name"/>'"/>
			&lt;/xsl:call-template>
</xsl:template>

</xsl:stylesheet> 
  