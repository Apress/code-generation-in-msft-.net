<?xml version="1.0" encoding="UTF-8" ?>
<!-- Copyright Kathleen Dollard 2003 -->
<xsl:stylesheet version="1.0" 
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:xs="http://www.w3.org/2001/XMLSchema"
		  xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"
        xmlns:dbs="http://kadgen/DatabaseStructure" >
    <xsl:strip-space elements="*"/>
    <xsl:output method="text" /> 

<xsl:template name="MakeUpper">
   <xsl:param name="string"/>
   <xsl:value-of select="translate($string,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" /> 
</xsl:template>
    
<xsl:template name="MakeLower">
   <xsl:param name="string"/>
   <xsl:value-of select="translate($string,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')" />
</xsl:template>

<xsl:template name="MakeLocal">
   <xsl:param name="string"/>
   <xsl:choose>
      <xsl:when test="contains($string,':')">
         <xsl:value-of select="substring-after($string,':')"/>
      </xsl:when>
      <xsl:otherwise>
         <xsl:value-of select="$string"/>
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>
    
<xsl:template name="NewLine">
	<xsl:text>&#x0d;&#x0a;</xsl:text>
</xsl:template>

<xsl:template name="CommaIfNotFirst">
	<xsl:if test="position()!=1">, </xsl:if>
</xsl:template>

<xsl:template name="CommaIfNotLast">
	<xsl:if test="position()!=last()">, </xsl:if>
</xsl:template>

<xsl:template name="FileOpen">
	<xsl:param name="imports" />

Option Strict On
Option Explicit On

Imports System
<xsl:call-template name="RecursiveImports">
	<xsl:with-param name="imports" select="normalize-space($imports)"/>
</xsl:call-template>

#Region "Description"
'
' <xsl:call-template name="StripPath">
	  <xsl:with-param name="fname" select="$filename" />
 </xsl:call-template>
'
' Last Genned On Date: <xsl:value-of select="$gendatetime"/>
'
'
#End Region
</xsl:template>

<xsl:template name="StoredProcFileOpen" >
	<xsl:variable name="shortfilename">
		<xsl:call-template name="StripPath">
			<xsl:with-param name="fname" select="$filename" />
		</xsl:call-template>
	</xsl:variable>
	<xsl:variable name="spname">
		<xsl:call-template name="StripExtension">
			<xsl:with-param name="fname" select="$shortfilename" />
		</xsl:call-template>
	</xsl:variable>
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[<xsl:value-of select="$spname"/>]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[<xsl:value-of select="$spname"/>]
GO





/******
Name:		<xsl:value-of select="$shortfilename"/>

Summary:	Select information from <xsl:value-of select="$spname"/>

Created:	3/3/2003 7:24:39 PM

*******/
</xsl:template>

<xsl:template name="StoredProcFileClose" >
	<xsl:variable name="shortfilename">
		<xsl:call-template name="StripPath">
			<xsl:with-param name="fname" select="$filename" />
		</xsl:call-template>
	</xsl:variable>
	<xsl:variable name="spname">
		<xsl:call-template name="StripExtension">
			<xsl:with-param name="fname" select="$shortfilename" />
		</xsl:call-template>
	</xsl:variable>

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[<xsl:value-of select="$spname"/>]  TO [public]
GO
</xsl:template>

<xsl:template name="RecursiveImports">
   <xsl:param name="imports"/>
   <xsl:variable name="remaining" select="substring-after($imports,',')"/>
   <xsl:choose>
   <xsl:when test="string-length($remaining) > 0">
Imports <xsl:value-of select="normalize-space(substring-before($imports,','))"/>
		<xsl:call-template name="RecursiveImports">
			<xsl:with-param name="imports" select="$remaining"/>
		</xsl:call-template>
   </xsl:when>
   <xsl:otherwise>
Imports <xsl:value-of select="normalize-space($imports)"/>
   </xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="StripPath">
	<xsl:param name="fname"/>
	<xsl:variable name="remaining" select="substring-after($fname,'&#x5c;')"/>
	<xsl:choose>
	<xsl:when test="string-length($remaining) > 0">
		<xsl:call-template name="StripPath">
			<xsl:with-param name="fname" select="$remaining"/>
		</xsl:call-template>	
	</xsl:when>
	<xsl:otherwise>
		<xsl:value-of select="$fname"/>
	</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="StripExtension">
	<xsl:param name="fname"/>
	<xsl:value-of select="substring-before($fname,'.')"/>
</xsl:template>

<!--         Code Generation Specific       -->
 
<xsl:template name="PKeyMatch">
	<xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
		<xsl:variable name="columnname" select="@Name" />
		<xsl:variable name="originalname">
			<xsl:call-template name="NameFromPrimaryKey"/>
		</xsl:variable>
 [<xsl:value-of select="ancestor::dbs:Tables/dbs:Table[@Name=$Name]/@OriginalName"/>
			<xsl:text/>].[<xsl:value-of select="$originalname"/>]<xsl:text/>
			<xsl:text/>= @<xsl:value-of select="@Name"/>
		<xsl:if test="position()!=last()"> AND  
		</xsl:if>
	</xsl:for-each>
</xsl:template>

<xsl:template name="SetPrimaryKeySig">
   <xsl:text/>Sub SetPrimaryKey(<xsl:text/>
	<xsl:call-template name="PrimaryKeySig" />)<xsl:text/>
</xsl:template>

<xsl:template name="PrimaryKeySig">
      <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField" >
      <xsl:text/>ByVal <xsl:value-of select="@Name"/> As <xsl:text/>
			<xsl:call-template name="NETTypeForColumn">
				<xsl:with-param name="tablename" select="$Name"/>
				<xsl:with-param name="columnname" select="@Name"/>
			</xsl:call-template>
			<xsl:if test="position()!=last()">, _
			            </xsl:if>
      </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeyList">
      <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField" >
      <xsl:text/> <xsl:value-of select="@Name"/>
			<xsl:if test="position()!=last()">, _
			            </xsl:if>
      </xsl:for-each>
</xsl:template>

<xsl:template name="PluralNameForTable">
	<xsl:param name="tablename" />
	<xsl:value-of select="ancestor::dbs:DataStructure/dbs:Tables/dbs:Table[@Name=$tablename]/@PluralName"/>

</xsl:template>

<xsl:template name="HasParents">
	<xsl:param name="tablename" />
	<!-- xsl:value-of select="1=1"/ -->
	<xsl:value-of select="boolean(ancestor::dbs:DataStructure/dbs:Tables/dbs:Table[@Name=$tablename]/dbs:TableConstraints/dbs:TableRelations/dbs:ParentTables/dbs:ParentTable)"/>
</xsl:template>


<xsl:template name="NETTypeForColumn">
	<xsl:param name="tablename"/>
	<xsl:param name="columnname" />
	<xsl:value-of select="//dbs:Tables/dbs:Table[@Name=$tablename]/dbs:TableColumns/dbs:TableColumn[@Name=$columnname]/@NETType"/>
</xsl:template>
									  
<xsl:template name="SQLTypeForColumn">
	<xsl:param name="tablename"/>
	<xsl:param name="columnname" />
	<xsl:value-of select="//dbs:Tables/dbs:Table[@Name=$tablename]/dbs:TableColumns/dbs:TableColumn[@Name=$columnname]/@SQLType"/>
</xsl:template>
									  
<xsl:template name="NameFromPrimaryKey">
	<xsl:variable name="columnname" select="@Name"/>
	<xsl:value-of select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$columnname]/@OriginalName" />
</xsl:template>

<xsl:template name="SQLTypeFromPrimaryKey">
	<xsl:variable name="columnname" select="@Name"/>
	<xsl:value-of select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$columnname]/@SQLType" />
</xsl:template>

<xsl:template name="NETTypeFromPrimaryKey">
	<xsl:variable name="columnname" select="@Name"/>
	<xsl:value-of select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$columnname]/@NETType" />
</xsl:template>

<xsl:template name="HasPrimaryKey">
	<xsl:choose>
		<xsl:when test ="count(dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField) > 0">true</xsl:when>
		<xsl:otherwise>false</xsl:otherwise>
	</xsl:choose> 
</xsl:template>

<xsl:template name="OnlyHasPrimaryKeys">
	<xsl:choose>
		<xsl:when test ="count(dbs:TableColumns/dbs:TableColumn[@IsPrimaryKey!='true']) > 0">true</xsl:when>
		<xsl:otherwise>false</xsl:otherwise>
	</xsl:choose> 
</xsl:template>

<xsl:template name="HasTimestamp">
	<xsl:choose>
		<xsl:when test ="count(dbs:TableColumns/dbs:TableColumn[@SQLType='Timestamp']) > 0">true</xsl:when>
		<xsl:otherwise>false</xsl:otherwise>
	</xsl:choose> 
</xsl:template>

<xsl:template name="GetControlPrefix">
	<xsl:variable name="columnname" select="@Name"/>
	<xsl:variable name="islookup">
		<xsl:call-template name="IsLookup">
			<xsl:with-param name="columnname" select="$columnname"/>
		</xsl:call-template>
	</xsl:variable>
   <xsl:choose>
   <xsl:when test="@IsAutoIncrement='true'">lbl</xsl:when>
   <xsl:when test="$islookup='true'">cbo</xsl:when>
   <xsl:when test="@NETType='System.Boolean'">chk</xsl:when>
   <xsl:when test="@NETType='System.DateTime'">cln</xsl:when>
   <xsl:otherwise>txt</xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="GetControlName">
	<xsl:call-template name="GetControlPrefix"/>
	<xsl:value-of select="@Name"/>
</xsl:template>

<xsl:template name="GetControlType">
	<xsl:variable name="columnname" select="@Name"/>
	<xsl:variable name="islookup">
		<xsl:call-template name="IsLookup">
			<xsl:with-param name="columnname" select="$columnname"/>
		</xsl:call-template>
	</xsl:variable>
   <xsl:choose>
   <xsl:when test="@IsAutoIncrement='true'">System.Windows.Forms.Label</xsl:when>
   <xsl:when test="$islookup='true'">System.Windows.Forms.ComboBox</xsl:when>
   <xsl:when test="@NETType='System.Boolean'">System.Windows.Forms.CheckBox</xsl:when>
   <xsl:when test="@NETType='System.DateTime'">System.Windows.Forms.DateTimePicker</xsl:when>
   <xsl:otherwise>System.Windows.Forms.TextBox</xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="SimpleNETType">
	<xsl:value-of select="substring-after(@NETType,'.')"/>
</xsl:template>

<xsl:template name="ConvertToNETType">
	<xsl:choose>
	<xsl:when test="@NETType='System.Guid'">New System.Guid</xsl:when>
	<xsl:when test="@NETType='System.Byte()'">(New Text.UTF8Encoding).GetBytes</xsl:when>
	<xsl:otherwise>System.Convert.To<xsl:text/>
				<xsl:call-template name="SimpleNETType"/></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="IsLookup">
	<xsl:param name="columnname"/>
	<xsl:value-of select="count(ancestor::dbs:Table/dbs:TableConstraints/dbs:TableRelations/dbs:ParentTables/dbs:ParentTable[dbs:ChildField/@Name=$columnname])>0"/>
</xsl:template>




</xsl:stylesheet>

  