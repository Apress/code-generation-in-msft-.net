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
				mode="BuildClass" />
</xsl:template>


<xsl:template match="dbs:Table" mode="BuildClass">
	<xsl:call-template name="FileOpen">
		<xsl:with-param name="imports" select="'KADGen,System.Data'" />
	</xsl:call-template>
Public Class <xsl:value-of select='@SingularName'/>DataAccessor
	Inherits DataAccessor
	
	<xsl:call-template name="PublicAndFriend" />
End Class

</xsl:template>


<xsl:template name="PublicAndFriend">
#Region "Public and Friend Properties, Methods and Events"
	<xsl:variable name="primarykeyname" select="dbs:TableConstraints/dbs:PrimaryKey/dbs:PKField/@Name" />
	<xsl:variable name="primarykey" select="dbs:TableColumns/dbs:TableColumn[@Name=$primarykeyname]" />
   Public Shared Sub Fill( _
		         ByVal coll As <xsl:value-of select="@SingularName"/>Collection, _<xsl:if test="$primarykey"> 
		         ByVal <xsl:value-of select="$primarykeyname"/> As <xsl:value-of select="$primarykey/@NETType"/>, _</xsl:if>
					ByVal UserID As Int32)
   End Sub

   Public Shared Sub Save( _
		         ByVal coll As <xsl:value-of select="@SingularName"/>Collection, _
					ByVal UserID As Int32)
   End Sub
#End Region
</xsl:template>

</xsl:stylesheet> 
  