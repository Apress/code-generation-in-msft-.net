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
  Summary: Generates the ASPX page for selecting items  -->

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

<xsl:variable name="projnamespace" select="//ui:UIRoot/@ProjectNamespace"/>
<xsl:variable name="formname" select="$objectname"/>
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="columns" 
      select="//orm:Object[@Name=$objectname]
      //orm:SetSelectSP//orm:SPRecordSet//orm:Column
      [@IsPrimaryKey='true' or @UseForDesc='true']"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
&lt;%@ Page Language="vb" AutoEventWireup="false" Codebehind="<xsl:text/>
      <xsl:value-of select="@Name"/>
      <xsl:text/>Select.aspx.vb" Inherits="WebPTracker.<xsl:text/>
      <xsl:value-of select="@Name"/>Select" enableViewState="False"%>
&lt;%@ Register TagPrefix="uc1" TagName="Header" Src="..\..\Header.ascx" %>
&lt;!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
&lt;HTML>
   &lt;HEAD>
      &lt;title><xsl:value-of select="@CollectionName"/>&lt;/title>
      &lt;meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
      &lt;meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
      &lt;meta name="vs_defaultClientScript" content="JavaScript"/>
      &lt;meta name="vs_targetSchema" 
            content="http://schemas.microsoft.com/intellisense/ie5"/>
   &lt;/HEAD>
   &lt;body>
      &lt;form id="Form1" method="post" runat="server">
         &lt;P>
            &lt;uc1:Header id="Header1" runat="server"/>
         &lt;/P>
         &lt;P>
            &lt;asp:Label id="Label2" runat="server" Font-Names="Bauhaus 93" 
                  Font-Size="XX-Large"><xsl:value-of select="@CollectionName"/>
            &lt;/asp:Label>
         &lt;/P>
         &lt;P>
            &lt;asp:HyperLink id="hlHome" runat="server" 
                  NavigateUrl="&lt;%# 
                        <xsl:value-of select="concat($projnamespace,
                                '.NavigateURL.Home')"/> %>">
               Home
            &lt;/asp:HyperLink>
            <xsl:text>&amp;nbsp;&amp;nbsp;&amp;nbsp;</xsl:text>
            &lt;asp:LinkButton id="btnNew" runat="server" >
                     Add new <xsl:value-of select="$objectname"/>
            &lt;/asp:LinkButton> 
            &lt;asp:DataGrid id="dg" runat="server" AutoGenerateColumns="False" 
                     BorderColor="#CC9966" BorderStyle="None"
                     BorderWidth="1px" BackColor="White" CellPadding="4">
               &lt;SelectedItemStyle Font-Bold="True" ForeColor="#663399" 
                        BackColor="#FFCC66"/>
               &lt;ItemStyle ForeColor="#330099" BackColor="White"/>
               &lt;HeaderStyle Font-Bold="True" ForeColor="#FFFFCC" 
                        BackColor="#990000"/>
               &lt;FooterStyle ForeColor="#330099" BackColor="#FFFFCC"/>
               &lt;Columns><xsl:text/>
               <xsl:for-each select="$columns">
                  <xsl:choose>
                     <xsl:when test="@IsPrimaryKey='true'">
                  &lt;asp:BoundColumn Visible="False" 
                        DataField="<xsl:value-of select="@Name"/>" 
                        HeaderText="<xsl:value-of select="@Caption"/>"/>
                     </xsl:when>
                     <xsl:otherwise>
                  &lt;asp:ButtonColumn 
                        DataTextField="<xsl:value-of select="@Name"/>" 
                        HeaderText="<xsl:value-of select="@Caption"/>" 
                        CommandName="Select"/><xsl:text/>
                    </xsl:otherwise>
                  </xsl:choose>
               </xsl:for-each>
                  &lt;asp:ButtonColumn Text="Remove" CommandName="Delete"/>
               &lt;/Columns>
               &lt;PagerStyle HorizontalAlign="Center" ForeColor="#330099" 
                     BackColor="#FFFFCC"/>
            &lt;/asp:DataGrid>
         &lt;/P>
      &lt;/form>
   &lt;/body>
&lt;/HTML>
</xsl:template> 

</xsl:stylesheet>  