<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="/">
	<xsl:apply-templates select="//xsl:param"/>
</xsl:template>

<xsl:template match="xsl:param">
	<xsl:value-of select="@name"/>
	<xsl:if test="position()!=last()">|</xsl:if>
</xsl:template>

</xsl:stylesheet> 
  