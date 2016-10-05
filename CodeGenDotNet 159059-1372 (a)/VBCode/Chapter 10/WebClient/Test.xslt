<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
			xmlns:uc1="Header.ascx"
			xmlns:asp="asp"
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="webSupport.xslt"/>
<xsl:import href="../../Chapter 8/BusinessObjects/CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="BusinessObject"/>

<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
&lt;%@ Page Language="vb" AutoEventWireup="false" Codebehind="<xsl:value-of select="@PluralName"/>.aspx.vb" Inherits="WebPTracker.<xsl:value-of select="@PluralName"/>" enableViewState="False"%>
&#x3c;%@ Register TagPrefix="uc1" TagName="Header" Src="Header.ascx" %>
&lt;!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
   <xsl:element name="HEAD">
      <title><xsl:value-of select="@PluralName"/></title>
      <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
      <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
      <meta name="vs_defaultClientScript" content="JavaScript"/>
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
   </xsl:element>
   <body>
      <form id="Form1" method="post" runat="server">
         <P>
            <uc1:Header id="Header1" runat="server"></uc1:Header></P>
         <P>
            <asp:Label id="Label2" runat="server" Font-Names="Bauhaus 93" Font-Size="XX-Large"><xsl:value-of select="@PluralName"/></asp:Label></P>
         <P>
            <asp:HyperLink id="hlHome" runat="server" NavigateUrl="Default.aspx">Home</asp:HyperLink><xsl:text>&#xa0;&#xa0;&#xa0;</xsl:text>
            <asp:LinkButton id="btnNew" runat="server">Add new <xsl:value-of select="$objectname"/></asp:LinkButton>
            <asp:DataGrid id="dg" runat="server" AutoGenerateColumns="False" BorderColor="#CC9966" BorderStyle="None"
               BorderWidth="1px" BackColor="White" CellPadding="4">
               <SelectedItemStyle Font-Bold="True" ForeColor="#663399" BackColor="#FFCC66"></SelectedItemStyle>
               <ItemStyle ForeColor="#330099" BackColor="White"></ItemStyle>
               <HeaderStyle Font-Bold="True" ForeColor="#FFFFCC" BackColor="#990000"></HeaderStyle>
               <FooterStyle ForeColor="#330099" BackColor="#FFFFCC"></FooterStyle>
               <Columns>
               <xsl:for-each select=".//orm:Property">
                  <asp:ButtonColumn Text="Remove" CommandName="Delete"></asp:ButtonColumn>
               </xsl:for-each>
               </Columns>
               <PagerStyle HorizontalAlign="Center" ForeColor="#330099" BackColor="#FFFFCC"></PagerStyle>
            </asp:DataGrid></P>
      </form>
   </body>
</HTML>
</xsl:template> 

</xsl:stylesheet>  