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
  Summary: Generates the plumbing class for business object collections !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="CollectionName"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@CollectionName=$CollectionName]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports CSLA.Data
Imports CSLA

&lt;Serializable()> _
Public Class gen<xsl:value-of select="$CollectionName"/>
	Inherits BusinessCollectionBase
	
	Protected mIsLoaded As Boolean 
	<xsl:call-template name="BusinessPropertiesAndMethods" />
	<xsl:call-template name="Contains" />
	<xsl:call-template name="SharedMethods" />
	<xsl:call-template name="Constructors" />
	<xsl:call-template name="Criteria" />
	<xsl:call-template name="DataAccess" />
End Class
</xsl:template>


<xsl:template name="BusinessPropertiesAndMethods">
#Region " Business Properties and Methods "
   <xsl:variable name="object" select="." />
   <xsl:variable name="childof" select="@ChildOf" />
	<xsl:variable name="otherparents" select="orm:ParentObject[@SingularName!=$childof]"/>
	<xsl:variable name="parents" select="orm:ParentObject"/>

	Default Public ReadOnly Property Item(ByVal Index As Integer) _
					As <xsl:value-of select="$Name"/>
		Get
			Return CType(list.Item(Index), <xsl:value-of select="$Name"/>)
		End Get
	End Property
	
	Public Function GetItem( _<xsl:text/>
	      <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
	            ByVal <xsl:value-of select="@Name"/> As <xsl:text/>
	            <xsl:value-of select="@NETType"/>
	            <xsl:if test="position()!=last()">, _</xsl:if>
	      </xsl:for-each>) _
					As <xsl:value-of select="$Name"/>
	   <xsl:call-template name="FindInList"/>
   End Function	      
	
	Public ReadOnly Property IsLoaded() As Boolean
	   Get 
	      Return mIsLoaded
	   End Get
	End Property
  
<xsl:if test="$Root='true'">
	Public Sub Add()
		List.Add(<xsl:value-of select="$Name"/>.New<xsl:text/>
					<xsl:value-of select="$Name"/>())
	End Sub
</xsl:if>

	Public Overridable Sub Remove(ByVal <xsl:value-of select="@Name" /> As <xsl:value-of select="@Name" />)
		list.Remove(<xsl:value-of select="@Name" />)
	End Sub
	
	<xsl:if test="@CollectionChild='true'">
	<xsl:variable name="parent" select="." />
	<!-- This creates a temporary XML tree, uses nodeset to change it to a tree, then iterates in the code that builds the methods -->
	<xsl:variable name="temp">
      <xsl:call-template name="otherparentparams">
         <xsl:with-param name="otherparents" select="$otherparents"/>
         <xsl:with-param name="object" select="$object"/>
      </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="tempparams" select="msxsl:node-set($temp)"/>
	<xsl:variable name="temp2">
      <xsl:call-template name="parentnonlookupparams">
         <xsl:with-param name="parents" select="$otherparents"/>
         <xsl:with-param name="object" select="$object"/>
      </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="tempparamsnolookup" select="msxsl:node-set($temp2)"/>
	<xsl:variable name="temp3">
      <xsl:call-template name="parentnonlookupparams">
         <xsl:with-param name="parents" select="$parents"/>
         <xsl:with-param name="object" select="$object"/>
      </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="tempallnolookup" select="msxsl:node-set($temp3)"/>
	<xsl:if test="$tempparams/*">
	Public Overridable Function NewItem(<xsl:text/> _<xsl:text/>
	   <xsl:for-each select="$tempparams//Field">
	            ByVal <xsl:value-of select="@ObjectName"/> As <xsl:value-of select="@ObjectType"/>
	      <xsl:if test="last()!=position()">, _<xsl:text/>
	      </xsl:if>
	   </xsl:for-each>) _
	            As <xsl:value-of select="$object/@Name"/>
	   Dim obj As <xsl:value-of select="$object/@Name"/>
		obj = <xsl:value-of select="$object/@Name" />.New<xsl:value-of select="$object/@Name" />Child(<xsl:text/>
	   <xsl:for-each select="$tempparams//Field" >
	      <xsl:value-of select="@ObjectName"/>
	      <xsl:if test="last()!=position()">, </xsl:if> 
	   </xsl:for-each>)
		If MakeNewItem(obj) Then
		   Return obj
		End If
	End Function
	</xsl:if>
	
	'nolookup
	<xsl:apply-templates select="$tempparamsnolookup" mode="NewItemContents"/>
   
	'nolookup
	<xsl:if test="count($tempallnolookup//Field) != count($tempparamsnolookup//Field)">
	   <xsl:apply-templates select="$tempallnolookup" mode="NewItemContents"/>
	</xsl:if>
	
	Public Function NewItem() As <xsl:value-of select="$Name"/>
	   Dim obj As <xsl:value-of select="$Name"/>
	   obj = <xsl:value-of select="$Name"/>.NewItemChild()
	   Return obj
	End Function 
   
	Protected Overridable Function MakeNewItem( _
	         ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@Name"/>) _
	         As Boolean
		If Not Contains(<xsl:value-of select="@Name"/>) Then
			list.Add(<xsl:value-of select="@Name"/>)
			Return True
		Else
			Throw New Exception("<xsl:value-of select="@Name"/> already assigned")
		End If
	End Function

   <!-- Added to support scenario 
   Steps:
      1. Create a new address object
      2. Use that object to open and populate the edit form
      3. User edits, including setting values of composit PK, other than app id
      4. User saves
      5. See if collection contains the object
      6. If yes, delete existing and add object
      7. If no, add object
   If you don't use this scenario, don't include this method. -->
   Public Sub AddItem(ByVal bObj As <xsl:value-of select="@Name"/>)
		MakeNewItem(bObj)
   End Sub
   
	Public Overloads Function Contains( _
	   <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	      <xsl:text/>ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@NETType"/>
	      <xsl:if test="position()!=last()">, _
	      </xsl:if>
	   </xsl:for-each>) As Boolean

		Return (Not GetItem( <xsl:text/>
	   <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	      <xsl:text/><xsl:value-of select="@Name"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) Is Nothing)
	End Function

	Public Overloads Function ContainsDeleted( _
	   <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	      <xsl:text/>ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@NETType"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) As Boolean
      <xsl:call-template name="FindInList">
         <xsl:with-param name="listname" select="'deletedlist'"/>
         <xsl:with-param name="return" select="'true'"/>
      </xsl:call-template>
	End Function
	</xsl:if>

#End Region
</xsl:template>

<xsl:template match="BaseObject" mode="NewItemContents">
	<xsl:if test="count(//Field)!=0">
	Public Overridable Function NewItem(<xsl:text/> _<xsl:text/>
	   <xsl:for-each select="//Field">
	            ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/>
	      <xsl:if test="last()!=position()">, _<xsl:text/>
	      </xsl:if>
	   </xsl:for-each>) _
	            As <xsl:value-of select="/BaseObject/@Name"/>
	   Dim obj As <xsl:value-of select="/BaseObject/@Name"/>
		obj = <xsl:value-of select="/BaseObject/@Name" />.New<xsl:value-of select="/BaseObject/@Name" />Child(<xsl:text/>
	   <xsl:for-each select="//Field" >
	      <xsl:value-of select="@Name"/>
	      <xsl:if test="last()!=position()">, </xsl:if> 
	   </xsl:for-each>)
		If MakeNewItem(obj) Then
		   Return obj
		End If
	End Function
	</xsl:if>
</xsl:template>

<xsl:template name="Contains">

#Region " Contains "

	Public Overloads Function Contains( _
					ByVal Item As <xsl:value-of select="$Name"/>) As Boolean

		Dim child As <xsl:value-of select="$Name"/>

		For Each child In list
			If child.Equals(Item) Then
				Return True
			End If
		Next
		Return False
	End Function

	Public Overloads Function ContainsDeleted( _
					ByVal Item As <xsl:value-of select="$Name"/>) As Boolean

		Dim child As <xsl:value-of select="$Name"/>

		For Each child In deletedList
			If child.Equals(Item) Then
				Return True
			End If
		Next
		Return False
	End Function

#End Region
</xsl:template>


<xsl:template name="SharedMethods">
#Region " Shared Methods "
<xsl:if test="@CollectionRoot='true'">
	Public Shared Function New<xsl:value-of select="$CollectionName"/>() _
					As <xsl:value-of select="$CollectionName"/>
		Return New <xsl:value-of select="$CollectionName"/>
	End Function
	
	Public Shared Function Get<xsl:value-of select="$CollectionName"/>( _
					<xsl:call-template name="PrimaryKeyArguments"/>) _
					As <xsl:value-of select="$CollectionName"/>
		Return CType(DataPortal.Fetch(New Criteria(<xsl:call-template name="PrimaryKeyList"/>)), <xsl:value-of select="$CollectionName"/>)
	End Function
	
	Public Shared Sub DeleteCollection(<xsl:call-template name="PrimaryKeyArguments"/>)
		DataPortal.Delete(New Criteria(<xsl:call-template name="PrimaryKeyList"/>))
	End Sub	
</xsl:if> 

<xsl:if test="@CollectionChild='true'">
	Friend Shared Function New<xsl:value-of select="$CollectionName"/>Child() As <xsl:value-of select="$CollectionName"/>
		Return New <xsl:value-of select="$CollectionName"/>
	End Function

	Friend Shared Function Get<xsl:value-of select="$CollectionName"/>Child( _
		ByVal dr As SafeDataReader) As <xsl:value-of select="$CollectionName"/>
		Dim col As New <xsl:value-of select="$CollectionName"/>
		col.Fetch(dr)
		Return col
	End Function
</xsl:if>
#End Region
</xsl:template>

<xsl:template name="Constructors">
#Region " Constructors "

	Protected Sub New()
		' disallow direct creation

		' mark us as a child collection
		MarkAsChild()
	End Sub

#End Region
</xsl:template>

<xsl:template name="Criteria">
<xsl:if test="@CollectionRoot='true'">
#Region " Criteria "
	<!-- You have to escape any less than signs that you want in the output -->
	&lt;Serializable()> _
	Protected Class Criteria
		Inherits CSLA.Server.CriteriaBase
	<xsl:call-template name="PrimaryKeyDeclarations">
		<xsl:with-param name="prefix" select="'m'" />
	</xsl:call-template>
	 
		Public Sub New(<xsl:call-template name="PrimaryKeyArguments"/>)<xsl:text/>
    		MyBase.New(GetType(<xsl:value-of select="$CollectionName"/>))
			<xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
			Me.<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>
			</xsl:for-each>
		End Sub
		
  <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		Public Property <xsl:value-of select="@Name" />() _
							As <xsl:value-of select="@NETType" />
			Get
				Return m<xsl:value-of select="@Name" />
			End Get
			Set (Value As <xsl:value-of select="@NETType" />)
				m<xsl:value-of select="@Name" /> = Value
			End Set
		End Property
</xsl:for-each>
		
		Public Sub AddParameters(cmd As IDBCommand)
		   Dim p As System.Data.SQLClient.SQLParameter
		<xsl:apply-templates select="orm:Property[@IsPrimaryKey='true']" mode="ParameterAssignment"/>
		End Sub 
		
	End Class

#End Region
</xsl:if>
</xsl:template>

<xsl:template name="DataAccess">

#Region " Data Access "

<xsl:if test="@CollectionRoot='true'">
	<xsl:call-template name="DataPortalFetch">
		<xsl:with-param name="iscollection" select="'true'"/>
	</xsl:call-template>
	
	Protected Overrides Sub DataPortal_Update()
		' Loop through each deleted child object and call its Update() method
		For Each child As <xsl:value-of select="$Name"/> In deletedList
			child.Update(<xsl:if test="@ChildOf">Nothing</xsl:if>)
		Next
		
		' Then clear the list of deleted objects because they are truly gone now
		deletedList.Clear
		
		' Loop through each non-deleted child object and call its Update() method
		For Each child As <xsl:value-of select="$Name"/> In list
			child.Update(<xsl:if test="@ChildOf">Nothing</xsl:if>)
		Next
	End Sub
	
	<!-- This code is in Rocky's book, but is apparently not needed 
	Protected Overrides Sub DataPortal_Delete(ByVal Criteria As Object)
		Dim crit As Criteria = CType(Criteria, Criteria)
		' TODO: Delete child object data that matches the criteria (the comment without implementation is from Rockys book)
	End Sub -->
</xsl:if> 

<xsl:if test="@CollectionChild='true'">
	' called to load data from the database
	Private Sub Fetch(ByVal dr As SafeDataReader)
	   Dim obj As <xsl:value-of select="$Name"/>
	   obj = <xsl:value-of select="$Name"/>.Get<xsl:value-of select="$Name"/>Child(dr)
		Do While Not obj Is Nothing
			List.Add(obj)
		   obj = <xsl:value-of select="$Name"/>.Get<xsl:value-of select="$Name"/>Child(dr)
		Loop 
		mIsLoaded = True
	End Sub

	' called by Project to delete/add/update data into the database
	Friend Sub Update(<xsl:text/>
		<xsl:if test="@ChildOf">
			<xsl:text/>ByVal <xsl:value-of select="@ChildOf"/> As <xsl:value-of select="@ChildOf"/>
		</xsl:if>)
		Dim obj As <xsl:value-of select="@Name"/>

		' update (thus deleting) any deleted child objects
		For Each obj In deletedList
			obj.Update(<xsl:if test="@ChildOf"><xsl:value-of select="@ChildOf"/></xsl:if>)
		Next
		' now that they are deleted, remove them from memory too
		deletedList.Clear()

		' add/update any current child objects
		For Each obj In List()
			obj.Update(<xsl:if test="@ChildOf"><xsl:value-of select="@ChildOf"/></xsl:if>)
		Next
	End Sub
</xsl:if> 
#End Region

</xsl:template>

<xsl:template name="StuffForChild">
	Default Public ReadOnly Property Item(ByVal ResourceID As String) As <xsl:value-of select="$Name"/>
		Get
			Dim r As ProjectResource

			For Each r In list
				If r.ResourceID = ResourceID Then
					Return r
				End If
			Next
			Return Nothing
		End Get
	End Property

	Public Sub Remove(ByVal ResourceID As String)
		Dim r As ProjectResource

		For Each r In list
			If r.ResourceID = ResourceID Then
				Remove(r)
				Exit For
			End If
		Next
	End Sub
 
</xsl:template>

<xsl:template name="FindInList">
   <xsl:param name="listname" select="'list'"/>
   <xsl:param name="return" select="'r'"/>
   Dim r As <xsl:value-of select="@Name"/>

   For Each r In <xsl:value-of select="$listname"/>
		If <xsl:text/>
		<xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
			<xsl:call-template name="Comparison">
			   <xsl:with-param name="first" select="concat('r.',@Name)"/>
			   <xsl:with-param name="second" select="@Name"/>
			   <xsl:with-param name="operand" select="'='"/>
			   <xsl:with-param name="type" select="@NETType"/>
			</xsl:call-template> 
	      <xsl:if test="position()!=last()"> And </xsl:if>
	   </xsl:for-each> 
			Return <xsl:value-of select="$return"/>
		End If
	Next
</xsl:template>

</xsl:stylesheet> 
  