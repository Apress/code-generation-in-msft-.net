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

<xsl:variable name="baseformname" select="'KADGen.WinSupport.BaseEditUserControl'" />
<xsl:variable name="formname" select="$Name"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 
<xsl:variable name="winnamespace" select="'KADGen.WinProject'" />

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
using System;
using System.Windows.Forms;
using System.Threading;
using KADGen.BusinessObjectSupport;

namespace <xsl:value-of select="$winnamespace"/>
{
   public class layout<xsl:value-of select="$formname"/> : <xsl:value-of select="$baseformname"/>
   {
   <xsl:call-template name="ClassDeclarations"/>
   <xsl:call-template name="WindowsFormDesigner"/>
   <xsl:call-template name="EventHandlers"/>
   <xsl:call-template name="RestOfForm"/>
   }
}
   </xsl:template>

   <xsl:template name="ClassDeclarations">
   #region  class Declarations 
      protected System.Drawing.Size mMinimumSize = new System.Drawing.Size(<xsl:text/>
               <xsl:value-of select="$formminwidth" />, <xsl:text/>
               <xsl:value-of select="$formminheight" />);
      protected int mLabelWidth = <xsl:value-of select="$labelwidth"/>;
      protected int mHorizontalMargin = <xsl:value-of select="$hmargin"/>;
      protected int mIdealHeight = <xsl:value-of 
            select="(count($properties) * ($vmargin + $height)) + (count($childcollections) * ($grpheight + $vmargin))" />;
      protected int mIdealWidth = -1;
      protected int mVerticalMargin = <xsl:value-of select="$vmargin"/>;
      protected System.Windows.Forms.Control mSampleControl ; 
   #endregion
   </xsl:template>


   <xsl:template name="WindowsFormDesigner">
   #region System.Windows Form Designer generated code 

	   public layout<xsl:value-of select="$formname"/>() : base()
      {
		   //This call is required by the System.Windows Form Designer.
		   InitializeComponent();

		   //Add any initialization after the InitializeComponent() call
		   mCaption = "<xsl:value-of select="@Caption" />";
		   <xsl:if test="count($properties)>0">
		   mSampleControl = <xsl:value-of select="$properties[1]/@ControlName" />;
		   </xsl:if>
		   <xsl:if test="count($childcollections) > 0 ">
		      <xsl:for-each select="$childcollections[count($childcollections)]">
		   grp<xsl:value-of select="@Name"/>.Height = this.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>;
		      </xsl:for-each>
		   </xsl:if>

	   }

	   //Form overrides dispose to clean up the component list.
	   protected override void Dispose(bool disposing )
	   {
		   if( disposing )
		   {
			   if( components != null )
			   {
				   components.Dispose();
			   }
		   }
		   base.Dispose(disposing);
	   }

	   //Required by the System.Windows Form Designer
	   private System.ComponentModel.IContainer components;
   	
	   <xsl:for-each select="$properties">
	   internal System.Windows.Forms.Label lbl<xsl:value-of select="@Name"/>;
	   internal <xsl:value-of select="@ControlType"/><xsl:text> </xsl:text><xsl:value-of select="@ControlName"/>; 
	   </xsl:for-each>
	   <xsl:for-each select="$childcollections">
	   internal System.Windows.Forms.Panel pnl<xsl:value-of select="@Name"/>;  
	   internal System.Windows.Forms.GroupBox grp<xsl:value-of select="@Name"/>; 
	   internal System.Windows.Forms.Button btnAdd<xsl:value-of select="@Name"/>;  
	   internal System.Windows.Forms.Button btnRemove<xsl:value-of select="@Name"/>;  
	   internal System.Windows.Forms.Button btnEdit<xsl:value-of select="@Name"/>;  
	   internal System.Windows.Forms.DataGrid dg<xsl:value-of select="@Name"/>;  
	   <xsl:if test="position() > 1">
	   internal System.Windows.Forms.Splitter split<xsl:value-of select="@Name"/>;  
	   </xsl:if>
	   </xsl:for-each>
	   internal System.Windows.Forms.ListBox lstRules;
	   internal System.Windows.Forms.CheckBox chkIsDirty;
	   internal System.Windows.Forms.Panel pnlDetail; 
	   internal System.Windows.Forms.Panel pnlAll;
	   internal System.Windows.Forms.Panel pnlBottomShim;
	   internal System.Windows.Forms.Label lblForceScroll;
   	
	   //NOTE: The following procedure is required by the System.Windows Form Designer
	   //It can be modified using the System.Windows Form Designer.  
	   //Do not modify it using the code editor.
	   [System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
	   {
		   <xsl:for-each select="$properties">
		   this.lbl<xsl:value-of select="@Name"/> = new System.Windows.Forms.Label();
		   this.<xsl:value-of select="@ControlName"/> = new <xsl:value-of select="@ControlType"/>();
		   </xsl:for-each> 
		   <xsl:for-each select="$childcollections">
		   this.grp<xsl:value-of select="@Name"/> = new System.Windows.Forms.GroupBox();
		   this.pnl<xsl:value-of select="@Name"/> = new System.Windows.Forms.Panel();
		   this.dg<xsl:value-of select="@Name"/> = new System.Windows.Forms.DataGrid();
		   this.btnAdd<xsl:value-of select="@Name"/> = new System.Windows.Forms.Button();
		   this.btnRemove<xsl:value-of select="@Name"/> = new System.Windows.Forms.Button();
		   this.btnEdit<xsl:value-of select="@Name"/> = new System.Windows.Forms.Button();
	      <xsl:if test="position() > 1">
		   this.split<xsl:value-of select="@Name"/> = new System.Windows.Forms.Splitter();
		   </xsl:if> 
		   this.grp<xsl:value-of select="@Name"/>.SuspendLayout();
		   </xsl:for-each>
		   this.lstRules = new System.Windows.Forms.ListBox();
		   this.chkIsDirty = new System.Windows.Forms.CheckBox();
	      this.pnlAll = new System.Windows.Forms.Panel();
	      this.pnlBottomShim = new System.Windows.Forms.Panel();
	      this.pnlDetail = new System.Windows.Forms.Panel();
	      this.lblForceScroll = new System.Windows.Forms.Label();
		   this.SuspendLayout();
		   //
		   // pnlAll
		   //
		   this.pnlAll.Dock = System.Windows.Forms.DockStyle.Fill;
		   this.pnlAll.AutoScroll = true;
         this.pnlAll.Controls.Add(lblForceScroll);
         this.lblForceScroll.Width = 100 + 120 + this.errProvider.Icon.Width + 16;
		   //
		   // pnlDetail
		   //
		   //this.pnlDetail.Anchor = ( System.Windows.Forms.AnchorStyles )_
		   //            ( System.Windows.Forms.AnchorStyles.Top |
		   //            System.Windows.Forms.AnchorStyles.Left |
		   //			   System.Windows.Forms.AnchorStyles.Right ),
         //         
         this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Top;
         this.pnlDetail.Size = new System.Drawing.Size(<xsl:value-of select="$ctlwidth + $labelwidth + 2 * $hmargin"/> + errProvider.Icon.Width , <xsl:value-of select="$lasttop"/>);

         //
         // pnlBottomShim
         //
         this.pnlBottomShim.Dock = System.Windows.Forms.DockStyle.Top;
         this.pnlBottomShim.Height = <xsl:value-of select="$vmargin"/>;

		   <xsl:for-each select="$properties">
		   <xsl:variable name="fudge">
		      <xsl:choose>
		         <xsl:when test="position()=1">3</xsl:when>
		         <xsl:otherwise>0</xsl:otherwise>
		      </xsl:choose>
		   </xsl:variable>
		   <xsl:variable name="top" select="$vmargin+(position()-1)*($height+$vmargin) - $fudge"/>
		   <xsl:variable name="tabindex" select="(position()-1)*2"/>
		   //
		   //lbl<xsl:value-of select="@Name"/>
		   //
		   this.lbl<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$hmargin"/>, <xsl:text/>
		            <xsl:value-of select="$top"/>);
		   this.lbl<xsl:value-of select="@Name"/>.Name = "lbl<xsl:text/>
		            <xsl:value-of select="@Name"/>";
		   this.lbl<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$labelwidth"/>, <xsl:text/>
		            <xsl:value-of select="$height"/>);
		   this.lbl<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$tabindex"/>;
		   this.lbl<xsl:value-of select="@Name"/>.Text = <xsl:text/>
		            <xsl:text/>"<xsl:value-of select="@Caption"/>";
		   //
		   //<xsl:value-of select="@ControlName"/>
		   //
		   this.<xsl:value-of select="@ControlName"/>.Anchor = <xsl:text/>
		            <xsl:text/>( System.Windows.Forms.AnchorStyles ) 
		                  ( System.Windows.Forms.AnchorStyles.Top | 
		                     System.Windows.Forms.AnchorStyles.Left |
					            System.Windows.Forms.AnchorStyles.Right );
		   this.<xsl:value-of select="@ControlName"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$ctlleft"/>,<xsl:text/>
		            <xsl:value-of select="$top - 1"/>);
		   this.<xsl:value-of select="@ControlName"/>.Name = <xsl:text/>
		            <xsl:text/>"<xsl:value-of select="@ControlName"/>";
		   this.<xsl:value-of select="@ControlName"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$ctlwidth"/>, <xsl:text/>
		            <xsl:value-of select="$height"/>);
		   <xsl:if test="@ControlType='System.Windows.Forms.ComboBox'">
		   this.<xsl:value-of select="@ControlName"/>.DropDownStyle = ComboBoxStyle.DropDownList;
		   </xsl:if>
		   <!--this.<xsl:value-of select="@ControlName"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            pnlDetail.Width - <xsl:value-of select="$labelwidth + 2 * $hmargin"/> - errProvider.Icon.Width, <xsl:text/>
		            <xsl:value-of select="$height"/>)-->
		   this.<xsl:value-of select="@ControlName"/>.TabIndex = <xsl:value-of select="$tabindex + 1"/>;
		   this.<xsl:value-of select="@ControlName"/>.Text = "";
		   <xsl:choose>
		      <xsl:when test="$showprimarykeys='true'">
		         <xsl:if test="@ReadOnly='true'">
		   this.<xsl:value-of select="@ControlName"/>.ReadOnly = true;
		         </xsl:if>
		      </xsl:when>
		      <xsl:otherwise>
		         <xsl:if test="@IsPrimaryKey='true' and not(@IsLookup='true')">
		   this.<xsl:value-of select="@ControlName"/>.Visible = false;
		         </xsl:if>
		      </xsl:otherwise>
		   </xsl:choose>
		   this.<xsl:value-of select="@ControlName"/>.Validated += new System.EventHandler(this.ctl_Validated);
		   this.<xsl:value-of select="@ControlName"/>.<xsl:value-of select="@ControlChangedEvent"/> += new System.EventHandler(this.ctl_Changed);
		   //this.txtDescription.Multiline = true;
		   //this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

		   this.pnlDetail.Controls.Add(this.<xsl:value-of select="@ControlName"/>);
		   this.pnlDetail.Controls.Add(this.lbl<xsl:value-of select="@Name"/>);
		   </xsl:for-each>
		   <xsl:variable name="lasttabindex" select="(count($properties))* 2"/>
		   <xsl:variable name="grpctlcount" select="6"/>
		   <xsl:for-each select="$childcollections">
		   // <xsl:value-of select="$grpheight" />
		   //
		   //grp<xsl:value-of select="@Name"/>
		   //
		   this.pnl<xsl:value-of select="@Name"/>.Dock = System.Windows.Forms.DockStyle.<xsl:text/>
		            <xsl:choose>
		               <xsl:when test="position() != last()">Top</xsl:when>
		               <xsl:otherwise>Fill</xsl:otherwise>
		            </xsl:choose>;
		   this.pnl<xsl:value-of select="@Name"/>.Height = <xsl:value-of select="$grpheight"/>;
		   this.pnl<xsl:value-of select="@Name"/>.Controls.Add(this.grp<xsl:value-of select="@Name"/>);
		   this.grp<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		            <xsl:text/>( System.Windows.Forms.AnchorStyles )<xsl:text/>
                  <xsl:text/>( System.Windows.Forms.AnchorStyles.Bottom |
		            System.Windows.Forms.AnchorStyles.Top |
		            System.Windows.Forms.AnchorStyles.Left |
		   		   System.Windows.Forms.AnchorStyles.Right );
		   this.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		            <xsl:text/>Add(this.dg<xsl:value-of select="@Name"/>);
		   this.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		            <xsl:text/>Add(this.btnAdd<xsl:value-of select="@Name"/>);
		   this.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		            <xsl:text/>Add(this.btnRemove<xsl:value-of select="@Name"/>);
		   this.grp<xsl:value-of select="@Name"/>.Controls.<xsl:text/>
		            <xsl:text/>Add(this.btnEdit<xsl:value-of select="@Name"/>);
		   this.grp<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$hmargin"/>, <xsl:text/>
		            <xsl:value-of select="$vmargin"/>);
		   this.grp<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		            <xsl:text/>"grp<xsl:value-of select="@Name"/>";
		   this.grp<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$childwidth"/>, <xsl:text/>
		            <xsl:value-of select="$grpheight"/>);
		   <!--this.grp<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:text/>pnlAll.Width, <xsl:text/>
		            <xsl:value-of select="$grpheight"/>);-->
		   this.grp<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+(position()-1)*$grpctlcount"/>;
		   this.grp<xsl:value-of select="@Name"/>.TabStop = <xsl:text/>false;
		   this.grp<xsl:value-of select="@Name"/>.Text = <xsl:text/>
		            <xsl:text/>"<xsl:value-of select="@Caption"/>";
   	   <xsl:if test="position() > 1">
         this.pnlAll.Controls.Add(this.split<xsl:value-of select="@Name"/>);
         this.split<xsl:value-of select="@Name"/>.BringToFront();
         </xsl:if> 
		   this.pnlAll.Controls.Add(this.pnl<xsl:value-of select="@Name"/>);
         this.pnl<xsl:value-of select="@Name"/>.BringToFront();
   		
		   //
		   //dg<xsl:value-of select="@Name"/>
		   //
		   this.dg<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		            <xsl:text/>( System.Windows.Forms.AnchorStyles )
		               ( System.Windows.Forms.AnchorStyles.Top | <xsl:text/>
		               System.Windows.Forms.AnchorStyles.Bottom |
						   System.Windows.Forms.AnchorStyles.Left |
						   System.Windows.Forms.AnchorStyles.Right );
		   this.dg<xsl:value-of select="@Name"/>.CaptionVisible = false;
		   this.dg<xsl:value-of select="@Name"/>.DataSource = null;
		   this.dg<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$hmargin"/>, <xsl:text/>
		            <xsl:value-of select="$grptopmargin"/>);
		   this.dg<xsl:value-of select="@Name"/>.Name = "dg<xsl:value-of select="@Name"/>";
		   this.dg<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of 
		                  select="$childwidth - $btnwidth - 3 * $hmargin"/>, <xsl:text/>
		            <xsl:value-of select="$grpheight - $grptopmargin - 2*$vmargin"/>);
		   <!--this.dg<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
                  <xsl:text/>me. grp<xsl:value-of select="@Name"/>.Width - <xsl:text/>
                  <xsl:value-of select="$btnwidth + 2 * $hmargin"/>, <xsl:text/>
		            <xsl:value-of select="$grpheight - $grptopmargin - 2*$vmargin"/>);-->
		   this.dg<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+1+(position()-1)*$grpctlcount"/>;
		   this.dg<xsl:value-of select="@Name"/>.DoubleClick += new System.EventHandler(this.dg<xsl:value-of select="@Name"/>_DoubleClick);
		   this.dg<xsl:value-of select="@Name"/>.CurrentCellChanged += new System.EventHandler(this.dg<xsl:value-of select="@Name"/>_CurrentCellChanged);
		   <xsl:variable name="childbtnleft" 
		                  select="$childwidth - $btnwidth - 1 * $hmargin" />;
		   //
		   //btnAdd<xsl:value-of select="@Name"/>
		   //
		   this.btnAdd<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		            <xsl:text/>(System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top<xsl:text/>
		            <xsl:text/> | <xsl:text/>
		            <xsl:text/>System.Windows.Forms.AnchorStyles.Right);
		   this.btnAdd<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		            <xsl:value-of select="$grptopmargin"/>);
		            //<xsl:value-of select="$vmargin"/>);
		   this.btnAdd<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		            <xsl:value-of select="$btnheight"/>);
		   this.btnAdd<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		            <xsl:text/>"btnAdd<xsl:value-of select="@Name"/>";
		   this.btnAdd<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+2+(position()-1)*$grpctlcount"/>;
		   this.btnAdd<xsl:value-of select="@Name"/>.Text = "&amp;Add";
		   this.btnAdd<xsl:value-of select="@Name"/>.Click += new System.EventHandler(this.btnAdd<xsl:value-of select="@Name"/>_Click);
		   //
		   //btnRemove<xsl:value-of select="@Name"/>
		   //
		   this.btnRemove<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		            <xsl:text/>( System.Windows.Forms.AnchorStyles )
		               ( System.Windows.Forms.AnchorStyles.Top | <xsl:text/>
		               System.Windows.Forms.AnchorStyles.Right);
		   this.btnRemove<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		            <xsl:value-of select="$grptopmargin + 1 * $btnheight"/>);
		            //<xsl:value-of select="$grptopmargin + 1 * ($btnheight + $vmargin)"/>);
		   this.btnRemove<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		            <xsl:value-of select="$btnheight"/>);
		   this.btnRemove<xsl:value-of select="@Name"/>.Name = "btnRemove<xsl:value-of select="@Name"/>";
		   this.btnRemove<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+3+(position()-1)*$grpctlcount"/>;
		   this.btnRemove<xsl:value-of select="@Name"/>.Text = "&amp;Remove";
		   this.btnRemove<xsl:value-of select="@Name"/>.Click += new System.EventHandler(this.btnRemove<xsl:value-of select="@Name"/>_Click);
		   //
		   //btnEdit<xsl:value-of select="@Name"/>
		   //
		   this.btnEdit<xsl:value-of select="@Name"/>.Anchor = <xsl:text/>
		            <xsl:text/>( System.Windows.Forms.AnchorStyles )
		               ( System.Windows.Forms.AnchorStyles.Top | <xsl:text/>
		            <xsl:text/>System.Windows.Forms.AnchorStyles.Right);
		   this.btnEdit<xsl:value-of select="@Name"/>.Location = <xsl:text/>
		            <xsl:text/>new System.Drawing.Point(<xsl:text/>
		            <xsl:value-of select="$childbtnleft"/>, <xsl:text/>
		            <xsl:value-of select="$grptopmargin + 2 * $btnheight"/>);
		            //<xsl:value-of select="$grptopmargin + 2 * ($btnheight + $vmargin)"/>);
		   this.btnEdit<xsl:value-of select="@Name"/>.Size = <xsl:text/>
		            <xsl:text/>new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$btnwidth"/>, <xsl:text/>
		            <xsl:value-of select="$btnheight"/>);
		   this.btnEdit<xsl:value-of select="@Name"/>.Name = <xsl:text/>
		            <xsl:text/>"btnEdit<xsl:value-of select="@Name"/>";
		   this.btnEdit<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+4+(position()-1)*$grpctlcount"/>;
		   this.btnEdit<xsl:value-of select="@Name"/>.Text = "&amp;Edit";
		   this.btnEdit<xsl:value-of select="@Name"/>.Click += new System.EventHandler(this.btnEdit<xsl:value-of select="@Name"/>_Click);
   	   <xsl:if test="position() > 1">
		   //
		   //split<xsl:value-of select="@Name"/>
		   //
         this.split<xsl:value-of select="@Name"/>.Name = "split<xsl:value-of select="@Name"/>";
         this.split<xsl:value-of select="@Name"/>.TabIndex = <xsl:text/>
		            <xsl:value-of select="$lasttabindex+4+(position()-1)*$grpctlcount"/>;
         this.split<xsl:value-of select="@Name"/>.TabStop = false;
         this.split<xsl:value-of select="@Name"/>.Dock = DockStyle.Top;
         </xsl:if> 
		   </xsl:for-each> 
		   <xsl:variable name="lasttabindex2" 
		            select="$lasttabindex+4+(count($childcollections)-1)*$grpctlcount"/>
		   <xsl:variable name="lasttop2" 
		            select="$lasttop+6+(count($childcollections)*8*6)"/>
		   //
		   //lstRules
		   //
		   this.lstRules.Anchor = ( System.Windows.Forms.AnchorStyles )
		               (System.Windows.Forms.AnchorStyles.Top | 
		                  System.Windows.Forms.AnchorStyles.Left |
						      System.Windows.Forms.AnchorStyles.Right);
		   this.lstRules.Location = new System.Drawing.Point(544, 104);
		   this.lstRules.Name = "lstRules";
		   this.lstRules.Size = new System.Drawing.Size(192, 108);
		   this.lstRules.TabIndex = <xsl:value-of select="$lasttabindex2+1"/>;
		   this.lstRules.Visible=false;
		   //
		   //chkIsDirty
		   //
		   this.chkIsDirty.Enabled = false;
		   this.chkIsDirty.Location = new System.Drawing.Point(488, 8);
		   this.chkIsDirty.Name = "chkIsDirty";
		   this.chkIsDirty.Size = new System.Drawing.Size(80, 0);
		   this.chkIsDirty.TabIndex = <xsl:value-of select="$lasttabindex2+2"/>;
		   this.chkIsDirty.Text = "IsDirty";
		   //
		   // <xsl:value-of select="@Name"/>Edit
		   //
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
		   // <xsl:value-of select="$lasttop"/>
		   // <xsl:value-of select="$grplasttop"/>
		   // <xsl:value-of select="count($childcollections)"/>
		   this.pnlAll.Controls.Add(this.pnlDetail);
		   this.pnlAll.Controls.Add(this.pnlBottomShim);
		   this.ClientSize = new System.Drawing.Size(<xsl:text/>
		            <xsl:value-of select="$formwidth"/>, <xsl:text/>
		            <xsl:value-of select="$newformheight"/> );
		   this.Controls.Add(this.chkIsDirty);
		   this.Controls.Add(this.lstRules);
		   this.Controls.Add(this.pnlAll);
		   <!--<xsl:for-each select="$childcollections">
		   this.Controls.Add(this.grp<xsl:value-of select="@Name"/>);
         //this.Controls.Add(this.split<xsl:value-of select="@Name"/>);
		   </xsl:for-each> 
		   <xsl:for-each select="$properties">
		   this.Controls.Add(<xsl:text/>
		            <xsl:text/>this.<xsl:value-of select="@ControlName"/>)<xsl:text/>;
		   </xsl:for-each> 
		   <xsl:for-each select="$properties">
		   this.Controls.Add(<xsl:text/>
		            <xsl:text/>this.lbl<xsl:value-of select="@Name"/>)<xsl:text/>;
		   </xsl:for-each> -->
		   this.Name = "gen<xsl:value-of select="@Name"/>Edit";
		   this.Text = "<xsl:value-of select="@Name"/> Edit";
		   <xsl:for-each select="$childcollections">
		   this.grp<xsl:value-of select="@Name"/>.ResumeLayout(false);
		   </xsl:for-each> 
		   this.ResumeLayout(false);

	   }

   #endregion
   </xsl:template>


   <xsl:template name="EventHandlers" >
   #region Event Handlers 
      private void ctl_Validated( System.Object sender, System.EventArgs e ) 
      {
         OnCtlValidated(sender, e);
      }
	   protected virtual void OnCtlValidated( System.Object sender, System.EventArgs e ) 
	   {
	   }

      private void ctl_Changed( System.Object sender, System.EventArgs e ) 
      {
         OnDataChanged(sender, e);
      }
	   protected override void OnDataChanged( System.Object sender, System.EventArgs e ) 
	   {
         mbIsDirty = true;
	      base.OnDataChanged(sender, e);
      }

	   <xsl:for-each select="$childcollections">
	      <xsl:variable name="childname" select="@Name" />
	      <xsl:variable name="childobject" 
	               select="//orm:Object[@CollectionName=$childname]/@Name" />
	      <xsl:variable name="childnamespace" 
	               select="//orm:Object[@CollectionName=$childname]/@Namespace" />
	   private void btnAdd<xsl:value-of select="@Name"/>_Click( System.Object sender, System.EventArgs e ) 
	   {
         OnBtnAdd<xsl:value-of select="@Name"/>Click(sender, e);
	   }
	   protected virtual void OnBtnAdd<xsl:value-of select="@Name"/>Click( System.Object sender, System.EventArgs e ) 
	   {
	   }

	   private void btnRemove<xsl:value-of select="@Name"/>_Click(  System.Object sender, System.EventArgs e ) 
      {
	      OnBtnRemove<xsl:value-of select="@Name"/>Click(sender, e);
	   }
	   protected virtual void OnBtnRemove<xsl:value-of select="@Name"/>Click(  System.Object sender, System.EventArgs e ) 
	   {
	   }

	   private void btnEdit<xsl:value-of select="@Name"/>_Click(  System.Object sender, System.EventArgs e ) 
	   {
	      OnBtnEdit<xsl:value-of select="@Name"/>Click(sender, e);
	   }
	   protected virtual void OnBtnEdit<xsl:value-of select="@Name"/>Click(  System.Object sender, System.EventArgs e ) 
	   {
	   }

	   private void dg<xsl:value-of select="@Name"/>_DoubleClick(  System.Object sender, System.EventArgs e ) 
	   {
	      OnDataGrid<xsl:value-of select="@Name"/>DoubleClick(sender, e);
	   }
	   protected virtual void OnDataGrid<xsl:value-of select="@Name"/><xsl:text/>
	               <xsl:text/>DoubleClick(  System.Object sender, System.EventArgs e ) 
	   {
	   }

	   private void dg<xsl:value-of select="@Name"/>_CurrentCellChanged(  System.Object sender, System.EventArgs e ) 
	   {
	      OnDataGrid<xsl:value-of select="@Name"/>CurrentCellChanged(sender, e);
	   }
	   protected virtual void OnDataGrid<xsl:value-of select="@Name"/><xsl:text/>
	               <xsl:text/>CurrentCellChanged(  System.Object sender, System.EventArgs e ) 
	   {
	   }
   </xsl:for-each>

      protected override void OnLayout( System.Windows.Forms.LayoutEventArgs e )
      {
         int width; 
         int height;
         base.OnLayout(e);

         //this.lblForceScroll.Height = 1;

         <xsl:for-each select="$childcollections">
         width = grp<xsl:value-of select="@Name"/>.Width - btnAdd<xsl:value-of select="@Name"/>.Width - <xsl:value-of select="3*$hmargin"/>;
         if( grp<xsl:value-of select="@Name"/>.ClientSize.Height - btnEdit<xsl:value-of select="@Name"/>.Bottom &lt; <xsl:value-of select="2*$vmargin"/> )
         {
            height = btnEdit<xsl:value-of select="@Name"/>.Bottom - dg<xsl:value-of select="@Name"/>.Top;
         }
         else
         {
            height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>;
         }
         //height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>;
		   dg<xsl:value-of select="@Name"/>.Size = new System.Drawing.Size(width, height);
		   //dg<xsl:value-of select="@Name"/>.Width = width;
		   btnAdd<xsl:value-of select="@Name"/>.Left = width + <xsl:value-of select="1.5*$hmargin"/>;
		   btnRemove<xsl:value-of select="@Name"/>.Left = btnAdd<xsl:value-of select="@Name"/>.Left;
		   btnEdit<xsl:value-of select="@Name"/>.Left = btnAdd<xsl:value-of select="@Name"/>.Left;
		   grp<xsl:value-of select="@Name"/>.Size = new System.Drawing.Size(
		               pnl<xsl:value-of select="@Name"/>.ClientSize.Width - 2*<xsl:value-of select="$hmargin"/>,
		               pnl<xsl:value-of select="@Name"/>.ClientSize.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="2*$vmargin"/>);
         </xsl:for-each>
      }
      
      protected override void OnLoad( System.EventArgs e )
      {
         base.OnLoad(e);
         //pnlDetail.Width = this.pnlAll.Width;
         //this.lblForceScroll.SendToBack();
         this.pnlBottomShim.SendToBack();
         this.lblForceScroll.Height = 1;
         //this.lblForceScroll.Visible = false;
      }
   #endregion
   </xsl:template>

   <xsl:template name="RestOfForm">
   #region Rest of form
	   protected override void ResizeUC()
	   {
	      base.ResizeUC();

		   <xsl:if test="count($childcollections) > 0 ">
		      <xsl:for-each select="$childcollections[count($childcollections)]">
		   grp<xsl:value-of select="@Name"/>.Height = this.Height - grp<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="$vmargin"/>;
		   dg<xsl:value-of select="@Name"/>.Height = grp<xsl:value-of select="@Name"/>.Height - dg<xsl:value-of select="@Name"/>.Top - <xsl:value-of select="2 * $vmargin" />;
		      </xsl:for-each>
		   </xsl:if>
         //this.pnlDetail.Size = new System.Drawing.Size(this.ClientSize.Width, <xsl:value-of select="$lasttop"/>);

	   }
   	
   #endregion 
   </xsl:template>

</xsl:stylesheet> 
 