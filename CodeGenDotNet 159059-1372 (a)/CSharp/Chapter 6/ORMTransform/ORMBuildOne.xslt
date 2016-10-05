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
  Summary:  One of the steps of the ORM transformation process. -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure"
         xmlns:orm="http://kadgen.com/KADORM.xsd" 
         xmlns:gen="http://kadgen.com/GenInput.xsd"
         xmlns:net="http://kadgen.com/NETTools">
<xsl:import href="ORMSupport2.xslt"/>
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

<xsl:key match="//orm:BuildTable"
         name="BuildTables"
         use="@Name"/>
         
<xsl:template match="/ | @* | node()">
	<xsl:choose>
		<xsl:when test="name()='orm:Build'" /> <!-- Handled Below -->
		<xsl:otherwise>
			<xsl:copy>
				<xsl:apply-templates select="@*"  />
				<xsl:choose>
	            <xsl:when test="name()='orm:MappingRoot'">
				      <xsl:call-template name="Build" /> 
	            </xsl:when>
	            <xsl:when test="name()='orm:Object'">
				      <xsl:call-template name="ObjectAttributesUpdate" /> 
	            </xsl:when>
				</xsl:choose>
				<xsl:apply-templates select="node()"/>
			</xsl:copy>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="Build">
   <xsl:choose>
		<xsl:when test="count(//orm:Build)!=0">
		   <xsl:for-each select="//orm:Build">
		      <xsl:copy>
   			   <xsl:call-template name="BuildAttributes" />
				   <xsl:apply-templates select="@*" />
   				<xsl:apply-templates select="node()[name()!='orm:SPSet']"/>
				   <xsl:call-template name="BuildSPSets" />
		      </xsl:copy>
		   </xsl:for-each>
		</xsl:when>
		<xsl:otherwise>
			<xsl:element name="orm:Build">
			   <xsl:call-template name="BuildAttributes"/>
				<xsl:call-template name="BuildSPSets"/>
			</xsl:element>
		</xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="BuildAttributes">
   <xsl:call-template name="StandardAttributes"/>
</xsl:template>

<xsl:template name="StandardAttributes" >
   <xsl:attribute name="MapDataStructure">
      <xsl:value-of select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
   </xsl:attribute>
</xsl:template>

<xsl:template name="BuildSPSets">
   <xsl:for-each select="//orm:UseSPSet"> <!-- once this is going try adding trick to avoid duplicates -->
      <xsl:variable name="setname" select="@Name" />
      <xsl:choose>
         <xsl:when test="preceding::*[name()='orm:UseSPSet' and @Name=$setname]"/> <!-- This test adds .7 sec -->
         <xsl:otherwise>
            <xsl:choose>
               <xsl:when test="//orm:Build//orm:SPSet[@Name=$setname]">
                  <xsl:variable name="usespset" select="."/>
		            <xsl:for-each select="//orm:Build//orm:SPSet[@Name=$setname]">
                     <xsl:variable name="spset" select="."/>
                     <xsl:copy>
                        <xsl:for-each select="$usespset">
   	   		            <xsl:call-template name="BuildSPSetAttributes" />
                        </xsl:for-each>
   				         <xsl:apply-templates select="@*" />
                        <xsl:for-each select="$usespset">
   	   		            <xsl:call-template name="BuildSPSetContents" /> 
                        </xsl:for-each>
         			      <xsl:apply-templates select="node()[
      			                           name()!='orm:BuildRecordset' and
      			                           name()!='orm:RetrieveParam' and 
      			                           name()!='orm:WhereClause' and 
      			                           name()!='orm:SetSelect' and 
      			                           name()!='orm:Privilege' ]"/>
		               </xsl:copy> 
		            </xsl:for-each>
               </xsl:when>
               <xsl:otherwise>
                  <xsl:element name="orm:SPSet">
                     <xsl:attribute name="Name"><xsl:value-of select="$setname"/></xsl:attribute>
   	   		      <xsl:call-template name="BuildSPSetAttributes" />
   	   		      <xsl:call-template name="BuildSPSetContents" /> 
                  </xsl:element>
               </xsl:otherwise>
            </xsl:choose>
         </xsl:otherwise>
      </xsl:choose>
   </xsl:for-each>
</xsl:template>

<xsl:template name="BuildSPSetAttributes">
   <xsl:attribute name="ObjectName">
      <xsl:call-template name="GetObjectName" />
   </xsl:attribute>   
   <xsl:attribute name="TableName">
      <xsl:value-of select="@TableName" />
   </xsl:attribute>
   <xsl:attribute name="Generate">
      <xsl:value-of select="ancestor::*[@Generate][1]/@Generate"/>
   </xsl:attribute>
   <xsl:call-template name="StandardAttributes"/>
</xsl:template>

<xsl:template name="GetObjectName" >
	<xsl:call-template name="IIF">
		<xsl:with-param name="test" select="boolean(@ObjectName)" />
		<xsl:with-param name="truevalue" select="@ObjectName" />
		<xsl:with-param name="falsevalue" select="net:GetSingular(@Name)"/>
	</xsl:call-template>
</xsl:template>

<xsl:template name="BuildSPSetContents">
	<xsl:if test="string-length(ancestor::orm:Object/@Inherits)=0">
	   <xsl:variable name="dsname" select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
      <xsl:variable name="setname" select="@Name" />
      <xsl:variable name="tablename" select="ancestor::orm:Object/@TableName" />
      <xsl:variable name="originaltablename" select="//dbs:DataStructure//dbs:Table[@Name=$tablename]/@OriginalName" />
	   <xsl:variable name="objectname" >
		   <xsl:call-template name="GetObjectName" />
	   </xsl:variable>
	   <xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]">
         <xsl:call-template name="BuildRetrieveParam">
   	      <xsl:with-param name="setname" select="$setname" />
   	      <xsl:with-param name="objectname" select="$objectname" />
         </xsl:call-template>
         <xsl:call-template name="BuildWhere">
   	      <xsl:with-param name="table" select="." />
   	      <xsl:with-param name="objectname" select="$objectname" />
   	      <xsl:with-param name="node" select="//orm:SPSet[@Name=$setname]" />
         </xsl:call-template>>
         <xsl:call-template name="BuildSetSelect">
   	      <xsl:with-param name="setname" select="$setname" />
         </xsl:call-template>
      </xsl:for-each> 
       <xsl:call-template name="BuildPrivileges">
   	   <xsl:with-param name="setname" select="$setname" />
      </xsl:call-template>
      <xsl:call-template name="BuildRecordsets">
   	   <xsl:with-param name="setname" select="$setname" />
  	      <xsl:with-param name="objectname" select="$objectname" />
      </xsl:call-template>
      <xsl:call-template name="AddRunSPs">
   	   <xsl:with-param name="objectname" select="$objectname" />
   	   <xsl:with-param name="dsname" select="$dsname" />
   	   <xsl:with-param name="tablename" select="$originaltablename" />
      </xsl:call-template>
	</xsl:if>
</xsl:template>

<xsl:template name="BuildRetrieveParam">
   <xsl:param name="setname"/>
   <xsl:param name="objectname"/>
   <xsl:if test="count(//orm:SPSet[@Name=$setname]//orm:RetrieveParam)=0">
	   <xsl:for-each select=".//dbs:TableColumn[@IsPrimaryKey='true']">
		   <xsl:element name="orm:RetrieveParam">
			   <xsl:call-template name="CopyImportantColumnAttributes"/>
			   <xsl:attribute name="FKName">
				   <xsl:call-template name="GetParamName">
					   <xsl:with-param name="objectname" select="$objectname"/>
					   <xsl:with-param name="fieldname" select="@Name"/>
				   </xsl:call-template>
			   </xsl:attribute>
		   </xsl:element>
	   </xsl:for-each>
   </xsl:if>
</xsl:template>

<xsl:template name="BuildWhere">
   <xsl:param name="objectname"/>
   <xsl:param name="node"/>
   <xsl:param name="table"/>
   <xsl:if test="count($node//orm:WhereClause)=0">
	   <xsl:element name="orm:WhereClause">
		   <xsl:attribute name="Clause">
			   <xsl:call-template name="WhereStartClause">
				   <xsl:with-param name="list" select="$table//dbs:TableColumn[@IsPrimaryKey='true']"/>
				   <xsl:with-param name="pass" select="0"/>
				   <xsl:with-param name="objectname" select="$objectname"/>
			   </xsl:call-template>
		   </xsl:attribute>
	   </xsl:element>
	</xsl:if> 
</xsl:template>

<xsl:template name="GetLaterWhereClause">
	<xsl:param name="list" />
	<xsl:param name="pass"  />
	<xsl:param name="tables"  />
	<xsl:for-each select="$tables">
	</xsl:for-each>
	<xsl:choose>
		<xsl:when test="$list">
			<xsl:variable name="andclause">
				<xsl:if test="$pass!=0"> AND </xsl:if>
			</xsl:variable>
			<xsl:variable name="first" select="$list[1]"/>
			<xsl:variable name="rest">
				<xsl:call-template name="GetLaterWhereClause">
					<xsl:with-param name="list" select="$list[position()!=1]"/>
					<xsl:with-param name="pass" select="pass + 1"/>
					<xsl:with-param name="tables" select="$tables"/>
				</xsl:call-template>
			</xsl:variable>
			
			<xsl:variable name="fkname">
			   <xsl:choose>
			      <xsl:when test="../@TableName">
			         <xsl:value-of select="../@TableName"/><xsl:text/>
			      </xsl:when>
			      <xsl:otherwise >
			         <xsl:value-of select="../@Name"/><xsl:text/>
			      </xsl:otherwise>
			   </xsl:choose>
			</xsl:variable>
			
			<xsl:variable name="fktable" select="//dbs:Table[@Name=$fkname]/@OriginalName"/>
			
			<!-- Deterimine which BuildTable to use -->
			<xsl:variable name="tablenamelist"> 
				<xsl:for-each select="$tables">
					<xsl:variable name="tbl" select="@Name"/>
					<xsl:if test="//dbs:Table[@Name=$tbl]//dbs:ParentTable[@Name=$fktable]">
						<xsl:value-of select="concat($tbl, '|')" />
					</xsl:if>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="tablename">
				<xsl:value-of select="substring-before($tablenamelist,'|')"/>
			</xsl:variable>  
			<xsl:variable name="origtablename">
			   <xsl:call-template name="GetTableNameFromName">
			      <xsl:with-param name="tablename" select="substring-before($tablenamelist,'|')" />
			   </xsl:call-template>
			</xsl:variable>  
			
			<!-- GetField -->
			<xsl:variable name="field">
				<xsl:for-each select="//dbs:Table[@Name=$tablename]//dbs:ParentTable[@Name=$fktable]">
					<xsl:variable name="ordinal" select="dbs:ParentKeyField[@Name=$first/@Name]/@Ordinal" />
					<xsl:value-of select="dbs:ChildField[@Ordinal=$ordinal]/@Name"/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="originalfield" 
			          select="//dbs:Table[@Name=$tablename]//dbs:TableColumn[@Name=$field]/@OriginalName" />
			<xsl:value-of select="concat( $andclause, '@', $first/@Name, ' = [', $origtablename, '].[', $originalfield, ']', $rest)"/>
		</xsl:when>
	</xsl:choose>
</xsl:template>

<xsl:template name="BuildSetSelect">
   <xsl:param name="setname"/>
   <xsl:if test="count(//orm:SPSet[@Name=$setname]//orm:SetSelect)=0">
	   <xsl:element name="orm:SetSelect">
         <xsl:attribute name="TableName">
         <!-- KAD 1/28 -->
            <xsl:value-of select="@Name" />
         </xsl:attribute>  
		   <xsl:for-each select=".//dbs:TableColumn[@IsPrimaryKey='true'] | 
										   .//dbs:TableColumn[@UseForDesc='true']">
			   <xsl:element name="orm:SetSelectColumn">
				   <xsl:if test="@IsPrimaryKey='true'">
					   <xsl:attribute name="IsPrimaryKey">true</xsl:attribute>
				   </xsl:if>
				   <xsl:call-template name="CopyImportantColumnAttributes"/>
				   <xsl:variable name="columnname" select="@Name"/>
				   <xsl:for-each select="//orm:SPSet[@Name=$setname]//orm:BuildRecordset[1]//orm:BuildColumn[@Column=$columnname]">
				      <xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
				   </xsl:for-each>
			   </xsl:element>
		   </xsl:for-each>
	   </xsl:element>
	</xsl:if> 
</xsl:template>

<xsl:template name="BuildPrivileges">
   <xsl:param name="setname" />
   <xsl:if test="count(//orm:SPSet[@Name=$setname]//orm:SetSelect)=0">
		<xsl:for-each select="ancestor::*[orm:DefaultPrivileges][1]//orm:DefaultPrivilege">
			<xsl:element name="orm:Privilege">
				<xsl:for-each select="@*">
					<xsl:copy/>
				</xsl:for-each>
			</xsl:element>
		</xsl:for-each>
	</xsl:if> 
</xsl:template>

<xsl:template name="BuildRecordsets">
   <xsl:param name="setname" />
   <xsl:param name="objectname" />
   <xsl:variable name="dsname" select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
   <xsl:variable name="firsttablename" select="ancestor::orm:Object/@TableName"/>
   <xsl:variable name="firsttable" select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$firsttablename]" />
   <xsl:choose>
      <xsl:when test="//orm:Build//orm:SPSet[@Name=$setname]//orm:BuildRecordset">
         <xsl:variable name="usespset" select="."/>
	   	<!-- moved previous code to template 1/24/04 -->
         <xsl:apply-templates select="//orm:Build//orm:SPSet[@Name=$setname]//orm:BuildRecordset"
                     mode="copyexistingrecordset">
             <xsl:with-param name="setname" select="$setname"/>
             <xsl:with-param name="firsttable" select="$firsttable"/>
         </xsl:apply-templates>
         <!-- end change -->
      </xsl:when>
      <xsl:otherwise>
         <xsl:element name="orm:BuildRecordset">
            <xsl:variable name="buildrsname" select="'RecSet'" />
            <xsl:variable name="tablename" select="ancestor::orm:Object/@TableName" />
            <xsl:attribute name="Name"><xsl:value-of select="$buildrsname"/></xsl:attribute>
            <xsl:attribute name="Ordinal"><xsl:value-of select="0"/></xsl:attribute>
   	   	<xsl:call-template name="BuildRecordsetAttributes" />
   	   	<xsl:call-template name="BuildRecordsetContents">
  	   		   <xsl:with-param name="setname" select="$setname" />
  	   		   <xsl:with-param name="objectname" select="$setname" />
   	   		<xsl:with-param name="buildrsname" select="$buildrsname" />
   	   		<xsl:with-param name="tablename" select="$tablename" />
   	   		<xsl:with-param name="recset" select="/xxx" />
               <xsl:with-param name="pos" select="0"/>
               <xsl:with-param name="firsttable" select="$firsttable"/>
   	   	</xsl:call-template>
         </xsl:element>
         <xsl:for-each select="ancestor::orm:Object//orm:ChildCollection">
            <xsl:element name="orm:BuildRecordset">
               <xsl:variable name="buildrsname" select="concat('RecSet',position())" />
               <xsl:variable name="childtablename" select="@ChildTableName" />
               <xsl:attribute name="Name"><xsl:value-of select="$buildrsname"/></xsl:attribute>
               <xsl:attribute name="Ordinal"><xsl:value-of select="position()"/></xsl:attribute>
   	   		<xsl:call-template name="BuildRecordsetAttributes" />
   	   		<xsl:call-template name="BuildRecordsetContents">
  	   		      <xsl:with-param name="setname" select="$setname" />
   	   		   <xsl:with-param name="objectname" select="$setname" />
   	   		   <xsl:with-param name="buildrsname" select="$buildrsname" />
   	   		   <xsl:with-param name="tablename" select="@ChildTableName" />
      	   		<xsl:with-param name="recset" select="/xxx" />
                  <xsl:with-param name="pos" select="position()"/>
                  <xsl:with-param name="firsttable" select="$firsttable"/>
	   		   </xsl:call-template>
            </xsl:element>
         </xsl:for-each>
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>

	   		<!-- moved 1/24/04 -->
<xsl:template match="orm:BuildRecordset" mode="copyexistingrecordset">
   <xsl:param name="setname"/>
   <xsl:param name="firsttable"/>
   <xsl:variable name="buildrecset" select="."/>
   <xsl:variable name="tablename" >
      <xsl:choose>
         <xsl:when test="@TableName">
            <xsl:value-of select="@TableName"/>
         </xsl:when>
         <xsl:when test="position()=1 and ancestor::orm:SPSet/@TableName">
            <xsl:value-of select="ancestor::orm:SPSet/@TableName" />
         </xsl:when>
         <xsl:when test="position()=1">
            <xsl:value-of select="ancestor::orm:SPSet/@Name" />
         </xsl:when>
         <xsl:otherwise>
            <xsl:value-of select="orm:BuildTable[1]/@Name" />
         </xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
	<xsl:copy>
   	<xsl:call-template name="BuildRecordsetAttributes"/>
     	<xsl:apply-templates select="@*" />
      <xsl:variable name="buildrsname">
         <xsl:call-template name="BuildRSName">
            <xsl:with-param name="name" select="@Name"/>
            <xsl:with-param name="pos" select="position()-1"/>
         </xsl:call-template>
      </xsl:variable> 
      <xsl:attribute name="Name">
         <xsl:value-of select="$buildrsname"/>
      </xsl:attribute>
      <xsl:apply-templates select="node()[name()!='orm:BuildTable']"/>
   	<xsl:call-template name="BuildRecordsetContents">
   	   <xsl:with-param name="setname" select="$setname" />
   	   <xsl:with-param name="objectname" select="$setname" />
   	   <xsl:with-param name="buildrsname" select="$buildrsname" />
   	   <xsl:with-param name="tablename" select="$tablename" />
   	   <xsl:with-param name="recset" select="." />
         <xsl:with-param name="pos" select="position()-1"/>
         <xsl:with-param name="firsttable" select="$firsttable"/>
   	</xsl:call-template>
	</xsl:copy>
</xsl:template>
            <!-- end change -->

<xsl:template name="BuildRSName">
   <xsl:param name="name"/>
   <xsl:param name="pos"/>
   <xsl:choose>
      <xsl:when test="string-length($name)!=0">
         <xsl:value-of select="$name"/> 
      </xsl:when>
      <xsl:when test="$pos=0">RecSet</xsl:when>
      <xsl:otherwise>
         <xsl:value-of select="concat('RecSet',$pos)" />
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="BuildRecordsetAttributes">
   <xsl:call-template name="StandardAttributes"/>
   <!-- Add additional attributes -->
</xsl:template>

<xsl:template name="BuildRecordsetContents">
   <xsl:param name="setname"  />
   <xsl:param name="objectname"  />
   <xsl:param name="buildrsname" />
   <xsl:param name="tablename" />
   <xsl:param name="recset" />
   <xsl:param name="pos" />
   <xsl:param name="firsttable" />
   <xsl:param name="dsname" select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure"/>
  
   <xsl:choose>
      <xsl:when test="$pos=0">
         <xsl:call-template name="BuildWhere">
   	      <xsl:with-param name="table" select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]" />
   	      <xsl:with-param name="objectname" select="$objectname" />
            <xsl:with-param name="node" 
                  select="//orm:SPSet[@Name=$setname]//orm:BuildRecordset[@Name='RecSet']" />
         </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
         <xsl:if test="count(//orm:SPSet[@Name=$setname]//orm:BuildRecordset[@Name='RecSet']/orm:WhereClause)=0">
            <xsl:element name="orm:WhereClause">
               <xsl:attribute name="Clause">
                  <xsl:variable name="rsname" select="$recset/@Name" />
                  <xsl:call-template name="GetLaterWhereClause">
   	               <xsl:with-param name="list" select="$firsttable//dbs:TableColumn[@IsPrimaryKey='true']" />
   	               <xsl:with-param name="tables" select="//orm:SPSet[@Name=$setname]//orm:BuildRecordset[@Name=$rsname]//orm:BuildTable | 
   	                        //dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]" />
   	               <xsl:with-param name="pass" select="0" />
                  </xsl:call-template>
               </xsl:attribute>
            </xsl:element>
         </xsl:if>
      </xsl:otherwise>
   </xsl:choose>
   <xsl:call-template name="BuildTables">
   	<xsl:with-param name="setname" select="$setname" />
   	<xsl:with-param name="buildrsname" select="$buildrsname" />
	   <xsl:with-param name="tablename" select="$tablename" />
      <xsl:with-param name="recset" select="$recset" />
   </xsl:call-template>
   <xsl:call-template name="BuildJoins">
   	<xsl:with-param name="setname" select="$setname" />
   	<xsl:with-param name="buildrsname" select="$buildrsname" />
   </xsl:call-template>
   
</xsl:template>

<xsl:template name="BuildTables">
   <xsl:param name="setname" />
   <xsl:param name="buildrsname" />
   <xsl:param name="tablename" />
   <xsl:param name="recset" />
   <xsl:variable name="runall">
      <xsl:if test="count($recset/orm:BuildTable)=0 or $recset/orm:AllTables">true</xsl:if>
   </xsl:variable>
   <xsl:variable name="dsname" select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
   <xsl:for-each select="$recset/orm:BuildTable">
      <xsl:variable name="buildtablename" select="@Name" />
		<xsl:copy>
   	   <xsl:call-template name="BuildTableAttributes"/>
   		<xsl:apply-templates select="@*" />
      	<xsl:apply-templates select="node()[name()!='orm:BuildColumn']"/>
   	   <xsl:call-template name="BuildColumns">
   	   	<xsl:with-param name="buildrsname" select="$buildrsname" />
   	   	<xsl:with-param name="tablename" select="$buildtablename" />
   	   	<xsl:with-param name="buildtable" select="." />
   	   </xsl:call-template>
		</xsl:copy>
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]"> <!-- This may be wrong -->
         <xsl:variable name="buildtablename" select="@Name" />
         <xsl:if test="count($recset/orm:BuildTable[@Name=$buildtablename])=0">
            <xsl:element name="orm:BuildTable">
               <xsl:attribute name="Name"><xsl:value-of select="$buildtablename"/></xsl:attribute>
   	   	   <xsl:call-template name="BuildTableAttributes"/>
   	   	   <xsl:call-template name="BuildColumns">
   	   		   <xsl:with-param name="buildrsname" select="$buildrsname" /> <!-- fix this -->
  	   		      <xsl:with-param name="tablename" select="$buildtablename" />
   	   		   <xsl:with-param name="buildtable" select="/xxx" />
   	   	   </xsl:call-template>
            </xsl:element>
         </xsl:if>
      </xsl:for-each> 
   </xsl:if>
</xsl:template>

<xsl:template name="BuildJoins">
   <!-- I am not currently building joins. If you have multiple tables, define the joins -->
</xsl:template>

<xsl:template name="BuildTableAttributes">
   <xsl:call-template name="StandardAttributes"/>
   <!-- Add additional attributes -->
</xsl:template>

<xsl:template name="BuildColumns">
   <xsl:param name="buildrsname" />
   <xsl:param name="tablename" />
   <xsl:param name="buildtable" />
   <xsl:variable name="runall">
      <xsl:if test="(count($buildtable//orm:BuildColumn)=0 or $buildtable/orm:AllColumns) and count($buildtable/orm:NoColumns)=0">true</xsl:if>
   </xsl:variable>
   <xsl:variable name="dsname" select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
   <xsl:for-each select="$buildtable//orm:BuildColumn">
      <xsl:variable name="columnname" >
         <xsl:choose>
            <xsl:when test="@Column"><xsl:value-of select="@Column"/></xsl:when>
            <xsl:otherwise><xsl:value-of select="@Name"/></xsl:otherwise>
         </xsl:choose>
      </xsl:variable>
		<xsl:copy>
         <xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]//dbs:TableColumn[@Name=$columnname or @Column=$columnname]" >
            <xsl:call-template name="CopyImportantColumnAttributes"/>
   	   </xsl:for-each>
   		<xsl:apply-templates select="@*" />
      	<xsl:apply-templates select="node()"/>
		</xsl:copy>
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <xsl:variable name="basebuildtable" select="key('BuildTables',$tablename)" />
      <xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]//dbs:TableColumn" >
         <xsl:variable name="columnname" select="@Name" />
         <xsl:if test="count($buildtable/orm:BuildColumn[@Name=$columnname or @Column=$columnname])=0">
            <xsl:element name="orm:BuildColumn">
               <xsl:attribute name="Name"><xsl:value-of select="$columnname"/></xsl:attribute>
               <xsl:call-template name="CopyImportantColumnAttributes"/>
               <xsl:if test="$basebuildtable/orm:BuildColumn[@Column=$columnname]">
                  <xsl:attribute name="Name"><xsl:value-of select="$basebuildtable//orm:BuildColumn[@Column=$columnname]/@Name"/></xsl:attribute>
               </xsl:if>               
            </xsl:element>
         </xsl:if>
      </xsl:for-each>
   </xsl:if> 
</xsl:template>

<xsl:template name="AddRunSPs">
	<xsl:param name="objectname" />
	<xsl:param name="dbname" />
	<xsl:param name="tablename" />

	<xsl:call-template name="OneRunSP">
		<xsl:with-param name="task" select="'Create'"/>
		<xsl:with-param name="pattern" select="ancestor-or-self::*[@CreatePattern][1]/@CreatePattern"/>
		<xsl:with-param name="objectname" select="$objectname"/>
		<xsl:with-param name="dbname" select="$dbname"/>
		<xsl:with-param name="tablename" select="$tablename"/>
	</xsl:call-template>
	<xsl:call-template name="OneRunSP">
		<xsl:with-param name="task" select="'Retrieve'"/>
		<xsl:with-param name="pattern" select="ancestor-or-self::*[@RetrievePattern][1]/@RetrievePattern"/>
		<xsl:with-param name="objectname" select="$objectname"/>
		<xsl:with-param name="dbname" select="$dbname"/>
		<xsl:with-param name="tablename" select="$tablename"/>
	</xsl:call-template>
	<xsl:call-template name="OneRunSP">
		<xsl:with-param name="task" select="'Update'"/>
		<xsl:with-param name="pattern" select="ancestor-or-self::*[@UpdatePattern][1]/@UpdatePattern"/>
		<xsl:with-param name="objectname" select="$objectname"/>
		<xsl:with-param name="dbname" select="$dbname"/>
		<xsl:with-param name="tablename" select="$tablename"/>
	</xsl:call-template>
	<xsl:call-template name="OneRunSP">
		<xsl:with-param name="task" select="'Delete'"/>
		<xsl:with-param name="pattern" select="ancestor-or-self::*[@DeletePattern][1]/@DeletePattern"/>
		<xsl:with-param name="objectname" select="$objectname"/>
		<xsl:with-param name="dbname" select="$dbname"/>
		<xsl:with-param name="tablename" select="$tablename"/>
	</xsl:call-template>
	<xsl:call-template name="OneRunSP">
		<xsl:with-param name="task" select="'SetSelect'"/>
		<xsl:with-param name="pattern" select="ancestor-or-self::*[@SetSelectPattern][1]/@SetSelectPattern"/>
		<xsl:with-param name="objectname" select="$objectname"/>
		<xsl:with-param name="dbname" select="$dbname"/>
		<xsl:with-param name="tablename" select="$tablename"/>
	</xsl:call-template>
</xsl:template>

<xsl:template name="OneRunSP">
	<xsl:param name="task" />
	<xsl:param name="pattern" />
	<xsl:param name="objectname" />
	<xsl:param name="tablename" />
	<xsl:variable name="runall">
      <xsl:if test="count(orm:RunSP)=0 or orm:AllRunSPs">true</xsl:if>
	</xsl:variable>
	<xsl:for-each select="orm:RunSP[@Task=$task]">
	   <xsl:copy>
	      <xsl:call-template name="RunSPAttributes">
	         <xsl:with-param name="pattern" select="$pattern"/>
	         <xsl:with-param name="objectname" select="$objectname"/>
	         <xsl:with-param name="tablename" select="$tablename"/>
	      </xsl:call-template>
		   <xsl:apply-templates select="@*" />
   	   <xsl:apply-templates select="node()"/>
	      <xsl:call-template name="RunSPContents">
	         <xsl:with-param name="task" select="$task"/>
	      </xsl:call-template>
	   </xsl:copy>
	</xsl:for-each>
	<xsl:if test="$runall='true'">
	   <xsl:element name="orm:RunSP">
	      <xsl:attribute name="Task"><xsl:value-of select="$task"/></xsl:attribute>
	      <xsl:call-template name="RunSPAttributes">
	         <xsl:with-param name="pattern" select="$pattern"/>
	         <xsl:with-param name="objectname" select="$objectname"/>
	         <xsl:with-param name="tablename" select="$tablename"/>
	      </xsl:call-template>
	   </xsl:element>
	</xsl:if>
</xsl:template>

<xsl:template name="RunSPAttributes">
   <xsl:param name="pattern"/>
   <xsl:param name="objectname"/>
   <xsl:param name="tablename"/>
	<xsl:attribute name="Name">
		<xsl:call-template name="GetSPName">
			<xsl:with-param name="pattern" select="$pattern" />
         <xsl:with-param name="objectname" select="$tablename"/>
		</xsl:call-template> 
	</xsl:attribute>
</xsl:template>


</xsl:stylesheet> 
  