<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" 
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
				xmlns:xs="http://www.w3.org/2001/XMLSchema" 
				xmlns:dbs="http://kadgen/DatabaseStructure" >
<xsl:import href="..\..\..\XFiles\Support.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/">
	<xsl:apply-templates select=
			"/dbs:DataStructures/dbs:DataStructure/dbs:Tables/dbs:Table[@Name=$Name]" 
				mode="BuildClasses" />
</xsl:template>


<xsl:template match="dbs:Table" mode="BuildClasses">
	<xsl:call-template name="FileOpen">
		<xsl:with-param name="imports" select="'KADGen, System'" />
	</xsl:call-template>
Public Class <xsl:value-of select='@SingularName'/>Collection
	Inherits CollectionBase
	<xsl:call-template name="CollectionConstructors" />
	<xsl:call-template name="PublicAndFriend" />
End Class

Public Class <xsl:value-of select="@SingularName" />
   Inherits RowBase
	<xsl:call-template name="ClassLevelDeclarations" />
	<xsl:call-template name="Constructors" />
	<xsl:call-template name="BaseClassImplementation" />
	<xsl:call-template name="FieldAccessProperties" />
End Class	
</xsl:template>


<xsl:template name="CollectionConstructors">
#Region "Constructors"
	Protected Sub New()
      MyBase.New("<xsl:value-of select="@SingularName" />Collection")
   End Sub
#End Region
</xsl:template>


<xsl:template name="PublicAndFriend">
#Region "Public and Friend Properties, Methods and Events"<xsl:text/>
	<xsl:variable name="primarykeyname" 
           select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField/@Name" />
	<xsl:variable name="primarykey" 
           select="dbs:TableColumns/dbs:TableColumn[@Name=$primarykeyname]" />
   Public Overloads Sub Fill( _<xsl:if test="$primarykey">
		      ByVal <xsl:value-of select="$primarykey/@Name"/> As <xsl:text/>
           <xsl:value-of select="$primarykey/@NETType"/>, _</xsl:if>
            ByVal UserID As Int32)
		<xsl:value-of select="@SingularName"/>DataAccessor.Fill(Me<xsl:text/>
           <xsl:if test="$primarykey">, <xsl:value-of select="$primarykey/@Name"/>
           </xsl:if>, UserID)
   End Sub
#End Region
</xsl:template>


<xsl:template name="ClassLevelDeclarations">
#Region "Class Level Declarations"
   Protected mCollection As <xsl:value-of select='@SingularName'/>Collection
   Private Shared mNextPrimaryKey As Int32 = -1
   <xsl:for-each select="dbs:TableColumns/*">
		<xsl:if test="string-length(@NETType)>0">
			Private m<xsl:value-of select="@Name" /> 
				<xsl:text/>As <xsl:value-of select="@NETType" />
		</xsl:if>
	</xsl:for-each>
#End Region
</xsl:template>


<xsl:template name="Constructors">
#Region "Constructors"
   Friend Sub New(ByVal <xsl:value-of select="@SingularName"/>Collection As <xsl:value-of select="@SingularName"/>Collection)
      MyBase.new()
      mCollection = <xsl:value-of select="@SingularName"/>Collection
   End Sub
#End Region
</xsl:template>


<xsl:template name="BaseClassImplementation">
#Region "Base Class Implementation"<text/>
	<xsl:variable name="primarykeyname" select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField/@Name" />
	<xsl:variable name="primarykey" select="dbs:TableColumns/dbs:TableColumn[@Name=$primarykeyname]" />
	<xsl:if test="$primarykey/@IsAutoIncrement">
   Friend Sub SetNewPrimaryKey()
		<xsl:value-of select="$primarykeyname" /> = mNextPrimaryKey
      mNextPrimaryKey -= 1
   End Sub	</xsl:if>
#End Region
</xsl:template>

<xsl:template name="FieldAccessProperties">
#Region "Field access properties"
   <xsl:apply-templates select="dbs:TableColumns/*" mode="ColumnMethods" />
#End Region
</xsl:template>

<xsl:template match="dbs:TableColumn" mode="ColumnMethods" >
	<xsl:choose>
		<xsl:when test="string-length(@NETType)=0">
		
			' Column <xsl:value-of select="@Name"/> is not included because it uses 
			' a SQLType (<xsl:value-of select="@SQLType"/>) that is not yet supported 
			
		</xsl:when>
		<xsl:otherwise>

   Public Function <xsl:value-of select="@Name"/>
              <xsl:text/>ColumnInfo As ColumnInfo
      Dim columnInfo As New ColumnInfo
      columnInfo.FieldName = "<xsl:value-of select="@Name"/>"
      columnInfo.FieldType = GetType(<xsl:text/><xsl:value-of select="@NETType"/>)
      columnInfo.SQLType = "<xsl:value-of select="@SQLType"/>"
      columnInfo.Caption = "<xsl:value-of select="@Caption"/>"
      columnInfo.Desc = "<xsl:value-of select="@Desc"/>"
      Return columnInfo
   End Function
	
   Public Property <xsl:value-of select="@Name"/> As <xsl:text/>
				  <xsl:value-of select="@NETType"/><xsl:call-template name="NewLine"/>
      Get
         Return m<xsl:value-of select="@Name"/><xsl:call-template name="NewLine"/>
      End Get
      Set(ByVal Value As <xsl:value-of select="@NETType"/>)
         m<xsl:value-of select="@Name"/> = Value
      End Set
   End Property
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="dbs:TableColumn" mode="ColumnMethods2" >
   <xsl:call-template name="NewLine"/>
   <xsl:variable name="pre">
		<xsl:choose>
			<xsl:when test="string-length(@NETType)=0">   ' </xsl:when>
			<xsl:otherwise><xsl:text>   </xsl:text></xsl:otherwise>
		</xsl:choose>
   </xsl:variable>
<xsl:value-of select="$pre"/>Public Function <xsl:value-of select="@Name"/>
              <xsl:text/>ColumnInfo As ColumnInfo
<xsl:value-of select="$pre"/>   Dim columnInfo As New ColumnInfo
<xsl:value-of select="$pre"/>   columnInfo.FieldName = "<xsl:text/>
              <xsl:value-of select="@Name"/>"
<xsl:value-of select="$pre"/>   columnInfo.FieldType = gettype(<xsl:text/>
              <xsl:value-of select="@NETType"/>)
<xsl:value-of select="$pre"/>   columnInfo.SQLType = "<xsl:text/>
              <xsl:value-of select="@SQLType"/>"
<xsl:value-of select="$pre"/>   columnInfo.Caption = "<xsl:text/>
              <xsl:value-of select="@Caption"/>"
<xsl:value-of select="$pre"/>   columnInfo.Desc = "<xsl:value-of select="@Desc"/>"
<xsl:value-of select="$pre"/>   Return columnInfo
<xsl:value-of select="$pre"/>End Function
	
<xsl:value-of select="$pre"/>Public Property <xsl:text/>
              <xsl:value-of select="@Name"/> As <xsl:text/>
				  <xsl:value-of select="@NETType"/><xsl:call-template name="NewLine"/>
<xsl:value-of select="$pre"/>   Get
<xsl:value-of select="$pre"/>      Return m<xsl:value-of select="@Name"/>
              <xsl:call-template name="NewLine"/>
<xsl:value-of select="$pre"/>   End Get
<xsl:value-of select="$pre"/>   Set(ByVal Value As <xsl:text/>
              <xsl:value-of select="@NETType"/>)
<xsl:value-of select="$pre"/>      m<xsl:value-of select="@Name"/> = Value
<xsl:value-of select="$pre"/>   End Set
<xsl:value-of select="$pre"/>End Property
</xsl:template>

</xsl:stylesheet> 
  