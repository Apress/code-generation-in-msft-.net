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


   <xsl:variable name="defaultheight" select="20" />
   <xsl:variable name="defaultvmargin" select="8" />
   <xsl:variable name="defaulthmargin" select="8" />
   <xsl:variable name="defaultlabelwidth" select="120" />
   <xsl:variable name="defaultbtnwidth" select="75" />
   <xsl:variable name="defaultbtnheight" select="24" />
   <xsl:variable name="defaultformwidth" select="750" />
   <xsl:variable name="defaultformheight" select="450" />
   <xsl:variable name="defaultminimumcontrolwidth" select="100" />
   <xsl:variable name="defaultshowprimarykeys" select="'false'" />
   <xsl:variable name="defaultondirtyclosingquestion" select="'You have unchanged changes that will be lost if you continue. Do you want to close the form anyway?'" />
   <xsl:variable name="defaultondirtyclosingtitle" select="'Close Confirmation'" />
   
   <!-- Build variables with UIInfo defaults - if any -->
   <xsl:variable name="height">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@Height">
		      <xsl:value-of select="//ui:Forms/@Height" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@Height">
		      <xsl:value-of select="//ui:UIRoot/@Height" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="vmargin">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@VerticalMargin">
		      <xsl:value-of select="//ui:Forms/@VerticalMargin" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@VerticalMargin">
		      <xsl:value-of select="//ui:UIRoot/@VerticalMargin" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultvmargin"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="hmargin">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@HorizontalMargin">
		      <xsl:value-of select="//ui:Forms/@HorizontalMargin" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@HorizontalMargin">
		      <xsl:value-of select="//ui:UIRoot/@HorizontalMargin" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaulthmargin"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="labelwidth">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@LabelWidth">
		      <xsl:value-of select="//ui:Forms/@LabelWidth" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@LabelWidth">
		      <xsl:value-of select="//ui:UIRoot/@LabelWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultlabelwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="btnwidth">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@ButtonWidth">
		      <xsl:value-of select="//ui:Forms/@ButtonWidth" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@ButtonWidth">
		      <xsl:value-of select="//ui:UIRoot/@ButtonWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultbtnwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="btnheight">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@ButtonHeight">
		      <xsl:value-of select="//ui:Forms/@ButtonHeight" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@ButtonHeight">
		      <xsl:value-of select="//ui:UIRoot/@ButtonHeight" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultbtnheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="formwidth">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@FormWidth">
		      <xsl:value-of select="//ui:Forms/@FormWidth" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@FormWidth">
		      <xsl:value-of select="//ui:UIRoot/@FormWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultformwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="formheight">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@FormHeight">
		      <xsl:value-of select="//ui:Forms/@FormHeight" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@FormHeight">
		      <xsl:value-of select="//ui:UIRoot/@FormHeight" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultformheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="minimumcontrolwidth">
		<xsl:choose>
		   <xsl:when test="//ui:minimumcontrols/@minimumcontrolWidth">
		      <xsl:value-of select="//ui:minimumcontrols/@MinimumControlWidth" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@MinimumControlWidth">
		      <xsl:value-of select="//ui:UIRoot/@MinimumControlWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultminimumcontrolwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="showprimarykeys">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@ShowPrimaryKeys">
		      <xsl:value-of select="//ui:Forms/@ShowPrimaryKeys" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@ShowPrimaryKeys">
		      <xsl:value-of select="//ui:UIRoot/@ShowPrimaryKeys" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultshowprimarykeys"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="ondirtyclosingquestion">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@OnDirtyClosingQuestion">
		      <xsl:value-of select="//ui:Forms/@OnDirtyClosingQuestion" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@OnDirtyClosingQuestion">
		      <xsl:value-of select="//ui:UIRoot/@OnDirtyClosingQuestion" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultondirtyclosingquestion"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="ondirtyclosingtitle">
		<xsl:choose>
		   <xsl:when test="//ui:Forms/@OnDirtyClosingTitle">
		      <xsl:value-of select="//ui:Forms/@OnDirtyClosingTitle" />
		   </xsl:when>
		   <xsl:when test="//ui:UIRoot/@OnDirtyClosingTitle">
		      <xsl:value-of select="//ui:UIRoot/@OnDirtyClosingTitle" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultondirtyclosingtitle"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>

   
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
   <xsl:variable name="thisparentname" select="substring-before(../@Name,../@SingularName)"/>
   <xsl:variable name="parentname" select="ancestor::orm:Object//orm:ChildKeyField[@Name=$name]/../@SingularName"/>
   <xsl:variable name="islookup">
      <xsl:choose>
         <xsl:when test="//orm:Object[@Name=$parentname]/@IsLookup='true'" >true</xsl:when>
         <xsl:when test="$parentname!=$thisparentname">true</xsl:when>
         <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:attribute name="Fred"><xsl:value-of select="$thisparentname"/></xsl:attribute>
   <xsl:attribute name="George"><xsl:value-of select="$parentname"/></xsl:attribute>
   <xsl:attribute name="Percy"><xsl:value-of select="$islookup"/></xsl:attribute>
   
   <xsl:if test="string-length($parentname) > 0">
      <xsl:attribute name="Parent"><xsl:value-of select="$parentname"/></xsl:attribute>
      <xsl:if test="$islookup='true'">
         <xsl:attribute name="IsLookup"><xsl:value-of select="$islookup"/></xsl:attribute>
         <xsl:attribute name="LookupCollection"><xsl:value-of select="net:GetPlural($parentname)"/></xsl:attribute>
         <xsl:attribute name="LookupSingular"><xsl:value-of select="net:GetSingular($parentname)"/></xsl:attribute>
         <xsl:attribute name="LookupName"><xsl:value-of select="$parentname"/></xsl:attribute>
      </xsl:if>
   </xsl:if>
   <xsl:variable name="controltype">
      <xsl:choose>
         <!-- Be sure to add these to the OnChanged method creation in layoutEdit.xslt -->
         <xsl:when test="$islookup='true'">Windows.Forms.ComboBox</xsl:when>
         <xsl:when test="@NETType='System.Boolean'">Windows.Forms.CheckBox</xsl:when>
         <!-- DK: Have to check if field allows nulls -->
         <!-- <xsl:when test="@NETType='System.DateTime'">Windows.Forms.DateTimePicker</xsl:when> -->
         <!-- <xsl:when test="@NETType='System.DateTime' or @NETType='CSLA.SmartDate'">
            <xsl:choose>
               <xsl:when test="@AllowNulls='true'">Windows.Forms.TextBox</xsl:when>
               <xsl:otherwise>Windows.Forms.DateTimePicker</xsl:otherwise>
            </xsl:choose>
         </xsl:when> -->
         <xsl:when test="@NETType='System.DateTime' or @NETType='CSLA.SmartDate'">
            <!--<xsl:text/>Umbrae.Windows.Forms.DateTimeSlicker<xsl:text/> -->
            <xsl:text/>WinSupport.DateTimeUC<xsl:text/>
         </xsl:when> 
         <!-- ********************** -->
         <xsl:when test="@NETType='System.DateTime'">Windows.Forms.DateTimePicker</xsl:when>
         <xsl:otherwise>Windows.Forms.TextBox</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="prefix">
      <xsl:choose>
         <xsl:when test="$controltype='Windows.Forms.ComboBox'">cbo</xsl:when>
         <xsl:when test="$controltype='Windows.Forms.CheckBox'">chk</xsl:when>
         <xsl:when test="$controltype='Windows.Forms.DateTimePicker'">cln</xsl:when>
         <xsl:when test="$controltype='Windows.Forms.TextBox'">txt</xsl:when>
         <xsl:when test="$controltype='Umbrae.Windows.Forms.DateTimeSlicker'">dts</xsl:when>
         <xsl:when test="$controltype='WinSupport.DateTimeUC'">dts</xsl:when>
         <xsl:otherwise>xxx</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="display">
      <xsl:choose>
         <xsl:when test="not(@IsPrimaryKey='true') or $islookup='true' or $showprimarykeys='true'">true</xsl:when>
         <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:attribute name="ControlType"><xsl:value-of select="$controltype" /></xsl:attribute>
   <xsl:attribute name="ControlPrefix"><xsl:value-of select="$prefix" /></xsl:attribute>
   <xsl:attribute name="ControlName"><xsl:value-of select="concat($prefix,$name)"/></xsl:attribute>
   <xsl:attribute name="ReadOnly">
         <xsl:value-of select="@IsPrimaryKey='true' and 
                         not($controltype='Windows.Forms.ComboBox') and
                         not($controltype='Windows.Forms.DateTimePicker')"/>
   </xsl:attribute>
   <xsl:attribute name="Display"><xsl:value-of select="$display"/></xsl:attribute>
</xsl:template> 

<xsl:template name="UIInfo">
   <!-- This template is monolithic to manage the variables. Watch the interspersed comments -->
   <!-- Set default values - change organization defaults here -->

   
   <!-- Build the ui:Form element. -->
   <xsl:variable name="runall" select="count(//ui:Form)=0 or //ui:AllForms"/>
   <xsl:for-each select="//ui:Form">
      <xsl:copy>
         <!-- This counts on attributes overwriting existing attributes. -->
         <!-- I hate Name not being first -->
         <xsl:attribute name="Name"/>
         <xsl:call-template name="CopyRootAttributes"/>
         <xsl:call-template name="FormInfo" >
            <xsl:with-param name="name" select="@Name" />
         </xsl:call-template>
 		   <xsl:apply-templates select="@*"  />
 		   <xsl:variable name="formtype" select="@FormType"/>
 		   <xsl:variable name="businessobject">
 		      <xsl:choose>
 		         <xsl:when test="string-length($formtype)>0">
 		            <xsl:choose>
 		               <xsl:when test="contains(@Name,$formtype)" >
    		               <xsl:value-of select="substring-before(@Name,$formtype)" />
 		               </xsl:when>
 		               <xsl:otherwise>
    		               <xsl:value-of select="@Name" />
 		               </xsl:otherwise>
 		            </xsl:choose>
 		         </xsl:when>
 		         <xsl:otherwise>
 		            <!-- Not sure what the templates will do with this -->
 		            <xsl:value-of select="@Name" />
 		         </xsl:otherwise>
 		      </xsl:choose>
 		   </xsl:variable>
 		   <xsl:attribute name="BusinessObject">
 		      <xsl:value-of select="$businessobject" />
 		   </xsl:attribute>
 		   <xsl:if test="//orm:Object[@Name=$businessobject and @HideInUI='true']">
 		      <xsl:attribute name="NoGen">true</xsl:attribute>
 		   </xsl:if>
 		</xsl:copy> 
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <!-- Lookup added back 11/30/2003 KAD -->
      <!--<xsl:apply-templates select="//orm:Object[@Root='true' and string(@IsLookup)!='true']" mode="AddUIList"> -->
      <xsl:apply-templates select="//orm:Object[@Root='true' ]" mode="AddUIList">
         <xsl:with-param name="formtype" select="'Edit'" />
      </xsl:apply-templates>
      <!--<xsl:apply-templates select="//orm:Object[@Root='true' and string(@IsLookup)!='true']" mode="AddUIList"> -->
      <xsl:apply-templates select="//orm:Object[@Root='true' ]" mode="AddUIList">
         <xsl:with-param name="formtype" select="'Select'" />
      </xsl:apply-templates>
      <xsl:apply-templates select="//orm:Object[@Root='true' ]" mode="AddUIList">
         <xsl:with-param name="formtype" select="'SelectUC'" />
      </xsl:apply-templates>
   </xsl:if>
</xsl:template>

<xsl:template match="orm:Object" mode="AddUIList">
   <xsl:param name="formtype"/>
   <xsl:variable name="objectname" select="@Name"/>
   <xsl:variable name="name" select="concat(@Name,$formtype)"/>
   '***<xsl:value-of select="$objectname"/>***<xsl:value-of select="$name"/>***
   <xsl:if test="count(//ui:Form[@Name=$name])=0">
      <xsl:element name="ui:Form">
         <!-- I hate Name not being first, but I need to insure this value is not overwritten -->
         <xsl:attribute name="Name"/>
         <xsl:call-template name="CopyRootAttributes"/>
         <xsl:attribute name="Name">
            <xsl:value-of select="$name"/>
         </xsl:attribute>
         <xsl:attribute name="FormType">
            <xsl:value-of select="$formtype" />
         </xsl:attribute>
         <xsl:if test="@HideInUI='true'">
            <xsl:attribute name="NoGen">true</xsl:attribute>
         </xsl:if>
         <xsl:attribute name="BusinessObject">
            <xsl:value-of select="$objectname"/>
         </xsl:attribute>
         <xsl:call-template name="FormInfo">
            <xsl:with-param name="name" select="$name" />
         </xsl:call-template>
      </xsl:element>
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
   <xsl:param name="name" />

   <xsl:attribute name="Height">
      <xsl:value-of select="$height"/>
   </xsl:attribute>
   <xsl:attribute name="VerticalMargin">
      <xsl:value-of select="$vmargin"/>
   </xsl:attribute>
   <xsl:attribute name="HorizontalMargin">
      <xsl:value-of select="$hmargin"/>
   </xsl:attribute>
   <xsl:attribute name="LabelWidth">
      <xsl:value-of select="$labelwidth"/>
   </xsl:attribute>
   <xsl:attribute name="ButtonWidth">
      <xsl:value-of select="$btnwidth"/>
   </xsl:attribute>
   <xsl:attribute name="ButtonHeight">
      <xsl:value-of select="$btnheight"/>
   </xsl:attribute>
   <xsl:attribute name="FormWidth">
      <xsl:value-of select="$formwidth"/>
   </xsl:attribute>
   <xsl:attribute name="FormHeight">
      <xsl:value-of select="$formheight"/>
   </xsl:attribute>
   <xsl:attribute name="MinimumControlWidth">
      <xsl:value-of select="$minimumcontrolwidth"/>
   </xsl:attribute>
   <xsl:attribute name="ShowPrimaryKeys">
      <xsl:value-of select="$showprimarykeys"/>
   </xsl:attribute>
   <xsl:attribute name="OnDirtyClosingQuestion">
      <xsl:value-of select="$ondirtyclosingquestion"/>
   </xsl:attribute>
   <xsl:attribute name="OnDirtyClosingTitle">
      <xsl:value-of select="$ondirtyclosingtitle"/>
   </xsl:attribute>
</xsl:template>

</xsl:stylesheet> 
  