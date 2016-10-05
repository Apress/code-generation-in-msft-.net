<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="TableName"/>

<xsl:template match="/DataSet">
<xsl:apply-templates select="Table[@Name=$TableName]"/>
</xsl:template>

<xsl:template match="Table">
Option Strict On
Option Explicit On

Imports System

'! Class Summary: 

Public Class <xsl:value-of select="$TableName"/>
   
#Region "Class level declarations"<xsl:for-each select="Column">
   Private m_<xsl:value-of select="@Name"/> As <xsl:value-of select="@Type"/>
</xsl:for-each>
#End Region

#Region "Public Methods and Properties"
<xsl:for-each select="Column">
   Public Property <xsl:value-of select="@Name"/>() As <xsl:value-of select="@Type"/>
      Get
         Return m_<xsl:value-of select="@Name"/>
      End Get
      Set(ByVal Value As <xsl:value-of select="@Type"/>)
         m_<xsl:value-of select="@Name"/> = Value
      End Set
   End Property
</xsl:for-each>

#End Region

End Class
</xsl:template>

</xsl:stylesheet>

  