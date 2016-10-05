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
  Summary: Transforms the metadata for windows.forms use  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
         xmlns:net="http://kadgen.com/NETTools">
<xsl:import href="WinSupport.xslt"/>
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

   <xsl:key match="//orm:Object"  
         name="Objects"
         use="@Name" />  

   <xsl:key match="//ui:ControlMapping"
         name="ControlMappings"
         use="@NETType"/>
         
   <xsl:variable name="uiroot" select="//ui:UIRoot"/>
   <xsl:variable name="uiforms" select="//ui:Forms"/>


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
		   <xsl:when test="$uiforms/@Height">
		      <xsl:value-of select="$uiforms/@Height" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@Height">
		      <xsl:value-of select="$uiroot/@Height" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="vmargin">
		<xsl:choose>
		   <xsl:when test="$uiforms/@VerticalMargin">
		      <xsl:value-of select="$uiforms/@VerticalMargin" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@VerticalMargin">
		      <xsl:value-of select="$uiroot/@VerticalMargin" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultvmargin"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="hmargin">
		<xsl:choose>
		   <xsl:when test="$uiforms/@HorizontalMargin">
		      <xsl:value-of select="$uiforms/@HorizontalMargin" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@HorizontalMargin">
		      <xsl:value-of select="$uiroot/@HorizontalMargin" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaulthmargin"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="labelwidth">
		<xsl:choose>
		   <xsl:when test="$uiforms/@LabelWidth">
		      <xsl:value-of select="$uiforms/@LabelWidth" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@LabelWidth">
		      <xsl:value-of select="$uiroot/@LabelWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultlabelwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="btnwidth">
		<xsl:choose>
		   <xsl:when test="$uiforms/@ButtonWidth">
		      <xsl:value-of select="$uiforms/@ButtonWidth" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@ButtonWidth">
		      <xsl:value-of select="$uiroot/@ButtonWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultbtnwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="btnheight">
		<xsl:choose>
		   <xsl:when test="$uiforms/@ButtonHeight">
		      <xsl:value-of select="$uiforms/@ButtonHeight" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@ButtonHeight">
		      <xsl:value-of select="$uiroot/@ButtonHeight" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultbtnheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="formwidth">
		<xsl:choose>
		   <xsl:when test="$uiforms/@FormWidth">
		      <xsl:value-of select="$uiforms/@FormWidth" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@FormWidth">
		      <xsl:value-of select="$uiroot/@FormWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultformwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="formheight">
		<xsl:choose>
		   <xsl:when test="$uiforms/@FormHeight">
		      <xsl:value-of select="$uiforms/@FormHeight" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@FormHeight">
		      <xsl:value-of select="$uiroot/@FormHeight" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultformheight"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="minimumcontrolwidth">
		<xsl:choose>
		   <xsl:when test="$uiforms/@MinimumControlWidth">
		      <xsl:value-of select="$uiforms/@MinimumControlWidth" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@MinimumControlWidth">
		      <xsl:value-of select="$uiroot/@MinimumControlWidth" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultminimumcontrolwidth"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="showprimarykeys">
		<xsl:choose>
		   <xsl:when test="$uiforms/@ShowPrimaryKeys">
		      <xsl:value-of select="$uiforms/@ShowPrimaryKeys" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@ShowPrimaryKeys">
		      <xsl:value-of select="$uiroot/@ShowPrimaryKeys" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultshowprimarykeys"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="ondirtyclosingquestion">
		<xsl:choose>
		   <xsl:when test="$uiforms/@OnDirtyClosingQuestion">
		      <xsl:value-of select="$uiforms/@OnDirtyClosingQuestion" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@OnDirtyClosingQuestion">
		      <xsl:value-of select="$uiroot/@OnDirtyClosingQuestion" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultondirtyclosingquestion"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
   <xsl:variable name="ondirtyclosingtitle">
		<xsl:choose>
		   <xsl:when test="$uiforms/@OnDirtyClosingTitle">
		      <xsl:value-of select="$uiforms/@OnDirtyClosingTitle" />
		   </xsl:when>
		   <xsl:when test="$uiroot/@OnDirtyClosingTitle">
		      <xsl:value-of select="$uiroot/@OnDirtyClosingTitle" />
		   </xsl:when>
			<xsl:otherwise><xsl:value-of select="$defaultondirtyclosingtitle"/></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>

   
<xsl:template match="/ | @* | node()">
   <xsl:if test="count($uiroot)=0">
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
   <xsl:variable name="nettype" select="@NETType"/>
   <xsl:variable name="islookup">
      <xsl:choose>
         <!--<xsl:when test="//orm:Object[@Name=$parentname]/@IsLookup='true'" >true</xsl:when> -->
         <xsl:when test="key('Objects',$parentname)/@IsLookup='true'" >true</xsl:when>
         <xsl:when test="$parentname!=$thisparentname">true</xsl:when>
         <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   
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
         <xsl:when test="$islookup='true'">System.Windows.Forms.ComboBox</xsl:when>
         <!--<xsl:when test="//ui:ControlMapping[@NETType=$nettype]">-->
         <xsl:when test="key('ControlMappings',$nettype)">
            <xsl:value-of select ="key('ControlMappings',$nettype)/@ControlType" />
         </xsl:when>
         <xsl:when test="$nettype ='System.Boolean'">System.Windows.Forms.CheckBox</xsl:when>
         <xsl:when test="$nettype='System.DateTime' or $nettype='CSLA.SmartDate'">
            <xsl:text/>System.Windows.Forms.DateTimePicker<xsl:text/>
         </xsl:when> 
         <xsl:otherwise>System.Windows.Forms.TextBox</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="shortcontroltype">
      <xsl:call-template name="StripNamespace">
         <xsl:with-param name="name" select="$controltype"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="prefix">
      <xsl:choose>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$controltype]/@Prefix">-->
         <xsl:when test="key('ControlMappings',$controltype)/@Prefix">
            <xsl:value-of select="key('ControlMappings',$controltype)/@Prefix" />
         </xsl:when>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$shortcontroltype]/@Prefix">-->
         <xsl:when test="key('ControlMappings',$shortcontroltype)">
            <xsl:value-of select="key('ControlMappings',$shortcontroltype)/@Prefix" />
         </xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.ComboBox'">cbo</xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.CheckBox'">chk</xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.DateTimePicker'">dtp</xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.TextBox'">txt</xsl:when>
         <xsl:otherwise>xxx</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="changedevent">
      <xsl:choose>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$controltype]/@ChangedEvent">-->
         <xsl:when test="key('ControlMappings',$controltype)/@ChangedEvent">
            <xsl:value-of select="key('ControlMappings',$controltype)/@ChangedEvent" />
         </xsl:when>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$shortcontroltype]/@ChangedEvent">-->
         <xsl:when test="key('ControlMappings',$shortcontroltype)/@ChangedEvent">
            <xsl:value-of select="key('ControlMappings',$shortcontroltype)/@ChangedEvent" />
         </xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.ComboBox'">SelectedValueChanged</xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.CheckBox'">CheckedChanged</xsl:when>
         <xsl:otherwise>TextChanged</xsl:otherwise>
      </xsl:choose>
   </xsl:variable>
   <xsl:variable name="bindproperty">
      <xsl:choose>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$controltype]/@BindProperty">-->
         <xsl:when test="key('ControlMappings',$controltype)/@BindProperty">
            <xsl:value-of select="key('ControlMappings',$controltype)/@BindProperty" />
         </xsl:when>
         <!--<xsl:when test="//ui:ControlInfo[@ControlType=$shortcontroltype]/@BindProperty">-->
         <xsl:when test="key('ControlMappings',$shortcontroltype)/@BindProperty">
            <xsl:value-of select="key('ControlMappings',$shortcontroltype)/@BindProperty" />
         </xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.ComboBox'">SelectedValue</xsl:when>
         <xsl:when test="$controltype='System.Windows.Forms.CheckBox'">Checked</xsl:when>
         <xsl:otherwise>Text</xsl:otherwise>
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
   <xsl:attribute name="ControlChangedEvent"><xsl:value-of select="$changedevent"/></xsl:attribute>
   <xsl:attribute name="ControlName"><xsl:value-of select="concat($prefix,$name)"/></xsl:attribute>
   <xsl:attribute name="BindProperty"><xsl:value-of select="$bindproperty"/></xsl:attribute>
   <xsl:attribute name="ReadOnly">
         <xsl:value-of select="@IsPrimaryKey='true' and 
                         not($islookup='true') and
                         not($nettype='System.DateTime' or $nettype='CSLA.SmartDate')"/>
   </xsl:attribute>
   <xsl:attribute name="Display"><xsl:value-of select="$display"/></xsl:attribute>
   <xsl:choose>
      <!--<xsl:when test="//ui:ControlInfo[@ControlType=$controltype]/ui:ClearControl">-->
      <xsl:when test="key('ControlMappings',$controltype)/ui:ClearControl">
         <!--<xsl:apply-templates select="//ui:ControlInfo[@ControlType=$controltype]/ui:ClearControl"/>-->
         <xsl:apply-templates select="key('ControlMappings',$controltype)/ui:ClearControl" />
         <!--<xsl:for-each select="//ui:ControlInfo[@ControlType=$controltype]/ui:ClearControl">
            <xsl:element name="ui:ClearControl">
               <xsl:attribute name="Call"><xsl:value-of select="@Call"/></xsl:attribute>
            </xsl:element>
         </xsl:for-each> -->
      </xsl:when>
      <!--<xsl:when test="//ui:ControlInfo[@ControlType=$shortcontroltype]/@ClearControl">-->
      <xsl:when test="key('ControlMappings',$shortcontroltype)/ui:ClearControl">
         <!--<xsl:apply-templates select="//ui:ControlInfo[@ControlType=$controltype]/ui:ClearControl"/>-->
         <xsl:apply-templates select="key('ControlMappings',$shortcontroltype)/ui:ClearControl" />
      </xsl:when>
      <xsl:when test="$controltype='System.Windows.Forms.ComboBox'">
         <xsl:element name="ui:ClearControl">
            <xsl:attribute name="Call">SelectedIndex = -1</xsl:attribute>
         </xsl:element>
         <xsl:element name="ui:ClearControl">
            <xsl:attribute name="Call">SelectedItem = Nothing</xsl:attribute>
         </xsl:element>
      </xsl:when>
      <xsl:when test="$controltype='System.Windows.Forms.CheckBox'">
         <xsl:element name="ui:ClearControl">
            <xsl:attribute name="Call">Checked = False</xsl:attribute>
         </xsl:element>
      </xsl:when>
      <xsl:otherwise>
         <xsl:element name="ui:ClearControl">
            <xsl:attribute name="Call">Text = ""</xsl:attribute>
         </xsl:element>
      </xsl:otherwise>
   </xsl:choose>
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
         <xsl:call-template name="FormAttributes" >
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
 		   <xsl:if test="key('Objects',$businessobject)[@HideInUI='true']">
 		      <xsl:attribute name="NoGen">true</xsl:attribute>
 		   </xsl:if>
 		   <xsl:apply-templates select="node()"/>
 		</xsl:copy> 
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <xsl:variable name="rootobjects" select="//orm:Object[@Root='true' ]" />
      <!-- Lookup added back 11/30/2003 KAD -->
      <!--<xsl:apply-templates select="//orm:Object[@Root='true' and string(@IsLookup)!='true']" mode="AddUIList"> -->
      <xsl:apply-templates select="$rootobjects" mode="AddUIList">
         <xsl:with-param name="formtype" select="'Edit'" />
      </xsl:apply-templates>
      <!--<xsl:apply-templates select="//orm:Object[@Root='true' and string(@IsLookup)!='true']" mode="AddUIList"> -->
      <xsl:apply-templates select="$rootobjects" mode="AddUIList">
         <xsl:with-param name="formtype" select="'Select'" />
      </xsl:apply-templates>
      <xsl:apply-templates select="$rootobjects" mode="AddUIList">
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
         <xsl:call-template name="FormAttributes">
            <xsl:with-param name="name" select="$name" />
         </xsl:call-template>
      </xsl:element>
   </xsl:if>
</xsl:template>

<xsl:template name="CopyRootAttributes">
   <xsl:for-each select="$uiroot">
 	   <xsl:apply-templates select="@*"  />
   </xsl:for-each>
   <xsl:for-each select="$uiforms">
      <xsl:apply-templates select="@*"  />
   </xsl:for-each>
</xsl:template>

<xsl:template name="FormAttributes">
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
  