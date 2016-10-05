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
  Summary:  The first step in providing TSQL constraint translation
  NOTE:     This is preliminary code. -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure"
         xmlns:orm="http://kadgen.com/KADORM.xsd"
			xmlns:msxsl="urn:schemas-microsoft-com:xslt"
			xmlns:net="http://kadgen.com/NETTools" >
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

	<xsl:template match="@* | node() ">
	   <xsl:choose>
	      <xsl:when test="name()='orm:CheckConstraint'">
	         <xsl:variable name="checkconstraint" 
	                  select="msxsl:node-set(net:TranslateSQLExpression(@OriginalClause, 
	                                 ancestor::orm:Object/@Name, 
	                                 ancestor::orm:Property/@Name))"/>
            Joe<xsl:value-of select="count($checkconstraint)"/>*<xsl:value-of select="count($checkconstraint)/*"/>
            <xsl:for-each select="$checkconstraint">
            Harry
               <xsl:element name="orm:CheckConstraint">
   			      <xsl:apply-templates select="./@*"/>
               </xsl:element>
            </xsl:for-each>	               
	       <!--  <xsl:variable name="temp2" select="."/>
	         <xsl:variable name="temp3"><xsl:value-of select="."/></xsl:variable> 
	         <xsl:variable name="temp">
	            <xsl:value-of select="msxsl:node-set(net:TranslateSQLExpression(@OriginalClause))" />
	         </xsl:variable>
	         <xsl:for-each select="msxsl:node-set(net:TranslateSQLExpression(@OriginalClause))" >
	            Joe<xsl:value-of select="name()" />
	         </xsl:for-each>
	         <xsl:value-of select="net:Test2($temp3)"/>
	         <xsl:value-of select="$temp" /> -->
	      </xsl:when>
	      <xsl:otherwise>
		      <xsl:copy>
			      <xsl:apply-templates select="./@*"/>
			      <xsl:apply-templates select="node()"/>
		      </xsl:copy>
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:template>


</xsl:stylesheet> 
  