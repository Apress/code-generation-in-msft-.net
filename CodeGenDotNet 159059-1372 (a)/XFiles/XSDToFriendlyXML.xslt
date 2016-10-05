<?xml version="1.0" encoding="UTF-8" ?>
<!-- Summary:  Creates friendly XML output for an XSD containing a single DataSet !-->
<!-- Created: Kathleen Dollard Feb 26, 2003 !-->
<!-- TODO: !-->
<!--Required header !-->
<xsl:stylesheet version="1.0" 
			xmlns:ext="http://www.kadgen/ExtensionObject"
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://www.kadgen/DatabaseStructure"
			xmlns:msdata="urn:schemas-microsoft-com:xml-msdata">
	<xsl:import href="Support.xslt" />
	<xsl:output method="xml" encoding="UTF-8" indent="yes" />
	<xsl:preserve-space elements="*" />
	
	<xsl:template match="/">
		<xsl:apply-templates select="xs:schema" mode="DataSet" />
	</xsl:template>
	
	<xsl:template match="xs:schema" mode="DataSet">
		<xsl:element name="dbs:DataStructures">
   		<xsl:attribute name="xmlns">http://kadgen/DatabaseStructure</xsl:attribute>
   		<xsl:element name="dbs:DataStructure">
				<xsl:attribute name="Name">
					<xsl:value-of select="@id" />
				</xsl:attribute>
				<xsl:element name="dbs:Tables">
					<xsl:apply-templates 
								select="xs:element/xs:complexType/xs:choice/xs:element" 
								mode="Table" >
						<xsl:sort select="name" case-order="upper-first" />
					</xsl:apply-templates>
				</xsl:element>
   		</xsl:element> 
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="xs:element" mode="Table">
		<xsl:variable name="tablename" select="@name" />
		<xsl:variable name="pKeyName">
			<xsl:call-template name="GetPKeyName">
				<xsl:with-param name="TableName" select="$tablename" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="fixedname" select="ext:FixName(@name)"/>
		<xsl:element name="dbs:Table">
			<xsl:attribute name="Name">
				<xsl:value-of select="$fixedname" />
			</xsl:attribute>
			<xsl:attribute name="OriginalName">
				<xsl:value-of select="$tablename" />
			</xsl:attribute>
			<xsl:attribute name="SingularName">
				<xsl:value-of select="ext:GetSingular($fixedname)" />
			</xsl:attribute>
			<xsl:attribute name="PluralName">
				<xsl:value-of select="ext:GetPlural($fixedname)" />
			</xsl:attribute>
			<xsl:element name="dbs:TableColumns">
	         <xsl:apply-templates select="xs:complexType/xs:sequence/xs:element" 
		                           mode="Column" />
			</xsl:element>
			<xsl:element name="dbs:TableConstraints">
				<xsl:call-template name="GetPrimaryKey">
					<xsl:with-param name="pKeyName" select="$pKeyName" />
				</xsl:call-template>
				<xsl:element name="dbs:TableRelations">
					<xsl:apply-templates select="//xs:keyref[@refer=$pKeyName]" 
												mode="GetChildRelations" />
					<xsl:apply-templates	select=
							"//xs:keyref[xs:selector/@xpath=concat('.//mstns:',$tablename)]" 
												mode="GetParentRelations" />
				</xsl:element>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="xs:element" mode="Column">
		<xsl:variable name="fixedname" select="ext:FixName(@name)"/>
		<xsl:element name="dbs:TableColumn">
			<xsl:attribute name="Name">
				<xsl:value-of select="$fixedname" />
			</xsl:attribute>
			<xsl:attribute name="OriginalName">
				<xsl:value-of select="@name" />
			</xsl:attribute>
			<xsl:attribute name="Ordinal">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<xsl:attribute name="Default">
				<xsl:value-of select="@Default"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@minOccurs='0'">
					<xsl:attribute name="AllowNulls">true</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="AllowNulls">false</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="basetype">
				<xsl:choose>
					<xsl:when test="@type">
						<xsl:value-of select="@type" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select=".//@base" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="sqltype">
			   <xsl:value-of select="ext:GetSQLTypeFromXSDType($basetype)"/>
			</xsl:variable>
			<xsl:attribute name="OriginalType">
				<xsl:value-of select="$basetype" />
			</xsl:attribute>
			<xsl:attribute name="SQLType">
				<xsl:value-of select="$sqltype" />
			</xsl:attribute>
			<xsl:attribute name="NETType">
			   <xsl:value-of select="ext:GetNETTypeFromSQLType($sqltype)"/>
			</xsl:attribute>
			<xsl:if test=".//xs:maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select=".//xs:maxLength/@value" />
				</xsl:attribute>
			</xsl:if>
		</xsl:element>
	</xsl:template>
	
	<xsl:template name="GetPKeyName">
		<xsl:param name="TableName" />
		<xsl:for-each select="//xs:unique[@msdata:PrimaryKey='true']/xs:selector[
		                     @xpath=concat('.//mstns:',$TableName)]">
			<!-- We are currently in the child node of the primary key, and have 
				  confirmed there is a primary key -->
			<xsl:value-of select="../@name" />
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template name="GetPrimaryKey">
		<xsl:param name="pKeyName" />
		<xsl:for-each select="//xs:unique[@name=$pKeyName]">
			<xsl:element name="dbs:PrimaryKey">
				<xsl:for-each select="xs:field">
					<xsl:element name="dbs:PKField">
						<xsl:attribute name="Name">
							<xsl:value-of select="substring-after(@xpath,'mstns:')" />
						</xsl:attribute>
						<xsl:attribute name="Ordinal">
							<xsl:value-of select="position()" />
						</xsl:attribute>
					</xsl:element>
				</xsl:for-each>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="xs:keyref" mode="GetChildRelations">
		<!--  I am including parent and child fields because otherwise people might
				be lazy, not track down the correct primary key and get messed up when
				they differ -->
		<xsl:variable name="refer" select="@refer" />
		<xsl:element name="dbs:ChildTable">
			<xsl:attribute name="Name">
				<xsl:value-of select="substring-after(xs:selector/@xpath,'mstns:')" />
			</xsl:attribute>
			<xsl:for-each select="xs:field">
				<xsl:element name="dbs:ChildKeyFields">
					<xsl:attribute name="Name">
						<xsl:value-of select="substring-after(@xpath,'mstns:')" />
					</xsl:attribute>
					<xsl:attribute name="Ordinal">
						<xsl:value-of select="position()" />
					</xsl:attribute>
				</xsl:element>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="xs:keyref" mode="GetParentRelations">
		<xsl:variable name="relationnode" select="." />
		<xsl:variable name="pKey" select="@refer" />
		<xsl:element name="dbs:ParentRelations">
			<xsl:for-each select="//xs:unique[@name=$pKey]/xs:selector">
				<xsl:element name="dbs:ParentRelation">
					<xsl:attribute name="ParentTable">
						<xsl:value-of select="substring-after(@xpath,'mstns:')" />
					</xsl:attribute>
					<xsl:for-each select="../xs:field">
						<xsl:element name="dbs:ParentKeyField">
							<xsl:attribute name="Name">
								<xsl:value-of select="substring-after(@xpath,'mstns:')" />
							</xsl:attribute>
							<xsl:attribute name="Ordinal">
								<xsl:value-of select="position()" />
							</xsl:attribute>
						</xsl:element>
					</xsl:for-each>
					<xsl:for-each select="$relationnode/xs:field">
						<xsl:element name="dbs:ChildField">
							<xsl:attribute name="Name">
								<xsl:value-of select="substring-after(@xpath,'mstns:')" />
							</xsl:attribute>
							<xsl:attribute name="Ordinal">
								<xsl:value-of select="position()" />
							</xsl:attribute>
						</xsl:element>
					</xsl:for-each>
				</xsl:element>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
