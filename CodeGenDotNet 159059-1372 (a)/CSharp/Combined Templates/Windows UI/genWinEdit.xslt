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
  Summary: Generates the plumbing class for the main edit windows uc  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="WinSupport.xslt"/>
<!--<xsl:import href="../../Chapter 8/BusinessObjects/CSLASupport.xslt"/>-->
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="editformname" select="concat($formname,'Edit')" />
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="singularobject" select="//orm:Assembly/orm:Object[@Name=$objectname]/@SingularName" />
<xsl:variable name="bonamespace">
   <xsl:for-each select="//ui:Form[@Name=$Name]">
      <xsl:value-of select="ancestor-or-self::*[@BusinessObjectNamespace]/@BusinessObjectNamespace"/>
   </xsl:for-each>
</xsl:variable> 
<xsl:variable name="winnamespace" select="'KADGen.WinProject'" />
<xsl:variable name="uiform" select="//ui:Form[@Name=$Name]" />

<xsl:variable name="properties" 
            select="//orm:Object[@Name=$objectname]/orm:Property[@Display='true']" />
<xsl:variable name="childcollections" 
            select="//orm:Object[@Name=$objectname and not(@IsLookup='true')]/orm:ChildCollection" />

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
using System;
using System.Windows.Forms;
using System.Threading;
using KADGen.BusinessObjectSupport;
using <xsl:value-of select="$bonamespace" />;

namespace <xsl:value-of select="$winnamespace"/>
{
   public class gen<xsl:value-of select="$singularobject"/>Edit : layout<xsl:value-of select="$singularobject"/>Edit, KADGen.WinSupport.IEditUserControl
   {
	   protected <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/> m<xsl:value-of select="$objectname"/>;
	   protected bool mIsLoaded; 
   	
   <xsl:call-template name="PublicPropertiesAndMethods"/>
   <xsl:call-template name="Implements"/>
   <xsl:call-template name="OverridableEventHandlers"/>
   <xsl:call-template name="PrivateAndProtectedPropertiesAndMethods"/>
   }
}</xsl:template>


   <xsl:template name="PublicPropertiesAndMethods">
   #region  Public Properties and Methods 
	   public <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/><xsl:text> </xsl:text><xsl:value-of select="$objectname"/> 
	   {         
		   get
		   {
			   return m<xsl:value-of select="$objectname"/>;
		   }
		   set
		   {
			   m<xsl:value-of select="$objectname"/> = value;
		   }
	   }
   	
   #endregion
   </xsl:template>


   <xsl:template name="Implements">
   #region Implementation for IEditUserControl

	   public override CSLA.ReadOnlyCollectionBase GetList() 
	   {
		   return <xsl:value-of select="$objectname"/>List.Get<xsl:value-of select="$objectname"/>List();
	   }
   	
	   public IBusinessObject Save() 
	   {
         m<xsl:value-of select="$objectname"/>.ApplyEdit();
         mbIsDirty = false;
         SetState();
         return (IBusinessObject) m<xsl:value-of select="$objectname"/>.Save();
      }

	   public void Delete() 
	   {
         m<xsl:value-of select="$objectname"/>.Delete();
         m<xsl:value-of select="$objectname"/>.Save();
         m<xsl:value-of select="$objectname"/> = null;
         this.Clear();
         SetState();
      }
      
      public void CancelEdit() 
      {
         m<xsl:value-of select="$objectname"/>.CancelEdit();
         if ( (this.EditMode &amp; KADGen.WinSupport.EditMode.IsNew) > 0 ) 
         {
            m<xsl:value-of select="$objectname"/> = null;
         }
         this.Clear();
         mbIsDirty = false;
         SetState();
      }

	   public virtual void OnClosing(  
	                     System.ComponentModel.CancelEventArgs e )
	   {
	      if ( m<xsl:value-of select="$objectname"/> != null )
	      {
	         if ( m<xsl:value-of select="$objectname"/>.IsDirty )
	         {
	            if ( System.Windows.Forms.MessageBox.Show(this.Parent, 
	                        "<xsl:value-of select="$uiform/@OnDirtyClosingQuestion"/>", 
	                        "<xsl:value-of select="$uiform/@OnDirtyClosingTitle"/>", 
	                        System.Windows.Forms.MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes )
	            {
	               e.Cancel = true;    
	            }
	         }
	      }
		   //m<xsl:value-of select="$objectname"/>.CancelEdit();
	   }
   	
      public virtual bool CanCreate() 
      {
         return <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanCreate();
      }
      
      public virtual bool CanRetrieve() 
      {
         return <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanRetrieve();
      }
      
      public virtual bool CanUpdate() 
      {
         return <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanUpdate();
      }
      
      public virtual bool CanDelete() 
      {
         return <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanDelete();
      }
      
      public IBusinessObject BusinessObject 
      {
	      get
	      {
	         return m<xsl:value-of select="$objectname"/>;
	      }
	   }
   	
      public System.Drawing.Size MinimumSize 
      {
         get
         {
		      return mMinimumSize;
         }
      }
      
      public int LabelWidth 
      {
         get
         {
		      return mLabelWidth;
         }
      }
      
      public int HorizontalMargin 
      {
         get
         {
		      return mHorizontalMargin;
         }
      }

      public int VerticalMargin 
      {
         get
         {
		      return mVerticalMargin;
         }
      }

      public int ControlWidth 
      {
         get
         {
		      return mSampleControl.Width;
         }
      }
      
      public int ControlTop 
      {
         get
         {
		      return mSampleControl.Top;
		   }
	   }
      
      public int ControlLeft 
      {
         get
         {
		      return mSampleControl.Left;
		   }
	   }

      public int IdealHeight 
      {
         get
         {
		      return mIdealHeight;
		   }
	   }

      public int IdealWidth 
      {
         get
         {
		      return mIdealWidth;
		   }
	   }

      public KADGen.WinSupport.EditMode EditMode 
      {
         get
         {
	         KADGen.WinSupport.EditMode retValue ;
            if ( m<xsl:value-of select="$objectname"/> == null )
            {
               retValue = KADGen.WinSupport.EditMode.IsEmpty;
            }
            else if ( m<xsl:value-of select="$objectname"/>.IsDirty || mbIsDirty ) 
            {
               retValue = KADGen.WinSupport.EditMode.IsDirty;
            }
            else if ( m<xsl:value-of select="$objectname"/>.IsDeleted )
            {
               retValue = KADGen.WinSupport.EditMode.IsDeleted;
            }
            else
            {
               retValue = KADGen.WinSupport.EditMode.IsClean;
            }
            if ( ( m<xsl:value-of select="$objectname"/> != null ) &amp;&amp;
                     m<xsl:value-of select="$objectname"/>.IsNew )
            {
               retValue = retValue | KADGen.WinSupport.EditMode.IsNew;
            }
            return retValue;
         }
      }

      public void SetupControl( IBusinessObject businessObject ) 
      {
	      SetupControl(businessObject, "");
	   }
	   public void SetupControl( IBusinessObject businessObject ,
	               string parentPrimaryKey ) 
	   {
	      if ( businessObject is <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/> )
	      {
	         <xsl:value-of select="$objectname"/> = (<xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>) businessObject;
	         RemoveControl(this, parentPrimaryKey);
	         mbIsDirty = false;
	         SetState();
	      }
	      else
	      {
            throw new ArgumentException("Type Mismatch: BusinessObject passed is not type <xsl:value-of select="$objectname"/>");
  	      }
     	   
  	      <xsl:variable name="parentcaption" select="@Caption"/>

		   <xsl:for-each select="$childcollections">
	         <xsl:variable name="childname" select="@Name" />
	         <xsl:variable name="childobject" select="//orm:Object[@CollectionName=$childname]" />
	         <xsl:variable name="childbaseobjectname" >
	            <xsl:choose>
	               <xsl:when test="$childobject/@Inherits">
                     <xsl:value-of select="$childobject/@Inherits"/>
	               </xsl:when>
	               <xsl:otherwise>
	                  <xsl:value-of select="$childobject/@Name"/>
	               </xsl:otherwise>
	            </xsl:choose>
	         </xsl:variable>
		   if ( m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>.IsLoaded )
		   {
   		   if ( <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanCreate() )
   		   {
	   		   btnAdd<xsl:value-of select="$childname"/>.Enabled = true;
	   		   this.toolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, 
	   		            "Add a new <xsl:value-of select="@Caption"/>");
	   	   }
	   	   else
	   	   {
	   		   btnAdd<xsl:value-of select="$childname"/>.Enabled = false;
	   		   this.toolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, 
	   		            "You don't have privileges to add a new <xsl:value-of select="@Caption"/>");
		      }
   		   if ( <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanDelete() ) 
   		   {
	   		   btnRemove<xsl:value-of select="$childname"/>.Enabled = true;
	   		   this.toolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, 
	   		            "Remove the current <xsl:value-of select="@Caption"/>");
	   	   }
	   	   else
	   	   {
	   		   btnRemove<xsl:value-of select="$childname"/>.Enabled = false;
	   		   this.toolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, 
	   		            "You don't have privileges to remove a <xsl:value-of select="@Caption"/>");
		      }
   		   if ( <xsl:value-of select="$bonamespace"/>.<xsl:value-of select="$objectname"/>.CanUpdate() )
   		   {
	   		   btnEdit<xsl:value-of select="$childname"/>.Enabled = true;
	   		   this.toolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, 
	   		            "Edit the current <xsl:value-of select="@Caption"/>");
	   	   }
	   	   else
	   	   {
	   		   btnEdit<xsl:value-of select="$childname"/>.Enabled = false;
	   		   this.toolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, 
	   		            "You don't have privileges to edit a <xsl:value-of select="@Caption"/>");
		      }
		      //grp<xsl:value-of select="$childname"/>.Visible = true;
		      //this.Height = grp<xsl:value-of select="$childname"/>.Bottom + 15;
		   }
		   else
		   {
	   	   btnAdd<xsl:value-of select="$childname"/>.Enabled = false;
	   	   this.toolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, 
	   		         "You can't add new <xsl:value-of select="@Caption"/> items until you save the <xsl:value-of select="$parentcaption"/>");
	   	   btnRemove<xsl:value-of select="$childname"/>.Enabled = false;
	   	   this.toolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, 
	   		         "You can't remove a <xsl:value-of select="@Caption"/> if there aren't any");
	   	   btnEdit<xsl:value-of select="$childname"/>.Enabled = false;
	   	   this.toolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, 
	   		         "You can't edit a <xsl:value-of select="@Caption"/> if there aren't any");
		      this.toolTip.SetToolTip(grp<xsl:value-of select="$childname"/>, 
		               "You can't add <xsl:value-of select="@Caption"/> until you save <xsl:value-of select="$parentcaption"/>");
		      this.toolTip.SetToolTip(dg<xsl:value-of select="$childname"/>, 
		               "You can't add <xsl:value-of select="@Caption"/> until you save <xsl:value-of select="$parentcaption"/>");
		      //grp<xsl:value-of select="$childname"/>.Visible = false;
		      //this.Height = grp<xsl:value-of select="$childname"/>.Top + 15;
		   }
		   </xsl:for-each>
   		
		   //ResizeUC();
		   DataBind();
		   mbIsDirty = false;
		   SetState();
		   mIsLoaded = true;
	   }
   	
   	
   #endregion 
   </xsl:template>

   <xsl:template name="OverridableEventHandlers">
      <xsl:variable name="primarykeys" select=".//orm:Property[@IsPrimaryKey='true']" />
      <xsl:variable name="object" select="." />
   #region Overridding Event Handlers

	   protected override void OnLoad( System.EventArgs e )
      {
	      base.OnLoad(e);
	      mbIsDirty = false;
	      SetState();
	   }
   	
	   protected override void OnCtlValidated( System.Object sender ,  
	                     System.EventArgs e)
	      {
         base.OnCtlValidated(sender, e);
         //Binds each control to the errProvider on the form
         Control ctl = (Control)sender;

         foreach (Binding bnd in ctl.DataBindings)
         {
            if ( bnd.IsBinding )
            {
               System.ComponentModel.IDataErrorInfo obj = (System.ComponentModel.IDataErrorInfo) bnd.DataSource; 
               errProvider.SetError(ctl, obj[bnd.BindingMemberInfo.BindingField]);
            }
         }
	   }
   	
	   protected override void OnDataChanged( System.Object sender, System.EventArgs e )
	   {
	      base.OnDataChanged(sender, e);
	   }
   	
	   <xsl:for-each select="$childcollections">
	   <xsl:variable name="childname" select="@Name" />
	   <xsl:variable name="childobject" select="//orm:Object[@CollectionName=$childname]" />
	   <xsl:variable name="parents" select="$childobject//orm:ParentObject"/>
	   <xsl:variable name="otherparents" select="$childobject//orm:ParentObject[@SingularName != $objectname]"/>
	   <xsl:variable name="childbaseobjectname" >
	      <xsl:choose>
	         <xsl:when test="//orm:Object[@CollectionName=$childname]/@Inherits">
               <xsl:value-of select="//orm:Object[@CollectionName=$childname]/@Inherits"/>
	         </xsl:when>
	         <xsl:otherwise>
	            <xsl:value-of select="//orm:Object[@CollectionName=$childname]/@Name"/>
	         </xsl:otherwise>
	      </xsl:choose>
	   </xsl:variable>

	   protected override void OnBtnAdd<xsl:value-of select="$childname"/>Click(  
	                     System.Object sender, System.EventArgs e )
	   {
         this.Add<xsl:value-of select="$childname"/>(sender,e);
      }

	   protected override void OnBtnRemove<xsl:value-of select="$childname"/>Click( 
	                     System.Object sender, System.EventArgs e )
	   {
         this.Remove<xsl:value-of select="$childname"/>(sender,e);
      }

	   protected override void OnBtnEdit<xsl:value-of select="$childname"/>Click( 
	                     System.Object sender, System.EventArgs e )
	   {
         this.Edit<xsl:value-of select="$childname"/>(sender, e);   
      }
      
      protected override void OnDataGrid<xsl:value-of select="$childname"/>CurrentCellChanged(
	                     System.Object sender, System.EventArgs e )
	   {
	      SetState();
      }

   	
     	
	   private void Add<xsl:value-of select="$childname"/>( 
	                     System.Object sender, System.EventArgs e )
	   {
         <xsl:value-of select="$childobject/@Name"/> obj;
         obj = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childobject/@CollectionName"/>.NewItem();
         <!-- REFACTOR: Change hte structure so this isn't so ugly  -->
         <xsl:variable name="childparent" select="$childobject//orm:ParentObject[@SingularName=$objectname]"/>
         <xsl:for-each select="$childparent/orm:ParentKeyField">
            <xsl:variable name="ordinal" select="@Ordinal"/>
            <xsl:variable name="childparentkey" select="@Name" />
            <xsl:for-each select="$childparent/orm:ChildKeyField[@Ordinal=$ordinal]">
         obj.<xsl:value-of select="@Name"/> = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childparentkey"/>;
            </xsl:for-each>
         </xsl:for-each>
         if ( obj != null )
         {
            if ( this.Edit<xsl:value-of select="$childname"/>(obj) == System.Windows.Forms.DialogResult.OK )
            {
               m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childobject/@CollectionName"/>.AddItem(obj);
               dg<xsl:value-of select="$childname"/>.Refresh();
               OnDataChanged(sender, e);
               SetState();
            }
         }
	   }
   	
	   private void Remove<xsl:value-of select="$childname"/>(  
	                     System.Object sender, System.EventArgs e )
	   {
		   <xsl:value-of select="$childobject/@Name"/> obj = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>[dg<xsl:value-of select="$childname"/>.CurrentRowIndex];
         string desc = ""<xsl:text/>;
         <xsl:call-template name="GetRuntimeChildDesc">
            <xsl:with-param name="childname" select="$childname"/>
         </xsl:call-template>

         
		   if ( MessageBox.Show("Remove <xsl:value-of select="@Name"/> " + desc + " from <xsl:value-of select="$objectname"/>?", 
			         "Remove <xsl:value-of select="@Name"/>", 
			         MessageBoxButtons.YesNo) == DialogResult.Yes )
	      {
			   dg<xsl:value-of select="$childname"/>.SuspendLayout();
			   dg<xsl:value-of select="$childname"/>.DataSource = null;
			   m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>.Remove(obj);
			   dg<xsl:value-of select="$childname"/>.DataSource =  m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>;
			   dg<xsl:value-of select="$childname"/>.ResumeLayout();
            OnDataChanged(sender, e);
            SetState();
		   }
	   }

      private void Edit<xsl:value-of select="@Name"/>(  
	                     System.Object sender, System.EventArgs e )
	   {
		   <xsl:value-of select="$childobject/@Name"/> obj = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>[dg<xsl:value-of select="$childname"/>.CurrentRowIndex];
		   Edit<xsl:value-of select="@Name"/>(obj);
      }
      private System.Windows.Forms.DialogResult Edit<xsl:value-of select="@Name"/>(  
	                     <xsl:value-of select="$childobject/@Name"/> obj ) 
	   {                  
         KADGen.WinSupport.ChildEditForm frm = new KADGen.WinSupport.ChildEditForm();
         System.Windows.Forms.DialogResult dlgResult; 
		   dlgResult = frm.ShowDialog(new <xsl:value-of select="$childbaseobjectname"/>Edit(), obj,  
		            "<xsl:value-of select="$primarykeys[1]/@Name"/>", 
		            KADGen.WinSupport.Utility.ParentForm(this));
		   if ( dlgResult == System.Windows.Forms.DialogResult.OK )
         {
            dg<xsl:value-of select="$childname"/>.Refresh();
            OnDataChanged( this, new System.EventArgs() );
            SetState();
		   }
         return dlgResult;
      }
      </xsl:for-each>
   #endregion 
   </xsl:template>

   <xsl:template name="GetNewItemParams">
	   <xsl:param name="parentkeys"/>
	   <xsl:param name="objectkeys" />
	   <xsl:for-each select="$objectkeys">
	      <xsl:variable name="objectname" select="../@Name"/>
	      <xsl:element name="Param">
	         <xsl:variable name="name" select="@Name"/>
	         <xsl:attribute name="ObjectName"><xsl:value-of select="$objectname"/></xsl:attribute>
	         <xsl:attribute name="Name"><xsl:value-of select="$objectname"/><xsl:value-of select="$objectname"/></xsl:attribute>
	         <xsl:attribute name="Type">
	            <xsl:value-of select="//orm:Object[@Name=$objectname]//orm:Property[@Name=$objectname]/@NETType"/>
	         </xsl:attribute>
	      </xsl:element>
	   </xsl:for-each>	
      <xsl:for-each select="$parentkeys">
	      <xsl:variable name="parentname" select="../@Name"/>
	      <xsl:element name="Param">
	         <xsl:variable name="name" select="@Name"/>
	         <xsl:attribute name="ParentName"><xsl:value-of select="$parentname"/></xsl:attribute>
	         <xsl:attribute name="Name"><xsl:value-of select="$parentname"/><xsl:value-of select="$objectname"/></xsl:attribute>
	         <xsl:attribute name="Type">
	            <xsl:value-of select="//orm:Object[@Name=$parentname]//orm:Property[@Name=$objectname]/@NETType"/>
	         </xsl:attribute>
	      </xsl:element>
	   </xsl:for-each>

   </xsl:template>



   <xsl:template name="PrivateAndProtectedPropertiesAndMethods">
      <xsl:variable name="parentprimarykeys" select=".//orm:Property[@IsPrimaryKey='true']"/>
   #region  Private and Protected Properties and Methods 
      protected void Clear()
      {
      <xsl:for-each select="$properties">
         <xsl:for-each select="ui:ClearControl">
         this.<xsl:value-of select="../@ControlName"/>.<xsl:value-of select="@Call" />;<xsl:text/>
         </xsl:for-each>
      </xsl:for-each>
      <xsl:for-each select="$childcollections">
         this.dg<xsl:value-of select="@Name"/>.DataSource = null;
      </xsl:for-each>
      }

	   protected virtual void DataBind()
      {
         bool bIsDirty = mbIsDirty;
		   KADGen.WinSupport.Utility.BindField(chkIsDirty, "Checked",  m<xsl:value-of select="$objectname"/>, "IsDirty");<xsl:text/>
	   <xsl:for-each select="$properties">
	      <xsl:choose>
	         <xsl:when test="@IsLookup='true'">
		         <xsl:variable name="ctlname" select="@Name"/>
		         <xsl:variable name="lookupname" select="@LookupSingular"/>
		         <xsl:variable name="object" select ="ancestor::orm:Object"/>
		         <xsl:variable name="parentobject" select="$object//orm:ParentObject[@SingularName=$lookupname]"/>
		         <!-- From a practical perspective, we can only deal with one Value/DisplayMember field. If you
		            have something more complex, you probably need to rethink/redesign, or offer a simple key.
		            Using that will probably require extending metadata to provide a "lookup column" that is
		            separate from the parent/child keys.
   		           
		            Also, I intend to spring a template error if there are no key fields. Having a lookup 
		            without a key makes no sense to me, so I don't know how to handle it. 
   		           
		            If things show up without ordinals, we could probably change this to take the first.
		            KAD -->
		         <xsl:for-each select="$parentobject/orm:ParentKeyField[@Ordinal='1']">
         cbo<xsl:value-of select="$ctlname"/>.DataSource = <xsl:value-of select="$lookupname"/>List.Get<xsl:value-of select="$lookupname"/>List();<xsl:text/>
         cbo<xsl:value-of select="$ctlname"/>.ValueMember = "<xsl:value-of select="@Name" />";
         cbo<xsl:value-of select="$ctlname"/>.DisplayMember = "DisplayText";<xsl:text/>
		         </xsl:for-each>
	         </xsl:when>
	      </xsl:choose> 
		   this.BindField(<xsl:value-of select="@ControlName"/>, "<xsl:text/>
		            <xsl:value-of select="@BindProperty"/>",  m<xsl:value-of select="$objectname"/>
		            <xsl:text/>, "<xsl:value-of select="@Name" />", 
		            typeof(<xsl:value-of select="@NETType"/>));<xsl:text/>
	   </xsl:for-each>
		   lstRules.DataSource =  m<xsl:value-of select="$objectname"/>.GetBrokenRulesCollection();
		   lstRules.DisplayMember = "Description";
		   <xsl:for-each select="$childcollections">
		   Databind<xsl:value-of select="@Name"/>Grid();
		   </xsl:for-each>
		   mbIsDirty = bIsDirty;
	   }
   	
	   <xsl:for-each select="$childcollections">
      <xsl:variable name="childcollection" select="@Name" />
      <xsl:variable name="childshortname" select="substring-after($childcollection, $objectname)"/>
      <xsl:variable name="singularname" select="ancestor::orm:Assembly//orm:Object[@CollectionName=$childcollection]/@Name"/>
      // <xsl:value-of select="$singularname"/>
	   protected void Databind<xsl:value-of select="$childcollection"/>Grid()
	   {
	      DataGridTableStyle dgtbs = new DataGridTableStyle();
	      DataGridColumnStyle dgcs; 
         System.Drawing.Graphics graphics = this.CreateGraphics();
		   dg<xsl:value-of select="$childcollection"/>.SuspendLayout();
   	   if ( ! mIsLoaded )
   	   {
		      dgtbs.MappingName = "<xsl:value-of select="$childcollection"/>";
		      dgtbs.RowHeadersVisible = true;
		      dg<xsl:value-of select="$childcollection"/>.RowHeaderWidth = 20;
		      <xsl:choose>
		         <xsl:when test="//ui:Form[@Name=$editformname]//ui:ChildGrid[@Name=$childshortname]//ui:GridColumn">
		            <xsl:for-each select="//ui:Form[@Name=$editformname]//ui:ChildGrid[@Name=$childshortname]//ui:GridColumn">
		               <xsl:variable name="childcolumnname" select="@Name"/>
		               <xsl:apply-templates select="//orm:Object[@CollectionName=$childcollection]//orm:Property[@Name=$childcolumnname]"
		                           mode="CreateDGColumn">
		                  <xsl:with-param name="parentprimarykeys" select="$parentprimarykeys"/>            
		                  <xsl:with-param name="childcollection" select="$childcollection"/>            
		               </xsl:apply-templates>
		            </xsl:for-each>
		         </xsl:when>
		         <xsl:otherwise>
		            <xsl:apply-templates select="//orm:Object[@CollectionName=$childcollection]//orm:Property[not(@IsLookup='true') and not(@IsAutoIncrement='true')]"
		                           mode="CreateDGColumn">
		               <xsl:with-param name="parentprimarykeys" select="$parentprimarykeys"/>            
		               <xsl:with-param name="childcollection" select="$childcollection"/>            
		            </xsl:apply-templates>
		         </xsl:otherwise>
		      </xsl:choose>

		      dg<xsl:value-of select="$childcollection"/>.TableStyles.Add(dgtbs);
		   }
		   dg<xsl:value-of select="$childcollection"/>.SetDataBinding(m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childcollection"/>, "");
		   dg<xsl:value-of select="$childcollection"/>.ResumeLayout();
	   }
   	
      <xsl:for-each select="//orm:Object[@CollectionName=$childcollection]//orm:Property[not(@IsLookup='true')]">
	   protected virtual int Get<xsl:value-of select="$childcollection"/>_<xsl:value-of select="@Name"/>ColumnWidth( 
	               System.Drawing.Font font, System.Drawing.Graphics graphics) 
	   {
         int temp = 0;
         int colWidth = 0;
         <xsl:choose>
            <xsl:when test="@NETType='System.Boolean'">
         colWidth = KADGen.WinSupport.Utility.GetWidth("W", Font, graphics);
            </xsl:when>
            <xsl:when test="@NETType='System.DateTime' or @NETType='CSLA.SmartDate'">
         colWidth = KADGen.WinSupport.Utility.GetWidth("99/99/9999", Font, graphics);
            </xsl:when>
            <xsl:otherwise>
         foreach ( <xsl:value-of select="$singularname"/> bo in m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childcollection"/> )
         {
            temp = KADGen.WinSupport.Utility.GetWidth(bo.<xsl:value-of select="@Name"/>.ToString(), font, graphics);
            if ( temp > colWidth )
            {
               colWidth = temp;
            }
         }
            </xsl:otherwise>
         </xsl:choose> 
         temp = KADGen.WinSupport.Utility.GetWidth("<xsl:value-of select="@Caption"/>", Font, graphics);
         if ( temp > colWidth )
         {
            colWidth = temp;
         }
         return colWidth;
      }
      </xsl:for-each>
	   </xsl:for-each>
   	
	   private void SetState()
	   {
	      KADGen.WinSupport.EditMode editMode = this.EditMode;
	      <xsl:for-each select="$childcollections">
      	   <xsl:variable name="childname" select="@Name" />
         btnAdd<xsl:value-of select="$childname"/>.Enabled = 
                           (editMode == KADGen.WinSupport.EditMode.IsClean) ||
                           (editMode == KADGen.WinSupport.EditMode.IsDirty);
	      btnRemove<xsl:value-of select="$childname"/>.Enabled = 
	                        (dg<xsl:value-of select="$childname"/>.CurrentRowIndex >= 0) &amp;&amp;
                           ((editMode == KADGen.WinSupport.EditMode.IsClean) || 
                           (editMode == KADGen.WinSupport.EditMode.IsDirty)); 	   
	      btnEdit<xsl:value-of select="$childname"/>.Enabled = 
	                        (dg<xsl:value-of select="$childname"/>.CurrentRowIndex >= 0) &amp;&amp; 
                           ((editMode == KADGen.WinSupport.EditMode.IsClean) || 
                           (editMode == KADGen.WinSupport.EditMode.IsDirty)); 	   
	      </xsl:for-each>

		   bool bEnabled = (this.EditMode != KADGen.WinSupport.EditMode.IsEmpty);
	      <xsl:for-each select="$properties">
	      this.<xsl:value-of select="@ControlName"/>.Enabled = bEnabled;<xsl:text/>
	      </xsl:for-each>

	   }

   #endregion 
   </xsl:template>

   <xsl:template match="orm:Property" mode="CreateDGColumn">
      <xsl:param name="parentprimarykeys"/>
      <xsl:param name="childcollection"/>
	   <xsl:variable name="childcolumnname" select="@Name" />
	   <xsl:if test="not($parentprimarykeys[@Name=$childcolumnname])">
	   <xsl:choose>
		   <xsl:when test="@NETType='System.Boolean'">
	            dgcs = new DataGridBoolColumn();
		   </xsl:when>
		   <xsl:otherwise>
	            dgcs = new DataGridTextBoxColumn();
		   </xsl:otherwise>
	   </xsl:choose>
               dgcs.MappingName = "<xsl:value-of select="@Name"/>";
               dgcs.HeaderText = "<xsl:value-of select="@Caption"/>";
               dgcs.Width = Get<xsl:value-of select="$childcollection"/>_<xsl:value-of select="@Name"/>ColumnWidth(Font, graphics) + HorizontalMargin;
      <xsl:if test="@NETType='System.Decimal'">
               ((System.Windows.Forms.DataGridTextBoxColumn) dgcs).Format = "c";
      </xsl:if>
               dgtbs.GridColumnStyles.Add(dgcs);
	   </xsl:if>
   </xsl:template>

</xsl:stylesheet> 
 