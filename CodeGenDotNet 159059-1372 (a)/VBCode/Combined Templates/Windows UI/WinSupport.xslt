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
  Summary:  Supporting templates for the Windows.Forms generation. -->

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />


<xsl:template name="GetRuntimeChildDesc">
   <xsl:param name="childname" />
      <xsl:choose>
         <xsl:when test="//orm:Object[@CollectionName=$childname]//orm:Property[@UseForDesc='true']">
            <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Property[@UseForDesc='true']" >
      desc &amp;= obj.<xsl:value-of select="@Name" />.ToString<xsl:text/>
               <xsl:if test="position()!=last()">
                  <xsl:text/> &amp; ", "<xsl:text/>
               </xsl:if>
            </xsl:for-each>
         </xsl:when>
         <xsl:otherwise>
            <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Property[@IsPrimaryKey='true']" >
      desc &amp;= obj.<xsl:value-of select="@Name" />.ToString<xsl:text/>
               <xsl:if test="position()!=last()">
                  <xsl:text/> &amp; ", "<xsl:text/>
               </xsl:if>
            </xsl:for-each>
         </xsl:otherwise>
      </xsl:choose>
</xsl:template>

<xsl:template name="AssignValue">
	<xsl:param name="value"  />
	<xsl:param name="type"  />
	<xsl:choose>
	   <xsl:when test="$type='System.Guid'">
	      <xsl:text/>New System.Guid(<xsl:value-of select="$value"/>.ToString)<xsl:text/>
	   </xsl:when>
	   <xsl:when test="$type='System.String'">
	      <xsl:value-of select="$value"/>.ToString<xsl:text/>
	   </xsl:when>
	   <xsl:otherwise>
	      <xsl:text/>CType(<xsl:value-of select="$value"/>, <xsl:text/>
	            <xsl:value-of select="$type"/>)
	   </xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="StripNamespace">
   <xsl:param name="name"/>
   <xsl:choose>
      <xsl:when test ="contains($name,'.')" >
         <xsl:call-template name="StripNamespace">
            <xsl:with-param name="name" select="substring-after($name,'.')"/>
         </xsl:call-template>
      </xsl:when>
      <xsl:otherwise><xsl:value-of select="$name"/></xsl:otherwise>
   </xsl:choose>
</xsl:template>

</xsl:stylesheet> 
  