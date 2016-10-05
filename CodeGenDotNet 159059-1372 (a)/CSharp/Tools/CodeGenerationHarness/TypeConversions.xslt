<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />


<xsl:template name="GetSQLType">
   <xsl:param name="type"/>
   
   <xsl:variable name="localType">
      <xsl:call-template name="MakeLocal">
         <xsl:with-param name="string" select="$type"/>
      </xsl:call-template>
   </xsl:variable>
   
   <xsl:variable name="lowerType">
      <xsl:call-template name="MakeLower">
         <xsl:with-param name="string" select="$localType"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:choose>
      <xsl:when test="$lowerType='string'">char</xsl:when>
      <xsl:when test="$lowerType='integer'">int</xsl:when>
      <xsl:when test="$lowerType='boolean'">bit</xsl:when>
      <xsl:otherwise><xsl:value-of select="$lowerType"/></xsl:otherwise> 
   </xsl:choose>
</xsl:template>

<xsl:template name="GetNETType" >
   <xsl:param name="type"/>
   <xsl:variable name="lowerType">
      <xsl:call-template name="MakeLower">
         <xsl:with-param name="string" select="$type"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:choose>
      <xsl:when test="$lowerType='int'">System.Int32</xsl:when>
      <xsl:when test="$lowerType='smallint'">System.Int16</xsl:when>
      <xsl:when test="$lowerType='bigint'">System.Int64</xsl:when>
      <xsl:when test="$lowerType='float'">System.Double</xsl:when>
      <xsl:when test="$lowerType='decimal'">System.Decimal</xsl:when>
      <xsl:when test="$lowerType='base64binary'">System.Byte()</xsl:when>
      <xsl:when test="$lowerType='boolean'">System.Boolean</xsl:when>
      <xsl:when test="$lowerType='datetime'">System.DateTime</xsl:when>
      <xsl:when test="$lowerType='real'">System.Single</xsl:when>
      <xsl:when test="$lowerType='unsignedbyte'">System.Byte</xsl:when>
      <xsl:when test="$lowerType='char'">System.String</xsl:when>
      <xsl:when test="$lowerType='nchar'">System.String</xsl:when>
      <xsl:when test="$lowerType='varchar'">System.String</xsl:when>
      <xsl:when test="$lowerType='nvarchar'">System.String</xsl:when>
      <xsl:when test="$lowerType='ntext'">System.String</xsl:when>
      <xsl:when test="$lowerType='text'">System.String</xsl:when>
      <xsl:when test="$lowerType='smalldatetime'">System.DateTime</xsl:when>
      <xsl:when test="$lowerType='money'">System.Decimal</xsl:when>
      <xsl:when test="$lowerType='smallmoney'">System.Decimal</xsl:when>
      <xsl:when test="$lowerType='numeric'">System.Boolean</xsl:when>
      <xsl:when test="$lowerType='bit'">System.Boolean</xsl:when>
      <xsl:when test="$lowerType='tinyint'">System.Byte</xsl:when>
      <xsl:when test="$lowerType='timestamp'">System.Int64</xsl:when>
      <xsl:when test="$lowerType='uniqueidentifier'">System.Guid</xsl:when>
      <xsl:otherwise></xsl:otherwise> 
   </xsl:choose>
</xsl:template>

</xsl:stylesheet> 
  