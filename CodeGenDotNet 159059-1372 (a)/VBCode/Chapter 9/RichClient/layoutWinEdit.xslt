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
  Summary: Generates the layout class for the main edit user control  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="WinSupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="baseformname" select="'WinSupport.BaseEditUserControl'" />
<xsl:variable name="formname" select="$Name"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 

<xsl:variable name="height" 
               select="//ui:Form[@Name=$Name]/@Height" />
<xsl:variable name="vmargin" 
               select="//ui:Form[@Name=$Name]/@VerticalMargin" />
<xsl:variable name="hmargin" 
               select="//ui:Form[@Name=$Name]/@HorizontalMargin" />
<xsl:variable name="labelwidth" 
               select="//ui:Form[@Name=$Name]/@LabelWidth" />
<xsl:variable name="btnwidth" 
               select="//ui:Form[@Name=$Name]/@ButtonWidth" />
<xsl:variable name="btnheight" 
               select="//ui:Form[@Name=$Name]/@ButtonHeight" />
<xsl:variable name="formwidth" 
               select="//ui:Form[@Name=$Name]/@FormWidth" />
<xsl:variable name="formheight" 
               select="//ui:Form[@Name=$Name]/@FormHeight" />
<xsl:variable name="minimumcontrolwidth" 
               select="//ui:Form[@Name=$Name]/@MinimumControlWidth" />
<xsl:variable name="showprimarykeys" 
               select="/ui:Form[@Name=$Name]/@ShowPrimaryKeys"/>

<xsl:variable name="ctlleft" select="$hmargin + $labelwidth"/>
<xsl:variable name="ctlwidth" select="$formwidth - 3 * $hmargin - $ctlleft"/>
<xsl:variable name="childwidth" select="$formwidth - 2 * $hmargin"/>
<xsl:variable name="grptopmargin" select="$height"/>
<xsl:variable name="properties" 
            select="//orm:Object[@Name=$objectname]/orm:Property[@Display='true']" />
<xsl:variable name="childcollections" 
            select="//orm:Object[@Name=$objectname and not(@IsLookup='true')]/orm:ChildCollection" />
	   
<!-- The following was "fixed" empirically -->
<xsl:variable name="lasttop" 
		      select="count($properties)*
		              ($height+$vmargin) + 2* $vmargin"/>
<xsl:variable name="formminheight" 
            select="$lasttop + 
                   (count($childcollections) * 
                   ($grptopmargin + (3 * $btnheight) + (7 * $vmargin)))"/>
		              
<xsl:variable name="formminwidth"
            select="$labelwidth + 2 * $btnwidth" />
            
<xsl:variable name="grpheight">
    <xsl:value-of select="$grptopmargin + (3 * $btnheight) + (4 * $vmargin)"/>
    <!--<xsl:value-of select="5 * $height + 3 * $vmargin"/>-->
</xsl:variable>
            
<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
' <xsl:value-of select="count(//orm:Object[@Name=$objectname]/orm:Property[@Display='true'])"/>
' <xsl:value-of select="count(//orm:Object[@Name=$objectname and not(@IsLookup='true')]/orm:ChildCollection)"/>
Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
Imports System.Threading
Imports KADGen.BusinessObjectSupport

Public Class layout<xsl:value-of select="$formname"/><xsl:text/>
	Inherits <xsl:value-of select="$baseformname"/>

  <xsl:call-template name="ClassDeclarations"/>
  <xsl:call-template name="WindowsFormDesigner"/>
  <xsl:call-template name="EventHandlers"/>
  <xsl:call-template name="RestOfForm"/>
End Class
</xsl:template>

<xsl:template name="ClassDeclarations">
#Region " Class Declarations "
   Protected mMinimumSize As New System.Drawing.Size(<xsl:text/>
            <xsl:value-of select="$formminwidth" />, <xsl:text/>
            <xsl:value-of select="$formminheight" />) 
   Protected mLabelWidth As Int32 = <xsl:value-of select="$labelwidth"/>
   Protected mHorizontalMargin As Int32 = <xsl:value-of select="$hmargin"/>
   Protected mIdealHeight As Int32 = <xsl:value-of 
         select="(count($properties) * ($vmargin + $height)) + (count($childcollections) * ($grpheight + $vmargin))" />
   Protected mIdealWidth As Int32 = -1
   Protected mVerticalMargin As Int32 = <xsl:value-of select="$vmargin"/>
   Protected mSampleControl As System.Windows.Forms.Control 
#End Region
</xsl:template>


<xsl:template name="WindowsFormDesigner">
#Region " Windows Form Designer generated code "

	Public Sub New()
		MyBase.New()

		'This call is required by the Windows Form Designer.
		InitializeComponent()

		'Add any initialization after the InitializeComponent() call
		mCaption = "<xsl:value-of select="@Caption" />"
		<xsl:if test="count($properties)>0">
		mSampleControl = <xsl:value-of select="$properties[1]/@ControlName" />
		</xsl:if>
		<xsl:if test="count($childcollections) > 0 ">
		   <xsl:for-each select="$childcollections[count($childcollections)]">
		grp<xsl:value-of select="@Name"/>.Height = Me.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>
		   </xsl:for-each>
		</xsl:if>

	End Sub

	'Form overrides dispose to clean up the component list.
	Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If Not (components Is Nothing) Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	
	<xsl:for-each select="$properties">
	Friend WithEvents lbl<xsl:value-of select="@Name"/> 
	         <xsl:text/> As System.Windows.Forms.Label
	Friend WithEvents <xsl:value-of select="@ControlName"/> 
	         <xsl:text/> As <xsl:value-of select="@ControlType"/>
	</xsl:for-each>
	<xsl:for-each select="$childcollections">
	Friend WithEvents pnl<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.Panel
	Friend WithEvents grp<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.GroupBox
	Friend WithEvents btnAdd<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.Button
	Friend WithEvents btnRemove<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.Button
	Friend WithEvents btnEdit<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.Button
	Friend WithEvents dg<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.DataGrid
	<xsl:if test="position() > 1">
	Friend WithEvents split<xsl:value-of select="@Name"/>  
	         <xsl:text/> As System.Windows.Forms.Splitter
	</xsl:if>
	</xsl:for-each>
	Friend WithEvents lstRules As System.Windows.Forms.ListBox
	Friend WithEvents chkIsDirty As System.Windows.Forms.CheckBox
	Friend WithEvents pnlDetail As System.Windows.Forms.Panel
	Friend WithEvents pnlAll As System.Windows.Forms.Panel
	Friend WithEvents pnlBottomShim As System.Windows.Forms.Panel
	Friend WithEvents lblForceScroll As System.Windows.Forms.Label
	
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	&lt;System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		<xsl:for-each select="$properties">
		Me.lbl<xsl:value-of select="@Name"/> = New System.Windows.Forms.Label
		Me.<xsl:value-of select="@ControlName"/> = New <xsl:text/>
		      <xsl:value-of select="@ControlType"/>
		</xsl:for-each> 
		<xsl:for-each select="$childcollections">
		Me.grp<xsl:value-of select="@Name"/> = New System.Windows.Forms.GroupBox
		Me.pnl<xsl:value-of select="@Name"/> = New System.Windows.Forms.Panel
		Me.dg<xsl:value-of select="@Name"/> = New System.Windows.Forms.DataGrid
		Me.btnAdd<xsl:value-of select="@Name"/> = New System.Windows.Forms.Button
		Me.btnRemove<xsl:value-of select="@Name"/> = New System.Windows.Forms.Button
		Me.btnEdit<xsl:value-of select="@Name"/> = New System.Windows.Forms.Button
	   <xsl:if test="position() > 1">
		Me.split<xsl:value-of select="@Name"/> = New System.Windows.Forms.Splitter
		</xsl:if> 
		Me.grp<xsl:value-of select="@Name"/>.SuspendLayout()
		</xsl:for-each>
		Me.lstRules = New System.Windows.Forms.ListBox
		Me.chkIsDirty = New System.Windows.Forms.CheckBox
	   Me.pnlAll = New System.Windows.Forms.Panel
	   Me.pnlBottomShim = New System.Windows.Forms.Panel
	   Me.pnlDetail = New System.Windows.Forms.Panel
	   Me.lblForceScroll = New System.Windows.Forms.Label
		Me.SuspendLayout()
		'
		' pnlAll
		'
		Me.pnlAll.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlAll.AutoScroll = True
      Me.pnlAll.Controls.Add(lblForceScroll)
      Me.lblForceScroll.Width = 100 + 120 + Me.errProvider.Icon.Width + 16
		'
		' pnlDetail
		'
		'Me.pnlDetail.Anchor = CType( _
		'            System.Windows.Forms.AnchorStyles.Top Or _
		'            System.Windows.Forms.AnchorStyles.Left Or _
		'			   System.Windows.Forms.AnchorStyles.Right, _
      '         System.Windows.Forms.AnchorStyles)
      Me.pnlDetail.Dock = System.Windows.Forms.DockStyle.Top
      Me.pnlDetail.Size = New System.Drawing.Size(<xsl:value-of select="$ctlwidth + $labelwidth + 2 * $hmargin"/> + errProvider.Icon.Width , <xsl:value-of select="$lasttop"/>)

      '
      ' pnlBottomShim
      '
      Me.pnlBottomShim.Dock = System.Windows.Forms.DockStyle.Top
      me.pnlBottomShim.Height = <xsl:value-of select="$vmargin"/>

		<xsl:for-each select="$properties">
		<xsl:variable name="fudge">
		   <xsl:choose>
		      <xsl:when test="position()=1">3</xsl:when>
		      <xsl:otherwise>0</xsl:otherwise>
		   </xsl:choose>
		</xsl:variable>
		<xsl:variable name="top" select="$vmargin+(position()-1)*($height+$vmargin) - $fudge"/>
		<xsl:variable name="tabindex" select="(position()-1)*2"/>
		'
		'lbl<xsl:value-of select="@Name"/>
		'
		Me.lbl<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$hmargin"/>, <xsl:text/>
		         <xsl:value-of select="$top"/>)
		Me.lbl<xsl:value-of select="@Name"/>.Name = "lbl<xsl:text/>
		         <xsl:value-of select="@Name"/>"
		Me.lbl<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$labelwidth"/>, <xsl:text/>
		         <xsl:value-of select="$height"/>)
		Me.lbl<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$tabindex"/>
		Me.lbl<xsl:value-of select="@Name"/>.Text = <xsl:text/>
		         <xsl:text/>"<xsl:value-of select="@Caption"/>"
		'
		'<xsl:value-of select="@ControlName"/>
		'
		Me.<xsl:value-of select="@ControlName"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType(((System.Windows.Forms.AnchorStyles<xsl:text/>
		         <xsl:text/>.Top Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Left) _
					<xsl:text/>Or System.Windows.Forms.AnchorStyles.Right),<xsl:text/>
		         <xsl:text/> System.Windows.Forms.AnchorStyles)
		Me.<xsl:value-of select="@ControlName"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$ctlleft"/>,<xsl:text/>
		         <xsl:value-of select="$top - 1"/>)
		Me.<xsl:value-of select="@ControlName"/>.Name = <xsl:text/>
		         <xsl:text/>"<xsl:value-of select="@ControlName"/>"
		Me.<xsl:value-of select="@ControlName"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$ctlwidth"/>, <xsl:text/>
		         <xsl:value-of select="$height"/>)
		<xsl:if test="@ControlType='System.Windows.Forms.ComboBox'">
		Me.<xsl:value-of select="@ControlName"/>.DropDownStyle = ComboBoxStyle.DropDownList 
		</xsl:if>
		<!--Me.<xsl:value-of select="@ControlName"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size( _<xsl:text/>
		         pnlDetail.Width - <xsl:value-of select="$labelwidth + 2 * $hmargin"/> - errProvider.Icon.Width, <xsl:text/>
		         <xsl:value-of select="$height"/>)-->
		Me.<xsl:value-of select="@ControlName"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$tabindex + 1"/>
		Me.<xsl:value-of select="@ControlName"/>.Text = ""
		<xsl:choose>
		   <xsl:when test="$showprimarykeys='true'">
		      <xsl:if test="@ReadOnly='true'">
		Me.<xsl:value-of select="@ControlName"/>.ReadOnly = True
		      </xsl:if>
		   </xsl:when>
		   <xsl:otherwise>
		      <xsl:if test="@IsPrimaryKey='true' and not(@IsLookup='true')">
		Me.<xsl:value-of select="@ControlName"/>.Visible = False
		      </xsl:if>
		   </xsl:otherwise>
		</xsl:choose>
		'Me.txtDescription.Multiline = True
		'Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical

		Me.pnlDetail.Controls.Add(Me.<xsl:value-of select="@ControlName"/>)
		Me.pnlDetail.Controls.Add(Me.lbl<xsl:value-of select="@Name"/>)
		</xsl:for-each>
		<xsl:variable name="lasttabindex" select="(count($properties))* 2"/>
		<xsl:variable name="grpctlcount" select="6"/>
		<xsl:for-each select="$childcollections">
		' <xsl:value-of select="$grpheight" />
		'
		'grp<xsl:value-of select="@Name"/>
		'
		Me.pnl<xsl:value-of select="@Name"/>.Dock = System.Windows.Forms.DockStyle.<xsl:text/>
		         <xsl:choose>
		            <xsl:when test="position() != last()">Top</xsl:when>
		            <xsl:otherwise>Fill</xsl:otherwise>
		         </xsl:choose>
		Me.pnl<xsl:value-of select="@Name"/>.Height = <xsl:value-of select="$grpheight"/>
		Me.pnl<xsl:value-of select="@Name"/>.Controls.Add(Me.grp<xsl:value-of select="@Name"/>)
		Me.grp<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType(<xsl:text/>
               <xsl:text/>System.Windows.Forms.AnchorStyles.Bottom Or _
		         System.Windows.Forms.AnchorStyles.Top Or _
		         System.Windows.Forms.AnchorStyles.Left Or _
		   		System.Windows.Forms.AnchorStyles.Right, <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		         <xsl:text/>Add(Me.dg<xsl:value-of select="@Name"/>)
		Me.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		         <xsl:text/>Add(Me.btnAdd<xsl:value-of select="@Name"/>)
		Me.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		         <xsl:text/>Add(Me.btnRemove<xsl:value-of select="@Name"/>)
		Me.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		         <xsl:text/>Add(Me.btnEdit<xsl:value-of select="@Name"/>)
		Me.grp<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$hmargin"/>, <xsl:text/>
		         <xsl:value-of select="$vmargin"/>)
		Me.grp<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		         <xsl:text/>"grp<xsl:value-of select="@Name"/>"
		Me.grp<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$childwidth"/>, <xsl:text/>
		         <xsl:value-of select="$grpheight"/>) 
		<!--Me.grp<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:text/>pnlAll.Width, <xsl:text/>
		         <xsl:value-of select="$grpheight"/>)-->
		Me.grp<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+(position()-1)*$grpctlcount"/>
		Me.grp<xsl:value-of select="@Name"/>.TabStop = <xsl:text/>
		         <xsl:text/>False
		Me.grp<xsl:value-of select="@Name"/>.Text = <xsl:text/>
		         <xsl:text/>"<xsl:value-of select="@Caption"/>"
   	<xsl:if test="position() > 1">
      Me.pnlAll.Controls.Add(Me.split<xsl:value-of select="@Name"/>)
      Me.split<xsl:value-of select="@Name"/>.BringToFront
      </xsl:if> 
		Me.pnlAll.Controls.Add(Me.pnl<xsl:value-of select="@Name"/>)
      Me.pnl<xsl:value-of select="@Name"/>.BringToFront
		
		'
		'dg<xsl:value-of select="@Name"/>
		'
		Me.dg<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType((((System.Windows.Forms.AnchorStyles.<xsl:text/>
		         <xsl:text/>Top Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Bottom) _
						Or System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.dg<xsl:value-of select="@Name"/>.CaptionVisible = False
		Me.dg<xsl:value-of select="@Name"/>.DataSource = Nothing
		Me.dg<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$hmargin"/>, <xsl:text/>
		         <xsl:value-of select="$grptopmargin"/>)
		Me.dg<xsl:value-of select="@Name"/>.Name = "dg<xsl:value-of select="@Name"/>"
		Me.dg<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of 
		               select="$childwidth - $btnwidth - 3 * $hmargin"/>, <xsl:text/>
		         <xsl:value-of select="$grpheight - $grptopmargin - 2*$vmargin"/>) 
		<!--Me.dg<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
               <xsl:text/>me. grp<xsl:value-of select="@Name"/>.Width - <xsl:text/>
               <xsl:value-of select="$btnwidth + 2 * $hmargin"/>, <xsl:text/>
		         <xsl:value-of select="$grpheight - $grptopmargin - 2*$vmargin"/>)-->
		Me.dg<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+1+(position()-1)*$grpctlcount"/>
		<xsl:variable name="childbtnleft" 
		               select="$childwidth - $btnwidth - 1 * $hmargin" />
		'
		'btnAdd<xsl:value-of select="@Name"/>
		'
		Me.btnAdd<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType((System.Windows.Forms.AnchorStyles.Top<xsl:text/>
		         <xsl:text/> Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Right), <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.btnAdd<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		         <xsl:value-of select="$grptopmargin"/>)
		         '<xsl:value-of select="$vmargin"/>)
		Me.btnAdd<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		         <xsl:value-of select="$btnheight"/>)
		Me.btnAdd<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		         <xsl:text/>"btnAdd<xsl:value-of select="@Name"/>"
		Me.btnAdd<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+2+(position()-1)*$grpctlcount"/>
		Me.btnAdd<xsl:value-of select="@Name"/>.Text = "&amp;Add"
		'
		'btnRemove<xsl:value-of select="@Name"/>
		'
		Me.btnRemove<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType((System.Windows.Forms.AnchorStyles.<xsl:text/>
		         <xsl:text/>Top Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Right), <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.btnRemove<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		         <xsl:value-of select="$grptopmargin + 1 * $btnheight"/>)
		         '<xsl:value-of select="$grptopmargin + 1 * ($btnheight + $vmargin)"/>)
		Me.btnRemove<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		         <xsl:value-of select="$btnheight"/>)
		Me.btnRemove<xsl:value-of select="@Name"/>.Name = "btnRemove<xsl:value-of select="@Name"/>"
		Me.btnRemove<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+3+(position()-1)*$grpctlcount"/>
		Me.btnRemove<xsl:value-of select="@Name"/>.Text = "&amp;Remove"
		'
		'btnEdit<xsl:value-of select="@Name"/>
		'
		Me.btnEdit<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		         <xsl:text/>CType((System.Windows.Forms.AnchorStyles.<xsl:text/>
		         <xsl:text/>Top Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Right), <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.btnEdit<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		         <xsl:text/>New System.Drawing.Point(<xsl:text/>
		         <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		         <xsl:value-of select="$grptopmargin + 2 * $btnheight"/>)
		         '<xsl:value-of select="$grptopmargin + 2 * ($btnheight + $vmargin)"/>)
		Me.btnEdit<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		         <xsl:text/>New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		         <xsl:value-of select="$btnheight"/>)
		Me.btnEdit<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		         <xsl:text/>"btnEdit<xsl:value-of select="@Name"/>"
		Me.btnEdit<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+4+(position()-1)*$grpctlcount"/>
		Me.btnEdit<xsl:value-of select="@Name"/>.Text = "&amp;Edit"
   	<xsl:if test="position() > 1">
		'
		'split<xsl:value-of select="@Name"/>
		'
      Me.split<xsl:value-of select="@Name"/>.Name = "split<xsl:value-of select="@Name"/>"
      Me.split<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		         <xsl:value-of select="$lasttabindex+4+(position()-1)*$grpctlcount"/>
      Me.split<xsl:value-of select="@Name"/>.TabStop = False
      Me.split<xsl:value-of select="@Name"/>.Dock = DockStyle.Top
      </xsl:if> 
		</xsl:for-each> 
		<xsl:variable name="lasttabindex2" 
		         select="$lasttabindex+4+(count($childcollections)-1)*$grpctlcount"/>
		<xsl:variable name="lasttop2" 
		         select="$lasttop+6+(count($childcollections)*8*6)"/>
		'
		'lstRules
		'
		Me.lstRules.Anchor = CType(((System.Windows.Forms.AnchorStyles.<xsl:text/>
		         <xsl:text/>Top Or <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles.Left) _
						Or System.Windows.Forms.AnchorStyles.Right), <xsl:text/>
		         <xsl:text/>System.Windows.Forms.AnchorStyles)
		Me.lstRules.Location = New System.Drawing.Point(544, 104)
		Me.lstRules.Name = "lstRules"
		Me.lstRules.Size = New System.Drawing.Size(192, 108)
		Me.lstRules.TabIndex = <xsl:value-of select="$lasttabindex2+1"/>
		Me.lstRules.Visible=False
		'
		'chkIsDirty
		'
		Me.chkIsDirty.Enabled = False
		Me.chkIsDirty.Location = New System.Drawing.Point(488, 8)
		Me.chkIsDirty.Name = "chkIsDirty"
		Me.chkIsDirty.Size = New System.Drawing.Size(80, 0)
		Me.chkIsDirty.TabIndex = <xsl:value-of select="$lasttabindex2+2"/>
		Me.chkIsDirty.Text = "IsDirty"
		'
		' <xsl:value-of select="@Name"/>Edit
		'
		<xsl:variable name="grplasttop">
		   <xsl:value-of select="$lasttop+(count($childcollections)*$grpheight)"/>
		</xsl:variable> 
		<xsl:variable name="newformheight">
		   <xsl:choose>
		      <xsl:when test="count($childcollections) > 0">
		         <xsl:value-of select="$grplasttop + 20*$vmargin"/>
		      </xsl:when>
		      <xsl:otherwise>
		         <xsl:value-of select="$lasttop"/>
		      </xsl:otherwise>
		   </xsl:choose>
		</xsl:variable>
		' <xsl:value-of select="$lasttop"/>
		' <xsl:value-of select="$grplasttop"/>
		' <xsl:value-of select="count($childcollections)"/>
		Me.pnlAll.Controls.Add(Me.pnlDetail)
		Me.pnlAll.Controls.Add(Me.pnlBottomShim)
		Me.ClientSize = New System.Drawing.Size(<xsl:text/>
		         <xsl:value-of select="$formwidth"/>, <xsl:text/>
		         <xsl:value-of select="$newformheight"/> )
		Me.Controls.Add(Me.chkIsDirty)
		Me.Controls.Add(Me.lstRules)
		Me.Controls.Add(Me.pnlAll)
		<!--<xsl:for-each select="$childcollections">
		Me.Controls.Add(Me.grp<xsl:value-of select="@Name"/>)
      'Me.Controls.Add(Me.split<xsl:value-of select="@Name"/>)
		</xsl:for-each> 
		<xsl:for-each select="$properties">
		Me.Controls.Add(<xsl:text/>
		         <xsl:text/>Me.<xsl:value-of select="@ControlName"/>)<xsl:text/>
		</xsl:for-each> 
		<xsl:for-each select="$properties">
		Me.Controls.Add(<xsl:text/>
		         <xsl:text/>Me.lbl<xsl:value-of select="@Name"/>)<xsl:text/>
		</xsl:for-each> -->
		Me.Name = "gen<xsl:value-of select="@Name"/>Edit"
		Me.Text = "<xsl:value-of select="@Name"/> Edit"
		<xsl:for-each select="$childcollections">
		Me.grp<xsl:value-of select="@Name"/>.ResumeLayout(False)
		</xsl:for-each> 
		Me.ResumeLayout(False)

	End Sub

#End Region
</xsl:template>


<xsl:template name="EventHandlers" >
#Region " Event Handlers "
   Private Sub ctl_Validated( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles <xsl:text/>
        <xsl:for-each select="$properties" >
            <xsl:value-of select="@ControlName"/>.Validated<xsl:if test="position()!=last()">, _
                     </xsl:if>
        </xsl:for-each>
      OnCtlValidated(sender, e)
   End Sub
	Protected Overridable Sub OnCtlValidated( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
   End Sub

   Private Sub ctl_Changed( _
                     ByVal sender As System.Object, _
                     ByVal e As System.EventArgs) _
                     Handles <xsl:text/>
        <xsl:for-each select="$properties" >
            <xsl:value-of select="concat(@ControlName,'.',@ControlChangedEvent)"/>
            <xsl:if test="position() != last()">, _
                             </xsl:if>
        </xsl:for-each>
      OnDataChanged(sender, e)
   End Sub
	Protected Overrides Sub OnDataChanged( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      mbIsDirty = True
	   MyBase.OnDataChanged(sender, e)
   End Sub

	<xsl:for-each select="$childcollections">
	   <xsl:variable name="childname" select="@Name" />
	   <xsl:variable name="childobject" 
	            select="//orm:Object[@CollectionName=$childname]/@Name" />
	   <xsl:variable name="childnamespace" 
	            select="//orm:Object[@CollectionName=$childname]/@Namespace" />
	Private Sub btnAdd<xsl:value-of select="@Name"/>_Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs) _ 
	                  Handles btnAdd<xsl:value-of select="@Name"/>.Click
      OnBtnAdd<xsl:value-of select="@Name"/>Click(sender, e)
	End Sub
	Protected Overridable Sub OnBtnAdd<xsl:value-of select="@Name"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
   End Sub

	Private Sub btnRemove<xsl:value-of select="@Name"/>_Click( _ 
	                   ByVal sender As System.Object,	_
	                   ByVal e As System.EventArgs) _ 
	                   Handles btnRemove<xsl:value-of select="@Name"/>.Click
	   OnBtnRemove<xsl:value-of select="@Name"/>Click(sender, e)
	End Sub
	Protected Overridable Sub OnBtnRemove<xsl:value-of select="@Name"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
   End Sub

	Private Sub btnEdit<xsl:value-of select="@Name"/>_Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs) _ 
	                  Handles btnEdit<xsl:value-of select="@Name"/>.Click
	   OnBtnEdit<xsl:value-of select="@Name"/>Click(sender, e)
	End Sub
	Protected Overridable Sub OnBtnEdit<xsl:value-of select="@Name"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
   End Sub

	Private Sub dg<xsl:value-of select="@Name"/>_DoubleClick( _ 
	                  ByVal sender As Object, _ 
	                  ByVal e As System.EventArgs) _ 
	                  Handles dg<xsl:value-of select="@Name"/>.DoubleClick
	   OnDataGrid<xsl:value-of select="@Name"/>DoubleClick(sender, e)
	End Sub
	Protected Overridable Sub OnDataGrid<xsl:value-of select="@Name"/><xsl:text/>
	            <xsl:text/>DoubleClick( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
   End Sub
  </xsl:for-each>

   Protected Overrides Sub OnLayout( _
                  ByVal e As System.Windows.Forms.LayoutEventArgs)
      Dim width As Int32
      Dim height As Int32
      MyBase.OnLayout(e)

      'Me.lblForceScroll.Height = 1

      <xsl:for-each select="$childcollections">
      width = grp<xsl:value-of select="@Name"/>.Width - btnAdd<xsl:value-of select="@Name"/>.Width - <xsl:value-of select="3*$hmargin"/>
      If grp<xsl:value-of select="@Name"/>.ClientSize.Height - btnEdit<xsl:value-of select="@Name"/>.Bottom &lt; <xsl:value-of select="2*$vmargin"/> Then
         height = btnEdit<xsl:value-of select="@Name"/>.Bottom - dg<xsl:value-of select="@Name"/>.Top
      Else
         height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>
      End If 
      'height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>
		dg<xsl:value-of select="@Name"/>.Size = New Drawing.Size(width, height)
		'dg<xsl:value-of select="@Name"/>.Width = width
		btnAdd<xsl:value-of select="@Name"/>.Left = width + <xsl:value-of select="1.5*$hmargin"/>
		btnRemove<xsl:value-of select="@Name"/>.Left = btnAdd<xsl:value-of select="@Name"/>.Left
		btnEdit<xsl:value-of select="@Name"/>.Left = btnAdd<xsl:value-of select="@Name"/>.Left
		grp<xsl:value-of select="@Name"/>.Size = New Drawing.Size( _
		            pnl<xsl:value-of select="@Name"/>.ClientSize.Width - 2*<xsl:value-of select="$hmargin"/>, _
		            pnl<xsl:value-of select="@Name"/>.ClientSize.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="2*$vmargin"/>)
      </xsl:for-each>
   End Sub
   
   Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
      MyBase.OnLoad(e)
      'pnlDetail.Width = Me.pnlAll.Width
      'Me.lblForceScroll.SendToBack
      Me.pnlBottomShim.SendToBack
      Me.lblForceScroll.Height = 1
      'Me.lblForceScroll.Visible = False
   End Sub
#End Region
</xsl:template>

<xsl:template name="RestOfForm">
#Region "Rest of form"
	Protected Overrides Sub ResizeUC()
	   MyBase.ResizeUC()

		<xsl:if test="count($childcollections) > 0 ">
		   <xsl:for-each select="$childcollections[count($childcollections)]">
		grp<xsl:value-of select="@Name"/>.Height = Me.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>
		dg<xsl:value-of select="@Name"/>.Height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="2 * $vmargin" />
		   </xsl:for-each>
		</xsl:if>
      'Me.pnlDetail.Size = New System.Drawing.Size(Me.ClientSize.Width, <xsl:value-of select="$lasttop"/>)

	End Sub 
	
#End Region 
</xsl:template>

</xsl:stylesheet> 
 