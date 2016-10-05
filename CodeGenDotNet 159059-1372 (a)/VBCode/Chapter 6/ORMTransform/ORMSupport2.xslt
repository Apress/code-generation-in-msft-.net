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
  Summary:  Supporting templates for the ORM process.
  Note:     The 2 is legacy and has no meaning -->

<xsl:stylesheet version="1.0" 
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:xs="http://www.w3.org/2001/XMLSchema"
		  xmlns:msdata="urn:schemas-microsoft-com:xml-msdata"
        xmlns:dbs="http://kadgen/DatabaseStructure" 
        xmlns:orm="http://kadgen.com/KADORM.xsd" 
        xmlns:net="http://kadgen.com/NETTools">
    <xsl:strip-space elements="*"/>
    <xsl:output method="text" /> 

<xsl:key match="//orm:SpecialType/orm:UseForAllType"
         name="SpecialTypes"
         use="@Name"/>


<xsl:template name="IIF">
	<xsl:param name="test"  />
	<xsl:param name="truevalue"  />
	<xsl:param name="falsevalue" />
	<xsl:choose>
   	<xsl:when test="$test">
	      <xsl:value-of select="$truevalue" />
	   </xsl:when>
	   <xsl:otherwise>
	      <xsl:value-of select="$falsevalue" />
	   </xsl:otherwise>
	</xsl:choose>
</xsl:template>


<xsl:template name="StandardAttributes" >
   <xsl:attribute name="Namespace">
      <xsl:value-of select="ancestor-or-self::*[@Namespace][1]/@Namespace" />
   </xsl:attribute>
   <xsl:attribute name="MapDataStructure">
      <xsl:value-of select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure" />
   </xsl:attribute>
   <xsl:attribute name="TransactionForRetrieve">
      <xsl:value-of select="ancestor-or-self::*[@TransactionForRetrieve][1]/@TransactionForRetrieve" />
   </xsl:attribute>
   <xsl:attribute name="RetrievePattern">
      <xsl:value-of select="ancestor-or-self::*[@RetrievePattern][1]/@RetrievePattern" />
   </xsl:attribute>
   <xsl:attribute name="SetSelectPattern">
      <xsl:value-of select="ancestor-or-self::*[@SetSelectPattern][1]/@SetSelectPattern" />
   </xsl:attribute>
   <xsl:attribute name="CreatePattern">
      <xsl:value-of select="ancestor-or-self::*[@CreatePattern][1]/@CreatePattern" />
   </xsl:attribute>
   <xsl:attribute name="UpdatePattern">
      <xsl:value-of select="ancestor-or-self::*[@UpdatePattern][1]/@UpdatePattern" />
   </xsl:attribute>
   <xsl:attribute name="DeletePattern">
      <xsl:value-of select="ancestor-or-self::*[@DeletePattern][1]/@DeletePattern" />
   </xsl:attribute>
</xsl:template>

<xsl:template name="Objects">
   <xsl:variable name="runall">
      <xsl:if test="count(.//orm:Object)=0 or .//orm:AllObjects" >true</xsl:if> 
   </xsl:variable>
   <xsl:for-each select=".//orm:Object">
      <xsl:copy>
         <xsl:variable name="dsname" select=
                  "ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure"/>
         <xsl:variable name="ds" select="//dbs:DataStructure[@Name=$dsname]"/>
         <xsl:variable name="singularname" select="net:GetSingular(@Name)"/>
         <xsl:variable name="tablename">
            <xsl:variable name="pluralname" select="net:GetPlural(@Name)"/>
            <xsl:choose>
               <xsl:when test="@TableName">
                  <xsl:value-of select="@TableName"/>
               </xsl:when>
               <xsl:when test="$ds//dbs:Table[@Name=$singularname]">
                  <xsl:value-of select="$singularname"/>
               </xsl:when>
               <xsl:when test="$ds//dbs:Table[@Name=$pluralname]">
                  <xsl:value-of select="$pluralname"/>
               </xsl:when>
            </xsl:choose>
         </xsl:variable> 
         <xsl:for-each select="$ds//dbs:Table[@Name=$tablename]" >
            <xsl:call-template name="ObjectAttributes">
               <xsl:with-param name="name" select="$singularname" />
            </xsl:call-template>
         </xsl:for-each>
   	   <xsl:apply-templates select="@*" />
         <xsl:call-template name="ObjectContents" >
            <xsl:with-param name="table" select="$ds//dbs:Table[@Name=$tablename]"/>
            <xsl:with-param name="object" select="." />
         </xsl:call-template>
         <xsl:apply-templates select="node()[name()!='orm:ChildCollection' and
                     name()!='orm:ParentObject' ]" />
      </xsl:copy>
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <xsl:variable name="dsname" select=
               "ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure"/>
	   <xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table">
		   <xsl:variable name="tablename" select="net:GetSingular(@Name)" />
		   <xsl:variable name="alreadyexists">
			   <xsl:for-each select="//orm:Object[@Name=$tablename]">
				   <xsl:if test="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure=$dsname">true</xsl:if>
			   </xsl:for-each> 
		   </xsl:variable>
		   <xsl:choose>
			   <xsl:when test="starts-with($alreadyexists,'true')"/>
			   <xsl:otherwise>
			      <xsl:call-template name="AddObject">
			         <xsl:with-param name="name" select="$tablename"/>
			      </xsl:call-template>
			   </xsl:otherwise>
		   </xsl:choose>
	   </xsl:for-each>      
   </xsl:if>
</xsl:template> 

<xsl:template name="AddObject" >
   <xsl:param name="name"/>
   <xsl:param name="childof"/>
   <xsl:param name="inherits"/>
   <xsl:param name="pos"/>
   <xsl:element name="orm:Object">
      <xsl:call-template name="ObjectAttributes">
         <xsl:with-param name="name" select="net:GetSingular($name)" />
         <xsl:with-param name="childof" select="$childof" />
         <xsl:with-param name="inherits" select="$inherits" />
         <xsl:with-param name="pos" select="$pos" />
      </xsl:call-template> 
      <xsl:call-template name="ObjectContents" >
         <xsl:with-param name="table" select="."/>
         <xsl:with-param name="object" select="/xxx" />
         <xsl:with-param name="allproperties" select="'true'" />
      </xsl:call-template>
		<xsl:element name="orm:AllProperties"/>
	</xsl:element>
</xsl:template>  
  
<xsl:template name="ObjectAttributes">
   <xsl:param name="name" />
   <xsl:param name="inherits"/>
   <xsl:param name="childof"/>
   <xsl:param name="pos"/>
   <!-- Name is included twice so it appears first, but isn't overwritten -->
	<xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
	<xsl:apply-templates select="@*"/>
	<xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
	<xsl:attribute name="TableName"><xsl:value-of select="@Name"/></xsl:attribute>
	<xsl:attribute name="CollectionName"><xsl:value-of select="net:GetPlural($name)"/></xsl:attribute>
	<xsl:attribute name="Caption"><xsl:value-of select="net:SpaceAtCaps($name)"/></xsl:attribute>
	<xsl:attribute name="Namespace"><xsl:value-of select="ancestor-or-self::*[@Namespace][1]/@Namespace"/></xsl:attribute>
	<xsl:attribute name="Generate">
	   <xsl:value-of select="ancestor::*[@Generate][1]/@Generate"/>
	</xsl:attribute>
	<!-- You can't copy all the existing attributes at this point, because you'd write over name which doesn't match and have duplicate classes within your output files. 12/3/03 KAD -->
	<!-- <xsl:if test="@IsLookup='true'">
	   <xsl:attribute name="IsLookup">true</xsl:attribute>
	</xsl:if> -->
	<xsl:attribute name="Prefix"><xsl:value-of  select="@Prefix"/></xsl:attribute>
	<xsl:if test="string-length($inherits) != 0">
	   <xsl:attribute name="Inherits"><xsl:value-of select="$inherits"/></xsl:attribute>
	</xsl:if>
	<xsl:if test="string-length($childof)!=0">
	   <xsl:attribute name="ChildOf"><xsl:value-of select="$childof"/></xsl:attribute>
	</xsl:if>
	<xsl:if test="string-length($pos)!=0">
	   <xsl:attribute name="Position"><xsl:value-of select="$pos"/></xsl:attribute>
	</xsl:if>
	<!-- Additional attributes are set in ObjectAttributesUpdate called from a later template-->
   <xsl:call-template name="StandardAttributes" />
</xsl:template>

<xsl:template name="ObjectAttributesUpdate">
   <!-- These attributes can't be calculated until we have full picture of the object -->
   <xsl:variable name="namespace" select="@Namespace" />
   <xsl:variable name="objectname" select="@Name" />
   <xsl:variable name="collectionname" select="@CollectionName" />
   <xsl:variable name="tablename" select="@TableName" />
	<xsl:variable name="iscollection" select="string-length(@CollectionName)!=0" />
<!--		<xsl:choose>
   		<xsl:when test="string-length(@CollectioName)!=0">true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>-->
	<xsl:variable name="isobjectchild" 
			select="count(//orm:Object[@Namespace=$namespace]/orm:ChildCollection[@Name=$objectname]) > 0"/>
	<xsl:variable name="iscollectionchild" 
			select="count(//orm:Object[@Namespace=$namespace]/orm:ChildCollection[@Name=$collectionname]) > 0"/>
	<xsl:variable name="istablechild" 
			select="count(//orm:Object[@Namespace=$namespace]/orm:ChildCollection[@ChildTableName=$tablename]) > 0"/>
	<xsl:variable name="isroot">
		<xsl:choose>
		   <xsl:when test="count(@ChildOf)=0">true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="ischild" select="$iscollection='true' or $isobjectchild='true' or 
			            $iscollectionchild='true' or $istablechild='true'"/>
<!--		<xsl:choose>
			<xsl:when test="" >
			<xsl:text/>true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>-->
	<xsl:variable name="isreadonly">
		<xsl:choose>
			<xsl:when test="@ReadOnly"><xsl:value-of select="@ReadOnly"/></xsl:when>
			<xsl:otherwise>
				<xsl:variable name="candelete" 
						select="count(orm:Privilege[contains(Rights,'D')])"/>
				<xsl:variable name="cancreate" 
						select="count(orm:Privilege[contains(Rights,'C')])"/>
				<xsl:variable name="canupdate" 
						select="count(orm:Privilege[contains(Rights,'U')])"/>
				<xsl:variable name="isreadonlyinner"
						select="$candelete or $canupdate or $cancreate" />
				<xsl:choose>
					<xsl:when test="$isreadonlyinner">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:attribute name="Root"><xsl:value-of select="$isroot"/></xsl:attribute>
	<xsl:attribute name="Collection"><xsl:value-of select="$iscollection"/></xsl:attribute>
	<xsl:attribute name="Child"><xsl:value-of select="$ischild"/></xsl:attribute>
	<xsl:attribute name="CollectionRoot"><xsl:value-of select="$isroot"/></xsl:attribute>
	<xsl:attribute name="CollectionChild"><xsl:value-of select="$iscollectionchild"/></xsl:attribute>
	<xsl:attribute name="ObjectChild"><xsl:value-of select="$isobjectchild"/></xsl:attribute>
	<xsl:attribute name="ReadOnly"><xsl:value-of select="$isreadonly"/></xsl:attribute>
	<!-- Ensure explicit values override -->
	<xsl:for-each select="@*"><xsl:copy/></xsl:for-each>
</xsl:template>

<xsl:template name="ObjectContents">
   <xsl:param name="table"/>
   <xsl:param name="object" />
   <xsl:param name="allproperties" />
   <xsl:call-template name="UseSPSets">
      <xsl:with-param name="table" select="$table" />
      <xsl:with-param name="object" select="$object" />
   </xsl:call-template>
   <xsl:call-template name="ExpandChildren">
      <xsl:with-param name="table" select="$table" />
      <xsl:with-param name="object" select="$object" />
   </xsl:call-template>
   <xsl:call-template name="ExpandParents">
      <xsl:with-param name="table" select="$table" />
      <xsl:with-param name="object" select="$object" />
   </xsl:call-template>
   <!--<xsl:call-template name="ExpandProperties">
      <xsl:with-param name="table" select="$table" />
      <xsl:with-param name="object" select="$object" />
      <xsl:with-param name="allproperties" select="$allproperties" />
   </xsl:call-template> -->
</xsl:template>

<xsl:template name="UseSPSets">
   <xsl:param name="table"/>
   <xsl:param name="object" />
   <xsl:variable name="runall">
      <xsl:if test="count($object//orm:UseSPSet)=0">true</xsl:if>
   </xsl:variable>
   <xsl:for-each select="$object//orm:UseSPSet">
	   <xsl:for-each select="$table">
         <xsl:call-template name="UseSPSetAttributes"/>
      </xsl:for-each> 
   	<xsl:apply-templates select="@*" />
      <xsl:apply-templates select="node()" />
   </xsl:for-each>
   <xsl:if test="$runall='true'">
	   <xsl:for-each select="$table">
		   <xsl:variable name="name" select="@TableName"/>
		   <xsl:if test="count($object//orm:UseSPSet[@Name=$name])=0">
			   <xsl:element name="orm:UseSPSet">
               <xsl:call-template name="UseSPSetAttributes"/>
			   </xsl:element>
		   </xsl:if>
	   </xsl:for-each>      
   </xsl:if>
</xsl:template>

<xsl:template name="UseSPSetAttributes">
   <xsl:attribute name="Name"><xsl:value-of select="@Name"/></xsl:attribute>
   <xsl:attribute name="TableName"><xsl:value-of select="@Name"/></xsl:attribute>
</xsl:template>

<xsl:template name="ExpandChildren">
   <xsl:param name="table"/>
   <xsl:param name="object" />
   <xsl:variable name="runall">
      <xsl:choose>
         <xsl:when test="$object//orm:NoChildCollections">false</xsl:when>
         <xsl:when test="count($object//orm:ChildCollection)=0 or 
                  $object//orm:AllChildren">true</xsl:when>
         <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="namespace" select="ancestor-or-self::*[@Namespace][1]/@Namespace"/>
   <xsl:for-each select="$object//orm:ChildCollection">
      <xsl:copy>
         <xsl:variable name="name" select="@Name" />
         <xsl:variable name="shortname" select="net:GetSingular(substring-after($name,$object/@Name))"/>
         <xsl:for-each select="$table//dbs:ChildTable[@Name=$shortname]" >
            <xsl:call-template name="ChildAttributes">
               <xsl:with-param name="namespace" select="$namespace"/>
            </xsl:call-template>
         </xsl:for-each> 
   	   <xsl:apply-templates select="@*" />
         <xsl:call-template name="ChildContents" >
      	   <xsl:with-param name="childtable" select="$table//dbs:ChildTable[@Name=$shortname]" />
      	   <xsl:with-param name="childobject" select="." />
         </xsl:call-template>
         <xsl:apply-templates select="node()" />
      </xsl:copy>
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <!-- I added the following conditional so we wouldn't get child tables of lookups which just clutters things 11/20/2003 KAD -->
      <!-- I took out the conditional when we decided to treat lookups as normal parents -->
      <!-- <xsl:if test="not($table/@IsLookup='true')"> -->
	      <xsl:for-each select="$table//dbs:ChildTable">
		      <xsl:variable name="name" select="@Name"/>
		      <xsl:if test="count($object//orm:ChildCollection[@Name=$name])=0">
			      <xsl:element name="orm:ChildCollection">
                  <xsl:call-template name="ChildAttributes">
                     <xsl:with-param name="namespace" select="$namespace"/>
                  </xsl:call-template>
      	         <xsl:call-template name="ChildContents" >
      	            <xsl:with-param name="childtable" select="." />
      	            <xsl:with-param name="childobject" select="/xxx" />
      	         </xsl:call-template>
			      </xsl:element>
		      </xsl:if>
	      </xsl:for-each>      
      <!--  </xsl:if> -->
   </xsl:if>
</xsl:template>

<xsl:template name="ChildAttributes">
   <xsl:param name="namespace"/>
	<xsl:variable name="tablename" select="ancestor::dbs:Table/@SingularName" />
	<xsl:variable name="childname" select="@Name" />
	<xsl:variable name="caption" select="ancestor::dbs:DataStructure//dbs:Table[@Name=$childname]/@PluralCaption" />
	<xsl:variable name="objectname">
		<xsl:choose>
		   <!-- I removed this becuase I couldn't figure out why it was here-->
			<!--<xsl:when test="starts-with(@Name,$tablename)">
				<xsl:value-of select="@Name" />
			</xsl:when> -->
			<xsl:when test="0=1" />
			<xsl:otherwise>
				<xsl:value-of select="$tablename" /><xsl:value-of select="net:GetPlural(@Name)" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:attribute name="Name"><xsl:value-of select="$objectname"/></xsl:attribute>
	<xsl:attribute name="ObjectName"><xsl:value-of select="net:GetSingular($objectname)"/></xsl:attribute>
	<xsl:attribute name="ChildTableName"><xsl:value-of select="@Name"/></xsl:attribute>
	<xsl:attribute name="Ordinal"><xsl:value-of select="position()"/></xsl:attribute>
	<xsl:attribute name="Namespace"><xsl:value-of select="$namespace"/></xsl:attribute>
	<xsl:attribute name="Caption"><xsl:value-of select="$caption"/></xsl:attribute>
</xsl:template>

<xsl:template name="ChildContents">
   <xsl:param name="childtable"/>
   <xsl:param name="childobject" />
   <xsl:variable name="runall">
      <xsl:if test="count($childobject//orm:ChildKeyField)=0">true</xsl:if>
   </xsl:variable>
   <xsl:for-each select="$childobject//orm:ChildKeyField">
      <xsl:for-each select="$childtable//dbs:ChildKeyField">
         <xsl:call-template name="ChildKeyAttributes"/>
      </xsl:for-each>
   	<xsl:apply-templates select="@*" />
      <xsl:apply-templates select="node()" />
   </xsl:for-each>
   <xsl:if test="$runall='true'">
	   <xsl:for-each select="$childtable//dbs:ChildKeyField">
		   <xsl:variable name="name" select="@Name"/>
		   <xsl:if test="count($childobject//orm:ChildKeyField[@Name=$name])=0">
			   <xsl:element name="orm:ChildKeyField">
               <xsl:call-template name="ChildKeyAttributes"/>
			   </xsl:element>
		   </xsl:if>
	   </xsl:for-each>      
   </xsl:if>
</xsl:template>

<xsl:template name="ChildKeyAttributes">
	<xsl:attribute name="Name">
		<xsl:value-of select="@Name" />
	</xsl:attribute>
	<xsl:attribute name="Ordinal">  
	   <xsl:value-of select="position()" />
	</xsl:attribute>
</xsl:template>

<xsl:template name="ExpandParents">
   <xsl:param name="table"/>
   <xsl:param name="object" />
   <xsl:variable name="runall">
      <xsl:if test="count($object//orm:ParentObject)=0 or 
               $object//orm:AllParents">true</xsl:if>
   </xsl:variable>
   <xsl:variable name="namespace" select="ancestor-or-self::*[@Namespace][1]/@Namespace"/>
   <xsl:for-each select="$object//orm:ParentObject">
      <xsl:for-each select="$table//dbs:ParentTable" >
         <xsl:call-template name="ParentAttributes">
            <xsl:with-param name="namespace" select="$namespace"/>
         </xsl:call-template>
      </xsl:for-each> 
   	<xsl:apply-templates select="@*" />
      <xsl:call-template name="ParentContents" >
      	<xsl:with-param name="parenttable" select="$table//dbs:ParentTable" />
      	<xsl:with-param name="parentobject" select="." />
      </xsl:call-template>
      <xsl:apply-templates select="node()" />
   </xsl:for-each>
   <xsl:if test="$runall='true'">
	   <xsl:for-each select="$table//dbs:ParentTable">
		   <xsl:variable name="name" select="@Name"/>
		   <!-- if added 20031121 KAD -->
		   <!-- if removed 20041201 KAD -->
		   <!-- <xsl:if test="not(//dbs:Table[@Name=$name]/@IsLookup='true')"> -->
   		   <xsl:if test="count($object//orm:ParentObject[@Name=$name])=0">
	   		   <xsl:element name="orm:ParentObject">
                  <xsl:call-template name="ParentAttributes">
                     <xsl:with-param name="namespace" select="$namespace"/>
                  </xsl:call-template>
      	         <xsl:call-template name="ParentContents" >
      	            <xsl:with-param name="parenttable" select="." />
         	         <xsl:with-param name="parentobject" select="/xxx" />
         	      </xsl:call-template>
		   	   </xsl:element>
		      </xsl:if>
		   <!-- </xsl:if> -->
	   </xsl:for-each>      
   </xsl:if>
</xsl:template>

<xsl:template name="ParentAttributes">
   <xsl:variable name="name" select="@Name" />
   <xsl:variable name="singular" select="net:GetSingular($name)" />
   <xsl:variable name="parentobject" select="//orm:Object[@Name=$singular]" />
	<xsl:attribute name="Name"><xsl:value-of select="$name" /></xsl:attribute>
	<xsl:attribute name="SingularName">
		<xsl:value-of select="$singular" />
	</xsl:attribute>
	<xsl:attribute name="PluralName">
		<xsl:value-of select="net:GetPlural($name)" />
	</xsl:attribute>
	<xsl:attribute name="Ordinal"><xsl:value-of select="position()" /></xsl:attribute>
	<xsl:attribute name="Namespace"><xsl:value-of select="@Namespace"/></xsl:attribute>
	<!--<xsl:attribute name="IsLookup"><xsl:value-of select="$parentobject/@IsLookup"/></xsl:attribute> -->
</xsl:template>

<xsl:template name="ParentContents">
   <xsl:param name="parenttable"/>
   <xsl:param name="parentobject" />
   <xsl:variable name="runall">
      <xsl:if test="count($parentobject//orm:ParentKeyField)=0" >true</xsl:if>
   </xsl:variable>
   <xsl:for-each select="$parentobject//orm:ParentKeyField">
      <xsl:for-each select="$parenttable//dbs:ParentKeyField">
         <xsl:call-template name="ParentKeyAttributes"/>
      </xsl:for-each>
   	<xsl:apply-templates select="@*" />
      <xsl:apply-templates select="node()" />
   </xsl:for-each>
   <xsl:if test="$runall='true'">
 	   <xsl:for-each select="$parenttable//dbs:ParentKeyField">
		   <xsl:variable name="name" select="@Name"/>
		   <xsl:if test="count($parentobject//orm:ParentKeyField[@Name=$name])=0">
			   <xsl:element name="orm:ParentKeyField">
               <xsl:call-template name="ParentKeyAttributes"/>
			   </xsl:element>
		   </xsl:if>
	   </xsl:for-each>      
 	   <xsl:for-each select="$parenttable//dbs:ChildField">
		   <xsl:variable name="name" select="@Name"/>
		   <xsl:if test="count($parentobject//orm:ChildKeyField[@Name=$name])=0">
			   <xsl:element name="orm:ChildKeyField">
               <xsl:call-template name="ParentKeyAttributes"/>
			   </xsl:element>
		   </xsl:if>
	   </xsl:for-each>      
   </xsl:if>
</xsl:template>

<xsl:template name="ParentKeyAttributes">
   <xsl:attribute name="Name">
      <xsl:value-of select="@Name"/>
   </xsl:attribute>
   <xsl:attribute name="Ordinal">
      <xsl:value-of select="position()"/>
   </xsl:attribute>
</xsl:template>

<xsl:template name="ExpandProperties" >
   <xsl:param name="table"/>
   <xsl:param name="object" />
   <xsl:param name="allproperties" />
   <xsl:variable name="runall">
      <xsl:if test="count($object//orm:Property)=0 or 
               $object//orm:AllProperties or 
               $allproperties='true'">true</xsl:if>
   </xsl:variable>
   <xsl:for-each select="$object//orm:Property">
      <xsl:for-each select="$table//dbs:TableColumn" >
         <xsl:call-template name="CopyImportantColumnAttributes"/>
      </xsl:for-each> 
   	<xsl:apply-templates select="@*" />
      <xsl:apply-templates select="node()" />
   </xsl:for-each>
   <xsl:if test="$runall='true'">
	   <xsl:for-each select="$table//dbs:TableColumn">
		   <xsl:variable name="name" select="@Name"/>
		   <xsl:if test="count($object//orm:Property[@Name=$name])=0">
			   <xsl:element name="orm:Property">
               <xsl:call-template name="CopyImportantColumnAttributes"/>
			   </xsl:element>
		   </xsl:if>
	   </xsl:for-each>      
   </xsl:if>
</xsl:template>

<xsl:template name="CopyImportantColumnAttributes">
   <xsl:if test="string-length(@Name)>0">
	   <xsl:attribute name="Name">
		   <xsl:value-of select="@Name"/>
	   </xsl:attribute>
   </xsl:if>
   
   <xsl:if test="string-length(@OriginalName)>0">
		<xsl:attribute name="Column">
			<xsl:value-of select="@OriginalName"/>
		</xsl:attribute>
   </xsl:if>
   
   <xsl:if test="string-length(@Column)>0">
		<xsl:attribute name="Column">
			<xsl:value-of select="@Column"/>
		</xsl:attribute>
   </xsl:if>
   
   <xsl:if test="string-length(@SQLType)>0">
	   <xsl:attribute name="SQLType">
		   <xsl:value-of select="@SQLType"/>
	   </xsl:attribute>
   </xsl:if>
   
	<xsl:variable name="nettype">
	   <xsl:choose>
	      <xsl:when test="@BaseNETType">
		      <xsl:value-of select="@BaseNETType"/>
	      </xsl:when>
	      <xsl:otherwise>
		      <xsl:value-of select="@NETType"/>
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:variable>
	<xsl:variable name="extnettype">
	      <xsl:choose>
	         <xsl:when test="count(key('SpecialTypes',$nettype))>0">
	            <xsl:value-of select="key('SpecialTypes',$nettype)/../@Name"/>
	         </xsl:when>
	         <xsl:otherwise>
	            <xsl:value-of select="$nettype"/>
	         </xsl:otherwise>
	      </xsl:choose>	</xsl:variable>
   <xsl:if test="string-length($nettype)>0">
	   <xsl:attribute name="BaseNETType">
		   <xsl:value-of select="$nettype"/>
	   </xsl:attribute>
	   <xsl:attribute name="NETType">
		   <xsl:value-of select="$extnettype"/>
	   </xsl:attribute>
	</xsl:if>
	
	<xsl:variable name="default">
	   <xsl:choose>
	      <xsl:when test="string-length(@Default)>0">
	         <xsl:value-of select="@Default" />
	      </xsl:when>
	      <xsl:otherwise>
	         <xsl:call-template name="SetDefault">
	            <xsl:with-param name="type" select="$extnettype" />
	         </xsl:call-template>
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:variable>
	<xsl:if test="string-length($default)>0">
	   <xsl:attribute name="Default">
	      <xsl:value-of select="$default" />
	   </xsl:attribute>
   </xsl:if>
   
	<xsl:variable name="defaultvb">
	   <xsl:choose>
	      <xsl:when test="string-length(@DefaultVB)>0">
	         <xsl:value-of select="@DefaultVB" />
	      </xsl:when>
	      <xsl:otherwise>
	         <xsl:value-of select="$default" />
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:variable>
	<xsl:if test="string-length($defaultvb)>0">
	   <xsl:attribute name="DefaultVB">
	      <xsl:value-of select="$defaultvb" />
	   </xsl:attribute>
   </xsl:if>
   
	<xsl:variable name="defaultcsharp">
	   <xsl:choose>
	      <xsl:when test="string-length(@DefaultCSharp)>0">
	         <xsl:value-of select="@DefaultCSharp" />
	      </xsl:when>
	      <xsl:otherwise>
	         <xsl:value-of select="$default" />
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:variable>
	<xsl:if test="string-length($defaultcsharp)>0">
	   <xsl:attribute name="DefaultCSharp">
	      <xsl:value-of select="$defaultcsharp" />
	   </xsl:attribute>
   </xsl:if>
   
   <xsl:if test="string-length(@NETType)>0">
	   <xsl:attribute name="Empty">
	      <xsl:call-template name="SetEmpty">
	         <xsl:with-param name="type" select="@NETType" />
	      </xsl:call-template>
	   </xsl:attribute>
   </xsl:if>
   
   <xsl:if test="string-length(@IsPrimaryKey)>0">
	   <xsl:attribute name="IsPrimaryKey">
		   <xsl:value-of select="@IsPrimaryKey"/>
	   </xsl:attribute>
   </xsl:if>
   
	<xsl:if test="@MaxLength and string-length(@MaxLength) > 0 and @MaxLength &lt; 35000 and @MaxLength > -1">
		<xsl:attribute name="MaxLength">
			<xsl:value-of select="@MaxLength"/>
		</xsl:attribute>
	</xsl:if>
   
   <xsl:if test="string-length(@IsAutoIncrement)>0">
		<xsl:attribute name="IsAutoIncrement">
			<xsl:value-of select="@IsAutoIncrement"/>
		</xsl:attribute>
	</xsl:if>
	
</xsl:template>

<xsl:template name="GetSPName">
	<xsl:param name="pattern"/>
	<xsl:param name="objectname"/>
	<xsl:variable name="lookfor" select="'[Object]'"/>
	<xsl:if test="contains($pattern,$lookfor)">
		<xsl:variable name="firstpart" select="substring-before($pattern,$lookfor)"/>
		<xsl:variable name="lastpart" select="substring-after($pattern,$lookfor)"/>
		<xsl:variable name="replace" select="$objectname"/>
		<xsl:value-of select="concat($firstpart,$replace,$lastpart)"/>
	</xsl:if>
</xsl:template>

<xsl:template name="GetParamName">
	<xsl:param name="fieldname" />
	<xsl:param name="objectname" />
	<xsl:choose>
		<xsl:when test="$fieldname='ID'">
			<xsl:value-of select="concat($objectname,'ID')"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$fieldname"/>
		</xsl:otherwise>
	</xsl:choose>
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
         <!-- changed the following column from Name to OriginalName 12/4/03 KAD -->
			<xsl:value-of select="concat( $andclause, '@', $first/@Name, ' = [', $first/../../@OriginalName, '].[', $first/@OriginalName, ']', $rest)"/>
		</xsl:when>
		<xsl:otherwise/>
	</xsl:choose>
</xsl:template>

<xsl:template name="SetDefault">
	<xsl:param name="type" select="@NETType"/>
	<xsl:choose>
	   <xsl:when test="$type='System.Guid' or $type='System.String'" >
			<xsl:call-template name="SetSystemDefault">
				<xsl:with-param name="type" select="$type" />
			</xsl:call-template>
	   </xsl:when>
		<xsl:when test="//orm:SpecialType[@Name=$type]/orm:Initialize">
			<xsl:text/>new <xsl:value-of select="$type"/>(<xsl:value-of select="//orm:SpecialType[@Name=$type]/orm:Initialize/@ParameterString"/>)<xsl:text/>
		</xsl:when>
	</xsl:choose> 
</xsl:template>
	
<xsl:template name="SetSystemDefault">
	<xsl:param name="type" select="@NETType"/>
	<xsl:choose>
		<xsl:when test="$type='System.Byte' or 
						$type='System.Int16' or 
						$type='System.Int32' or 
						$type='System.Int64' or 
						$type='System.Single' or 
						$type='System.Double' or 
						$type='System.Decimal'">0</xsl:when>
		<xsl:when test="$type='System.String'">""</xsl:when>
		<xsl:when test="$type='System.Boolean'">false</xsl:when><!-- Case doesn't hurt VB and let's the template be language neutral -->
		<xsl:when test="$type='System.DateTime'">#1/1/1800#</xsl:when>
		<xsl:when test="$type='System.Guid'">System.Guid.NewGuid()</xsl:when>
		<xsl:otherwise>Who Knows</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="SetEmpty">
	<xsl:param name="type" select="@NETType"/>
	<xsl:choose>
		<xsl:when test="//orm:SpecialType[@Name=$type]">Nothing</xsl:when>
		<xsl:when test="string-length(@Empty)=0">
			<xsl:choose>
				<xsl:when test="starts-with($type,'System.')">
					<xsl:call-template name="SetSystemEmpty">
					   <xsl:with-param name="type" select="$type" />
					</xsl:call-template>
				</xsl:when>
				<!-- Assume its an object, although its an invalid assumption -->
				<xsl:otherwise>Nothing</xsl:otherwise>
			</xsl:choose>
		</xsl:when>
		<xsl:otherwise>
			CType(<xsl:value-of select="@Empty"/>, <xsl:value-of select="$type"/>)
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>
	
<xsl:template name="SetSystemEmpty">
	<xsl:param name="type" select="@NETType"/>
	<xsl:choose>
		<xsl:when test="$type='System.Guid'">System.Guid.Empty</xsl:when>
		<xsl:otherwise>
		   <xsl:call-template name="SetSystemDefault">
		      <xsl:with-param name="type" select="$type" />
		   </xsl:call-template>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="GetTableNameFromName">
   <xsl:param name="tablename"/>
	<xsl:for-each select="//dbs:Table[@Name=$tablename]">
		<xsl:value-of select="@OriginalName" />
	</xsl:for-each>
</xsl:template>
 
 <xsl:template name="SQLTokenReplace">
   <xsl:param name="string" select="."/>
   <xsl:param name="targetlanguage" select="'VB'"/>
   <xsl:param name="nodeset" select="//orm:SQLCodeReplacement//orm:SQLToken"/>
   <xsl:param name="pass" select="1"/>
   <xsl:choose>
   <xsl:when test="$pass &lt; 5">
   <xsl:if test="string-length($string) > 0">
	   <xsl:choose>
		   <xsl:when test="$nodeset">
			   <xsl:variable name="first" select="$nodeset[1]"/>
			   <xsl:variable name="rest">
				   <xsl:call-template name="SQLTokenReplace">
					   <xsl:with-param name="nodeset" 
					                  select="$nodeset[position()!=1]"/>
					   <xsl:with-param name="targetlanguage"  
					                  select="$targetlanguage" />
					   <xsl:with-param name="pass" select="$pass + 1"/>
					   <xsl:with-param name="string" select="$string"/>
				   </xsl:call-template>
			   </xsl:variable>
			   <xsl:call-template name="SQLReplaceOne">
			      <xsl:with-param name="rest" select="$rest" />
			      <xsl:with-param name="first" select="$first" />
					<xsl:with-param name="targetlanguage" 
					                  select="$targetlanguage" />
			   </xsl:call-template>
		   </xsl:when>
		   <xsl:otherwise>
		      <xsl:value-of select="$string"/>
		   </xsl:otherwise>
	   </xsl:choose>
	</xsl:if>
   </xsl:when>
   <xsl:otherwise>
      ' FAILED IN PARENT - PASS = <xsl:value-of select="$pass"/>
   </xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="SQLReplaceOne">
   <xsl:param name="rest"/>
   <xsl:param name="first"/>
   <xsl:param name="targetlanguage" />
   <xsl:param name="pass" select="1"/>
   <xsl:choose>
   <xsl:when test="$pass &lt; 5">
	   <xsl:variable name="ustring" 
	            select="translate($rest,'abcdefghijklmnopqrstuvwxyz', 
	                  'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
	   <xsl:variable name="usearch" 
	            select="translate($first/@Token,'abcdefghijklmnopqrstuvwxyz',
	                  'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
	   <xsl:choose>
	      <xsl:when test="contains($ustring,$usearch)">
	         <xsl:variable name="replacewith">
		         <xsl:choose>
			         <xsl:when test="$targetlanguage='VB' and 
			               $first/@ReplaceWithVB">
			            <xsl:value-of select="$first/@ReplaceWithVB"/>
			         </xsl:when>
			         <xsl:when test="$targetlanguage='CSharp' and 
			               $first/@ReplaceWithCSharp">
			            <xsl:value-of select="$first/@ReplaceWithCSharp"/>
			         </xsl:when>
			         <xsl:otherwise>
			            <xsl:value-of select="$first/@ReplaceWith"/>
			         </xsl:otherwise>
		         </xsl:choose>
	         </xsl:variable>
	         <xsl:variable name="pos" 
	               select="string-length(substring-before($ustring,$usearch))" />
	         <xsl:variable name="outputstart" 
	               select="concat(substring($rest,1,$pos),$replacewith)"/>
	         <xsl:variable name="outputlast" 
	               select="substring($rest,$pos + string-length($usearch)+1)" />
	         <xsl:variable name="uoutputlast" 
	               select="translate($outputlast,'abcdefghijklmnopqrstuvwxyz',
	                     'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
	         <xsl:variable name="outputlastreplaced">
	            <xsl:choose>
	               <xsl:when test="contains($uoutputlast, $usearch)">
			            <xsl:call-template name="SQLReplaceOne">
			               <xsl:with-param name="rest" select="$outputlast" />
			               <xsl:with-param name="first" select="$first" />
			               <xsl:with-param name="pass" select="$pass + 1" />
                        <xsl:with-param name="targetlanguage" 
                              select="$targetlanguage"/>
			            </xsl:call-template>
	               </xsl:when>
	               <xsl:otherwise>
	                  <xsl:value-of select="$outputlast"/>
	               </xsl:otherwise>
	            </xsl:choose> 
	         </xsl:variable>
	         <!--<xsl:value-of select="concat('***',$usearch,'***')" />-->
	         <xsl:value-of 
	               select="concat($outputstart,$outputlastreplaced)" />
	      </xsl:when>
	      <xsl:otherwise>
	         <xsl:value-of select="$rest" />
	      </xsl:otherwise>
	   </xsl:choose>

   </xsl:when>
   <xsl:otherwise>
      ' FAILED IN CHILD - PASS = <xsl:value-of select="$pass"/>
   </xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="FixSQLNames">
   <xsl:param name="string" />
   <xsl:param name="removeprefix" />
   <xsl:param name="pass" select="1" />
   <xsl:choose>
      <xsl:when test="contains($string,'[')">
         <xsl:variable name="start" select="substring-before($string,'[')"/>
         <xsl:variable name="name" 
               select="substring-before(substring-after($string,'['),']')"/>
         <xsl:variable name="after" 
               select="substring-after(substring-after($string,'['),']')"/>
         <xsl:variable name="useafter">
            <xsl:call-template name="FixSQLNames">
               <xsl:with-param name="string" select="$after"/>
               <xsl:with-param name="removeprefix" select="$removeprefix"/>
               <xsl:with-param name="pass" select="$pass+1"/>
            </xsl:call-template>
         </xsl:variable>
         <xsl:value-of 
               select="concat($start,net:FixName($name,$removeprefix),
                     $useafter)"/>
      </xsl:when>
      <xsl:otherwise>
         <xsl:value-of select="$string"/>
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>
                  
 <!--<xsl:template name="SQLTokenReplace">
   <xsl:param name="string" select="."/>
   <xsl:param name="targetlanguage" select="'VB'"/>
   <xsl:param name="nodeset" select="//orm:SQLCodeReplacement//orm:SQLToken"/>
   <xsl:param name="pass" select="1"/>
   <xsl:if test="string-length($string) > 0">
	   <xsl:choose>
		   <xsl:when test="$nodeset">
			   <xsl:variable name="first" select="$nodeset[1]"/>
			   <xsl:variable name="rest">
				   <xsl:call-template name="SQLTokenReplace">
					   <xsl:with-param name="nodeset" select="$nodeset[position()!=1]"/>
					   <xsl:with-param name="targetlanguage" select="$targetlanguage" />
					   <xsl:with-param name="pass" select="pass + 1"/>
					   <xsl:with-param name="string" select="$string"/>
				   </xsl:call-template>
			   </xsl:variable>
			   <xsl:variable name="replacewith">
			      <xsl:choose>
			         <xsl:when test="$targetlanguage='VB' and $first/@ReplaceWithVB">
			            <xsl:value-of select="$first/@ReplaceWithVB"/>
			         </xsl:when>
			         <xsl:when test="$targetlanguage='CSharp' and $first/@ReplaceWithCSharp">
			            <xsl:value-of select="$first/@ReplaceWithCSharp"/>
			         </xsl:when>
			         <xsl:otherwise>
			            <xsl:value-of select="$first/@ReplaceWith"/>
			         </xsl:otherwise>
			      </xsl:choose>
			   </xsl:variable>
			   <xsl:variable name="ustring" select="translate($rest,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
			   <xsl:variable name="usearch" select="translate($first/@Token,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
			   <xsl:variable name="pos" select="string-length(substring-before($ustring,$usearch))" />
			   <xsl:variable name="output" >
			      <xsl:value-of select="concat(substring($rest,1,$pos),$replacewith,
			                                 substring($rest,$pos+string-length($usearch)+1))"/>
			   </xsl:variable>
			   <xsl:value-of select="$output"/>
		   </xsl:when>
		   <xsl:otherwise>
		      <xsl:value-of select="$string"/>
		   </xsl:otherwise>
	   </xsl:choose>
	</xsl:if>
</xsl:template> -->
                  
</xsl:stylesheet>

  