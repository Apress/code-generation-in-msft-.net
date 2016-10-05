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
  Summary: Generates the plumbing class for the selection dialog  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="baseformname" select="'KADGen.WinSupport.BaseSelectForm'" />

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 
<xsl:variable name="winnamespace" select="'KADGen.WinProject'" />

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
using System;
using System.Windows.Forms;
using System.Threading;
using KADGen.BusinessObjectSupport;
using <xsl:value-of select="$bonamespace"/>;

namespace <xsl:value-of select="$winnamespace"/>
{
   public class gen<xsl:value-of select="$Name"/> : <xsl:text/>
   <xsl:choose>
	   <xsl:when test="@Inherits">
         <xsl:value-of select="@Inherits"/>
	   </xsl:when>
	   <xsl:otherwise>
	      <xsl:value-of select="$baseformname"/>
	   </xsl:otherwise>
   </xsl:choose>
   {
   <xsl:call-template name="WindowsFormDesigner"/>
   <xsl:call-template name="RestOfForm"/>
   }
}
</xsl:template>

   <xsl:template name="WindowsFormDesigner">

   #region  System.Windows Form Designer generated code 

	   public gen<xsl:value-of select="$Name"/>() : base()
	   {
		   //This call is required by the System.Windows Form Designer.
		   InitializeComponent();

		   //Add any initialization after the InitializeComponent() call
      }

	   //Form overrides dispose to clean up the component list.
	   protected override void Dispose( bool disposing )
	   {
		   if ( disposing )
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

	   //NOTE: The following procedure is required by the System.Windows Form Designer
	   //It can be modified using the System.Windows Form Designer.  
	   //Do not modify it using the code editor.

	   [System.Diagnostics.DebuggerStepThrough()] 
	   private void InitializeComponent()
      {
		   this.SuspendLayout();
		   //
		   //<xsl:value-of select="$Name"/>
		   //
		   this.Name = "<xsl:value-of select="$Name"/>";
		   this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		   this.Text = "<xsl:value-of select="$objectname"/> Select";
		   this.ResumeLayout(false);

	   }

   #endregion


   </xsl:template>

   <xsl:template name="RestOfForm">
	   private <xsl:value-of select="orm:Property[@IsPrimaryKey='true'][1]/@NETType"/> mResult; 
	   private <xsl:value-of select="$objectname"/>List mList;
   	
	   protected override CSLA.ReadOnlyCollectionBase GetList() 
	   {
		   mList = <xsl:value-of select="$objectname"/>List.Get<xsl:value-of select="$objectname"/>List();
		   return mList;
	   }
   	
	   protected override void SetResult()
	   {
		   if ( dgDisplay.CurrentRowIndex >= 0 )
		   {
			   mResult = mList[dgDisplay.CurrentRowIndex].<xsl:value-of select="orm:Property[@IsPrimaryKey='true'][1]/@Name"/>;
	      }
		   else
		   {
			   mResult = <xsl:value-of select="orm:Property[@IsPrimaryKey='true'][1]/@Empty"/>;
         }
	   }
   	
	   protected override void SetEmptyResult()
	   {
		   mResult = <xsl:value-of select="orm:Property[@IsPrimaryKey='true'][1]/@Empty"/>;
	   }
   	
	   protected override object GetResult
	   {
	      get
	      {
	         return mResult;
	      }
	   }

	   //public <xsl:value-of select="orm:Property[@IsPrimaryKey='true'][1]/@NETType"/> Result
	   //{ 
	   //	  get
	   //   {
	   //	 	  return mResult;
	   //	  }
	   //}
   </xsl:template>

</xsl:stylesheet> 
  