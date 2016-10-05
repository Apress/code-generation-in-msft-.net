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
  Summary:  One of the steps of the ORM transformation process. -->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
         xmlns:dbs="http://kadgen/DatabaseStructure"
         xmlns:orm="http://kadgen.com/KADORM.xsd" 
         xmlns:net="http://kadgen.com/NETTools">
<xsl:import href="ORMSupport2.xslt"/>
<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:key match="//orm:Object//orm:RetrieveSP//orm:SPRecordset[1]"
         name="RecSetColumns"
         use="concat('InObject-',
               ancestor::orm:Assembly/@Name,'-',
               ancestor::orm:Object/@Name)"/>
               
<xsl:key match="//dbs:DataStructure//dbs:StoredProc//dbs:Recordset"
         name="RecSetColumns"
         use="concat('InStoredProc-',
               ancestor::dbs:DataStructure/@Name,'-',
               ancestor::dbs:StoredProc/@Name,'-',@Name)"/>
   
<xsl:key match="//dbs:DataStructure//dbs:StoredProc"
         name="StoredProcs"
         use="@Name" />            

<xsl:key match="//dbs:Table//dbs:TableColumn"
         name="TableColumns"
         use="concat(ancestor::dbs:DataStructure/@Name,'-',ancestor::dbs:Table/@Name,'-',@Name)" />
         
<xsl:key match="//orm:BuildTable//orm:BuildColumn"
         name="BuildColumns"
         use="concat(ancestor::orm:BuildTable/@Name,'-',@Name)" />
         
<xsl:key match="//orm:SPSet//orm:RunSP"  
         name="SPSets"
         use="concat(ancestor::orm:SPSet/@Name,'-',@Task)" />  

<xsl:key match="//orm:Object"  
         name="Objects"
         use="@Name" />  
         
<!-- <xsl:key match="//orm:Object//orm:RetrieveSP//orm:SPRecordset"
         name="RecSetColumns"
         use="concat('InParent-',
               ancestor::orm:Assembly/@Name,'-',
               ancestor::orm:Object/@Name,'-',
               position())"/> -->

<!--<xsl:key match="//dbs:DataStructure//dbs:StoredProc//dbs:Recordset"
         name="RecSetColumns"
         use="concat('InParentStoredProc-',
               ancestor::dbs:DataStructure/@Name,'-',
               ancestor::dbs:StoredProc/@Name,'-',@Name)"/> -->

<xsl:param name="filename"/>
<xsl:param name="gendatetime"/>

<xsl:template match="/ | @* | node()">
	<xsl:copy>
		<xsl:apply-templates select="@*"  />
		<xsl:apply-templates select="node()"/>
		<xsl:choose>
	      <xsl:when test="name()='orm:RunSP'">
				<xsl:call-template name="RunSPParams" />
	      </xsl:when>
	      <xsl:when test="name()='orm:Object'">
	         <xsl:call-template name="Objects"/>
	      </xsl:when>
		</xsl:choose>
	</xsl:copy>
	<xsl:if test="name()='orm:MappingRoot'">
	</xsl:if>
</xsl:template>

<!-- Add RunSPParam to object  -->
<xsl:template name="RunSPParams">
   <xsl:param  name="task"/>
	<xsl:if test="orm:AllRunSPParam or count(orm:RunSPParam)=0">
		<xsl:choose>
			<xsl:when test="@Task='Retrieve'">
				<xsl:call-template name="SPParamUniqueRecord"/>
			</xsl:when>
			<xsl:when test="@Task='Create'">
				<xsl:call-template name="SPParamAllColumns" />
			</xsl:when>
			<xsl:when test="@Task='Update'">
				<xsl:call-template name="SPParamAllColumns"/>
			</xsl:when>
			<xsl:when test="@Task='Delete'">
				<xsl:call-template name="SPParamUniqueRecord"/>
			</xsl:when>
			<xsl:when test="@Task='SetSelect'">
				<!-- No parameters by default -->
			</xsl:when>
		</xsl:choose>
	</xsl:if>
</xsl:template>

<xsl:template name="SPParamAllColumns">
	<xsl:for-each select="ancestor::orm:SPSet/orm:BuildRecordset[@Name='RecSet']/orm:BuildTable[1]/orm:BuildColumn">
		<xsl:element name="orm:RunSPParam">
			<xsl:call-template name="CopyImportantColumnAttributes"/>
			<xsl:attribute name="TableName">
				<xsl:value-of select="ancestor::orm:BuildTable/@Name"/>
			</xsl:attribute>
		</xsl:element>
	</xsl:for-each>
</xsl:template>

<xsl:template name="SPParamUniqueRecord">
	<xsl:for-each select="../orm:RetrieveParam">
		<xsl:element name="orm:RunSPParam">
			<xsl:call-template name="CopyImportantColumnAttributes"/>
		</xsl:element>
	</xsl:for-each>
</xsl:template>


<!-- Add SP information to object -->
<xsl:template name="Objects">
		<xsl:apply-templates select="@*"  />
		<xsl:apply-templates select="node()
		               [name()!='orm:CreateSP' and
		                name()!='orm:ChildCollection' and
		                name()!='orm:ParentObject' and
		                name()!='orm:UpdateSP' and
		                name()!='orm:DeleteSP' and
		                name()!='orm:SetSelectSP' and
		                name()!='orm:AllProperties' and
		                name()!='orm:Property' ]"/>
   <xsl:call-template name="UseSPSets"/> 
   <xsl:call-template name="Properties"/>
</xsl:template>

<xsl:template name="UseSPSets">
	<xsl:variable name="deftranstype" >
		<xsl:value-of select="ancestor-or-self::*[@TransactionType][1]/@TransactionType" />
	</xsl:variable>
	<xsl:variable name="deffetchtrans" >
		<xsl:value-of select="ancestor-or-self::*[@TransactionForRetrieve][1]/@TransactionForRetrieve" />
	</xsl:variable>
	<xsl:variable name="skipfetch" >
		<xsl:choose>
			<xsl:when test="orm:UsePSSet[@UseFor]">
				<xsl:for-each select="orm:UseSPSet[contains(@UseFor,'R')]">
					<xsl:value-of select="@NoFetch" />
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="orm:UseSPSet">
					<xsl:value-of select="@NoFetch" />
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	
	<xsl:call-template name="ListSP">
		<xsl:with-param name="type" select="'Create'"/>
		<xsl:with-param name="deftranstype" select="$deftranstype"/>
		<xsl:with-param name="deffetchtrans" select="$deffetchtrans"/>
	</xsl:call-template>
	<xsl:choose>
		<xsl:when test="starts-with($skipfetch,'true')"/>
		<xsl:otherwise>
			<xsl:call-template name="ListSP">
				<xsl:with-param name="type" select="'Retrieve'"/>
				<xsl:with-param name="deftranstype" select="$deftranstype"/>
				<xsl:with-param name="deffetchtrans" select="$deffetchtrans"/>
			</xsl:call-template>
		</xsl:otherwise>
	</xsl:choose>
	<xsl:call-template name="ListSP">
		<xsl:with-param name="type" select="'Update'"/>
		<xsl:with-param name="deftranstype" select="$deftranstype"/>
		<xsl:with-param name="deffetchtrans" select="$deffetchtrans"/>
	</xsl:call-template>
	<xsl:call-template name="ListSP">
		<xsl:with-param name="type" select="'Delete'"/>
		<xsl:with-param name="deftranstype" select="$deftranstype"/>
		<xsl:with-param name="deffetchtrans" select="$deffetchtrans"/>
	</xsl:call-template>
	<xsl:call-template name="ListSP">
		<xsl:with-param name="type" select="'SetSelect'"/>
		<xsl:with-param name="deftranstype" select="$deftranstype"/>
		<xsl:with-param name="deffetchtrans" select="$deffetchtrans"/>
	</xsl:call-template>
</xsl:template>

<xsl:template name="ListSP">
	<xsl:param name="type"/>
	<xsl:param name="deftranstype"/>
	<xsl:param name="deffetchtrans"/>
	<xsl:variable name="elementname" select="concat('orm:',$type,'SP')"/>
	<xsl:variable name="spsetname" >
	   <xsl:call-template name="GetUseSPSetName">
	      <xsl:with-param name="type" select="$type"/>
	   </xsl:call-template>
	</xsl:variable>
	<!--<xsl:for-each select="//orm:SPSet[@Name=$spsetname]/orm:RunSP[@Task=$type]">-->
   <xsl:for-each select="key('SPSets',concat($spsetname,'-',$type))">
	   <xsl:choose>
	      <xsl:when test="*[name()=$elementname]" >
	         <xsl:copy>
   	         <xsl:call-template name="ListSPAttributes" >
   	            <xsl:with-param name="type" select="$type" />
   	            <xsl:with-param name="deftranstype" select="$deftranstype" />
   	            <xsl:with-param name="deffetchtrans" select="$deffetchtrans" />
   	         </xsl:call-template>
      		   <xsl:apply-templates select="@*"  />
	      	   <xsl:apply-templates select="node()" />
	            <xsl:call-template name="ListSPContents" />
	         </xsl:copy>
	      </xsl:when>
	      <xsl:otherwise>
		      <xsl:element name="{$elementname}">
   	         <xsl:call-template name="ListSPAttributes" >
   	            <xsl:with-param name="type" select="$type" />
   	            <xsl:with-param name="deftranstype" select="$deftranstype" />
   	            <xsl:with-param name="deffetchtrans" select="$deffetchtrans" />
   	         </xsl:call-template>
	            <xsl:call-template name="ListSPContents" />
            </xsl:element>
	      </xsl:otherwise> 
	   </xsl:choose>
	</xsl:for-each>
</xsl:template>

<xsl:template name="GetUseSPSetName">
   <xsl:param name="type"/>
	<!-- Grab either the UseSPSet with the matching UseFor attribute, or an UseSPSet that does not have a UseFor attribute -->
	<xsl:variable name="typefirstletter" select="substring($type,1,1)"/>
	<xsl:choose>
		<xsl:when test="count(orm:UseSPSet[contains(@UseFor,$typefirstletter)]) > 0">
			<xsl:value-of select="orm:UseSPSet[contains(@UseFor,$typefirstletter)]/@Name"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="orm:UseSPSet[count(@UseFor)=0]/@Name" />
			<!-- <xsl:for-each select="orm:UseSPSet">
				<xsl:choose>
					<xsl:when test="@UseFor"/>
					<xsl:otherwise>
						<xsl:value-of select="@Name" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>-->
		</xsl:otherwise>
	</xsl:choose> 
</xsl:template>

<xsl:template name="ListSPAttributes">
	<xsl:param name="type" />
	<xsl:param name="deftranstype" />
	<xsl:param name="deffetchtrans" />
	<xsl:variable name="name"  select="@Name" />
	<xsl:variable name="allowtrans">
		<xsl:choose> 
			<!-- if the build element has a transactionforretrieve it overrides -->
			<xsl:when test="($type='Retrieve' or $type='SetSelect') and @TransactionForRetrieve" >
				<xsl:value-of select="@TransactionForRetrieve" />
			</xsl:when>
			<xsl:when test="$type='Retrieve' or $type='SetSelect'">
				<xsl:value-of select="$deffetchtrans" />
			</xsl:when>
			<xsl:otherwise>true</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
	<xsl:choose>
		<xsl:when test="$allowtrans='false'">
			<xsl:attribute name="TransactionType">None</xsl:attribute>
		</xsl:when>
		<xsl:otherwise>
			<xsl:attribute name="TransactionType"><xsl:value-of select="$deftranstype"/></xsl:attribute>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="ListSPContents">
	<xsl:variable name="name"  select="@Name" />
	<xsl:variable name="tablename" select="ancestor-or-self::orm:SPSet/@TableName" />
   <xsl:variable name="dsname" 
           select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure"/>
	<!--<xsl:for-each select="//dbs:DataStructure//dbs:StoredProc[@Name=$name]"> -->
   <xsl:for-each select="key('StoredProcs',$name)">
		<xsl:for-each select=".//dbs:StoredProcParameter">
			<xsl:element name="orm:Parameter">
				<xsl:attribute name="Name"><xsl:value-of select="substring-after(@Name,'@')"/></xsl:attribute> 
				<xsl:attribute name="Type"><xsl:value-of select="@Type"/></xsl:attribute> 
			</xsl:element>
		</xsl:for-each>
		<xsl:for-each select=".//dbs:StoredProcPrivilege">
			<xsl:element name="orm:Privilege">
				<xsl:for-each select="@*">
					<xsl:copy/>
				</xsl:for-each>
			</xsl:element>
		</xsl:for-each>
		<xsl:for-each select=".//dbs:Recordset">
			<xsl:element name="orm:SPRecordSet">
				<xsl:attribute name="Name">
					<xsl:value-of select="@Name" />
				</xsl:attribute>
				<xsl:attribute name="Test1">Here</xsl:attribute>
				<xsl:for-each select="dbs:Column">
					<xsl:element name="orm:Column">
				      <xsl:variable name="columnname" select="@Name"/>
				      <!--<xsl:for-each select="//dbs:Table[@Name=$tablename]//dbs:TableColumn[@Name=$columnname]">-->
                  <xsl:for-each select="key('TableColumns',concat($dsname,'-',$tablename,'-',$name))" >
				         <xsl:call-template name="CopyImportantColumnAttributes" />
						   <xsl:for-each select="@*">
							   <xsl:copy/>
						   </xsl:for-each>
				      </xsl:for-each>
						<xsl:for-each select="@*">
						   <xsl:if test="string-length(.)">
							   <xsl:copy/>
						   </xsl:if>
						</xsl:for-each>
					</xsl:element>
				</xsl:for-each>
			</xsl:element>
		</xsl:for-each>
	</xsl:for-each> 
</xsl:template>

<!-- Add Property information to object  -->
<xsl:template name="Properties">
   <xsl:variable name="runall">
      <xsl:if test="count(orm:Property)=0 or orm:AllProperties">true</xsl:if>
   </xsl:variable>
   <xsl:variable name="objecttablename" select="@TableName"/>
   <xsl:variable name="dsname" 
           select="ancestor-or-self::*[@MapDataStructure][1]/@MapDataStructure"/>
	<xsl:variable name="spsetname">
	   <xsl:call-template name="GetUseSPSetName">
	      <xsl:with-param name="type" select="'Retrieve'"/>
	   </xsl:call-template>
	</xsl:variable> 
	<xsl:variable name="columnkey">
	   <xsl:choose>
	      <!-- Added 10/16/03 -->
	      <xsl:when test="string-length(@ChildOf)!=0 and string-length(@Position)!=0">
	         <xsl:variable name="childof" select="@ChildOf" />
	         <xsl:variable name="parentspsetname" >
	            <!--<xsl:for-each select="//orm:Object[@Name=$childof]">-->
               <xsl:for-each select="key('Objects',$childof)">
	               <xsl:call-template name="GetUseSPSetName">
	                  <xsl:with-param name="type" select="'Retrieve'"/>
	               </xsl:call-template>
	            </xsl:for-each>
	         </xsl:variable>
	         <!--<xsl:variable name="parentspname" 
	               select="//orm:SPSet[@Name=$parentspsetname]//orm:RunSP[@Task='Retrieve']/@Name"/>-->
	         <xsl:variable name="parentspname" 
	               select="key('SPSets',concat($parentspsetname,'-Retrieve'))/@Name"/>
	         <xsl:value-of select="concat('InStoredProc-',
	            $dsname,'-',$parentspname,'-Table',@Position)"/>
	      </xsl:when>
	      <xsl:when test="orm:RetrieveSP/orm:SPRecordset/orm:Column">
	         <xsl:value-of select="concat('InObject-',
               ancestor::orm:Assembly/@Name,'-',
               ancestor::orm:Object/@Name)"/>
	      </xsl:when>
	      <xsl:when test="orm:RetrieveSP">
	         <xsl:variable name="spname" select="@Name"/>
            <xsl:value-of select="concat('InStoredProc-',
               $dsname,'-',$spname,'-Table')"/>
  	      </xsl:when>
	      <xsl:otherwise>
	         <!--<xsl:variable name="spname" 
	               select="//orm:SPSet[@Name=$spsetname]//orm:RunSP[@Task='Retrieve']/@Name"/>-->
	         <xsl:variable name="spname" 
	               select="key('SPSets',concat($spsetname,'-Retrieve'))/@Name"/>
            <xsl:value-of select="concat('InStoredProc-',$dsname,'-',$spname,'-Table')"/>
	      </xsl:otherwise>
	   </xsl:choose>
	</xsl:variable>
   <xsl:for-each select="orm:Property">
      <xsl:copy>
         <xsl:call-template name="PropertyAttributes"/>
         <xsl:apply-templates select="@*"  />
	      <xsl:apply-templates select="node()" />
         <xsl:call-template name="PropertyContents">
            <xsl:with-param name="tablename" select="$objecttablename"/>
         </xsl:call-template>
      </xsl:copy>
   </xsl:for-each>
   <xsl:if test="$runall='true'">
      <xsl:for-each select="key('RecSetColumns',$columnkey)/dbs:Column | key('RecSetColumns',$columnkey)/orm:Column">
         <xsl:variable name="name" select="@Name"/>
         <xsl:variable name="recsetcolumn" select="."/>
         <xsl:if test="count(orm:Property[@Name=$name])=0">
            <xsl:variable name="tablename">
               <xsl:choose>
                  <xsl:when test="name()='orm:Column'">
                     <!-- This does not look right 10/16/03 KAD -->
                     <xsl:value-of select="ancestor::orm:Table/@Name"/>
                  </xsl:when>
                  <xsl:otherwise>
                     <xsl:value-of select="$objecttablename"/>
                  </xsl:otherwise>
               </xsl:choose>
            </xsl:variable>
            <!-- OK, the following is _really_ ugly. I copy the non-empty attributes from the table, then the important ones, then the non-empty attributes from the stored proc. It may all be necessary, but it's effective -->
            <xsl:element name="orm:Property">
               <xsl:for-each select="key('BuildColumns',concat($tablename,'-',$name))">
                  <xsl:for-each select="@*">
                     <xsl:if test="string-length(.)!=0">
                        <xsl:copy/>
                     </xsl:if>
                  </xsl:for-each>
               </xsl:for-each>
               <!--<xsl:for-each select="//dbs:DataStructure[@Name=$dsname]//dbs:Table[@Name=$tablename]//dbs:TableColumn[@Name=$name]">-->
               <xsl:for-each select="key('TableColumns',concat($dsname,'-',$tablename,'-',$name))">
                  <!-- A problem arose here in that empty values from the stored proc were overwriting real values already put in the element, but we want the sp info to overwrite if it has a real value -->
                  <xsl:for-each select="@*">
                     <xsl:if test="string-length(.)!=0">
                        <xsl:copy/>
                     </xsl:if>
                  </xsl:for-each>
                  <!-- In this case, the Property Attributes should overwrite the ones directly inferred from database. -->
                  <xsl:call-template name="PropertyAttributes"/>
	               <!-- <xsl:apply-templates select="node()" /> -->
                  <!-- <xsl:call-template name="PropertyContents"/> -->
               </xsl:for-each>
               <xsl:for-each select="@*">
                  <xsl:choose>
                     <xsl:when test="name()='MaxLength' and .&lt;=0"/>
                     <xsl:when test="string-length(.)!=0">
                        <xsl:copy/>
                     </xsl:when>
                  </xsl:choose>
               </xsl:for-each>
               <xsl:call-template name="PropertyAttributes" />
               <xsl:call-template name="PropertyContents">
                  <xsl:with-param name="tablename" select="$objecttablename"/>
               </xsl:call-template>
            </xsl:element>
         </xsl:if>
      </xsl:for-each>
   </xsl:if>
</xsl:template>

<xsl:template name="PropertyAttributes">
   <xsl:call-template name="CopyImportantColumnAttributes"/>
</xsl:template>

<xsl:template name="PropertyContents">
   <xsl:param name="tablename" />
   <xsl:variable name="propertyname" select="@Name"/>
   <xsl:for-each select="ancestor::dbs:DataStructure//dbs:Table[@Name=$tablename]//dbs:TableColumn[@Name=$propertyname]">
      <xsl:for-each select=".//dbs:TableColumnPrivilege">
         <xsl:element name="orm:TableColumnPrivilege">
            <xsl:apply-templates select="@*" />
         </xsl:element>
      </xsl:for-each>
      <xsl:for-each select=".//dbs:CheckConstraint">
         <xsl:element name="orm:CheckConstraint">
            <xsl:apply-templates select="@*" />
         </xsl:element>
      </xsl:for-each>
   </xsl:for-each>
</xsl:template>


<!--  -->

</xsl:stylesheet> 
  