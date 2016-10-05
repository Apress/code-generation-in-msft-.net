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
  Summary: Provides support routines for generating the middle tier !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd"  
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<!-- Probably move this into a generic location -->

<xsl:template name="NLIndent">
	<xsl:param name="level"/>
	<xsl:text>&#x0d;&#x0a;</xsl:text>
	<xsl:call-template name="Indent"/>
</xsl:template>

<xsl:template match="orm:CreateSP" mode="CreatePrivileges">
		<xsl:call-template name="CheckPrivileges">
			<xsl:with-param name="privilegetype" select="'C'"/>
			<xsl:with-param name="failuremessage" 
						select="concat('User not authorized to add a', $Name)"/>
			<xsl:with-param name="space" select="'      '"/>
		</xsl:call-template>
</xsl:template>

<xsl:template match="orm:RetrieveSP" mode="RetrievePrivileges">
  <xsl:call-template name="CheckPrivileges">
		<xsl:with-param name="privilegetype" select="'R'"/>
		<xsl:with-param name="failuremessage" select="concat('User not authorized to retrieve a', $Name)"/>
		<xsl:with-param name="space" select="'      '"/>
  </xsl:call-template>
</xsl:template>

<xsl:template match="orm:UpdateSP" mode="UpdatePrivileges">
	<xsl:call-template name="CheckPrivileges">
			<xsl:with-param name="privilegetype" select="'U'"/>
			<xsl:with-param name="failuremessage"
				select="concat('User not authorized to update a', $Name)"/>
			<xsl:with-param name="space" select="'        '"/>
	</xsl:call-template>
</xsl:template> 

<xsl:template match="orm:DeleteSP" mode="DeletePrivileges">
  <xsl:call-template name="CheckPrivileges">
		<xsl:with-param name="privilegetype" select="'D'"/>
		<xsl:with-param name="failuremessage" select="concat('User not authorized to delete a', $Name)"/>
		<xsl:with-param name="space" select="'      '"/>
  </xsl:call-template>
</xsl:template>

<xsl:template name="CheckPrivileges">
  <xsl:param name="privilegetype"/>
  <xsl:param name="failuremessage" />
  <xsl:param name="space" />
  <!-- I am worried about the case sensitivity of this -->
  <xsl:text>&#x0d;&#x0a;</xsl:text>
  <xsl:choose>
  <xsl:when test="string-length($privilegetype)=0">
		<xsl:value-of select="$space"/>// HARNESS: There seems to be an error in setting privileges in the code generation template
		<xsl:value-of select="$space"/>// NOTE: No security was defined for this action. All uses will receive an exception
		<xsl:value-of select="$space"/>throw new System.Security.SecurityException( "<xsl:value-of select="$failuremessage"/>" );
  </xsl:when>
  <xsl:when test="orm:Privilege[@Grantee='Public']">
		<!-- Just a quick and dirty way to ignore these -->
		<xsl:value-of select="$space"/>// Everyone (Public) is allowed to perform this action
  </xsl:when>
  <xsl:when test="count(orm:Privilege)=0">
		<xsl:value-of select="$space"/>// NOTE: No security was defined for this action. All uses will receive an exception
		<xsl:value-of select="$space"/>throw new System.Security.SecurityException( "<xsl:value-of select="$failuremessage"/>" );
  </xsl:when>
  <xsl:otherwise>
    <xsl:value-of select="$space"/>if (<xsl:text/>
	 <xsl:for-each select="orm:Privilege">
		<xsl:text/>!System.Threading.Thread.CurrentPrincipal.IsInRole( "<xsl:value-of select="@Grantee" />" ) <xsl:text/>
		<xsl:choose>
			<xsl:when test="position() != last()"> &amp;&amp;
			</xsl:when>
			<xsl:otherwise> )<xsl:text/>
				<xsl:text>&#x0d;&#x0a;</xsl:text>
				<xsl:value-of select="$space"/>{
				<xsl:text>&#x0d;&#x0a;</xsl:text>
				<xsl:value-of select="$space"/>  throw new System.Security.SecurityException(
					"<xsl:value-of select="$failuremessage"/>" );<xsl:text/>
				<xsl:text>&#x0d;&#x0a;</xsl:text>
				<xsl:value-of select="$space"/>}
			</xsl:otherwise>
		</xsl:choose> 
	 </xsl:for-each>
  </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:template name="CanPrivileges">
  <xsl:param name="privilegetype"/>
  <xsl:choose>
  <xsl:when test="string-length($privilegetype)=0">
		// HARNESS: There's an error setting privileges in the code generation template
		// NOTE: No security defined for action. All uses will receive an exception
		return false;
  </xsl:when>
  <xsl:when test="orm:Privilege[@Grantee='Public']">
		<!-- Just a quick and dirty way to ignore these -->
		// Everyone (Public) is allowed to perform this action
		return true;
  </xsl:when>
  <xsl:when test="count(orm:Privilege)=0">
		// NOTE: No security defined for action. All uses will receive an exception
		return false;
  </xsl:when>
  <xsl:otherwise>
      if( <xsl:text/>
	 <xsl:for-each select="orm:Privilege">
		<xsl:text/>System.Threading.Thread.CurrentPrincipal.IsInRole( "<xsl:text/>
		           <xsl:value-of select="@Grantee" />" ) <xsl:text/>
			<xsl:if test="position() != last()"> || 
			</xsl:if>
	 </xsl:for-each>
			<xsl:text/> )
		{
		   return true;
		}
		else
		{
		   return false;
		}
  </xsl:otherwise>
  </xsl:choose>
</xsl:template>

<xsl:template match="orm:Column | orm:Property" mode="RetrieveFromReader">
	<xsl:param name="useme"	select="'true'"/>
	<xsl:param name="forcefield" select="'false'" />
  	<xsl:variable name="type" select="@NETType" />
  	<xsl:variable name="object" select="ancestor::orm:Object" />
  	<xsl:variable name="safeproc">
		<xsl:choose>
			<xsl:when test="primarykey"/>
			<xsl:when test="starts-with($type,'System.')">
				<xsl:text/>Get<xsl:value-of select="substring-after($type,'System.')"/>
					<xsl:text/>( <xsl:value-of select="position()-1"/> )<xsl:text/>
			</xsl:when>
			<xsl:when test="//orm:SpecialType[@Name=$type]/orm:SafeDataRetrieve">
				<xsl:variable name="pos" select="position()-1"/>
				<xsl:for-each
							select="//orm:SpecialType[@Name=$type]/orm:SafeDataRetrieve" >
					<xsl:text/><xsl:value-of 
							select="@Name"/> 
					<xsl:text/>( <xsl:value-of select="$pos"/>
					<xsl:call-template name ="SafeDataParam"/>
					<xsl:text/> )<xsl:if test="orm:SafeDataProperty">.<xsl:value-of select="orm:SafeDataProperty/@Explicit"/></xsl:if>
				</xsl:for-each> 
			</xsl:when>
			<xsl:otherwise>
				<xsl:text/>Get<xsl:value-of select="$type"/> 
					<xsl:text/>(<xsl:value-of select="position()-1"/>)<xsl:text/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<!--<xsl:choose>
		<xsl:when test="$object/orm:Property/@ReadOnly='true'">
			m<xsl:value-of select="@Name"/> = .<xsl:value-of select="$safeproc"/> 
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>&#xd;&#xa;&#x9;&#x9;</xsl:text>
			<xsl:if test="$useme='true'">Me.</xsl:if>
			<xsl:value-of select="@Name"/> = .<xsl:value-of select="$safeproc"/> 
		</xsl:otherwise>
	</xsl:choose> -->
   <xsl:text>&#x09;</xsl:text>
	<xsl:call-template name="PropertyOrSet" >
	   <xsl:with-param name="forcefield" select="$forcefield" />
	   <xsl:with-param name="object" select="$object" />
	   <xsl:with-param name="val" select="concat('dr.',$safeproc)"/>
	</xsl:call-template>
</xsl:template>

<xsl:template name="PropertyOrSet">
   <xsl:param name="forcefield" select="'false'"/>
   <xsl:param name="object"/>
   <xsl:param name="val"/>
   <xsl:variable name="name" select="@Name"/>
   <xsl:choose>
      <xsl:when test="$forcefield='true'" >
      m<xsl:value-of select="$name"/> = <xsl:value-of select="$val"/>;<xsl:text/>
      </xsl:when> 
      <xsl:when test="$object//orm:Property[@Name=$name]/@IsPrimaryKey='true'">
      Set<xsl:value-of select="$name"/>( <xsl:value-of select="$val"/> );<xsl:text/>   
      </xsl:when>
      <xsl:otherwise>
      this.<xsl:value-of select="$name" /> = <xsl:value-of select="$val"/>;<xsl:text/>
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>



<xsl:template match="orm:Property" mode="RetrieveFromReaderOld">
	<xsl:param name="prefix" select="'m'"/>
  	<xsl:variable name="type" select="@NETType" />
  	<xsl:variable name="propname" select="concat($prefix,@Name)"/>
  	<xsl:variable name="safeproc">
		<xsl:choose>
			<xsl:when test="primarykey"/>
			<xsl:when test="starts-with($type,'System.')">
				<xsl:text/>Get<xsl:value-of select="substring-after($type,'System.')"/>
					<xsl:text/>( <xsl:value-of select="position()-1"/> );<xsl:text/>
			</xsl:when>
			<xsl:when test="//orm:SpecialType[@Name=$type]/orm:SafeDataRetrieve">
				<xsl:variable name="pos" select="position()-1"/>
				<xsl:for-each
							select="//orm:SpecialType[@Name=$type]/orm:SafeDataRetrieve" >
					<xsl:text/><xsl:value-of 
							select="@Name"/> 
					<xsl:text/>( <xsl:value-of select="$pos"/>
					<!-- xsl:call-template name ="SafeDataParam" This was causing trouble and I didn't see it's usefulness -->
					<xsl:text/> );<xsl:text/>
				</xsl:for-each> 
			</xsl:when>
			<xsl:otherwise>
				<xsl:text/>Get<xsl:value-of select="$type"/> 
					<xsl:text/>( <xsl:value-of select="position()-1"/> );<xsl:text/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:text>&#x0d;&#x0a;              </xsl:text>   				  
	<xsl:value-of select="$propname"/> = dr.<xsl:value-of select="$safeproc"/> 
</xsl:template>

<xsl:template name="SafeDataParam">
	<xsl:for-each select="orm:SafeDataParam">
		<xsl:text/>, <xsl:text/>
		<xsl:choose>
			<xsl:when test="contains(@Explicit,'@')">
				<xsl:value-of select="substring-before(@Explicit, '@')" />
				<xsl:value-of select="@Name" />
				<xsl:value-of select="substring-after(@Explicit, '@')" />
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="@Explicit" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:for-each>
</xsl:template>

<!--  Generic Routines -->
<xsl:template name="Property">
   <xsl:param name="incriteria" select="'false'" />
	<xsl:variable name="readonly">
 <!-- The scope of this method was changed from Protected to Public to support teh following scenario
   Steps:
      1. Create a new address object
      2. Use that object to open and populate the edit form
      3. User edits, including setting values of composit PK, other than app id
      4. User saves
      5. See if collection contains the object
      6. If yes, delete existing and add object
      7. If no, add object
   If you don't use this scenario, don't include this method. Note that this scenario can occur with 
   lookup values if you have database constraints. There is also a change at the end of this template-->
      <!--<xsl:if test="@ReadOnly='true' or @IsPrimaryKey='true'">ReadOnly </xsl:if>-->
      <!--<xsl:if test="@ReadOnly='true' or ($incriteria='true' and @IsPrimaryKey='true')">ReadOnly </xsl:if>-->
	</xsl:variable>
	<xsl:variable name="type">
		<xsl:value-of select="@NETType"/>
	</xsl:variable>
	<xsl:variable name="usetype">
		<xsl:choose>
			<xsl:when test="//orm:SpecialType[@Name=$type and orm:Property/@AccessVia]" >
				<xsl:value-of select="//orm:SpecialType[@Name=$type]/orm:Property/@AccessVia"/>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="@NETType"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="propget">
	   <xsl:call-template name="GetMethod" >
	      <xsl:with-param name="usetype" select="$usetype" />
	   </xsl:call-template>
	</xsl:variable>
	<xsl:variable name="propset">
	   <xsl:call-template name="SetMethod" />
	</xsl:variable>
	<xsl:variable name="propvalue">
	   <xsl:call-template name="GetValue" />
	</xsl:variable>
	<xsl:variable name="msgstart" select="concat(../@Name,' ',@Caption)"/>
   public <xsl:value-of select="$usetype"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
   {
		get
		{
		   <xsl:value-of select="$propget" />
			<!--Return m<xsl:value-of select="@Name"/><xsl:value-of select="$propget" /> -->
		}<xsl:text/>
      <xsl:if test="string-length($readonly)=0" >
		set
		{
			if( <xsl:call-template name="Comparison">
				<xsl:with-param name="first" select="concat('m',@Name, $propvalue)" />
				<xsl:with-param name="second" select="'value'" />
				<xsl:with-param name="operand" select="'!='" />
				<xsl:with-param name="type" select="$usetype" />
			</xsl:call-template> )
			{
				m<xsl:value-of select="@Name"/>
						<xsl:value-of select="$propset"/> = value;
				<xsl:if test="not($incriteria='true')">
				Validate<xsl:value-of select="@Name"/>();<xsl:text/>
				MarkDirty();
				</xsl:if> 
			}
		}<xsl:text/>
   </xsl:if>
   }<xsl:text/>
  
   <xsl:if test="$incriteria!='true' and string-length($readonly)=0">
   protected virtual void Validate<xsl:value-of select="@Name"/>()
   {<xsl:text/>
        <xsl:if test="@MaxLength &gt; 0">
             <xsl:if test="string-length(@MaxLength) &lt; 6">
        BrokenRules.Assert( "<xsl:value-of select="@Name"/>Len",
                       "<xsl:value-of select="$msgstart"/> too long  (" + m<xsl:value-of select="@Name"/>.Length.ToString() + " characters); keep at or below <xsl:value-of select="@MaxLength"/> characters",
                       "<xsl:value-of select="@Name"/>",
                       m<xsl:value-of select="concat(@Name,'.Length > ', @MaxLength)"/>
                       <xsl:text/> );<xsl:text/>
             </xsl:if>
        </xsl:if>
        <!-- DK: IsRequired doesn't appear in XML file. Use AllowNulls instead, for now. -->
        <!-- <xsl:if test="@IsRequired='true' and @NETType='System.String'" > -->
        <xsl:if test="@AllowNulls='false' and @NETType='System.String'" >
        BrokenRules.Assert( "<xsl:value-of select="@Name"/>Required",
                       "<xsl:value-of select="$msgstart"/> is required",
                       "<xsl:value-of select="@Name"/>",
                       <xsl:text/>m<xsl:value-of select="concat(@Name,'.Length == 0')"/> );<xsl:text/>
        </xsl:if>  
        <xsl:if test="@AllowNulls='false' and @NETType='System.Int32' and @Empty='0'">
        BrokenRules.Assert( "<xsl:value-of select="@Name"/>Required",
                       "<xsl:value-of select="$msgstart"/> is required",
                       "<xsl:value-of select="@Name"/>",
                       <xsl:text/>m<xsl:value-of select="concat(@Name,' &lt;= 0')"/> );<xsl:text/>
        </xsl:if> 
        <xsl:if test="@AllowNulls='false' and @NETType='System.Guid' and @IsPrimaryKey='false'">
        BrokenRules.Assert("<xsl:value-of select="@Name"/>Required",
                       "<xsl:value-of select="$msgstart"/> is required",
                       "<xsl:value-of select="@Name"/>",
                       <xsl:text/>m<xsl:value-of select="concat(@Name,'.Equals(System.Guid.Empty)')"/> );<xsl:text/>
        </xsl:if> 
        <xsl:variable name="propertyname" select="@Name"/>
        <xsl:for-each select="orm:CheckConstraint">
        BrokenRules.Assert( "<xsl:value-of select="concat($propertyname, 'CheckConstraint', position())"/>",
                       "<xsl:value-of select="@EnglishClause"/>",
                       "<xsl:value-of select="$propertyname"/>",
                       !<xsl:value-of select="@CSharpClause"/> );<xsl:text/>
        </xsl:for-each> 
   }
   <!-- Protected Overridable Sub Validate<xsl:value-of select="@Name"/>()<xsl:text/>
		<xsl:if test="@MaxLength &gt; 0">
			<xsl:if test="string-length(@MaxLength) &lt; 6">
		BrokenRules.Assert("<xsl:value-of select="@Name"/>Len", "<xsl:text/>
					<xsl:value-of select="$msgstart"/> too long", "<xsl:text/>
					<xsl:value-of select="@Name"/>", <xsl:text/>
					<xsl:text/>m<xsl:value-of select="concat(@Name,'.Length >', @MaxLength)"/>
					<xsl:text/>)<xsl:text/>
			</xsl:if>
		</xsl:if>
		<xsl:if test="@IsRequired='true' and @NETType='System.String'" >
		BrokenRules.Assert("<xsl:value-of select="@Name"/>Required", "<xsl:text/>
					<xsl:value-of select="$msgstart"/> is required", "<xsl:text/> 
					<xsl:value-of select="@Name"/>", <xsl:text/>
					<xsl:text/>m<xsl:value-of select="concat(@Name,'.Length = 0')"/>)<xsl:text/>
		</xsl:if>   
   End Sub -->
	</xsl:if> 
  
  <!-- Matching change to the one at the start of this routien -->
  <!--<xsl:if test="string-length($readonly) != 0">-->
  <xsl:if test="@ReadOnly='true' or @IsPrimaryKey='true'">
   protected void Set<xsl:value-of select="@Name"/>( <xsl:value-of select="$usetype"/> value )
   { 
      m<xsl:value-of select="@Name"/> = value;
   }
  </xsl:if>
</xsl:template>

<xsl:template name="ArticulateConstraint">
   <!-- For simplicity, always use the VB constraint as they should be the same -->
   <xsl:variable name="temp1" select="@ClauseVB"/>
   <xsl:variable name="temp2">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp1"/>
         <xsl:with-param name="searchfor" select="'>='"/>
         <xsl:with-param name="replacewith" select="' must be greater than or equal to '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="temp3">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp2"/>
         <xsl:with-param name="searchfor" select="'&lt;='"/>
         <xsl:with-param name="replacewith" select="' must be less than or equal to '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="temp4">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp3"/>
         <xsl:with-param name="searchfor" select="'!='"/>
         <xsl:with-param name="replacewith" select="' must not equal '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="temp5">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp4"/>
         <xsl:with-param name="searchfor" select="'='"/>
         <xsl:with-param name="replacewith" select="' must equal '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="temp6">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp5"/>
         <xsl:with-param name="searchfor" select="'>'"/>
         <xsl:with-param name="replacewith" select="' must be greater than '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:variable name="temp7">
      <xsl:call-template name="ReplaceValue">  
         <xsl:with-param name="searchstring" select="$temp6"/>
         <xsl:with-param name="searchfor" select="'&lt;'"/>
         <xsl:with-param name="replacewith" select="' must be less than '"/>
      </xsl:call-template>
   </xsl:variable>
   <xsl:value-of select="$temp7"/>
</xsl:template>

<xsl:template name="ReplaceValue">
   <xsl:param name="searchstring" />
   <xsl:param name="searchfor" />
   <xsl:param name="replacewith" />
   <xsl:choose>
      <xsl:when test="contains($searchstring, $searchfor)">
         <xsl:variable name="start" select="substring-before($searchstring,$searchfor)"/>
         <xsl:variable name="usestart">
            <xsl:choose>
               <xsl:when test="substring($start,string-length($start))=' '">  
                  <xsl:value-of select="substring($start,1,string-length($start)-1)"/>
               </xsl:when>
               <xsl:otherwise>
                  <xsl:value-of select="$start"/>
               </xsl:otherwise>
            </xsl:choose>
         </xsl:variable>
         <xsl:variable name="after" select="substring-after($searchstring, $searchfor)"/>
         <xsl:variable name="useafter">
            <xsl:call-template name="ReplaceValue">
               <xsl:with-param name="searchstring">
                  <xsl:choose>
                     <xsl:when test="substring($after,string-length($start))=' '">  
                        <xsl:value-of select="substring($after,1,string-length($start)-1)"/>
                     </xsl:when>
                     <xsl:otherwise>
                        <xsl:value-of select="$after"/>
                     </xsl:otherwise>
                  </xsl:choose>
               </xsl:with-param>
               <xsl:with-param name="searchfor" select="$searchfor"/>
               <xsl:with-param name="replacewith" select="$replacewith"/>
            </xsl:call-template>
         </xsl:variable>
         <xsl:value-of select="concat($usestart, $replacewith, $useafter)" />
      </xsl:when>
      <xsl:otherwise>
         <xsl:value-of select="$searchstring"/>
      </xsl:otherwise>
   </xsl:choose>
</xsl:template>

<!-- Changed because SmartDate is passing a null which blows up in some scenarios -->
<xsl:template name="GetMethod" >
	<!-- Allow invalid code if there is no Retrieve so programmer will fix -->
   <xsl:param name="type" select="@NETType"/>
   <xsl:param name="usetype" />
	      <xsl:value-of select="$usetype"/> obj;
	      obj = m<xsl:value-of select="@Name"/><xsl:call-template name="GetValue" />;<xsl:text/>
	<xsl:choose>
	   <xsl:when test="$usetype='System.String'">
	      if( obj == null )
	      {
	         return "";
	      }
	      else
	      {
	         return obj;
	      }
	   </xsl:when>
	   <xsl:otherwise>
	      return obj;
	   </xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="GetValue" >
   <xsl:param name="type" select="@NETType"/>
	<xsl:if test="//orm:SpecialType[@Name=$type]">.<xsl:text/>
		<xsl:value-of 
				select="//orm:SpecialType[@Name=$type]/orm:Property/@Retrieve"/>
	</xsl:if>
</xsl:template>

<xsl:template name="SetMethod" >
	<!-- Allow invalid code if there is no Retrieve so programmer will fix -->
   <xsl:param name="type" select="@NETType"/>
	<xsl:if test="//orm:SpecialType[@Name=$type]">.<xsl:text/>
		<xsl:value-of 
				select="//orm:SpecialType[@Name=$type]/orm:Property/@Set"/>
	</xsl:if>
</xsl:template>

<xsl:template name="DataPortalFetch">
	<xsl:param name="iscollection" select="'false'" />
	// called by DataPortal to load data from the database<xsl:text/>
	<xsl:if test="orm:RetrieveSP/@TransactionType='Enterprise'">	
	[Transactions()]<xsl:text/>
	</xsl:if>
	protected override void DataPortal_Fetch( object Criteria )
	{
		// retrieve data from db<xsl:text/>
		<xsl:apply-templates select="orm:RetrieveSP" mode="StartDPWithCrit"/>
		<xsl:value-of select="net:InsertNLIndent()"/>SafeDataReader dr = new SafeDataReader( cm.ExecuteReader() );
		<xsl:value-of select="net:InsertNLIndent()"/>try
		{
		<xsl:choose>
			<xsl:when test="$iscollection='true'">
				<xsl:value-of select="net:InsertNLIndent()"/>	while( dr.Read() )<xsl:text/>
				<xsl:value-of select="net:InsertNLIndent()"/>	{<xsl:text/>
				<xsl:value-of select="net:InsertNLIndent()"/>		List.Add( <xsl:value-of select="$Name"/>.Get<xsl:value-of select="$Name"/>Child( dr ) );<xsl:text/>
				<xsl:value-of select="net:InsertNLIndent()"/>	}<xsl:text/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="net:InsertNLIndent()"/>	this.AssignFromDataReader( dr );
				
				<xsl:value-of select="net:InsertNLIndent()"/>	// load child objects<xsl:text/>
				<xsl:value-of select="net:InsertNLIndent()"/>	dr.NextResult();<xsl:text/>
				<xsl:for-each select="orm:ChildCollection">
				<xsl:value-of select="net:InsertNLIndent()"/>	m<xsl:value-of select="@Name"/> = <xsl:text/>
																				<xsl:value-of select="@Namespace"/>
																				<xsl:text>.gen</xsl:text>
	                                              				<xsl:value-of select="@Name"/>.Get<xsl:value-of select="@Name"/>
	                                             				<xsl:text/>Child( dr );<xsl:text/>
				<xsl:value-of select="net:InsertNLIndent()"/>	<xsl:if test="position() != last()">dr.NextResult();</xsl:if>
				<xsl:value-of select="net:InsertNLIndent()"/></xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="net:InsertNLIndent()"/>}
		<xsl:value-of select="net:InsertNLIndent()"/>catch( System.Exception ex )<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>	throw ex;
		<xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>finally
		<xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
		<xsl:value-of select="net:InsertNLIndent()"/>  dr.Close();
		<xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
		<xsl:if test="$iscollection!='true'">
			<xsl:value-of select="net:InsertNLIndent()"/>MarkOld();
		</xsl:if>
		<xsl:apply-templates select="orm:RetrieveSP" mode="CloseDP"/>
	}
</xsl:template>

<xsl:template match="orm:RetrieveSP | orm:DeleteSP" mode="StartDPWithCrit">
    Criteria crit = (Criteria ) Criteria ;<xsl:text/>
    <xsl:call-template name="StartDP"/>
      <xsl:value-of select="net:InsertNLIndent()"/>cm.CommandText = "<xsl:value-of select="@Name"/>";<xsl:text/>
      <xsl:value-of select="net:InsertNLIndent()"/>crit.AddParameters( cm );<xsl:text/>
</xsl:template>

<xsl:template match="orm:Parameter | orm:Property" mode="ParameterAssignment">
	<xsl:param name="isobject" select="false"/>
	<xsl:variable name="type" select="@NETType" />
	<xsl:variable name="name" select="@Name" />
	<xsl:variable name="readonly"			
						select="ancestor::orm:Object/orm:Property[@Name=$name]/@ReadOnly" />
	<xsl:variable name="autoincrement"			
						select="ancestor::orm:Object/orm:Property[@Name=$name]/@IsAutoIncrement" />
	<xsl:variable name="iscreate">
	   <xsl:for-each select="..">
	      <xsl:value-of select="name()='orm:CreateSP'" />
	   </xsl:for-each>
	</xsl:variable>
	<xsl:variable name="store">
		<xsl:if test="//orm:SpecialType[@Name=$type]/orm:SafeDataStore">
			<xsl:text>.</xsl:text>
			<xsl:value-of select="//orm:SpecialType[@Name=$type]/orm:SafeDataStore/@Name"/>
 		</xsl:if>
 	</xsl:variable>
 	<xsl:variable name="value">
 		<xsl:choose>
  			<xsl:when test="$readonly='true' and $isobject='true'">this.Get</xsl:when>
  			<xsl:when test="$readonly='true'">m</xsl:when>
 		</xsl:choose>
 		<xsl:value-of select="$name" />
 	</xsl:variable>
 	<xsl:for-each select="..">
 	 	 <xsl:for-each select="..">
 	   </xsl:for-each>
 	</xsl:for-each>
	<xsl:value-of select="net:InsertNLIndent()"/>   
			<xsl:text/>   p = new System.Data.SqlClient.SqlParameter( "@<xsl:text/>
			<xsl:value-of select="@Name"/>", <xsl:value-of select="$value"/>
			<xsl:value-of select="$store"/> );<xsl:text/>
	<xsl:value-of select="net:InsertNLIndent()"/>   cmd.Parameters.Add( p );
	<xsl:if test="$autoincrement='true' and $iscreate='true'">
	   <xsl:value-of select="net:InsertNLIndent()"/>   p.Direction = ParameterDirection.InputOutput;
	</xsl:if>
</xsl:template>

 <xsl:template match="orm:Parameter" mode="ParameterAssignmentOld">
	<xsl:param name="prefix"/>
	<xsl:variable name="type" select="@NETType" />
	<xsl:variable name="name" select="@Name" />
	<xsl:variable name="store">
		<xsl:if test="//orm:SpecialType[@Name=$type]/orm:SafeDataStore">
			<xsl:text>.</xsl:text>
			<xsl:value-of select="//orm:SpecialType[@Name=$type]/orm:SafeDataStore/@Name"/>
 		</xsl:if>
 	</xsl:variable>
 	<xsl:variable name="dataname">
 		<xsl:choose>
 			<xsl:when test="ancestor::orm:Object/orm:Property[@Name=$name]/@ReadOnly='true'">
 				<xsl:value-of select="$prefix"/><xsl:value-of select="@Name"/>
 			</xsl:when>
 			<xsl:otherwise>
 				<xsl:value-of select="$prefix"/>m<xsl:value-of select="@Name"/>
 			</xsl:otherwise>
 		</xsl:choose>
 	</xsl:variable>
	<xsl:value-of select="net:InsertNLIndent()"/>   cmd.Parameters.Add( <xsl:text/>
			<xsl:text/>new System.Data.SqlClient.SqlParameter( "@<xsl:text/>
			<xsl:value-of select="@Name"/>", <xsl:value-of select="$dataname"/>
			<xsl:value-of select="$store"/> ) );<xsl:text/>
</xsl:template>

<xsl:template name="StartDP">
	<!-- Context should be correct SP node -->
	<xsl:variable name="db" select="../@MapDataStructure" />
	<xsl:variable name="isadotrans" select="@TransactionType='ADONET'"  />
    SqlConnection cn = new SqlConnection( DB( "<xsl:value-of select="$db" />" ) );
    SqlCommand cm = new SqlCommand();
    <xsl:choose>
	 <xsl:when test="$isadotrans='true'">
    SqlTransaction tr;
    cn.Open();
    try
    {
    <xsl:value-of select="net:SetIndent(3)"/>
	 </xsl:when>
	 <xsl:otherwise>
	 cn.Open();
    <xsl:value-of select="net:SetIndent(2)"/>
	 </xsl:otherwise>
    </xsl:choose>

  	 <xsl:if test="$isadotrans='true'">
		<xsl:value-of select="net:InsertNLIndent()"/>tr = cn.BeginTransaction( IsolationLevel.<xsl:text/><!--<xsl:text>&#x2e;</xsl:text>-->
		<xsl:choose>
			<xsl:when test="name() = 'orm:RetrieveSP'"><xsl:text>ReadCommitted</xsl:text></xsl:when>
			<xsl:when test="name() = 'orm:UpdateSP' or 'orm:DeleteSP'"><xsl:text>Serializable</xsl:text></xsl:when>
		</xsl:choose> );
	  </xsl:if>
     <xsl:value-of select="net:InsertNLIndent()"/>try<xsl:text/>
     <xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
     <xsl:value-of select="net:Indent()" />
     <xsl:value-of select="net:InsertNLIndent()"/>cm.Connection = cn;<xsl:text/>
	  <xsl:if test="$isadotrans='true'">
	      <xsl:value-of select="net:InsertNLIndent()"/>cm.Transaction = tr;<xsl:text/>
	  </xsl:if>
     <xsl:value-of select="net:InsertNLIndent()"/>cm.CommandType = CommandType.StoredProcedure;<xsl:text/>
</xsl:template>

<xsl:template match="orm:RetrieveSP | orm:DeleteSP | orm:UpdateSP" mode="CloseDP">

		<xsl:if test="@TransactionType='ADONET'">
         <xsl:value-of select="net:InsertNLIndent()"/>tr.Commit();<xsl:text/>
         <xsl:value-of select="net:Outdent()"/>
			<xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
			<xsl:value-of select="net:InsertNLIndent()"/>catch( Exception ex )<xsl:text/>
			<xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
			<xsl:value-of select="net:InsertNLIndent()"/>  tr.Rollback();<xsl:text/>
			<xsl:value-of select="net:InsertNLIndent()"/>  throw ex;<xsl:text/>
			<xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
		</xsl:if>
    }
    finally
    {
      cn.Close();
    }
  <xsl:value-of select="net:SetIndent(1)" />

</xsl:template>


<xsl:template name="PrimaryKeyList">
  <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		<xsl:value-of select="@Name"	 />
		<xsl:if test="position()!=last()">, </xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeyArguments">
  <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		<xsl:variable name="name" select="@Name"	 />
		<xsl:variable name="keytype" select="@NETType"	 />
		<xsl:text/><xsl:value-of select="$keytype"/><xsl:text> </xsl:text><xsl:value-of select="$name"/>
		<xsl:if test="position()!=last()">,
		</xsl:if>
  </xsl:for-each>
</xsl:template>

<xsl:template name="PrimaryKeyDeclarations">
	<xsl:param name="prefix" />
  <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		<xsl:variable name="name" select="@Name"	 />
		<xsl:variable name="keytype" select="@NETType"	 />
		public <xsl:value-of select="$keytype"/><xsl:text> </xsl:text><xsl:value-of select="concat($prefix, $name)"/>;
  </xsl:for-each>
</xsl:template>

<xsl:template name="Comparison">
	<xsl:param name="first"  />
	<xsl:param name="second"  />
	<xsl:param name="operand"  />
	<xsl:param name="type"  />
	<xsl:choose>
	   <xsl:when test="$type='System.String'">
	      <xsl:text/>( <xsl:value-of select="$first"/> ==  null || <xsl:text/>
	         <xsl:value-of select="$second"/> == null ) || <xsl:text/>
	         <xsl:choose>
	            <xsl:when test="$operand='='">
	               <xsl:value-of select="concat($first,' == ', $second)"/>
	            </xsl:when>
	            <xsl:otherwise>
	               <xsl:value-of select="concat($first,' ', $operand,' ', $second)"/>
	            </xsl:otherwise>
	         </xsl:choose>
	   </xsl:when>
	   <xsl:when test="$operand='='">
	      <xsl:value-of select="concat($first,' == ', $second)"/>
	   </xsl:when>
	   <xsl:otherwise>
	      <xsl:value-of select="concat($first,' ', $operand,' ', $second)"/>
	   </xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="Test">
	<xsl:param name="test1"/>
	<xsl:param name="test2" />
	<xsl:value-of select="$test1"/>
	<xsl:value-of select="$test2"/>
</xsl:template>

<xsl:template name="otherparentparams">
   <xsl:param name="otherparents" />
   <xsl:param name="object" />
	<xsl:for-each select="$otherparents">
	   <xsl:variable name="parentname" select="@SingularName" />
		<xsl:element name="ParentObject">
		   <xsl:variable name="islookup" 
		         select="//orm:Object[@Name=$parentname]/@IsLookup" />
		   <xsl:attribute name="Name"><xsl:value-of select="$parentname"/></xsl:attribute>
		   <xsl:attribute name="IsLookup"><xsl:value-of select="$islookup"/></xsl:attribute>
		   <xsl:attribute name="ObjectName"><xsl:value-of select="$object/@Name"/></xsl:attribute>
		   <xsl:for-each select="$object">
		      <xsl:attribute name="ObjectName2"><xsl:value-of select="name()"/></xsl:attribute>
		   </xsl:for-each>
		   <xsl:choose>
	         <xsl:when test="$islookup='true'">
	            <xsl:for-each select="orm:ChildKeyField">
	               <xsl:element name="Field">
   	               <xsl:variable name="name" select="@Name"/>
	                  <xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
	                  <xsl:variable name="ordinal" select="@Ordinal" />
	                  <xsl:variable name="keyname" select="../orm:ParentKeyField[@Ordinal=$ordinal]/@Name" />
	                  <xsl:attribute name="Type">
	                     <xsl:value-of 
	                        select="//orm:Object[@Name=$parentname]//orm:Property[@Name=$keyname]/@NETType"/>
	                  </xsl:attribute> 
                     <xsl:attribute name="ObjectName"><xsl:value-of select="$parentname"/></xsl:attribute> 
                     <xsl:attribute name="ObjectType"><xsl:value-of select="$parentname"/></xsl:attribute> 
	               </xsl:element>
	            </xsl:for-each>
	         </xsl:when>
	         <xsl:otherwise>
	            <xsl:element name="Field">
   	            <xsl:attribute name="Name"><xsl:value-of select="@SingularName"/></xsl:attribute> 
	               <xsl:attribute name="Type"><xsl:value-of select="@SingularName"/></xsl:attribute> 
                  <xsl:attribute name="ObjectName"><xsl:value-of select="@SingularName"/></xsl:attribute> 
                  <xsl:attribute name="ObjectType"><xsl:value-of select="@SingularName"/></xsl:attribute> 
	            </xsl:element>
	         </xsl:otherwise>
	      </xsl:choose>
		</xsl:element>
	</xsl:for-each>
</xsl:template>

<xsl:template name="parentnonlookupparams">
   <xsl:param name="parents" />
   <xsl:param name="object" />
   <xsl:element name="BaseObject">
      <xsl:attribute name="Name"><xsl:value-of select="$object/@Name"/></xsl:attribute>
	   <xsl:for-each select="$parents">
	      <xsl:variable name="parent" select="." />
	      <xsl:variable name="parentname" select="@SingularName" />
	      <xsl:for-each select="//orm:Object[@Name=$parentname]">
	         <xsl:if test="string-length(@IsLookup)=0 or @Lookup!='true'">
		         <xsl:element name="ParentObject">
		            <xsl:variable name="islookup" 
		                  select="@IsLookup" />
		            <xsl:attribute name="Name"><xsl:value-of select="$parentname"/></xsl:attribute>
		            <xsl:attribute name="IsLookup"><xsl:value-of select="$islookup"/></xsl:attribute>
	               <xsl:for-each select="$parent/orm:ChildKeyField">
	                  <xsl:element name="Field">
   	                  <xsl:variable name="name" select="@Name"/>
	                     <xsl:attribute name="Name"><xsl:value-of select="$name"/></xsl:attribute>
	                     <xsl:variable name="ordinal" select="@Ordinal" />
	                     <xsl:variable name="keyname" select="../orm:ParentKeyField[@Ordinal=$ordinal]/@Name" />
	                     <xsl:attribute name="ParentProperty"><xsl:value-of select="$keyname"/></xsl:attribute>
	                     <xsl:attribute name="Type">
	                        <xsl:value-of 
	                           select="//orm:Object[@Name=$parentname]//orm:Property[@Name=$keyname]/@NETType"/>
	                     </xsl:attribute> 
                        <xsl:attribute name="ObjectName"><xsl:value-of select="$parentname"/></xsl:attribute> 
                        <xsl:attribute name="ObjectType"><xsl:value-of select="$parentname"/></xsl:attribute> 
	                  </xsl:element>
	               </xsl:for-each>
		         </xsl:element>
	         </xsl:if>
	      </xsl:for-each>
   	</xsl:for-each>
   </xsl:element>
</xsl:template>

<xsl:template match="*" mode="OutputFragment">
   <!-- This is for temporary use during debugging, but is valuable so don't delete during cleanup -->
   '<xsl:value-of select="name()"/>
   <xsl:for-each select="@*">
      '<xsl:value-of select="name()"/> = <xsl:value-of select="."/>
   </xsl:for-each>
   <xsl:apply-templates select="*" mode="OutputFragment"/>
</xsl:template>

</xsl:stylesheet> 
  
