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
  Summary: Generates the editable class for the main edit user control  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 
<xsl:variable name="winnamespace" select="'KADGen.WinProject'" />

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">

using System;
using System.Windows.Forms;
using System.Threading;
using KADGen.BusinessObjectSupport;
using <xsl:value-of select="$bonamespace"/>;

namespace <xsl:value-of select="$winnamespace"/>
{
   public class <xsl:value-of select="$objectname"/>Edit : gen<xsl:value-of select="$objectname"/>Edit
   {
   }
}
</xsl:template>

</xsl:stylesheet> 
  