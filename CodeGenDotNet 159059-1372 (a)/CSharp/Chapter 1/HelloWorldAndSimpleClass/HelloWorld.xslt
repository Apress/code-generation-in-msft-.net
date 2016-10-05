<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="/">
using System;

/// &lt;summary>
/// 
/// &lt;/summary>

public class TargetOutput
{
	#region Public Methods and Properties
	public static void Main()
	{
		Console.WriteLine("Hello World");
	}
	#endregion
}
</xsl:template>


</xsl:stylesheet>

  