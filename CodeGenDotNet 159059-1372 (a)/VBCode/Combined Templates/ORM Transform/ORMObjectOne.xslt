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

<xsl:variable name="removeprefix">
   <xsl:choose>
      <xsl:when test="ancestor-or-self::*[@RemovePrefix][1]/@RemovePrefix='false'">false</xsl:when>
      <xsl:otherwise>true</xsl:otherwise>
   </xsl:choose>
</xsl:variable>

<xsl:template match="/ | node()">
	<xsl:choose>
		<xsl:when test="name()='orm:Assembly'" /> <!-- Handled Below -->
		<xsl:when test="name()='dbs:TableColumn'" > 
		   <xsl:call-template name="TableColumn"/>
		</xsl:when>
		<xsl:when test="name()='dbs:CheckConstraint'" > 
		   <xsl:call-template name="CheckConstraint"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:copy>
				<xsl:apply-templates select="@*"  />
				<xsl:apply-templates select="node()"/>
	         <xsl:if test="name()='orm:MappingRoot'">
				   <xsl:call-template name="Assembly" />
	         </xsl:if>
			</xsl:copy>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match ="@*">
   <xsl:copy/>
</xsl:template>

<xsl:template name="Assembly">
	<xsl:for-each select="//orm:Assembly">
		<xsl:copy>
   		<xsl:call-template name="AssemblyAttributes" />
			<xsl:apply-templates select="@*" />
   		<xsl:apply-templates select="node()[name()!='orm:Objects' and name()!='orm:Object']"/>
			<xsl:call-template name="Objects" />
		</xsl:copy>
	</xsl:for-each>
</xsl:template>

<xsl:template name="AssemblyAttributes">
   <xsl:call-template name="StandardAttributes"/>
</xsl:template>

<xsl:template name="TableColumn">
   <xsl:variable name="tablename" select="ancestor::dbs:Table/@Name" />
   <xsl:variable name="name" select="@Name" />
   <xsl:variable name="delimiter" select="ancestor-or-self::*/@UseForDescDelimiter[1]"/>
   <xsl:attribute name="TableNameTest"><xsl:value-of select="$tablename"/></xsl:attribute>
	<xsl:copy>
		<xsl:apply-templates select="@*" mode="TableColumnAttributes" />
		<xsl:choose>
		   <xsl:when test="@IsPrimaryKey='true'" />
		   <xsl:when test="count(ancestor::dbs:Table//dbs:TableColumn[@UseForDesc='true'])=0">
		      <xsl:attribute name="UseForDesc">true</xsl:attribute>
		      <xsl:attribute name="UseForDescDelimiter">
		         <xsl:value-of select="$delimiter" />
		      </xsl:attribute>
		      <xsl:attribute name="UseForDescOrdinal">
               <xsl:value-of select="position()" />
		      </xsl:attribute>
		   </xsl:when>
		   <xsl:when test="@UseForDesc='true'">
		      <!-- This is ugly, but effective -->
		      <xsl:attribute name="UseForDescOrdinal">
		         <xsl:for-each select="//dbs:TableInfo[@Name=$tablename]//dbs:TableColumnInfo[@UseForDesc='true']" >
		            <xsl:if test="@Name=$name">
		               <xsl:value-of select="position()" />
		            </xsl:if>
		         </xsl:for-each>
		      </xsl:attribute>
		   </xsl:when>
		</xsl:choose>
		
		<xsl:apply-templates select="node()"/>
	</xsl:copy> 
</xsl:template>

<xsl:template match="@*" mode="TableColumnAttributes">
   <xsl:choose>
      <xsl:when test="name()='Default'">
         <xsl:attribute name="OriginalDefault">
            <xsl:value-of select="."/>
         </xsl:attribute>
         <xsl:variable name="default">
            <xsl:call-template name="FixSQLNames">
               <xsl:with-param name="string" select="."/>
               <xsl:with-param name="removeprefix" select="$removeprefix"/>
            </xsl:call-template>
         </xsl:variable>
         <xsl:attribute name="DefaultVB">
            <xsl:call-template name="SQLTokenReplace">
               <xsl:with-param name="string" select="$default"/>
               <xsl:with-param name="targetlanguage" select="'VB'"/>
            </xsl:call-template>
         </xsl:attribute>
         <xsl:attribute name="DefaultCSharp">
            <xsl:call-template name="SQLTokenReplace">
               <xsl:with-param name="string" select="$default"/>
               <xsl:with-param name="targetlanguage" select="'CSharp'"/>
            </xsl:call-template>
         </xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
         <xsl:copy />
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>

<xsl:template name="CheckConstraint">
   <xsl:copy>
      <xsl:for-each select="@*">
         <xsl:choose>
            <xsl:when test="name()='Clause'">
               <xsl:attribute name="OriginalClause">
                  <xsl:value-of select="."/>
               </xsl:attribute>
               <xsl:variable name="clause">
                  <xsl:call-template name="FixSQLNames">
                     <xsl:with-param name="string" select="."/>
                     <xsl:with-param name="removeprefix" select="$removeprefix"/>
                  </xsl:call-template>
               </xsl:variable>
               <xsl:attribute name="ClauseVB">
                  <xsl:call-template name="SQLTokenReplace">
                     <xsl:with-param name="string" select="$clause"/>
                     <xsl:with-param name="targetlanguage" select="'VB'"/>
                  </xsl:call-template>
               </xsl:attribute>
               <xsl:attribute name="ClauseCSharp">
                  <xsl:call-template name="SQLTokenReplace">
                     <xsl:with-param name="string" select="$clause"/>
                     <xsl:with-param name="targetlanguage" select="'CSharp'"/>
                  </xsl:call-template>
               </xsl:attribute>
            </xsl:when>
         </xsl:choose>
      </xsl:for-each>
   </xsl:copy>
</xsl:template>

</xsl:stylesheet> 
  