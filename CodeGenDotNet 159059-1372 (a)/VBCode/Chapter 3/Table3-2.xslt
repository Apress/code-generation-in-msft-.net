<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema"	xmlns:kd="http://kadgen.com/KADGenDriving4x.xsd">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="/">
Subset<xsl:text>&#x09;</xsl:text>Description<xsl:text>&#x09;</xsl:text>Attributes
<xsl:for-each select="/xs:schema/xs:complexType" >
<xsl:value-of select="@name"/><xsl:text>&#x09;</xsl:text>
<xsl:value-of select="@kd:Desc"/><xsl:text>&#x09;</xsl:text>
		<xsl:for-each select="xs:attribute">
			<xsl:if test="position()>1">
				<xsl:text>&#x09;&#x09;</xsl:text>
			</xsl:if>
			<xsl:value-of select="@name"/><xsl:text>&#x0d;&#x0;</xsl:text>
		</xsl:for-each>
	</xsl:for-each>
</xsl:template>

</xsl:stylesheet> 
  