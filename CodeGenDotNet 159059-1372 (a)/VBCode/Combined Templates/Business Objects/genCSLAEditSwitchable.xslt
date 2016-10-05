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
  Summary: Generates the plumbing class for business objects !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt"
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" 
			xmlns:log="http://kadgen.com/NETTools" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>

<!--<xsl:key match="//orm:Object//orm:Property"  
         name="Properties"
         use="concat(ancestor::orm:Object/@Name,'-',@Name)" /> -->

<xsl:variable name="properties" 
             select="//orm:Object[@Name=$Name]//orm:Property"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]" 
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
Imports KADGen.BusinessObjectSupport

&lt;Serializable()> _
Public Mustinherit Class gen<xsl:value-of select="$Name"/>
<xsl:choose>
	<xsl:when test="@Inherits">
	Inherits <xsl:value-of select="@Inherits"/>
	</xsl:when>
	<xsl:otherwise>
	Inherits BusinessBase
	</xsl:otherwise>
</xsl:choose>
<xsl:if test="string-length(@Inherits)=0">
   Implements IBusinessObject
</xsl:if> 
   <xsl:call-template name="ClassDeclarations"/>
   <!--<xsl:value-of select="log:LogEntry('Before BusinessPropertiesAndMethods')"/>-->
   <xsl:call-template name="BusinessPropertiesAndMethods"/>
   <!--<xsl:value-of select="log:LogEntry('Before SystemObjectOverrides')"/>-->
   <xsl:call-template name="SystemObjectOverrides"/>
   <!--<xsl:value-of select="log:LogEntry('Before SharedMethods')"/>-->
   <xsl:call-template name="SharedMethods"/>
   <!--<xsl:value-of select="log:LogEntry('Before Constructors')"/>-->
   <xsl:call-template name="Constructors"/>
   <!--<xsl:value-of select="log:LogEntry('Before Criteria')"/>-->
   <xsl:call-template name="Criteria"/>
   <!--<xsl:value-of select="log:LogEntry('Before DataAccess')"/>-->
   <xsl:call-template name="DataAccess"/>
   <!--<xsl:value-of select="log:LogEntry('Before InternalMethodsAndProperties')"/>-->
   <xsl:call-template name="InternalMethodsAndProperties"/>
   <!--<xsl:value-of select="log:LogEntry('End of Class')"/>-->
End Class
<!--<xsl:value-of select="log:WriteLogAndClear(concat($filename,'.log'))"/>-->
</xsl:template>


<xsl:template name="ClassDeclarations">
#Region " Class Declarations "
<xsl:if test="string-length(@Inherits)=0">
   Protected mIsLoaded As Boolean
</xsl:if> 
<xsl:variable name="inherits" select="@Inherits" />
<xsl:for-each select="$properties">
   <xsl:variable name="propname" select="@Name"/>
   <xsl:if test="count(//orm:Object[@Name=$inherits]//orm:Property[@Name=$propname])=0">
   <!--<xsl:if test="count(key('Properties',concat($inherits,'-',$propname)))=0">-->
	Private m<xsl:value-of select="@Name"/> As <xsl:value-of select="@NETType"/> 
   </xsl:if>
</xsl:for-each>
<xsl:text>&#x0d;&#x0a;</xsl:text>
<xsl:if test="string-length($inherits)=0">
   <xsl:for-each select="orm:ChildCollection ">
	Protected m<xsl:value-of select="@Name"/> As <xsl:value-of select="@Name"/> = _
         <xsl:value-of select="@Name"/>.New<xsl:value-of select="@Name"/>Child()<xsl:text/>
   </xsl:for-each>
</xsl:if>
#End Region
</xsl:template>

<xsl:template name="BusinessPropertiesAndMethods">
#Region " Business Properties and Methods "
<xsl:if test="string-length(@Inherits)=0">
   Public ReadOnly Property IsLoaded() As Boolean
      Get
         Return mIsLoaded
      End Get
   End Property
</xsl:if>

<xsl:variable name="inherits" select="@Inherits" />
<xsl:for-each select="$properties">
   <xsl:variable name="propname" select="@Name"/>
   <xsl:if test="count(//orm:Object[@Name=$inherits]//orm:Property[@Name=$propname])=0">
   <!--<xsl:if test="count(key('Properties',concat($inherits,'-',$propname)))=0">-->
		<xsl:call-template name="Property"/>
   </xsl:if>
</xsl:for-each>

<xsl:if test="string-length(@Inherits)=0">
	<xsl:for-each select="orm:ChildCollection">
	Public ReadOnly Property <xsl:value-of select="@Name"/>() As <xsl:value-of select="@Name"/>
		Get
			Return m<xsl:value-of select="@Name"/>
		End Get
	End Property
	</xsl:for-each>

	Public Shared Function CanRetrieve() _
				As Boolean<xsl:text/> 
   <xsl:for-each select="orm:RetrieveSP">
      <xsl:call-template name="CanPrivileges">
         <xsl:with-param name="privilegetype" select="'R'"/>
      </xsl:call-template>
	</xsl:for-each>
	End Function

	Public Shared Function CanCreate() _
				As Boolean<xsl:text/> 
   <xsl:for-each select="orm:CreateSP">
      <xsl:call-template name="CanPrivileges">
         <xsl:with-param name="privilegetype" select="'C'"/>
      </xsl:call-template>
	</xsl:for-each>
	End Function

	Public Shared Function CanDelete() _
				As Boolean<xsl:text/> 
   <xsl:for-each select="orm:DeleteSP">
      <xsl:call-template name="CanPrivileges">
         <xsl:with-param name="privilegetype" select="'D'"/>
      </xsl:call-template>
	</xsl:for-each>
	End Function

	Public Shared Function CanUpdate() _
				As Boolean<xsl:text/> 
   <xsl:for-each select="orm:UpdateSP">
      <xsl:call-template name="CanPrivileges">
         <xsl:with-param name="privilegetype" select="'U'"/>
      </xsl:call-template>
	</xsl:for-each>
	End Function

	Private Function getCanCreate() _
				As Boolean _
				Implements IBusinessObject.CanCreate
	   Return Me.CanRetrieve()
	End Function

	Private Function getCanRetrieve() _
				As Boolean _
				Implements IBusinessObject.CanRetrieve
	   Return Me.CanRetrieve()
	End Function

	Private Function getCanUpdate() _
				As Boolean _
				Implements IBusinessObject.CanUpdate
	   Return Me.CanRetrieve()
	End Function

	Private Function getCanDelete() _
				As Boolean _
				Implements IBusinessObject.CanDelete
	   Return Me.CanRetrieve()
	End Function

   Public Shared ReadOnly Property Caption() As String 
      Get
         Return "<xsl:value-of select="$Name" />"
      End Get
   End Property

   Private ReadOnly Property GetCaption() As String _
				Implements IBusinessObject.Caption
      Get
         Return Caption
      End Get
   End Property

   Public Shared ReadOnly Property ObjectName() As String 
      Get
         Return "<xsl:value-of select="$Name" />"
      End Get
   End Property

   Private ReadOnly Property GetObjectName() As String _
				Implements IBusinessObject.ObjectName
      Get
         Return ObjectName
      End Get
   End Property

</xsl:if>

<!-- Added 11/29/03 becuase the required class level variables don't exist -->
<xsl:if test="string-length($inherits)=0">

	Public Overrides ReadOnly Property IsValid() _
	         As Boolean <xsl:text/>
	   <xsl:if test="string-length(@Inherits)=0"> _
				Implements IBusinessObject.IsValid
	   </xsl:if>
		Get
			Return MyBase.IsValid <xsl:text/>
	<xsl:for-each select="orm:ChildCollection">
      <xsl:text/> AndAlso m<xsl:value-of select="@Name"/>.IsValid<xsl:text/>
	</xsl:for-each>
		End Get
	End Property

	Public Overrides ReadOnly Property IsDirty() _
	         As Boolean _
	   <xsl:if test="string-length(@Inherits)=0"> _
				Implements IBusinessObject.IsDirty
	   </xsl:if>
		Get
			Return MyBase.IsDirty<xsl:text/>
	<xsl:for-each select="orm:ChildCollection">
      <xsl:text/> OrElse m<xsl:value-of select="@Name"/>.IsDirty<xsl:text/>
	</xsl:for-each>
		End Get
	End Property
</xsl:if>
	
#End Region
</xsl:template>

<xsl:template name="SystemObjectOverrides">
#Region " System.Object Overrides "

<xsl:if test="string-length(@Inherits)=0">

	Public Overrides Function ToString() As String
	 	 Return <xsl:text />
	 	 <xsl:choose>
	 	    <xsl:when test="count(orm:Property[@UseForDesc='true']) > 0">
            <xsl:for-each select="orm:Property[@UseForDesc='true']">
               <xsl:sort select="@UseForDescOrdinal" data-type="number" />
	            <xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:text/>
	            <xsl:if test="position()!=last()"> 
	               <xsl:text/> &amp; "<xsl:text/>
	               <xsl:choose>
	                  <xsl:when test="@UseForDescDelimiter">
	                     <xsl:value-of select="@UseForDescDelimiter"  />
	                  </xsl:when>
	                  <xsl:otherwise>, </xsl:otherwise>
	               </xsl:choose>" &amp; _
	            </xsl:if>
            </xsl:for-each>
	 	    </xsl:when>
	 	    <xsl:otherwise >UniqueKey</xsl:otherwise>
	 	 </xsl:choose>
  <!-- <xsl:for-each select="$properties[@IsPrimaryKey='true']">
	<xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:if test="position()!=last()"> &amp; </xsl:if>
  </xsl:for-each> -->
	End Function

	Public Overloads Function Equals(ByVal <xsl:value-of select="$Name"/> As <xsl:value-of select="$Name"/>) As Boolean
		Return <xsl:text />
  <xsl:for-each select="$properties[@IsPrimaryKey='true']">
	<xsl:text/>m<xsl:value-of select="@Name"/>.Equals(<xsl:value-of select="$Name"/>.<xsl:text/>
	         <xsl:value-of select="@Name"/>)<xsl:if test="position()!=last()"> AndAlso </xsl:if>
  </xsl:for-each>
	End Function
	
	Public Property UniqueKey() _
	            As String _
	            Implements BusinessObjectSupport.IBusinessObject.UniqueKey
      Get
	      Return <xsl:text/>
	  <!-- THe followign assumes all primary keys are in set select -->
  <xsl:for-each select="$properties[@IsPrimaryKey='true']">
	<xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:if test="position()!=last()"> &amp; </xsl:if>
  </xsl:for-each>
      End Get
      Set
         Throw New System.Exception("Unexpected call to setUniqueKey")
      End Set
   End Property
   
   Public Overridable Property DisplayText() _
	            As String _
	            Implements BusinessObjectSupport.IBusinessObject.DisplayText
      Get
	      Return ToString
      End Get
      Set
         Throw New System.Exception("Unexpected call to DisplayText")
      End Set
   End Property

	Public Overrides Function GetHashCode() As Integer
		Return UniqueKey.GetHashCode
	End Function
</xsl:if>
#End Region
</xsl:template>

<xsl:template name="SharedMethods">
#Region " Shared Methods "

	' create new object
<xsl:if test="$Root='true'">
	Public Shared Function New<xsl:value-of select="$Name"/>() _
						As <xsl:value-of select="$Name"/>
		If Not CanCreate() Then
		   Throw New System.Security.SecurityException( _
            "<xsl:value-of select="concat('User not authorized to add a ', $Name)"/>")
		Else
		   Dim obj As New <xsl:value-of select="$Name"/>
		   obj.MarkClean
	   	Return obj 
	   End If
	End Function
	
	' load existing object by id
	Public Shared Function Get<xsl:value-of select="$Name"/>(<xsl:text/>
            <xsl:call-template name="PrimaryKeyArguments"/>) _
					As <xsl:value-of select="$Name"/>
		If Not CanRetrieve() Then
		   Throw New System.Security.SecurityException( _
            "<xsl:value-of select="concat('User not authorized to retrieve a ', $Name)"/>")
		Else
		   Return CType(DataPortal.Fetch(New Criteria(<xsl:text/>
            <xsl:call-template name="PrimaryKeyList"/>)), <xsl:text/>
			   <xsl:value-of select="@Name"/>)
	   End If
	End Function
	
	<xsl:variable name="deletefailuremsg" select="concat('User not authorized to delete a ', $Name)" />
	' delete object
	<!-- Only available for root -->
	Public Shared Sub Delete<xsl:value-of select="$Name"/>(<xsl:text/>
                   <xsl:call-template name="PrimaryKeyArguments"/>)<xsl:text/>
		If Not CanDelete() Then
		   Throw New System.Security.SecurityException( _
            "<xsl:value-of select="$deletefailuremsg"/>")
		Else
		   DataPortal.Delete(New Criteria(<xsl:call-template name="PrimaryKeyList"/>))
		End If
	End Sub

	<!-- Only available for root -->
	Public Overrides Function Save() _
	         As BusinessBase 
		If IsDeleted Then<xsl:text/>
		   If Not CanDelete() Then
		      Throw New System.Security.SecurityException( _
               "<xsl:value-of select="$deletefailuremsg"/>")
		   End If
		Else
		   If Not CanUpdate() Then
		      Throw New System.Security.SecurityException( _
               "<xsl:value-of select="concat('User not authorized to update a ', $Name)"/>")
		   End If
		End If
		Return MyBase.Save
	End Function
	<xsl:if test="string-length(@Inherits)=0"> _
	Private Function ISave() _
	         As IBusinessObject _
				Implements IBusinessObject.Save
		Return CType(Me.Save, IBusinessObject)
	End Function
	
	Private Function IGetNew() _
	         As IBusinessObject _
				Implements IBusinessObject.GetNew
		Return CType(Me.New<xsl:value-of select="$Name"/>, IBusinessObject)
	End Function
	</xsl:if>
</xsl:if>
  
<xsl:if test="$Child='true'">
	<xsl:variable name="childof" select="@ChildOf"/>
	<xsl:variable name="otherparents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
	<xsl:variable name="parents" select="orm:ParentObject"/>
	Friend Shared Function New<xsl:value-of select="$Name"/>Child(<xsl:text/>
      <xsl:for-each select="$otherparents">
	      <xsl:text/> _
	         ByVal <xsl:value-of select="@SingularName"/> As <xsl:value-of select="@SingularName"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) _
						As <xsl:value-of select="$Name"/>
		<xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		Dim obj As <xsl:value-of select="$Name"/> 
		obj = New <xsl:value-of select="$Name"/>()
		obj.Init(<xsl:text/>
      <xsl:for-each select="$otherparents">
         <xsl:value-of select="@SingularName"/>
         <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>)
		obj.MarkAsChild
		Return obj
	End Function
	
	Friend Shared Shadows Function NewItemChild() As <xsl:value-of select="$Name" />
	   Dim obj As New <xsl:value-of select="$Name" />
		obj.MarkAsChild
	   Return obj
	End Function 
	
	<xsl:variable name="temp">
	   <xsl:call-template name="parentnonlookupparams">
	      <xsl:with-param name="object" select="."/>
	      <xsl:with-param name="parents" select="$otherparents" />
	   </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="nolookups" select="msxsl:node-set($temp)"/>
	<!--<xsl:if test="count($nolookups/*)!=count($otherparents)">-->
	<xsl:if test="count($nolookups/BaseObject/*)>0">
   Friend Shared Function New<xsl:value-of select="$Name"/>Child(<xsl:text/>
      <xsl:for-each select="$nolookups//Field">
	      <xsl:text/> _
	         ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) _
						As <xsl:value-of select="$Name"/>
		<xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		Dim obj As <xsl:value-of select="$Name"/> 
		obj = New <xsl:value-of select="$Name"/>()
		obj.Init(<xsl:text/>
      <xsl:for-each select="$nolookups//Field">
         <xsl:value-of select="@Name"/>
         <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>)
		obj.MarkAsChild
		Return obj
	End Function
   </xsl:if>

	<xsl:variable name="temp2">
	   <xsl:call-template name="parentnonlookupparams">
	      <xsl:with-param name="object" select="."/>
	      <xsl:with-param name="parents" select="$parents" />
	   </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="allnolookups" select="msxsl:node-set($temp2)"/>
	<!--<xsl:if test="count($allnolookups/*)!=count($parents)">-->
	<xsl:if test="count($allnolookups//Field)>0 and (count($allnolookups//Field) != count($nolookups//Field))">
   Friend Shared Function New<xsl:value-of select="$Name"/>Child(<xsl:text/>
      <xsl:for-each select="$allnolookups//Field">
	      <xsl:text/> _
	         ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) _
						As <xsl:value-of select="$Name"/>
		<xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		Dim obj As <xsl:value-of select="$Name"/> 
		obj = New <xsl:value-of select="$Name"/>()
		obj.Init(<xsl:text/>
      <xsl:for-each select="$allnolookups//Field">
         <xsl:value-of select="@Name"/>
         <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>)
		obj.MarkAsChild
		Return obj
	End Function
   </xsl:if>
   
	Friend Shared Function Get<xsl:value-of select="$Name"/>Child( _
						ByVal dr As SafeDataReader) _
						As <xsl:value-of select="$Name"/>
		<xsl:apply-templates select="orm:RetrieveSP" mode="RetrievePrivileges"/>
		Dim obj As New <xsl:value-of select="$Name"/> 
		If obj.AssignFromDataReader(dr) Then 
		   obj.MarkAsChild
   		Return obj
   	Else
   	   Return Nothing
		End If
	End Function
</xsl:if>

#End Region
</xsl:template>

<xsl:template name="Constructors">
#Region " Constructors "

<xsl:if test="@ChildOf">
	<xsl:variable name="object" select="."/>
	<xsl:variable name="childof" select="@ChildOf"/>
	<xsl:variable name="parents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
	<xsl:if test="count($parents)!=0">
   Protected Sub New(<xsl:text/>
      <xsl:for-each select="$parents">
	      <xsl:text/> _
	         ByVal <xsl:value-of select="@SingularName"/> As <xsl:value-of select="@SingularName"/>
	      <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each>) 
	   MyBase.New()
      <xsl:for-each select="$parents/orm:ParentKeyField">
         <xsl:variable name="ordinal" select="@Ordinal"/>
         <xsl:variable name="name" select="@Name"/>
	      <xsl:for-each select="../orm:ChildKeyField[@Ordinal=$ordinal]"> 
            <xsl:call-template name="PropertyOrSet">
               <xsl:with-param name="object" select="$object"/>
               <xsl:with-param name="val" select="concat(../@SingularName,'.',$name)"/>
            </xsl:call-template> 
	      </xsl:for-each>
	   </xsl:for-each>
	   
	End Sub
	</xsl:if>
</xsl:if> 

	Protected Sub New()
		' Scope Prevents Direct Instantiation
		' Must be protected, not private, to avoid children needing constructors
		Init() ' Sets Defaults
	End Sub

#End Region
</xsl:template>

<xsl:template name="Criteria">
#Region " Criteria "
<xsl:if test="string-length(@Inherits)=0">
	<!-- You have to escape any less than signs that you want in the output -->
	&lt;Serializable()> _
	Protected Class Criteria
		Inherits CSLA.Server.CriteriaBase
	<xsl:call-template name="PrimaryKeyDeclarations">
		<xsl:with-param name="prefix" select="'m'" />
	</xsl:call-template>
	 
		Public Sub New(<xsl:call-template name="PrimaryKeyArguments"/>)<xsl:text/>
    		MyBase.New(GetType(<xsl:value-of select="$Name"/>))
			<xsl:for-each select="$properties[@IsPrimaryKey='true']">
			m<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>
			</xsl:for-each>
		End Sub
		
		<xsl:for-each select="$properties[@IsPrimaryKey='true']">
			<xsl:call-template name="Property">
			   <xsl:with-param name="incriteria" select="'true'"/>
			</xsl:call-template>
		</xsl:for-each>
		
		Public Sub AddParameters(cmd As IDBCommand)
		   Dim p As System.Data.SQLClient.SQLParameter
		<xsl:apply-templates select="$properties[@IsPrimaryKey='true']" mode="ParameterAssignment"/>
		End Sub 
		
	End Class
</xsl:if>
#End Region

</xsl:template>

<xsl:template name="DataAccess">
#Region " Data Access "

   Private Sub IApplyEdit()<xsl:text/> 
  	   <xsl:if test="string-length(@Inherits)=0"> _
            Implements IBusinessObject.ApplyEdit
	   </xsl:if>
      MyBase.ApplyEdit
   End Sub

   Private Sub ICancelEdit()<xsl:text/>
	   <xsl:if test="string-length(@Inherits)=0"> _
            Implements IBusinessObject.CancelEdit
	   </xsl:if>
      MyBase.CancelEdit
   End Sub

<xsl:if test="$Root='true' and string-length(@Inherits)=0">
	<xsl:if test="orm:RetrieveSP/orm:Privilege">
		<xsl:call-template name="DataPortalFetch"/>
	</xsl:if>
	
	<xsl:if test="orm:UpdateSP/orm:Privilege">
	' called by DataPortal to delete/add/update data into the database<xsl:text/>
	<xsl:if test="orm:UpdateSP/@TransactionType='Enterprise'">
	&lt;Transactions()> _ <xsl:text/>
	</xsl:if>
	Protected Overrides Sub DataPortal_Update()
		' save data into db
	<xsl:for-each select="orm:UpdateSP">
		<xsl:call-template name="StartDP"/>
	</xsl:for-each>
		<xsl:value-of select="net:InsertNLIndent()"/>If Me.IsDeleted Then<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   ' we're being deleted<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   If Not Me.IsNew Then <xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     ' we're not new, so get rid of our data<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     .CommandText = "<xsl:value-of select="orm:DeleteSP/@Name" />"<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     AddDeleteParameters(cm)<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     .ExecuteNonQuery()<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   End If<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   ' reset our status to be a new object<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   MarkNew()<xsl:text/>

		<xsl:value-of select="net:InsertNLIndent()"/> Else<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   ' we're not being deleted, so insert or update<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>   If Me.IsNew Then<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     ' we're new, so insert<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     .CommandText = "<xsl:value-of select="orm:CreateSP/@Name" />"<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>		AddCreateParameters(cm)<xsl:text/>

		<xsl:value-of select="net:InsertNLIndent()"/>   Else<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     ' we're not new, so update<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>     .CommandText = "<xsl:value-of select="orm:UpdateSP/@Name" />"<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>		AddUpdateParameters(cm)<xsl:text/>

		<xsl:value-of select="net:InsertNLIndent()"/>   End If<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>  .ExecuteNonQuery()<xsl:text/>

		<xsl:value-of select="net:InsertNLIndent()"/>  ' make sure we're marked as an old object<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>  GetOutputParameters(cm)<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>  MarkOld()<xsl:text/>

		<xsl:value-of select="net:InsertNLIndent()"/>End If<xsl:text/>
		<xsl:apply-templates select="orm:UpdateSP" mode="CloseDP"/>
		<!-- Moved the child update into a sub 11/29/03. Inherited objects don't know what children 
		     are supported, and if some were excluded from the parent this creates a bug. So, I'm going 
		     to have the derived classes call the base class to save children -->
		UpdateChildren()
	End Sub										
	</xsl:if>
	
   <xsl:if test="string-length(@Inherits)=0">
   Protected Sub UpdateChildren()
	<xsl:for-each select="orm:ChildCollection">
		m<xsl:value-of select="@Name"/>.Update(<xsl:text/>
		<xsl:if test="orm:UpdateSP/@TransactionType='ADONET'">tr, </xsl:if>
 		<xsl:text/>CType(Me, <xsl:value-of select="$Name"/>))
	</xsl:for-each>
   End Sub
   </xsl:if>

	<xsl:if test="orm:DeleteSP/orm:Privilege">
	<xsl:if test="@orm:DeleteSP/TransactionType='Enterprise'">
	&lt;Transactions()> _ <xsl:text/>
	</xsl:if>
	Protected Overrides Sub DataPortal_Delete(ByVal Criteria As Object)
		<xsl:apply-templates select="orm:DeleteSP" mode="StartDPWithCrit" />
		<!--<xsl:value-of select="net:InsertNLIndent()"/>.CommandText = "<xsl:value-of select="orm:DeleteSP/@Name"/> "-->
		<xsl:value-of select="net:InsertNLIndent()"/>.ExecuteNonQuery()
		<xsl:apply-templates select="orm:DeleteSP" mode="CloseDP"/>
	End Sub
	</xsl:if>

</xsl:if>
	  
<xsl:if test="@Child='true'">
   <xsl:if test="string-length(@Inherits)=0">
	Protected Sub Fetch(ByVal dr as SafeDataReader)
		Me.AssignFromDataReader(dr)
		MarkOld()
	End Sub
   </xsl:if>

	Friend Overridable<xsl:text/>
	<xsl:if test="string-length(@Inherits)!=0"> Overloads</xsl:if>
	<xsl:text/> Sub Update(<xsl:text/>
		<xsl:if test="@ChildOf">
			<xsl:text/>ByVal <xsl:value-of select="@ChildOf"/> As <xsl:value-of select="@ChildOf"/>
		</xsl:if>)
		If Not Me.IsDirty Then Exit Sub

		' do the update
		' Assume the transaction is managed by the parent - this method is only used in the child
		Dim cn As New SQLConnection(db("<xsl:value-of select="@MapDataStructure"/>"))
		cn.Open
	   
		Try 
		Dim cm As New SQLCommand
		With cm
			.Connection = cn
			.CommandType = CommandType.StoredProcedure
			If Me.IsDeleted Then<xsl:text/>
				' we're being deleted<xsl:text/>
				If Not Me.IsNew Then <xsl:text/>
					' we're not new, so get rid of our data<xsl:text/>
					.CommandText = "<xsl:value-of select="orm:DeleteSP/@Name" />"
					AddDeleteParameters(cm)
					.ExecuteNonQuery()<xsl:text/>
				End If<xsl:text/>
				' reset our status to be a new object<xsl:text/>
				MarkNew()<xsl:text/>

			Else<xsl:text/>
				' we're not being deleted, so insert or update<xsl:text/>
				If Me.IsNew Then<xsl:text/>
					' we're new, so insert<xsl:text/>
					.CommandText = "<xsl:value-of select="orm:CreateSP/@Name" />"
					AddCreateParameters(cm)
				Else<xsl:text/>
					' we're not new, so update<xsl:text/>
					.CommandText = "<xsl:value-of select="orm:UpdateSP/@Name" />"
					AddUpdateParameters(cm)
				End If<xsl:text/>
				.ExecuteNonQuery()

				' make sure we're marked as an old object
				MarkOld()

			End If<xsl:text/>              
		End With 
	   
		Finally
		cn.Close()
		End Try     

      UpdateChildren()
		<!--' update child objects	<xsl:text/>
		<xsl:for-each select="orm:ChildCollection">
		m<xsl:value-of select="@Name"/>.Update(<xsl:text/>
			<xsl:if test="orm:UpdateSP/@TransactionType='ADONET'">tr, </xsl:if>
 			<xsl:text/>CType(Me, <xsl:value-of select="$Name"/>))
		</xsl:for-each> -->
	End Sub	
 </xsl:if>

#End Region

</xsl:template>

<xsl:template name="InternalMethodsAndProperties">
	<xsl:variable name="object" select="."/>
	<xsl:variable name="childof" select="@ChildOf"/>
	<xsl:variable name="parents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
#Region "Internal Methods and Properties"

   <xsl:if test="string-length($childof)=0">
	Protected <xsl:text/>
	<xsl:choose>
      <xsl:when test="string-length($childof)=0">Overridable </xsl:when>
      <xsl:otherwise>Overrides </xsl:otherwise>
	</xsl:choose>
	<xsl:text/>Overloads Sub Init()<xsl:text/>
	<!--<xsl:for-each select="$properties">
	   <xsl:choose>
	      <xsl:when test="@IsPrimaryKey='true'">
		Set<xsl:value-of select="@Name"/>(<xsl:call-template name="EmptyForColumnNoEquals"/>)</xsl:when>
	      <xsl:otherwise>
		m<xsl:value-of select="@Name"/><xsl:call-template name="EmptyForColumn"/></xsl:otherwise>
	   </xsl:choose>
	</xsl:for-each> -->
	   <xsl:for-each select="$properties">
	      <xsl:choose>
	         <xsl:when test="@Default">
		         <xsl:choose>
		         <xsl:when test="@IsPrimaryKey='true'">
		   Set<xsl:value-of select="@Name"/>(<xsl:text/>
		         <xsl:value-of select="@Default"/>)</xsl:when>
		         <xsl:otherwise>
         m<xsl:value-of select="@Name"/>
		         <xsl:text/> = <xsl:value-of select="@Default"/><xsl:text/>
		         </xsl:otherwise>
		         </xsl:choose>
	         </xsl:when>
 		      <xsl:when test="not(starts-with(@NETType,'System.'))">
  		   m<xsl:value-of select="@Name"/> = New <xsl:value-of select="@NETType" />();<xsl:text/>
	   	   </xsl:when>
	      </xsl:choose>
	   </xsl:for-each>
	   

	<!-- REmoved 1/28/04 KAD <xsl:for-each select="$properties">
	   <xsl:if test="not(starts-with(@NETType,'System.'))">
	   m<xsl:value-of select="@Name"/> = New <xsl:value-of select="@NETType" />
	   </xsl:if>
	</xsl:for-each>
	<xsl:for-each select="$properties[string-length(@Default) > 0]">
	   <xsl:choose>
	      <xsl:when test="@IsPrimaryKey='true'">
		Set<xsl:value-of select="@Name"/>(<xsl:text/>
		         <xsl:value-of select="@Default"/>)</xsl:when>
	      <xsl:otherwise>
		m<xsl:value-of select="@Name"/><xsl:call-template name="SetMethod"/>
		         <xsl:text/> = <xsl:value-of select="@Default"/>
		   </xsl:otherwise>
	   </xsl:choose>
	</xsl:for-each> -->
	End Sub
   </xsl:if>

   <xsl:if test="count($parents)!=0">
   Protected Overridable Overloads Sub Init( _<xsl:text/>
      <xsl:for-each select="$parents">
            ByVal <xsl:value-of select="@SingularName"/> As <xsl:text/>
         <xsl:value-of select="@SingularName"/>
         <xsl:if test="position()!=last()">, _</xsl:if>
	   </xsl:for-each>)<xsl:text/>
      <xsl:for-each select="$parents/orm:ChildKeyField">
      <xsl:variable name="ordinal" select="@Ordinal"/>
      <xsl:call-template name="PropertyOrSet">
         <xsl:with-param name="object" select="$object"/>
         <xsl:with-param name="val" select="concat(../@SingularName,'.',../orm:ParentKeyField[@Ordinal=$ordinal]/@Name)"/>
      </xsl:call-template> 
	   </xsl:for-each>
	End Sub
   </xsl:if>
 
	<xsl:variable name="temp">
	   <xsl:call-template name="parentnonlookupparams">
	      <xsl:with-param name="object" select="."/>
	      <xsl:with-param name="parents" select="$parents" />
	   </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="nolookups" select="msxsl:node-set($temp)"/>
	<xsl:if test="count($nolookups/BaseObject/*)>0">
   Protected Overridable Overloads Sub Init( _<xsl:text/>
      <xsl:for-each select="$nolookups//Field">
	         ByVal <xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/>
	      <xsl:if test="position()!=last()">, _</xsl:if>
	   </xsl:for-each>)
      <xsl:for-each select="$nolookups//Field">
      <xsl:call-template name="PropertyOrSet">
         <xsl:with-param name="object" select="$object"/>
         <xsl:with-param name="val" select="@Name"/>
      </xsl:call-template> 
	   </xsl:for-each>
	End Sub
   </xsl:if>

	
	Private Function AssignFromDataReader( _
	            dr As SafeDataReader) _
	            As Boolean 
		If dr.Read() Then
			With dr<xsl:text/>
				<!--<xsl:apply-templates select="orm:RetrieveSP/orm:SPRecordSet[1]/orm:Column" mode="RetrieveFromReader"/>-->
				<xsl:apply-templates select="$properties" mode="RetrieveFromReader"/>
			End With
			mIsLoaded = True
			MarkOld()
			Return True
		End If
	End Function
	
	Private Sub AddCreateParameters(cmd As IDBCommand)<xsl:text/>
	   Dim p As System.Data.SQLClient.SQLParameter
	<xsl:apply-templates select="orm:CreateSP/orm:Parameter" mode="ParameterAssignment">
	   <xsl:with-param name="isobject" select="'true'"/>
	</xsl:apply-templates>
	End Sub 
	
	Private Sub GetOutputParameters(cmd As IDBCommand)
	   Dim p As System.Data.SQLClient.SQLParameter
	<xsl:for-each select="orm:CreateSP/orm:Parameter">
	   <xsl:variable name="paramname" select="@Name"/>
	   <xsl:for-each select="../../orm:Property[@Name=$paramname]" >
	      <xsl:if test="@IsAutoIncrement='true'">
	    p = CType(cmd.Parameters.Item("@<xsl:text/>
			<xsl:value-of select="@Name"/>"), System.Data.SQLClient.SQLParameter)
	    Me.Set<xsl:value-of select="@Name"/>(CType(p.Value,<xsl:value-of select="@NETType"/>))
			</xsl:if>
	   </xsl:for-each>
	</xsl:for-each>
	End Sub

	<xsl:if test="orm:RetrieveSP">
	Private Sub AddRetrieveParameters(cmd As IDBCommand)<xsl:text/>
	   Dim p As System.Data.SQLClient.SQLParameter
	<xsl:apply-templates select="orm:RetrieveSP/orm:Parameter" mode="ParameterAssignment">
	   <xsl:with-param name="isobject" select="'true'"/>
	</xsl:apply-templates>
	End Sub 
	</xsl:if>

	Private Sub AddUpdateParameters(cmd As IDBCommand)<xsl:text/>
	   Dim p As System.Data.SQLClient.SQLParameter
	<xsl:apply-templates select="orm:UpdateSP/orm:Parameter" mode="ParameterAssignment">
	   <xsl:with-param name="isobject" select="'true'"/>
	</xsl:apply-templates>
	End Sub 

	Private Sub AddDeleteParameters(cmd As IDBCommand)<xsl:text/>
	   Dim p As System.Data.SQLClient.SQLParameter
	<xsl:apply-templates select="orm:DeleteSP/orm:Parameter" mode="ParameterAssignment">
	   <xsl:with-param name="isobject" select="'true'"/>
	</xsl:apply-templates>
	End Sub 

#End Region 
</xsl:template>

 <!-- End of main template list -->

</xsl:stylesheet> 
  