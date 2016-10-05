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
  Summary: Generates the ASPX page for editing  -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:ui="http://kadgen.com/UserInterface.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
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
<xsl:variable name="objectname" select="$BusinessObject"/>
<xsl:variable name="objectsingular" 
                  select="//orm:Object[@Name=$objectname]/@Name"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$objectname]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
&lt;%@ Page Language="vb" AutoEventWireup="false" 
         Codebehind="<xsl:value-of select="$objectsingular"/>Edit.aspx.vb" 
         Inherits="<xsl:value-of select="concat($projnamespace,'.codebehind',
                         $objectsingular)"/>Edit"%>
&lt;%@ Register TagPrefix="uc1" TagName="Header" Src="..\..\Header.ascx" %>
&lt;!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
&lt;HTML>
   &lt;HEAD>
      &lt;title><xsl:value-of select="$objectsingular"/>Edit&lt;/title>
      &lt;meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
      &lt;meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
      &lt;meta content="JavaScript" name="vs_defaultClientScript">
      &lt;meta content="http://schemas.microsoft.com/intellisense/ie5" 
                  name="vs_targetSchema">
   &lt;/HEAD>
   &lt;body>
      &lt;form id="<xsl:value-of select="$objectsingular"/>Edit" 
                  method="post" runat="server">
         &lt;P>
            &lt;uc1:header id="Header1" runat="server">
            &lt;/uc1:header>
         &lt;/P>
         &lt;P>
            &lt;asp:label id="Label2" 
                  runat="server" Font-Names="Bauhaus 93" Font-Size="XX-Large">
               Edit <xsl:value-of select="$objectsingular"/>
            &lt;/asp:label>&lt;/P>
         &lt;P>
            &lt;asp:hyperlink id="hplHome" 
                  runat="server" 
                  NavigateUrl="&lt;%# 
                        <xsl:value-of select="concat($projnamespace,
                                '.NavigateURL.Home')"/> %>">
               Home
            &lt;/asp:hyperlink>&amp;nbsp;&amp;nbsp; 
            &lt;asp:hyperlink id="hplList" 
                  runat="server" 
                  NavigateUrl="&lt;%# <xsl:value-of 
                        select="concat($projnamespace,'.NavigateURL.',
                                 $objectsingular)"/>Select %>">
               <xsl:value-of select="$objectsingular"/> List
            &lt;/asp:hyperlink>&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;
            &lt;asp:linkbutton id="btnNew" runat="server" >
               Add new <xsl:value-of select="$objectsingular"/>
            &lt;/asp:linkbutton>
         &lt;/P>
         &lt;P>
            &lt;TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0">
               <xsl:for-each select="orm:Property">
               &lt;TR>
                  &lt;TD <xsl:if test="@MaxLength>=100">
                                 <xsl:text/>style="HEIGHT: 39px"</xsl:if>>
                     <xsl:value-of select="@Caption"/>
                  &lt;/TD>
                  &lt;TD <xsl:if test="@MaxLength>=100">
                                 <xsl:text/>style="HEIGHT: 39px"</xsl:if>>
                     &lt;<xsl:value-of select="@ASPControlType"/> 
                                 id=<xsl:value-of select="@ControlName"/> 
                                 runat="server" 
                                 Text="&lt;%# m<xsl:value-of
                                       select="$objectsingular"/>.<xsl:value-of 
                                       select="@Name"/>.ToString %>" 
                                 <xsl:if test="@ReadOnly='true'">
                                 ReadOnly="True"</xsl:if>
                                 <xsl:if test="@MaxLength>=100"> 
                                 TextMode="MultiLine"</xsl:if>>
                     &lt;/<xsl:value-of select="@ASPControlType"/>>
                     <xsl:if test="@IsRequired='true'">
                     &lt;asp:requiredfieldvalidator 
                                 id="rqd<xsl:value-of select="@Name"/>" 
                                 runat="server" 
                                 ControlToValidate=
                                       "<xsl:value-of select="@ControlName"/>" 
                                 ErrorMessage=
                                       "<xsl:value-of select="@Caption"/> 
                                       <xsl:text/> is required">
                     &lt;/asp:requiredfieldvalidator>&lt;/TD>
                     </xsl:if>
                  &lt;/TD>
               &lt;/TR>
               </xsl:for-each>
               <xsl:for-each select="orm:ChildCollection">
               &lt;TR>
                  &lt;TD><xsl:value-of select="@ChildTableName"/>&lt;/TD>
                  &lt;TD>
                     &lt;asp:datagrid 
                              id="dg<xsl:value-of select="@Name"/>" 
                              runat="server" AutoGenerateColumns="False" 
                              EnableViewState="False">
                        &lt;Columns>
                        <xsl:variable name="name" select="@Name"/>
                        <xsl:for-each select=
                                       "//orm:Object[@CollectionName=$name]">
                           <xsl:for-each select="orm:Property">
                           &lt;asp:BoundColumn<xsl:text/>
                              <xsl:if test="@IsPrimaryKey='true' or 
                                       @IsLookup='true'">
                               Visible="False"</xsl:if>
                               DataField="<xsl:value-of select="@Name"/>" 
                               HeaderText="<xsl:value-of select="@Caption"/>">
                           &lt;/asp:BoundColumn>
                           </xsl:for-each>
                           <!--There's a couple of problems where. You don't want  
                               the current parent to be displayed and the other  
                               parents should not be displayed with their primary 
                               keys. -->
                           <xsl:for-each select="orm:ParentObject">
                           &lt;asp:ButtonColumn 
                                 Visible="false" 
                                 DataTextField="<xsl:value-of 
                                          select="orm:ChildKeyField/@Name"/>" 
                                 HeaderText="<xsl:value-of 
                                          select="@SingularName"/>" 
                                 CommandName="Select<xsl:value-of 
                                          select="@SingularName"/>">
                           &lt;/asp:ButtonColumn>
                           </xsl:for-each>
                           &lt;asp:ButtonColumn 
                                 Text="Remove" 
                                 CommandName="Delete">
                           &lt;/asp:ButtonColumn>
                           &lt;asp:ButtonColumn 
                                 Text="Details" 
                                 CommandName="Select">
                           &lt;/asp:ButtonColumn>
                        </xsl:for-each>
                        &lt;/Columns>
                     &lt;/asp:datagrid>
                     &lt;asp:button 
                           id="btnAdd<xsl:value-of select="@Name"/>" 
                           runat="server" Width="168px" 
                           Text="Add <xsl:value-of select="@Caption"/>">
                     &lt;/asp:button>&lt;/TD>
               &lt;/TR>
               </xsl:for-each>
            &lt;/TABLE>
         &lt;/P>
         &lt;asp:button id="btnSave" 
               runat="server" Text="Save">
         &lt;/asp:button>&amp;nbsp;&amp;nbsp;&amp;nbsp;
         &lt;asp:button id="btnCancel" 
               runat="server" Text="Cancel">
         &lt;/asp:button>&lt;/form>
   &lt;/body>
&lt;/HTML>

</xsl:template> 


</xsl:stylesheet>

  