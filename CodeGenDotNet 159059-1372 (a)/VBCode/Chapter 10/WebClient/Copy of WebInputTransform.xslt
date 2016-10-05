<?xml version="1.0" encoding="UTF-8" ?>

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
         xmlns:net="http://kadgen.com/NETTools">
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="/ | @* | node()">
   <xsl:if test="count(//ui:UIRoot)=0">
      <xsl:element name="UIRoot">
         <xsl:call-template name="UIInfo"/>
      </xsl:element>
   </xsl:if>
	<xsl:copy>
		<xsl:apply-templates select="@*"  />
		<xsl:choose>
	      <xsl:when test="name()='orm:Property'">
				<xsl:call-template name="PropertyControlInfo" />
   		   <xsl:apply-templates select="node()"/>
	      </xsl:when>
	      <xsl:when test="name()='ui:UIRoot'">
				<xsl:call-template name="UIInfo" />
      		<xsl:apply-templates select="@*"  />
	      </xsl:when>
	      <xsl:otherwise>
   		   <xsl:apply-templates select="node()"/>
	      </xsl:otherwise>
		</xsl:choose>
	</xsl:copy>
</xsl:template>

<xsl:template name="PropertyControlInfo" >
   <xsl:variable name="name" select="@Name"/>
   <xsl:variable name="parentname" select="ancestor::orm:Object//orm:ChildKeyField[@Name=$name]/../@SingularName"/>
   <xsl:variable name="islookup" select="//orm:Object[@Name=$parentname]/@IsLookup" />
   <xsl:if test="string-length($parentname) > 0">
      <xsl:attribute name="Parent"><xsl:value-of select="$parentname"/></xsl:attribute>
      <xsl:if test="$islookup='true'">
         <xsl:attribute name="IsLookup"><xsl:value-of select="$islookup"/></xsl:attribute>
         <xsl:attribute name="LookupCollection"><xsl:value-of select="net:GetPlural($parentname)"/></xsl:attribute>
      </xsl:if>
   </xsl:if>
   <xsl:variable name="controltype">
      <xsl:choose>
         <xsl:when test="0=1"/>
         <xsl:otherwise>Web.UI.WebControls.TextBox</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="aspcontroltype">
      <xsl:choose>
         <xsl:when test="0=1"/>
         <xsl:otherwise>asp:textbox</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="prefix">
      <xsl:choose>
         <xsl:when test="$controltype='Web.UI.WebControls.TextBox'">txt</xsl:when>
         <xsl:otherwise>xxx</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:attribute name="ControlType"><xsl:value-of select="$controltype" /></xsl:attribute>
   <xsl:attribute name="ASPControlType"><xsl:value-of select="$aspcontroltype" /></xsl:attribute>
   <xsl:attribute name="ControlPrefix"><xsl:value-of select="$prefix" /></xsl:attribute>
   <xsl:attribute name="ControlName"><xsl:value-of select="concat($prefix,$name)"/></xsl:attribute>
   <xsl:attribute name="ReadOnly"><xsl:value-of select="@IsPrimaryKey"/></xsl:attribute>
</xsl:template> 

<xsl:template name="UIInfo">
  
   <!-- Build the ui:Form element. -->
   <xsl:variable name="runall" select="count(//ui:Form)=0 or //ui:AllForms"/>
   <xsl:for-each select="//ui:Form">
      <xsl:choose>
         <xsl:when test="@NoGen='true'"/>
         <xsl:otherwise>
            <xsl:copy>
               <!-- This counts on attributes overwriting existing attributes. -->
               <!-- I hate Name not being first -->
               <xsl:attribute name="Name"/>
               <xsl:call-template name="CopyRootAttributes"/>
               <xsl:call-template name="FormInfo" >
               </xsl:call-template>
 		         <xsl:apply-templates select="@*"  />
 		         <xsl:choose>
 		            <xsl:when test="@BusinessObject"/>
 		            <xsl:otherwise>
 		               <xsl:attribute name="BusinessObject">
 		                  <xsl:choose>
 		                     <xsl:when test="ends-with(@Name,'Edit')">
 		                        <xsl:value-of select="substring-before(@Name,'Edit')" />
 		                     </xsl:when>
 		                     <xsl:otherwise>
 		                        <xsl:value-of select="@Name" />
 		                     </xsl:otherwise>
 		                  </xsl:choose>
 		               </xsl:attribute>
 		            </xsl:otherwise>
 		         </xsl:choose>
 		         <xsl:apply-templates select="node()"  />
            </xsl:copy>
         </xsl:otherwise>
      </xsl:choose>

   </xsl:for-each>
   <xsl:element name="runall"><xsl:value-of select="$runall" /></xsl:element>
   <xsl:if test="$runall='true'">
      <xsl:for-each select="//orm:Object[@Root='true' and string(@IsLookup)!='true']">
         <xsl:variable name="objectname" select="@Name"/>
         <xsl:variable name="name" select="concat(@Name,'Edit')"/>
         <xsl:element name="Fred"><xsl:value-of select="$name" /></xsl:element>
         <xsl:if test="count(//ui:Form[@Name=$name])=0">
            <xsl:element name="ui:Form">
               <!-- I hate Name not being first, but I need to insure this value is used -->
               <xsl:attribute name="Name"/>
               <xsl:call-template name="CopyRootAttributes"/>
               <xsl:attribute name="Name">
                  <xsl:value-of select="$name"/>
               </xsl:attribute>
               <xsl:attribute name="BusinessObject">
                  <xsl:value-of select="$objectname"/>
               </xsl:attribute>
               <xsl:call-template name="FormInfo">
               </xsl:call-template>
            </xsl:element>
         </xsl:if>
      </xsl:for-each>
      </xsl:if>
</xsl:template>

<xsl:template name="CopyRootAttributes">
   <xsl:for-each select="//ui:UIRoot">
 	   <xsl:apply-templates select="@*"  />
   </xsl:for-each>
   <xsl:for-each select="//ui:Forms">
      <xsl:apply-templates select="@*"  />
   </xsl:for-each>
</xsl:template>

<xsl:template name="FormInfo">
</xsl:template>

</xsl:stylesheet> 
  