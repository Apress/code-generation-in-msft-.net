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
  Summary:  Supporting templates for stored proc creation -->

<xsl:stylesheet version="1.0" 
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:xs="http://www.w3.org/2001/XMLSchema"
		  xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"
        xmlns:dbs="http://kadgen/DatabaseStructure" 
        xmlns:orm="http://kadgen.com/KADORM.xsd" 
        xmlns:net="http://kadgen.com/NETTools">
    <xsl:strip-space elements="*"/>
    <xsl:output method="text" /> 

<!-- Templates added for Rocky's Code Gen -->


<!-- Probably move this into a generic location -->
<xsl:template name="DefaultForColumn">
	<xsl:variable name="default">
		<xsl:if test="string-length(@Default)>0">CType(<xsl:text/>
			<xsl:value-of select="substring-before(substring-after(@Default,'('),')')"/>
			<xsl:text/>, <xsl:value-of select="@NETType" />)</xsl:if>
	</xsl:variable>
   <xsl:if test="string-length($default)>0"> = <xsl:value-of select="$default"/></xsl:if>
</xsl:template>

<xsl:template name="CheckPriveleges">
  <xsl:param name="privelegetype" />
  <xsl:param name="failuremessage" />
  <!-- I am worried about the case sensitivity of this -->
  <xsl:choose>
  <xsl:when test="dbs:TablePriveleges/dbs:TablePrivelege/@Type=$privelegetype and @Grantee='public'">
  <!-- Just a quick and dirty way to ignore these -->
  </xsl:when>
  <xsl:otherwise>
    If <xsl:text/>
	 <xsl:for-each select="dbs:TablePriveleges/dbs:TablePrivelege[@Type=$privelegetype]">
		<xsl:text/>Not Threading.Thread.CurrentPrincipal.IsInRole("<xsl:value-of select="@Grantee" />") <xsl:text/>
		<xsl:choose>
			<xsl:when test="position() != last()">  AndAlso _
			</xsl:when>
			<xsl:otherwise> Then
      Throw New System.Security.SecurityException("<xsl:value-of select="$failuremessage"/>")
    End If
			</xsl:otherwise>
		</xsl:choose> 
	 </xsl:for-each>
  </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:template match="dbs:ChildTable" mode="ExpandChildren">
	<xsl:variable name="tablename" select="ancestor::dbs:Table/@SingularName" />
	<xsl:variable name="childname" select="@Name" />
	<xsl:variable name="objectname">
		<xsl:choose>
			<xsl:when test="starts-with(@Name,$tablename)">
				<xsl:value-of select="@Name" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tablename" /><xsl:value-of select="net:GetPlural(@Name)" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:element name="orm:ChildCollection">
		<xsl:attribute name="Name"><xsl:value-of select="$objectname"/></xsl:attribute>
		<xsl:attribute name="ChildTableName"><xsl:value-of select="@Name"/></xsl:attribute>
		<xsl:attribute name="Ordinal"><xsl:value-of select="position()"/></xsl:attribute>
		<xsl:for-each select="dbs:ChildKeyField">
			<xsl:element name="orm:ChildKeyField">
				<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			</xsl:element>
		</xsl:for-each>
		
		<xsl:for-each select="ancestor::dbs:DataStructure//dbs:Table[@Name=$childname]" >
			<xsl:apply-templates select=".//dbs:ChildTable" mode="ExpandChildren" />
		</xsl:for-each>
	</xsl:element>
</xsl:template>

<xsl:template match="dbs:ParentTable" mode="ExpandParents">
	<xsl:variable name="tablename" select="ancestor::dbs:Table/@SingularName" />
	<xsl:element name="orm:ParentObject">
		<xsl:variable name="name" select="@Name"/>
		<xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
		<xsl:for-each select="ancestor::dbs:DataStructure//dbs:Table[@Name=$name]">
			<xsl:attribute name="SingularName">
				<xsl:value-of select="@SingularName"/>
			</xsl:attribute>
			<xsl:attribute name="PluralName">
				<xsl:value-of select="@PluralName"/>
			</xsl:attribute>
		</xsl:for-each>
		<xsl:for-each select="dbs:ParentKeyField">
			<xsl:element name="orm:ParentField">
				<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			</xsl:element>
		</xsl:for-each>
		<xsl:for-each select="dbs:ChildField">
			<xsl:element name="orm:ChildField">
				<xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
			</xsl:element>
		</xsl:for-each>
	</xsl:element>
</xsl:template>

<xsl:template name="PrimaryKeyList">
  <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
		<xsl:value-of select="@Name"	 />
		<xsl:if test="position()!=last()">, </xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeyArguments">
  <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
		<xsl:variable name="name" select="@Name"	 />
		<xsl:variable name="keytype" select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$name]/@NETType"	 />
		<xsl:text/>ByVal <xsl:value-of select="$name"/> As <xsl:value-of select="$keytype"/>) As <xsl:value-of select="$singularname"/>
		<xsl:if test="position()!=last()">, _
		</xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeyDeclarations">
  <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
		<xsl:variable name="name" select="@Name"	 />
		<xsl:variable name="keytype" select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$name]/@NETType"	 />
		<xsl:text/>Public <xsl:value-of select="$name"/> As <xsl:value-of select="$keytype"/>) As <xsl:value-of select="$singularname"/>
  </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeySPDeclare">
	<xsl:variable name="tablename" select="orm:Build/orm:BuildTable[1]/@Name"/>
	<xsl:variable name="datastructure" select="orm:Build/@DataStructure"/>
	<xsl:for-each select="//dbs:DataStructure[@Name=$datastructure]/dbs:Tables/dbs:Table[@Name=$tablename]"  >
		<xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
			<xsl:variable name="name" select="@Name"/>
			@<xsl:value-of select="$name" /><xsl:text>   </xsl:text>
			<xsl:value-of select="ancestor::dbs:Table/dbs:TableColumns/dbs:TableColumn[@Name=$name]/@SQLType" />
			<xsl:if test="position()!=last()">, 
			</xsl:if>
		</xsl:for-each>
	</xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeySPWhere">
	WHERE <xsl:for-each select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField">
		<xsl:value-of select="@Name" /> = @<xsl:value-of select="@Name" /> <xsl:text>   </xsl:text>
		<xsl:if test="position()!=last()"> AND 
		</xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="WhereStartClause">
	<xsl:param name="list" />
	<xsl:param name="pass"  />
	<xsl:param name="objectname" />
	<xsl:choose>
		<xsl:when test="$list">
			<xsl:variable name="andclause">
				<xsl:if test="$pass!=0"> AND </xsl:if>
			</xsl:variable>
			<xsl:variable name="first" select="$list[1]"/>
			<xsl:variable name="rest">
				<xsl:call-template name="WhereStartClause">
					<xsl:with-param name="list" select="$list[position()!=1]"/>
					<xsl:with-param name="pass" select="pass + 1"/>
					<xsl:with-param name="objectname" select="$objectname"/>
				</xsl:call-template>
			</xsl:variable>
			<!--<xsl:variable name="paramname">
				<xsl:call-template name="GetParamName">
					<xsl:with-param name="objectname" select="$objectname"/>
					<xsl:with-param name="fieldname" select="$first/@Name"/>
				</xsl:call-template>
			</xsl:variable> -->

			<xsl:value-of select="concat( $andclause, '@', $first/@Name, ' = ', $first/@Name, $rest)"/>
		</xsl:when>
		<xsl:otherwise/>
	</xsl:choose>
</xsl:template>

<xsl:template name="ColumnSPDeclare">
	<xsl:for-each select="dbs:TableColumns/dbs:TableColumn">
		@<xsl:value-of select="@Name" /><xsl:text>   </xsl:text><xsl:value-of select="@SQLType" />
		<xsl:if test="position()!=last()">, 
		</xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template match="orm:BuildColumn | orm:SetSelectColumn" mode="ColumnSPList">
   <xsl:variable name="tablename">
      <xsl:choose>
         <xsl:when test="name()='orm:SetSelectColumn'">
            <xsl:call-template name="GetTableNameFromName">
	            <xsl:with-param  name="tablename" select="ancestor::orm:SetSelect/@TableName"/>
            </xsl:call-template>
         </xsl:when>
         <xsl:otherwise>
            <xsl:call-template name="GetTableNameFromName">
	            <xsl:with-param  name="tablename" select="ancestor::orm:BuildTable/@Name"/>
            </xsl:call-template>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
	[<xsl:value-of select="$tablename"/>].[<xsl:text/>
	<xsl:value-of select="@Column"/>]<xsl:text/>
	<xsl:if test="@Name!=@Column">
	<xsl:text/> AS <xsl:value-of select="@Name"/>
	</xsl:if>
	<xsl:if test="position()!=last()">, </xsl:if>
</xsl:template>


<xsl:template name="GetRecordSetName">
	<xsl:choose>
		<xsl:when test="@Recordset">@Recordset</xsl:when>
		<xsl:when test="position()=1">Table</xsl:when>
		<xsl:otherwise>Table<xsl:value-of select="position()-1"/></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="ColumnSPParamList">
	<xsl:for-each select="dbs:TableColumns/dbs:TableColumn">
		@<xsl:value-of select="@Name"/><xsl:if test="position()!=last()">, 
		</xsl:if>
	</xsl:for-each>
</xsl:template>

<xsl:template name="ColumnSPAssign">
	<xsl:for-each select="dbs:TableColumns/dbs:TableColumn">
		[<xsl:value-of select="@OriginalName"/>] = @<xsl:value-of select="@Name"/><xsl:if test="position()!=last()">, 
		</xsl:if>
	</xsl:for-each>
</xsl:template>

<xsl:template name="ORMJoinList">
	FROM <xsl:text/>
	<xsl:choose>
		<xsl:when test="count(orm:BuildTable)= 0" >
		ERROR - Where are the tables?
		</xsl:when>
		<xsl:when test="count(orm:BuildTable)=1" >
		   [<xsl:call-template name="GetTableName"/>]
		</xsl:when>
		<xsl:when test="count(.//orm:Join) = 0">
		   ERROR - You need a join if you have multiple tables
		</xsl:when>
		<xsl:otherwise>
			<!-- OK, The fun starts - let's build a join -->
			<xsl:for-each select=".//orm:Join[1]"	>
				<xsl:call-template name="BuildJoin"/>
			</xsl:for-each>
		</xsl:otherwise>
	</xsl:choose> 
</xsl:template>

<xsl:template name="GetTableName">
   <xsl:call-template name="GetTableNameFromName">
	    <xsl:with-param  name="tablename" select="orm:BuildTable/@Name"/>
   </xsl:call-template>
</xsl:template>

<xsl:template name="GetTableNameFromName">
   <xsl:param name="tablename"/>
	<xsl:for-each select="//dbs:Table[@Name=$tablename]">
		<xsl:value-of select="@OriginalName" />
	</xsl:for-each>
</xsl:template>

<xsl:template match="orm:RunSPParam" mode="SPParameters">
	@<xsl:value-of select="@Name" /><xsl:text> </xsl:text>
	<xsl:value-of select="@SQLType" />
	<xsl:if test="string-length(@MaxLength)>0">
		<xsl:text/> (<xsl:value-of select="@MaxLength"/>)<xsl:text/>
	</xsl:if>
	<xsl:if test="@IsAutoIncrement='true'">
		<xsl:if test="ancestor::orm:RunSP[@Task='Create']"> OUTPUT </xsl:if>
	</xsl:if>
	<xsl:if test="position()!=last()">, </xsl:if>
	<!--@<xsl:value-of select="@Name"/><xsl:text> </xsl:text> 
		<xsl:call-template name="SQLTypeForColumn">
			<xsl:with-param name="tablename" select="$Name"/>
			<xsl:with-param name="columnname" select="@Name"/>
		</xsl:call-template>
		<xsl:if test="@SQLType='VarChar' or @SQLType='NVarChar'">
		<xsl:text/>(<xsl:value-of select="@MaxLength"/>)<xsl:text/>
		</xsl:if>
		<xsl:if test="position()!=last()">,</xsl:if> -->
</xsl:template>

<xsl:template name="BuildJoin">
	<xsl:for-each select="orm:Left">
		<xsl:call-template name="BuildSingleJoin" />
	</xsl:for-each>
	<xsl:variable name="type" >
		<xsl:choose>
			<xsl:when test="@Type=''">Inner</xsl:when>
			<xsl:when test="@Type"><xsl:value-of select="@Type"/></xsl:when>
			<xsl:otherwise>Inner</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:text> </xsl:text><xsl:value-of select="$type"/> JOIN <xsl:text/>
	<xsl:for-each select="orm:Right">
		<xsl:call-template name="BuildSingleJoin" />
	</xsl:for-each>
	<xsl:variable name="clause">
		<xsl:value-of select="orm:JoinOn/@Clause"/>
	</xsl:variable>
	<xsl:if test="$clause=''">
		ERROR - CARTISIAN PRODUCT
	</xsl:if>
		ON <xsl:value-of select="$clause" />
	
</xsl:template>

<xsl:template name="BuildSingleJoin">
	<xsl:choose>
		<xsl:when test="@Join">
			<xsl:variable name="joinname" select="@Join" />
			<xsl:for-each select="ancestor::orm:Joins/orm:Join[@Name=$joinname]">
			(<xsl:call-template name="BuildJoin" />)<xsl:text/>
			</xsl:for-each>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="@JoinTable"/>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="*" mode="SetAttributes">
	<!-- This relies on XSLT overwritign previous value without error -->
	<xsl:for-each select="@*">
		<xsl:copy/>
	</xsl:for-each>
	<xsl:if test="@NETType">
		<xsl:attribute name="Default">
			<xsl:call-template name="SetDefault"/>
		</xsl:attribute>
	</xsl:if>
</xsl:template>


<xsl:template name="OpenSP">
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[<xsl:value-of select="$spname"/>]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[<xsl:value-of select="$spname"/>]
GO

CREATE PROCEDURE <xsl:value-of select="$spname"/>
</xsl:template>

<xsl:template name="CloseSP">
	<xsl:param name="privileges"/>

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

<xsl:for-each select="$privileges">
GRANT EXECUTE ON <xsl:value-of select="$spname"/> TO <xsl:value-of select="@Grantee"/>
</xsl:for-each>
</xsl:template>

<xsl:template match="dbs:TableColumn" mode="SPParameterDeclarations">
		@<xsl:value-of select="@Name"/><xsl:text> </xsl:text> 
			<xsl:call-template name="SQLTypeForColumn">
				<xsl:with-param name="tablename" select="$Name"/>
				<xsl:with-param name="columnname" select="@Name"/>
			</xsl:call-template>
			<xsl:if test="@SQLType='VarChar' or @SQLType='NVarChar'">
				(<xsl:value-of select="@MaxLength"/>)
			</xsl:if>
			<xsl:if test="@AutoIncrement='true'"> OUTPUT </xsl:if>
			<xsl:if test="position()!=last()">,</xsl:if> 
</xsl:template>
    
<!-- Old templates -->

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

<xsl:template name="StripPathAndExtension">
	<xsl:param name="fname"/>
	<xsl:variable name="nopath">
	   <xsl:call-template name="StripPath">
		   <xsl:with-param name="fname" select="$filename" />
		</xsl:call-template>
	</xsl:variable>
   <xsl:call-template name="StripExtension">
	   <xsl:with-param name="fname" select="$nopath" />
   </xsl:call-template>
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

  