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
using System;

/// &lt;summary>
/// 
/// &lt;/summary>

public class <xsl:value-of select="$TableName"/>
{
	#region Class level declarations<xsl:for-each select="Column">
	private <xsl:value-of select="@Type"/> m_<xsl:value-of select="@Name"/>;
</xsl:for-each>
	#endregion

	#region Public Methods and Properties
<xsl:for-each select="Column">
	public <xsl:value-of select="@Type"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
	{
		get
		{
			Return m_<xsl:value-of select="@Name"/>;
		}
		set
		{
			m_<xsl:value-of select="@Name"/> = value;
		}
	}
</xsl:for-each>

	#endregion

}
</xsl:template>

</xsl:stylesheet>

  