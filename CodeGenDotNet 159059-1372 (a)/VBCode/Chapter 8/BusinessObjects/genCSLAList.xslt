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
  Summary:  Generates plumbing class for lists!-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]//orm:SetSelectSP" 
								mode="SetSelectSP"/>
</xsl:template>

<xsl:template match="orm:SetSelectSP" mode="SetSelectSP">
Option Explicit On
Option Strict On

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports CSLA.Data
Imports CSLA

&lt;Serializable()> _
Public Class gen<xsl:value-of select="$Name"/>List
  Inherits ReadOnlyCollectionBase
  
  Private Shared m<xsl:value-of select="$Name"/> As <xsl:value-of select="$Name"/> 
  <xsl:call-template name="DataStructure"/>
  <xsl:call-template name="BusinessPropertiesAndMethods"/>
  <xsl:call-template name="SharedMethods"/>
  <xsl:call-template name="Criteria"/>
  <xsl:call-template name="Constructors"/>
  <xsl:call-template name="DataAccess"/>
End Class
</xsl:template>

<xsl:template name="DataStructure">
#Region " Data Structure "

  &lt;Serializable()> _
  Public Class <xsl:value-of select="$Name"/>Info
    Implements BusinessObjectSupport.IListInfo 
    ' this has private members, public properties because
    ' ASP.NET can't databind to public members of a structure...
    <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">
    Private m<xsl:value-of select="@Name"/> As <xsl:value-of select="@NETType"/>
    </xsl:for-each>

    <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">
    Public Property <xsl:value-of select="@Name"/>() As <xsl:value-of select="@NETType"/>
      Get
        Return m<xsl:value-of select="@Name"/>
      End Get
      Set(ByVal Value As <xsl:value-of select="@NETType"/>)
        m<xsl:value-of select="@Name"/> = Value
      End Set
    End Property
    </xsl:for-each> 

   <xsl:if test="count(orm:SPRecordSet)" > 
    Public Overloads Function Equals( _
               ByVal info As <xsl:value-of select="$Name"/>Info) _
               As Boolean
      Return <xsl:text/>
      <xsl:for-each select="orm:SPRecordSet[1]/orm:Column">m<xsl:text/>
			<xsl:value-of select="@Name"/>.Equals(info.<xsl:text/>
			<xsl:value-of select="@Name"/>)<xsl:text/>
			<xsl:if test="last() != position()"> AndAlso </xsl:if>
      </xsl:for-each>
    End Function
   </xsl:if>

	Public Property UniqueKey() _
	            As String _
	            Implements BusinessObjectSupport.IListInfo.UniqueKey
      Get
	      Return <xsl:text/>
	  <!-- THe followign assumes all primary keys are in set select -->
  <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']">
	<xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:if test="position()!=last()"> &amp; </xsl:if>
  </xsl:for-each>
      End Get
      Set
         Throw New System.Exception("Unexpected call to setUniqueKey")
      End Set
   End Property

	Public Property DisplayText() _
	            As String _
	            Implements BusinessObjectSupport.IListInfo.DisplayText
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
	
	Public Sub AssignFromDataReader(dr As SafeDataReader)
		With dr<xsl:text/>
			<xsl:apply-templates select="orm:SPRecordSet[1]/orm:Column" mode="RetrieveFromReader">
				<xsl:with-param name="forcefield" select="'true'" />
				<xsl:with-param name="useme" select="'false'" />
			</xsl:apply-templates>
		End With
	 End Sub
	
 <xsl:if test="count(..//orm:Property[@UseForDesc='true'])>0">
    Public Overrides Function ToString() As String
	 	 Return <xsl:text />
      <xsl:for-each select="..//orm:Property[@UseForDesc='true']">
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
  <!-- <xsl:for-each select="..//orm:Property[@UseForDesc='true']">
	<xsl:text/>m<xsl:value-of select="@Name"/>.ToString<xsl:if test="position()!=last()"> &amp; ", " &amp; </xsl:if>
  </xsl:for-each> -->
	 End Function
 </xsl:if>
 
    Public Function GetPrimaryKey() _
            As Object Implements BusinessObjectSupport.IListInfo.GetPrimaryKey
    <xsl:choose>
       <xsl:when test="count(..//orm:Property[@IsPrimaryKey='true'])>1">
       Throw New System.ApplicationException("This function doesn't work for multi-column primary keys")
       </xsl:when>
       <xsl:otherwise>
       <xsl:for-each select="..//orm:Property[@IsPrimaryKey='true']">
       Return <xsl:value-of select="@Name"/>
       </xsl:for-each>
       </xsl:otherwise>
    </xsl:choose>
    End Function

  End Class

#End Region

</xsl:template>


<xsl:template name="BusinessPropertiesAndMethods">
#Region " Business Properties and Methods "

   Default Public ReadOnly Property Item( _
				ByVal Index As Integer) _
				As <xsl:value-of select="$Name"/>Info
      Get
         Return CType(list.Item(Index), <xsl:value-of select="$Name"/>Info)
      End Get
   End Property
  
   Public Function Get<xsl:value-of select="$Name"/>Info( <xsl:text/>
   <xsl:if test="count(../orm:Property[@IsPrimaryKey='true'])"> _<xsl:text/>
      <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']">
            ByVal <xsl:value-of select="@Name"/> As <xsl:text/>
            <xsl:value-of select="@NETType" />
            <xsl:if test="last()!=position()">, _</xsl:if>
      </xsl:for-each>
   </xsl:if>) _
            <xsl:text/>As <xsl:value-of select="$Name"/>Info
      <xsl:if test="count(../orm:Property[@IsPrimaryKey='true'])"> 
      For i As Int32 = 0 To Me.Count - 1
         If <xsl:text/>
         <xsl:for-each select="../orm:Property[@IsPrimaryKey='true']">
			   <xsl:call-template name="Comparison">
			      <xsl:with-param name="first" select="concat('Me.Item(i).',@Name)"/>
			      <xsl:with-param name="second" select="@Name"/>
			      <xsl:with-param name="operand" select="'='"/>
			      <xsl:with-param name="type" select="@NETType"/>
			   </xsl:call-template> 
            <xsl:if test="position()!=last()"> And </xsl:if>
         </xsl:for-each>
            Return Me.Item(i)
         End If
      Next
      </xsl:if> 
   End Function


#End Region

#Region " Contains "

  Public Overloads Function Contains( _
    ByVal item As <xsl:value-of select="$Name"/>Info) As Boolean

    Dim child As <xsl:value-of select="$Name"/>Info

    For Each child In list
      If child.Equals(item) Then
        Return True
      End If
    Next

    Return False

  End Function

#End Region

</xsl:template>

<xsl:template name="SharedMethods">
#Region " Shared Methods "

  Public Shared Function Get<xsl:value-of select="$Name"/>List() As <xsl:value-of select="$Name"/>List
    Return CType(DataPortal.Fetch(New <xsl:value-of select="$Name"/>List.Criteria), <xsl:value-of select="$Name"/>List)
  End Function

#End Region

</xsl:template>


<xsl:template name="Criteria">
#Region " Criteria "

  &lt;Serializable()> _
  Public Class Criteria
		Inherits CSLA.Server.CriteriaBase
    ' no criteria - we retrieve all <xsl:value-of select="$Name"/>
    
    Public Sub New()
    	MyBase.New(GetType(<xsl:value-of select="$Name"/>List))
	 End Sub
  End Class

#End Region

</xsl:template>


<xsl:template name="Constructors">
#Region " Constructors "

  Protected Sub New()
    ' prevent direct creation
  End Sub

#End Region

</xsl:template>


<xsl:template name="DataAccess">
#Region " Data Access "

  Protected Overrides Sub DataPortal_Fetch(ByVal Criteria As Object)
    Dim cn As New SqlConnection(db("<xsl:value-of select="../@MapDataStructure"/>"))
    Dim cm As New SqlCommand

    cn.Open()
    Try
      With cm
        .Connection = cn
        .CommandType = CommandType.StoredProcedure
        .CommandText = "<xsl:value-of select="@Name"/>"

        Dim dr As New SafeDataReader(.ExecuteReader)
        Try
          While dr.Read()
            Dim info As New <xsl:value-of select="$Name"/>Info
            info.AssignFromDataReader(dr)
            innerlist.Add(info)
          End While
        Finally
          dr.Close()
        End Try

      End With

    Finally
      cn.Close()
    End Try
  End Sub

#End Region

</xsl:template>

</xsl:stylesheet> 
  