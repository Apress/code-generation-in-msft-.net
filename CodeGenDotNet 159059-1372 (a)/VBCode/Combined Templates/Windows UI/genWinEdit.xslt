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
' <xsl:value-of select="count($uiform)" />
' <xsl:value-of select="count(//ui:Form[@Name=$Name])" />
' <xsl:value-of select="count(//ui:Form)" />
' <xsl:value-of select="$Name" />
Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
Imports System.Threading
Imports KADGen.BusinessObjectSupport
Imports <xsl:value-of select="$bonamespace" />

Public Class gen<xsl:value-of select="$singularobject"/>Edit
	Inherits layout<xsl:value-of select="$singularobject"/>Edit
	Implements WinSupport.IEditUserControl

	Protected m<xsl:value-of select="$objectname"/> As <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>
	Protected mIsLoaded As Boolean 
	
  <xsl:call-template name="PublicPropertiesAndMethods"/>
  <xsl:call-template name="Implements"/>
  <xsl:call-template name="OverridableEventHandlers"/>
  <xsl:call-template name="PrivateAndProtectedPropertiesAndMethods"/>
End Class
</xsl:template>


<xsl:template name="PublicPropertiesAndMethods">
#Region " Public Properties and Methods "
	Public Property <xsl:value-of select="$objectname"/>() _
	         As <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>
		Get
			Return m<xsl:value-of select="$objectname"/>
		End Get
		Set(ByVal Value As <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>)
			m<xsl:value-of select="$objectname"/> = Value
		End Set
	End Property
	
#End Region
</xsl:template>


<xsl:template name="Implements">
#Region "Implementation for IEditUserControl"

	Public Overrides Function GetList() _
	            As CSLA.ReadOnlyCollectionBase _
	            Implements WinSupport.IEditUserControl.GetList
		Return <xsl:value-of select="$objectname"/>List.Get<xsl:value-of select="$objectname"/>List
	End Function
	
	Protected Function Save() _
	         As IBusinessObject _
            Implements WinSupport.IEditUserControl.Save
      m<xsl:value-of select="$objectname"/>.ApplyEdit()
      mbIsDirty = False
      SetState()
      Return CType(m<xsl:value-of select="$objectname"/>.Save, IBusinessObject)
   End Function

	Protected Sub Delete() _
            Implements WinSupport.IEditUserControl.Delete
      m<xsl:value-of select="$objectname"/>.Delete
      m<xsl:value-of select="$objectname"/>.Save
      m<xsl:value-of select="$objectname"/> = Nothing
      Me.Clear()
      SetState()
   End Sub
   
   Protected Sub CancelEdit() _
            Implements WinSupport.IEditUserControl.CancelEdit
      m<xsl:value-of select="$objectname"/>.CancelEdit
      If (Me.EditMode And EditMode.IsNew) > 0 Then
         m<xsl:value-of select="$objectname"/> = Nothing
      End If
      Me.Clear
      mbIsDirty = False
      SetState()
   End Sub

	Protected Overridable Sub OnClosing( _ 
	                  ByVal e As System.ComponentModel.CancelEventArgs) _
	                  Implements WinSupport.IEditUserControl.OnClosing
	   If Not m<xsl:value-of select="$objectname"/> Is Nothing Then
	      If m<xsl:value-of select="$objectname"/>.IsDirty Then
	         If Windows.Forms.MessageBox.Show(Me.Parent, _
	                     "<xsl:value-of select="$uiform/@OnDirtyClosingQuestion"/>", _
	                     "<xsl:value-of select="$uiform/@OnDirtyClosingTitle"/>", _
	                     Windows.Forms.MessageBoxButtons.YesNo) &lt;> _
	                     Windows.Forms.DialogResult.Yes
	            e.Cancel = True    
	         End If
	      End If 
	   End If
		'm<xsl:value-of select="$objectname"/>.CancelEdit()
	End Sub
	
   Protected Overridable Function CanCreate() _
            As Boolean _
            Implements WinSupport.IEditUserControl.CanCreate
      Return <xsl:value-of select="$objectname"/>.CanCreate()
   End Function
   
   Protected Overridable Function CanRetrieve() _
            As Boolean _
            Implements WinSupport.IEditUserControl.CanRetrieve
      Return <xsl:value-of select="$objectname"/>.CanRetrieve()
   End Function
   
   Protected Overridable Function CanUpdate() _
            As Boolean _
            Implements WinSupport.IEditUserControl.CanUpdate
      Return <xsl:value-of select="$objectname"/>.CanUpdate()
   End Function
   
   Protected Overridable Function CanDelete() _
            As Boolean _
            Implements WinSupport.IEditUserControl.CanDelete
      Return <xsl:value-of select="$objectname"/>.CanDelete()
   End Function
   
   Protected Readonly Property BusinessObject() _
            As IBusinessObject _
	         Implements WinSupport.IEditUserControl.BusinessObject
	   Get
	      Return m<xsl:value-of select="$objectname"/>
	   End Get
	End Property
	
   <!-- Protected Overridable Property BusinessObject() _
            As IBusinessObject _
	         Implements WinSupport.IEditUserControl.BusinessObject
	   Get
	      Return m<xsl:value-of select="$objectname"/>
	   End Get
	   Set(Value As IBusinessObject)
	      If TypeOf Value Is <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/> Then
	         <xsl:value-of select="$objectname"/> = CType(Value, <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>)
	         mbIsDirty = False
	         SetState()
	      Else
            Throw New ArgumentException("Type Mismatch: BusinessObject passed is not type <xsl:value-of select="$objectname"/>")
  	      End If
	   End Set
	End Property -->

   Private Readonly Property MinimumSize _
            As System.Drawing.Size _
	         Implements WinSupport.IEditUserControl.MinimumSize
      Get
		   Return mMinimumSize
      End Get
   End Property
   
   Private ReadOnly Property LabelWidth _
            As Int32 _
	         Implements WinSupport.IEditUserControl.LabelWidth
      Get
		   Return mLabelWidth
      End Get
   End Property
   
   Private ReadOnly Property HorizontalMargin _
            As Int32 _
	         Implements WinSupport.IEditUserControl.HorizontalMargin
      Get
		   Return mHorizontalMargin
      End Get
   End Property

   Private ReadOnly Property VerticalMargin _
            As Int32 _
	         Implements WinSupport.IEditUserControl.VerticalMargin
      Get
		   Return mVerticalMargin
      End Get
   End Property

   Private ReadOnly Property ControlWidth _
            As Int32 _
	         Implements WinSupport.IEditUserControl.ControlWidth
      Get
		   Return mSampleControl.Width
      End Get
   End Property
   
   Private ReadOnly Property ControlTop _
            As Int32 _
	         Implements WinSupport.IEditUserControl.ControlTop
      Get
		   Return mSampleControl.Top
      End Get
   End Property
   
   Private ReadOnly Property ControlLeft _
            As Int32 _
	         Implements WinSupport.IEditUserControl.ControlLeft
      Get
		   Return mSampleControl.Left
      End Get
   End Property

   Private ReadOnly Property IdealHeight _
            As Int32 _
	         Implements WinSupport.IEditUserControl.IdealHeight
      Get
		   Return mIdealHeight
      End Get
   End Property

   Private ReadOnly Property IdealWidth _
            As Int32 _
	         Implements WinSupport.IEditUserControl.IdealWidth
      Get
		   Return mIdealWidth
      End Get
   End Property

   Private ReadOnly Property EditMode _
            As WinSupport.EditMode _
	         Implements WinSupport.IEditUserControl.EditMode
      Get
	      Dim retValue As WinSupport.EditMode
         If m<xsl:value-of select="$objectname"/> Is Nothing Then
            retValue = WinSupport.EditMode.IsEmpty
         ElseIf m<xsl:value-of select="$objectname"/>.IsDirty Or mbIsDirty Then
            retValue = WinSupport.EditMode.IsDirty
         ElseIf m<xsl:value-of select="$objectname"/>.IsDeleted Then
            retValue = WinSupport.EditMode.IsDeleted
         Else
            retValue = WinSupport.EditMode.IsClean
         End If
         If Not m<xsl:value-of select="$objectname"/> Is Nothing AndAlso _
                  m<xsl:value-of select="$objectname"/>.IsNew Then
            retValue = retValue Or WinSupport.EditMode.IsNew
         End If
         Return retValue
      End Get
   End Property

   Protected Sub SetupControl( _
	            businessObject As IBusinessObject) _
	            Implements WinSupport.IEditUserControl.SetupControl 
	   SetUpControl(businessObject, "")
	End Sub
	Protected Sub SetupControl( _
	            businessObject As IBusinessObject, _
	            parentPrimaryKey As String) _
	            Implements WinSupport.IEditUserControl.SetupControl 
	   If TypeOf businessObject Is <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/> Then
	      <xsl:value-of select="$objectname"/> = CType(businessObject, <xsl:value-of select="$bonamespace" />.<xsl:value-of select="$objectname"/>)
	      RemoveControl(Me, parentPrimaryKey)
	      mbIsDirty = False
	      SetState()
	   Else
         Throw New ArgumentException("Type Mismatch: BusinessObject passed is not type <xsl:value-of select="$objectname"/>")
  	   End If
  	   
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
		If m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>.IsLoaded Then
   		If m<xsl:value-of select="$objectname"/>.CanCreate() Then
	   		btnAdd<xsl:value-of select="$childname"/>.Enabled = True
	   		Me.ToolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, _
	   		         "Add a new <xsl:value-of select="@Caption"/>")
	   	Else
	   		btnAdd<xsl:value-of select="$childname"/>.Enabled = False
	   		Me.ToolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, _
	   		         "You don't have privileges to add a new <xsl:value-of select="@Caption"/>")
		   End If
   		If m<xsl:value-of select="$objectname"/>.CanDelete() Then
	   		btnRemove<xsl:value-of select="$childname"/>.Enabled = True
	   		Me.ToolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, _
	   		         "Remove the current <xsl:value-of select="@Caption"/>")
	   	Else
	   		btnRemove<xsl:value-of select="$childname"/>.Enabled = False
	   		Me.ToolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, _
	   		         "You don't have privileges to remove a <xsl:value-of select="@Caption"/>")
		   End If
   		If m<xsl:value-of select="$objectname"/>.CanUpdate() Then
	   		btnEdit<xsl:value-of select="$childname"/>.Enabled = True
	   		Me.ToolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, _
	   		         "Edit the current <xsl:value-of select="@Caption"/>")
	   	Else
	   		btnEdit<xsl:value-of select="$childname"/>.Enabled = False
	   		Me.ToolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, _
	   		         "You don't have privileges to edit a <xsl:value-of select="@Caption"/>")
		   End If
		   'grp<xsl:value-of select="$childname"/>.Visible = True
		   'Me.Height = grp<xsl:value-of select="$childname"/>.Bottom + 15
		Else
	   	btnAdd<xsl:value-of select="$childname"/>.Enabled = False
	   	Me.ToolTip.SetToolTip(btnAdd<xsl:value-of select="$childname"/>, _
	   		      "You can't add new <xsl:value-of select="@Caption"/> items until you save the <xsl:value-of select="$parentcaption"/>")
	   	btnRemove<xsl:value-of select="$childname"/>.Enabled = False
	   	Me.ToolTip.SetToolTip(btnRemove<xsl:value-of select="$childname"/>, _
	   		      "You can't remove a <xsl:value-of select="@Caption"/> if there aren't any")
	   	btnEdit<xsl:value-of select="$childname"/>.Enabled = False
	   	Me.ToolTip.SetToolTip(btnEdit<xsl:value-of select="$childname"/>, _
	   		      "You can't edit a <xsl:value-of select="@Caption"/> if there aren't any")
		   Me.ToolTip.SetToolTip(grp<xsl:value-of select="$childname"/>, _
		            "You can't add <xsl:value-of select="@Caption"/> until you save <xsl:value-of select="$parentcaption"/>")
		   Me.ToolTip.SetToolTip(dg<xsl:value-of select="$childname"/>, _
		            "You can't add <xsl:value-of select="@Caption"/> until you save <xsl:value-of select="$parentcaption"/>")
		   'grp<xsl:value-of select="$childname"/>.Visible = False
		   'Me.Height = grp<xsl:value-of select="$childname"/>.Top + 15
		End If
		</xsl:for-each>
		
		'ResizeUC()
		DataBind()
		mbIsDirty = False
		SetState()
		mIsLoaded = True 
	End Sub
	
	
#End Region 
</xsl:template>

<xsl:template name="OverridableEventHandlers">
   <xsl:variable name="primarykeys" select=".//orm:Property[@IsPrimaryKey='true']" />
   <xsl:variable name="object" select="." />
#Region "Overridding Event Handlers"

	Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
	   MyBase.OnLoad(e)
	   mbIsDirty = False
	   SetState()
	End Sub
	
	Protected Overrides Sub OnCtlValidated( _
                     ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      MyBase.OnCtlValidated(sender, e)
      'Binds each control to the errProvider on the form
      Dim ctl As Control = CType(sender, Control)
      Dim bnd As Binding

      For Each bnd In ctl.DataBindings
         If bnd.IsBinding Then
            Dim obj As System.ComponentModel.IDataErrorInfo = _
                   CType(bnd.DataSource, System.ComponentModel.IDataErrorInfo)
            errProvider.SetError(ctl, obj.Item(bnd.BindingMemberInfo.BindingField))
         End If
      Next
	End Sub
	
	Protected Overrides Sub OnDataChanged( _
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
	   MyBase.OnDataChanged(sender, e)
	End Sub
	
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

	Protected Overrides Sub OnBtnAdd<xsl:value-of select="$childname"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      Me.Add<xsl:value-of select="$childname"/>(sender,e)
   End Sub

	Protected Overrides Sub OnBtnRemove<xsl:value-of select="$childname"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      Me.Remove<xsl:value-of select="$childname"/>(sender,e)
   End Sub

	Protected Overrides Sub OnBtnEdit<xsl:value-of select="$childname"/>Click( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      Me.Edit<xsl:value-of select="$childname"/>(sender, e)   
   End Sub
   
   Protected Sub OnDataGrid<xsl:value-of select="$childname"/>CurrentCellChanged( _
                     ByVal sender As System.Object, _
	                  ByVal e As System.EventArgs) _
	                  Handles dg<xsl:value-of select="$childname"/>.CurrentCellChanged
	   SetState()
	End Sub

	<!--Protected Overrides Sub OnDataGrid<xsl:value-of select="$childname"/>DoubleClick( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
		Dim obj As <xsl:value-of select="$childobject/@Name"/> = <xsl:text/>
		          <xsl:text/>m<xsl:value-of select="$objectname"/>.<xsl:text/>
		          <xsl:value-of select="$childname"/>
		          <xsl:text/>(dg<xsl:value-of select="$childname"/>.CurrentRowIndex)
	   <xsl:for-each select="//orm:Object[@CollectionName=$childname]//orm:Parent[@SingularName!=$objectname]">
		Dim frm<xsl:value-of select="@SingularName"/> As New <xsl:value-of select="@SingularName"/>Edit
		Cursor.Current = Cursors.WaitCursor
		frm<xsl:value-of select="@SingularName"/>.MdiParent = Me.MdiParent
		frm<xsl:value-of select="@SingularName"/>.<xsl:text/>
		       <xsl:value-of select="@SingularName"/> = <xsl:text/>
		       <xsl:value-of select="@Namespace"/>.<xsl:text/>
		       <xsl:value-of select="@Name"/>.Get<xsl:value-of select="@Name"/>(obj)
		Cursor.Current = Cursors.Default
		frm<xsl:value-of select="@SingularName"/>.Show()
		</xsl:for-each>
   End Sub -->
  	
  	
	Private Sub Add<xsl:value-of select="$childname"/>( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
      Dim obj As <xsl:value-of select="$childobject/@Name"/>
      obj = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childobject/@CollectionName"/>.NewItem()
      <!-- REFACTOR: Change the structure so this isn't so ugly  -->
      <xsl:variable name="childparent" select="$childobject//orm:ParentObject[@SingularName=$objectname]"/>
      <xsl:for-each select="$childparent/orm:ParentKeyField">
         <xsl:variable name="ordinal" select="@Ordinal"/>
         <xsl:variable name="childparentkey" select="@Name" />
         <xsl:for-each select="$childparent/orm:ChildKeyField[@Ordinal=$ordinal]">
      obj.<xsl:value-of select="@Name"/> = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childparentkey"/>
         </xsl:for-each>
      </xsl:for-each>
      If Not obj Is Nothing Then
         If Me.Edit<xsl:value-of select="$childname"/>(obj) = Windows.Forms.DialogResult.OK Then
            m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childobject/@CollectionName"/>.AddItem(obj)
            dg<xsl:value-of select="$childname"/>.Refresh
            OnDataChanged(sender, e)
            SetState()
         End If
      End If
	End Sub
	
	<!-- The following commented out section builds something similer to Rocky's exmaples. 
     The one above does not use the multiple screens to edit children 
  	Private Sub Add<xsl:value-of select="$childname"/>( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
	<xsl:for-each select="$otherparents">
	   Add<xsl:value-of select="$childname"/><xsl:value-of select="@SingularName"/>(sender,e)
   </xsl:for-each>
	End Sub
	
	<xsl:for-each select="$otherparents">
	Private Sub Add<xsl:value-of select="$childname"/><xsl:value-of select="@SingularName"/>( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
		<xsl:variable name="parentname" select="@SingularName"/>
	   <xsl:variable name="parentnamespace" select="@Namespace"/>
	   <xsl:if test="string(//orm:Object[@Name=$parentname]/@IsLookup)!='true'">
		Dim dlg<xsl:value-of select="$parentname"/> As New <xsl:value-of select="$parentname"/>Select
		dlg<xsl:value-of select="$parentname"/>.Text = "New <xsl:value-of select="@Name"/>"
		dlg<xsl:value-of select="$parentname"/>.ShowDialog(Me)
		
		<xsl:variable name="temp">
		   <xsl:call-template name="parentnonlookupparams">
		      <xsl:with-param name="parents" select="$parents"/>
		      <xsl:with-param name="object" select="$object" />
		   </xsl:call-template>
		</xsl:variable>
		<xsl:variable name="params" select="msxsl:node-set($temp)" />
		
      <xsl:for-each select="$params//Field">
      ' <xsl:value-of select="@ObjectName"/> - <xsl:value-of select="@ParentName"/> - <xsl:value-of select="@Type"/>
      Dim <xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/> 
      <xsl:choose>
         <xsl:when test="@ObjectName=$objectname">
            <xsl:text/> = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="@ParentProperty"/>
         </xsl:when>
         <xsl:when test="@ObjectName=$parentname"> = <xsl:text/>
            <xsl:call-template name="AssignValue">
               <xsl:with-param name="value" select="concat('dlg',$parentname,'.Result')"/>
               <xsl:with-param name="type" select="@Type"/>
            </xsl:call-template>  
            '.<xsl:value-of select="$parentname"/>.<xsl:value-of select="@Name"/>
         </xsl:when>
      </xsl:choose>
      </xsl:for-each>
      <xsl:variable name="parentproperty" select="$params//Field[@ObjectName=$parentname]/@Name"/>
		If Not (<xsl:text/>
		   <xsl:for-each select="$params/*">WinSupport.Utility.IsEmpty( <xsl:text/>
		               <xsl:value-of select="$parentproperty"/>)<xsl:text/> 
		      <xsl:if test="position()!=last()"> Or </xsl:if>
		   </xsl:for-each>) Then
			dg<xsl:value-of select="$childname"/>.SuspendLayout()
			dg<xsl:value-of select="$childname"/>.DataSource = Nothing
			Try<xsl:text/>
			   ' <xsl:value-of select="count($params)"/>
			   ' <xsl:value-of select="count($params[@IsLookup='true'])"/>
            Dim obj As <xsl:value-of select="$childobject/@Name"/> = _	
            			m<xsl:value-of select="$objectname"/>.<xsl:text/>
            			<xsl:value-of select="$childname"/>.NewItem(<xsl:text/>
		               <xsl:for-each select="$params//Field">
		                  <xsl:value-of select="@Name"/><xsl:text/> 
		                  <xsl:if test="position()!=last()">, </xsl:if>
		               </xsl:for-each> )
            If Not obj Is Nothing Then
               Me.Edit<xsl:value-of select="$childname"/>(obj)
            End If
			Catch ex As Exception
				Windows.Forms.MessageBox.Show(ex.Message)
			Finally
				dg<xsl:value-of select="$childname"/>.DataSource =  m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>
				dg<xsl:value-of select="$childname"/>.ResumeLayout()
			End Try
		End If
	   </xsl:if>
	End Sub
	</xsl:for-each> -->
	
	Private Sub Remove<xsl:value-of select="$childname"/>( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
		Dim obj As <xsl:value-of select="$childobject/@Name"/> = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>(dg<xsl:value-of select="$childname"/>.CurrentRowIndex)
      Dim desc As String = ""<xsl:text/>
      <xsl:call-template name="GetRuntimeChildDesc">
         <xsl:with-param name="childname" select="$childname"/>
      </xsl:call-template>

      
		If MessageBox.Show("Remove <xsl:value-of select="@Name"/> " &amp; desc &amp; " from <xsl:value-of select="$objectname"/>?", _
			      "Remove <xsl:value-of select="@Name"/>", _
			      MessageBoxButtons.YesNo) = DialogResult.Yes Then
			dg<xsl:value-of select="$childname"/>.SuspendLayout()
			dg<xsl:value-of select="$childname"/>.DataSource = Nothing
			m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>.Remove(obj)
			dg<xsl:value-of select="$childname"/>.DataSource =  m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>
			dg<xsl:value-of select="$childname"/>.ResumeLayout()
         OnDataChanged(sender, e)
         SetState()
		End If
	End Sub

   Private Sub Edit<xsl:value-of select="@Name"/>( _ 
	                  ByVal sender As System.Object, _ 
	                  ByVal e As System.EventArgs)
		Dim obj As <xsl:value-of select="$childobject/@Name"/> = m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childname"/>(dg<xsl:value-of select="$childname"/>.CurrentRowIndex)
		Edit<xsl:value-of select="@Name"/>(obj)
   End Sub
   Private Function Edit<xsl:value-of select="@Name"/>( _ 
	                  ByVal obj As <xsl:value-of select="$childobject/@Name"/>) _
	                  As Windows.Forms.DialogResult
      Dim frm As New WinSupport.ChildEditForm
      Dim dlgResult As Windows.Forms.DialogResult
		dlgResult = frm.ShowDialog(New <xsl:value-of select="$childbaseobjectname"/>Edit, obj,  _
		         "<xsl:value-of select="$primarykeys[1]/@Name"/>", _
		         WinSupport.Utility.ParentForm(Me)) 
		If dlgResult = Windows.Forms.DialogResult.OK Then 
         dg<xsl:value-of select="$childname"/>.Refresh
         OnDataChanged(Me, New System.EventArgs)
         SetState()
		End If
      Return dlgResult
   End Function
   </xsl:for-each>
#End Region 
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
#Region " Private and Protected Properties and Methods "
   Protected Sub Clear()
   <xsl:for-each select="$properties">
      <xsl:for-each select="ui:ClearControl">
      Me.<xsl:value-of select="../@ControlName"/>.<xsl:value-of select="@Call" />
      </xsl:for-each>
   </xsl:for-each>
   <xsl:for-each select="$childcollections">
      Me.dg<xsl:value-of select="@Name"/>.DataSource = Nothing
   </xsl:for-each>
   End Sub

	Protected Overridable Sub DataBind()
      Dim bIsDirty As Boolean = mbisdirty
		WinSupport.Utility.BindField(chkIsDirty, "Checked",  m<xsl:value-of select="$objectname"/>, "IsDirty")<xsl:text/>
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
      cbo<xsl:value-of select="$ctlname"/>.DataSource = <xsl:value-of select="$lookupname"/>List.Get<xsl:value-of select="$lookupname"/>List<xsl:text/>
      cbo<xsl:value-of select="$ctlname"/>.ValueMember = "<xsl:value-of select="@Name" />"
      cbo<xsl:value-of select="$ctlname"/>.DisplayMember = "DisplayText"<xsl:text/>
		      </xsl:for-each>
	      </xsl:when>
	   </xsl:choose> 
		Me.BindField(<xsl:value-of select="@ControlName"/>, "<xsl:text/>
		           <xsl:value-of select="@BindProperty"/>",  m<xsl:value-of select="$objectname"/>
		           <xsl:text/>, "<xsl:value-of select="@Name" />", _
		           GetType(<xsl:value-of select="@NETType"/>))<xsl:text/>
	</xsl:for-each>
		lstRules.DataSource =  m<xsl:value-of select="$objectname"/>.GetBrokenRulesCollection
		lstRules.DisplayMember = "Description"
		<xsl:for-each select="$childcollections">
		Databind<xsl:value-of select="@Name"/>Grid()
		</xsl:for-each>
		mbIsDirty = bIsDirty 
	End Sub
	
	<xsl:for-each select="$childcollections">
   <xsl:variable name="childcollection" select="@Name" />
   <xsl:variable name="childshortname" select="substring-after($childcollection, $objectname)"/>
   <xsl:variable name="singularname" select="ancestor::orm:Assembly//orm:Object[@CollectionName=$childcollection]/@Name"/>
   ' <xsl:value-of select="$singularname"/>
	Protected Sub Databind<xsl:value-of select="$childcollection"/>Grid()
	   Dim dgtbs As New DataGridTableStyle
	   Dim dgcs As DataGridColumnStyle
      Dim graphics As Drawing.Graphics = Me.CreateGraphics
		With dg<xsl:value-of select="$childcollection"/>
		   .SuspendLayout()
   	   If Not mIsLoaded Then 
		      dgtbs.MappingName = "<xsl:value-of select="$childcollection"/>"
		      dgtbs.RowHeadersVisible = True
		      .RowHeaderWidth = 20
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

		      .TableStyles.Add(dgtbs)
		   End If
		   .SetDataBinding(m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childcollection"/>, "")
		   .ResumeLayout()
	   End With
	End Sub
	
   <xsl:for-each select="//orm:Object[@CollectionName=$childcollection]//orm:Property[not(@IsLookup='true')]">
	Protected Overridable Function Get<xsl:value-of select="$childcollection"/>_<xsl:value-of select="@Name"/>ColumnWidth( _
	              ByVal font As Drawing.Font, _
	              ByVal graphics As Drawing.Graphics) _
	              As Int32
      Dim temp As Int32
      Dim colWidth As Int32
      <xsl:choose>
         <xsl:when test="@NETType='System.Boolean'">
      colWidth = WinSupport.Utility.GetWidth("W", font, graphics)
         </xsl:when>
         <xsl:when test="@NETType='System.DateTime' or @NETType='CSLA.SmartDate'">
      colWidth = WinSupport.Utility.GetWidth("99/99/9999", font, graphics)
         </xsl:when>
         <xsl:otherwise>
      For Each bo As <xsl:value-of select="$singularname"/> In m<xsl:value-of select="$objectname"/>.<xsl:value-of select="$childcollection"/>
         temp = WinSupport.Utility.GetWidth(bo.<xsl:value-of select="@Name"/>.ToString, font, graphics)
         If temp > colWidth Then
            colWidth = temp
         End If 
      Next
         </xsl:otherwise>
      </xsl:choose> 
      temp = WinSupport.Utility.GetWidth("<xsl:value-of select="@Caption"/>", font, graphics)
      If temp > colWidth Then
         colWidth = temp
      End If
      Return colWidth
   End Function      
   </xsl:for-each>
	</xsl:for-each>
	
	Private Sub SetState()
	   Dim editMode As WinSupport.EditMode = Me.EditMode
	   <xsl:for-each select="$childcollections">
      	<xsl:variable name="childname" select="@Name" />
      btnAdd<xsl:value-of select="$childname"/>.Enabled = _
                        (editMode = WinSupport.EditMode.IsClean) Or _
                        (editMode = WinSupport.EditMode.IsDirty)
	   btnRemove<xsl:value-of select="$childname"/>.Enabled = _
	                     (dg<xsl:value-of select="$childname"/>.CurrentRowIndex >= 0) And _
                        ((editMode = WinSupport.EditMode.IsClean) Or _
                        (editMode = WinSupport.EditMode.IsDirty)) 	   
	   btnEdit<xsl:value-of select="$childname"/>.Enabled = _
	                     (dg<xsl:value-of select="$childname"/>.CurrentRowIndex >= 0) And _
                        ((editMode = WinSupport.EditMode.IsClean) Or _
                        (editMode = WinSupport.EditMode.IsDirty)) 	   
	   </xsl:for-each>

		Dim bEnabled As Boolean = (Me.EditMode &lt;> EditMode.IsEmpty)
	   <xsl:for-each select="$properties">
	   Me.<xsl:value-of select="@ControlName"/>.Enabled = bEnabled
	   </xsl:for-each>

	End Sub

#End Region 
</xsl:template>

<xsl:template match="orm:Property" mode="CreateDGColumn">
   <xsl:param name="parentprimarykeys"/>
   <xsl:param name="childcollection"/>
	<xsl:variable name="childcolumnname" select="@Name" />
	<xsl:if test="not($parentprimarykeys[@Name=$childcolumnname])">
	<xsl:choose>
		<xsl:when test="@NETType='System.Boolean'">
	         dgcs = New DataGridBoolColumn
		</xsl:when>
		<xsl:otherwise>
	         dgcs = New DataGridTextBoxColumn
		</xsl:otherwise>
	</xsl:choose>
            dgcs.MappingName = "<xsl:value-of select="@Name"/>"
            dgcs.HeaderText = "<xsl:value-of select="@Caption"/>"
            dgcs.Width = Get<xsl:value-of select="$childcollection"/>_<xsl:value-of select="@Name"/>ColumnWidth(font, graphics) + HorizontalMargin
   <xsl:if test="@NETType='System.Decimal'">
            CType(dgcs, Windows.Forms.DataGridTextBoxColumn).Format = "c"
   </xsl:if>
            dgtbs.GridColumnStyles.Add(dgcs)
	</xsl:if>
</xsl:template>

</xsl:stylesheet> 
 