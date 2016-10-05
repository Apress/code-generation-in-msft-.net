<?xml version="1.0" encoding="UTF-8" ?>
<!-- ====================================================================
   Copyright ©2004, Kathleen Dollard, All Rights Reserved.
  ====================================================================
   I'm distributing this code so you'll be able to use it to see code
   generation in action and I hope it will be useful and you'll enjoy 
   using it. This code is provided "AS IS" without warranty, either 
   expressed or implied, including implied warranties of merchantability 
   and/or fitness for a particular purpose. 
  ====================================================================
  Summary: Generates the handcrafted class for business object collections !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="CollectionName"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/">
   <!-- We can't compare to the CollectionName here becasue there may be multiple collections -->
	<xsl:apply-templates select="//orm:Object[@Name=$Name]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
using System;
using System.Data;
using System.Data.SqlClient;
using CSLA.Data;
using CSLA;

namespace KADGen.BusinessObjects
{
   [Serializable()]
   public class <xsl:value-of select="$CollectionName"/> : gen<xsl:value-of select="$CollectionName"/>
   {
     
   // This is an empty class waiting for the programmer to override stuff

   }
}
</xsl:template>

</xsl:stylesheet> 
  
