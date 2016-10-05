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
  Summary: Generates the plumbing class for name/value pairs !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/">
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
   public class gen<xsl:value-of select="$Name"/> : NameValueList
   {
	   private static <xsl:value-of select="@Name"/> m<xsl:value-of select="@Name"/>;
	   <xsl:call-template name="SharedMethods"/>
	   <xsl:call-template name="Constructors"/>
	   <xsl:call-template name="Criteria"/>
	   <xsl:call-template name="DataAccess"/>
   }
}
   </xsl:template>

   <xsl:template name="SharedMethods">
	   #region Shared Methods
	   public static <xsl:value-of select="$Name"/> CachedList()
	   {
		   if( m<xsl:value-of select="@Name"/> == null )
		   {
			   m<xsl:value-of select="@Name"/> = GetList();
		   }
		   return m<xsl:value-of select="@Name"/>;
	   }

	   public static <xsl:value-of select="$Name"/> GetList()
	   {
		   m<xsl:value-of select="@Name"/> = <xsl:text/>
			   <xsl:text/>(<xsl:value-of select="@Name"/>)DataPortal.Fetch( new Criteria() );
		   return m<xsl:value-of select="@Name"/>;
	   }

	   #endregion
   </xsl:template>

   <xsl:template name="Constructors">
	   #region Constructors

	   static gen<xsl:value-of select="$Name"/>()
	   {
		   m<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>.GetList();
	   }
	   protected gen<xsl:value-of select="$Name"/>() : base()
	   {
		   // prevent direct instantiation
	   }

	   // this constructor overload is required because
	   // the base class (NameObjectCollectionBase) implements
	   // ISerializable
	   private gen<xsl:value-of select="$Name"/>(
			   System.Runtime.Serialization.SerializationInfo info,
			   System.Runtime.Serialization.StreamingContext context ) : base( info, context )
	   {
	   }

	   #endregion
   </xsl:template>

   <xsl:template name="Criteria">
	   #region Criteria
	   <!-- You have to escape any less than signs that you want in the output -->
	   [Serializable()]
	   private class Criteria : CSLA.Server.CriteriaBase
	   {
		   <xsl:call-template name="PrimaryKeyDeclarations"/>

		   public Criteria( <xsl:call-template name="PrimaryKeyArguments"/> ) : base( GetType( <xsl:value-of select="$Name"/> ) )
		   {
		   <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
			   Me.<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>
		   </xsl:for-each>
		   }
	   }

	   #endregion
   </xsl:template>

   <xsl:template name="DataAccess">
	   #region Data Access

	   <xsl:if test="orm:Privilege[contains(@Rights,'R')]">
	   // called by DataPortal to load data from the database
	   protected void DataPortal_Fetch( object Criteria )
	   {
		   SimpleFetch( <xsl:value-of select="@MapDataStructure"/>, <xsl:text>
			   </xsl:text><xsl:value-of select="@MapTable"/>, <xsl:text>
			   </xsl:text><xsl:value-of select="orm:Property[1]"/>, <xsl:text>
			   </xsl:text><xsl:value-of select="orm:Property[2]"/> );
	   }
	   </xsl:if>
	   #endregion
   </xsl:template> 

   <!-- Repeated from CSLAEditRoot -->
   <xsl:template name="PrimaryKeyDeclarations">
	   <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		   <xsl:variable name="name" select="@Name"	 />
		   <xsl:variable name="keytype" select="@NETType"	 />
		   public <xsl:value-of select="$keytype"/> <xsl:value-of select="$name"/>;
	   </xsl:for-each>
   </xsl:template>

   <xsl:template name="PrimaryKeyArguments">
	   <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		   <xsl:variable name="name" select="@Name"	 />
		   <xsl:variable name="keytype" select="@NETType"	 />
		   <xsl:text/><xsl:value-of select="$keytype"/> <xsl:value-of select="$name"/>;
		   <xsl:if test="position()!=last()">,
		   </xsl:if>
	   </xsl:for-each>
   </xsl:template>
</xsl:stylesheet> 
  
