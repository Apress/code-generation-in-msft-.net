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
  Summary: Generates the plumbing class for business objects !-->

<xsl:stylesheet version="1.0" 
			xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
			xmlns:xs="http://www.w3.org/2001/XMLSchema"
			xmlns:dbs="http://kadgen/DatabaseStructure" 
			xmlns:orm="http://kadgen.com/KADORM.xsd" 
			xmlns:msxsl="urn:schemas-microsoft-com:xslt"
			xmlns:net="http://kadgen.com/StandardNETSupport.xsd" >
<xsl:import href="CSLASupport.xslt"/>
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:param name="Name"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>
<xsl:param name="Child"/>

<xsl:variable name="properties" 
             select="//orm:Object[@Name=$Name]//orm:Property"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@Name=$Name]" 
								mode="Object"/>
</xsl:template>

<xsl:template match="orm:Object" mode="Object">
using System;
using System.Data;
using System.Data.SqlClient;
using CSLA.Data;
using CSLA;
using KADGen.BusinessObjectSupport;

namespace KADGen.BusinessObjects
{
   [Serializable()]
   public abstract class gen<xsl:value-of select="$Name"/> : <xsl:text/>
   <xsl:choose>
	   <xsl:when test="@Inherits">
		   <xsl:value-of select="@Inherits"/>
	   </xsl:when>
	   <xsl:otherwise>
		   <xsl:text/>BusinessBase<xsl:text/>
	   </xsl:otherwise>
   </xsl:choose>
   <xsl:if test="string-length(@Inherits)=0">
	   <xsl:text/>, IBusinessObject<xsl:text/>
   </xsl:if>
   {
	   <xsl:call-template name="ClassDeclarations"/>
	   <xsl:call-template name="BusinessPropertiesAndMethods"/>
	   <xsl:call-template name="SystemObjectOverrides"/>
	   <xsl:call-template name="SharedMethods"/>
	   <xsl:call-template name="Constructors"/>
	   <xsl:call-template name="Criteria"/>
	   <xsl:call-template name="DataAccess"/>
	   <xsl:call-template name="InternalMethodsAndProperties"/>
   }
}
   </xsl:template>


   <xsl:template name="ClassDeclarations">
	   #region Class Declarations
   <xsl:if test="string-length(@Inherits)=0">
	   protected bool mIsLoaded;
   </xsl:if> 
   <xsl:variable name="inherits" select="@Inherits" />
   <xsl:for-each select="$properties">
      <xsl:variable name="propname" select="@Name"/>
      <xsl:if test="count(//orm:Object[@Name=$inherits]//orm:Property[@Name=$propname])=0">
	   private <xsl:value-of select="@NETType"/> m<xsl:value-of select="@Name"/>;
      </xsl:if>
   </xsl:for-each>
   <xsl:text>&#x0d;&#x0a;</xsl:text>
   <xsl:if test="string-length($inherits)=0">
      <xsl:for-each select="orm:ChildCollection ">
	   protected <xsl:value-of select="@Name"/> m<xsl:value-of select="@Name"/> =
            <xsl:value-of select="@Name"/>.New<xsl:value-of select="@Name"/>Child();<xsl:text/>
      </xsl:for-each>
   </xsl:if>
	   #endregion
   </xsl:template>

   <xsl:template name="BusinessPropertiesAndMethods">
	   #region Business Properties and Methods
   <xsl:if test="string-length(@Inherits)=0">
	   public bool IsLoaded
	   {
		   get
		   {
			   return mIsLoaded;
		   }
	   }
   </xsl:if>

   <xsl:variable name="inherits" select="@Inherits" />
   <xsl:for-each select="$properties">
      <xsl:variable name="propname" select="@Name"/>
      <xsl:if test="count(//orm:Object[@Name=$inherits]//orm:Property[@Name=$propname])=0">
		   <xsl:call-template name="Property"/>
      </xsl:if>
   </xsl:for-each>

   <xsl:if test="string-length(@Inherits)=0">
	   <xsl:for-each select="orm:ChildCollection">
	   public <xsl:value-of select="@Name"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
	   {
		   get
		   {
			   return m<xsl:value-of select="@Name"/>;
		   }
	   }
	   </xsl:for-each>

	   public static bool CanRetrieve()
	   {
	   <xsl:for-each select="orm:RetrieveSP">
		   <xsl:call-template name="CanPrivileges">
			   <xsl:with-param name="privilegetype" select="'R'"/>
		   </xsl:call-template>
	   </xsl:for-each>
	   }

	   public static bool CanCreate()
	   {
	   <xsl:for-each select="orm:CreateSP">
		   <xsl:call-template name="CanPrivileges">
			   <xsl:with-param name="privilegetype" select="'C'"/>
		   </xsl:call-template>
	   </xsl:for-each>
	   }

	   public static bool CanDelete()
	   {
	   <xsl:for-each select="orm:DeleteSP">
		   <xsl:call-template name="CanPrivileges">
			   <xsl:with-param name="privilegetype" select="'D'"/>
		   </xsl:call-template>
	   </xsl:for-each>
	   }

	   public static bool CanUpdate()
	   {
	   <xsl:for-each select="orm:UpdateSP">
		   <xsl:call-template name="CanPrivileges">
			   <xsl:with-param name="privilegetype" select="'U'"/>
		   </xsl:call-template>
	   </xsl:for-each>
	   }

	   bool IBusinessObject.CanCreate()
	   {
		   return gen<xsl:value-of select="$Name"/>.CanRetrieve();
	   }

	   bool IBusinessObject.CanRetrieve()
	   {
		   return gen<xsl:value-of select="$Name"/>.CanRetrieve();
	   }

	   bool IBusinessObject.CanUpdate()
	   {
		   return gen<xsl:value-of select="$Name"/>.CanRetrieve();
	   }

	   bool IBusinessObject.CanDelete()
	   {
		   return gen<xsl:value-of select="$Name"/>.CanRetrieve();
	   }

	   public static string Caption
	   {
		   get
		   {
			   return "<xsl:value-of select="$Name" />";
		   }
	   }

	   string IBusinessObject.Caption
	   {
		   get
		   {
			   return gen<xsl:value-of select="$Name"/>.Caption;
		   }
	   }

	   public static string ObjectName
	   {
		   get
		   {
			   return "<xsl:value-of select="$Name" />";
		   }
	   }

	   string IBusinessObject.ObjectName
	   {
		   get
		   {
			   return gen<xsl:value-of select="$Name"/>.ObjectName;
		   }
	   }

   </xsl:if>

   <!-- Added 11/29/03 becuase the required class level variables don't exist -->
   <xsl:if test="string-length($inherits)=0">

	   public override bool IsValid
	   {
		   get
		   {
			   return base.IsValid<xsl:text/>
			   <xsl:for-each select="orm:ChildCollection">
				   <xsl:text/> &amp;&amp; m<xsl:value-of select="@Name"/>.IsValid<xsl:text/>
			   </xsl:for-each>;
		   }
	   }

	   public override bool IsDirty
	   {
		   get
		   {
			   return base.IsDirty<xsl:text/>
				   <xsl:for-each select="orm:ChildCollection">
				   <xsl:text/> || m<xsl:value-of select="@Name"/>.IsDirty<xsl:text/>
				   </xsl:for-each>;
		   }
	   }
   </xsl:if>
   	
	   #endregion
   </xsl:template>

   <xsl:template name="SystemObjectOverrides">
	   #region System.Object Overrides

   <xsl:if test="string-length(@Inherits)=0">

	   public override string ToString()
	   {
		   return <xsl:text />
			   <xsl:choose>
			   <xsl:when test="count(orm:Property[@UseForDesc='true']) > 0">
				   <xsl:for-each select="orm:Property[@UseForDesc='true']">
					   <xsl:sort select="@UseForDescOrdinal" data-type="number" />
					   <xsl:text/>m<xsl:value-of select="@Name"/>.ToString()<xsl:text/>
					   <xsl:if test="position()!=last()"> 
						   <xsl:text/> + "<xsl:text/>
						   <xsl:choose>
							   <xsl:when test="@UseForDescDelimiter">
								   <xsl:value-of select="@UseForDescDelimiter"  />
							   </xsl:when>
							   <xsl:otherwise>, </xsl:otherwise>
						   </xsl:choose>" +
					   </xsl:if>
				   </xsl:for-each>;<xsl:text/>
			   </xsl:when>
				   <xsl:otherwise >UniqueKey;</xsl:otherwise>
			   </xsl:choose>
	   }

	   public bool Equals( <xsl:value-of select="$Name"/><xsl:text> </xsl:text><xsl:value-of select="$Name"/> )
	   {
		   return <xsl:text />
		   <xsl:for-each select="$properties[@IsPrimaryKey='true']">
			   <xsl:text/>m<xsl:value-of select="@Name"/>.Equals(<xsl:value-of select="$Name"/>.<xsl:text/>
			   <xsl:value-of select="@Name"/>)<xsl:if test="position()!=last()"> &amp;&amp; 
			   </xsl:if>
		   </xsl:for-each>;
	   }
   	
	   public string UniqueKey
	   {
		   get
		   {
			   return <xsl:text/>
			   <!-- THe followign assumes all primary keys are in set select -->
			   <xsl:for-each select="$properties[@IsPrimaryKey='true']">
				   <xsl:text/>m<xsl:value-of select="@Name"/>.ToString()<xsl:if test="position()!=last()"> + </xsl:if>
			   </xsl:for-each>;
		   }
		   set
		   {
			   throw new System.Exception( "Unexpected call to setUniqueKey" );
		   }
	   }
      
	   public virtual string DisplayText
	   {
		   get
		   {
			   return ToString();
		   }
		   set
		   {
			   throw new System.Exception( "Unexpected call to DisplayText" );
		   }
	   }

	   public override int GetHashCode()
	   {
		   return UniqueKey.GetHashCode();
	   }
   </xsl:if>
	   #endregion
   </xsl:template>

   <xsl:template name="SharedMethods">
	   #region Static Methods

	   // create new object
   <xsl:if test="$Root='true'">
	   public static <xsl:value-of select="$Name"/> New<xsl:value-of select="$Name"/>()
	   {
		   if( !CanCreate() )
		   {
			   throw new System.Security.SecurityException(
				   "<xsl:value-of select="concat('User not authorized to add a ', $Name)"/>" );
		   }
		   else
		   {
			   <xsl:value-of select="$Name"/> obj = new <xsl:value-of select="$Name"/>();
			   obj.MarkClean();
			   return obj;
		   }
	   }
   	
	   // load existing object by id
	   public static <xsl:value-of select="$Name"/> Get<xsl:value-of select="$Name"/>( <xsl:text/>
	   <xsl:call-template name="PrimaryKeyArguments"/> )
	   {
		   if( !CanRetrieve() )
		   {
			   throw new System.Security.SecurityException(
				   "<xsl:value-of select="concat('User not authorized to retrieve a ', $Name)"/>" );
		   }
		   else
		   {
			   return (<xsl:value-of select="@Name"/>)DataPortal.Fetch( new Criteria( <xsl:text/>
				   <xsl:call-template name="PrimaryKeyList"/> ) );
		   }
	   }
   	
	   <xsl:variable name="deletefailuremsg" select="concat('User not authorized to delete a ', $Name)" />
	   // delete object
	   <!-- Only available for root -->
	   public static void Delete<xsl:value-of select="$Name"/>( <xsl:text/>
	   <xsl:call-template name="PrimaryKeyArguments"/> )<xsl:text/>
	   {
		   if( !CanDelete() )
		   {
			   throw new System.Security.SecurityException(
				   "<xsl:value-of select="$deletefailuremsg"/>" );
		   }
		   else
		   {
			   DataPortal.Delete( new Criteria( <xsl:call-template name="PrimaryKeyList"/> ) );
		   }
	   }

	   <!-- Only available for root -->
	   public override BusinessBase Save()
	   {
		   if( IsDeleted )
		   {
			   if( !CanDelete() )
			   {
				   throw new System.Security.SecurityException(
					   "<xsl:value-of select="$deletefailuremsg"/>" );
			   }
			   else if( !CanUpdate() )
			   {
				   throw new System.Security.SecurityException(
					   "<xsl:value-of select="concat('User not authorized to update a ', $Name)"/>" );
			   }
		   }
		   return base.Save();
	   }
	   <xsl:if test="string-length(@Inherits)=0">
	   IBusinessObject IBusinessObject.Save()
	   {
		   return (IBusinessObject)this.Save();
	   }
   	
	   IBusinessObject IBusinessObject.GetNew()
	   {
		   return (IBusinessObject)gen<xsl:value-of select="$Name"/>.New<xsl:value-of select="$Name"/>();
	   }
	   </xsl:if>
   </xsl:if>
     
   <xsl:if test="$Child='true'">
	   <xsl:variable name="childof" select="@ChildOf"/>
	   <xsl:variable name="otherparents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
	   <xsl:variable name="parents" select="orm:ParentObject"/>
	   internal static <xsl:value-of select="$Name"/> New<xsl:value-of select="$Name"/>Child( <xsl:text/>
		   <xsl:for-each select="$otherparents">
			   <xsl:value-of select="@SingularName"/><xsl:text> </xsl:text><xsl:value-of select="@SingularName"/>
			   <xsl:if test="position()!=last()">, </xsl:if>
		   </xsl:for-each> )
	   {
		   <xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		   <xsl:value-of select="$Name"/> obj;
		   obj = new <xsl:value-of select="$Name"/>();
		   obj.Init( <xsl:text/>
			   <xsl:for-each select="$otherparents">
				   <xsl:value-of select="@SingularName"/>
				   <xsl:if test="position()!=last()">, </xsl:if>
			   </xsl:for-each> );
		   obj.MarkAsChild();
		   return obj;
	   }
   	
	   //internal static Shadows <xsl:value-of select="$Name"/> NewItemChild()
	   internal static new <xsl:value-of select="$Name"/> NewItemChild()
	   {
		   <xsl:value-of select="$Name" /> obj = new <xsl:value-of select="$Name" />();
		   obj.MarkAsChild();
		   return obj;
	   }
   	
	   <xsl:variable name="temp">
	      <xsl:call-template name="parentnonlookupparams">
	         <xsl:with-param name="object" select="."/>
	         <xsl:with-param name="parents" select="$otherparents" />
	      </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="nolookups" select="msxsl:node-set($temp)"/>
	   <!--<xsl:if test="count($nolookups/*)!=count($otherparents)">-->
	   <xsl:if test="count($nolookups/BaseObject/*)>0">
	   internal static <xsl:value-of select="$Name"/> New<xsl:value-of select="$Name"/>Child( <xsl:text/>
	   <xsl:for-each select="$nolookups//Field">
		   <xsl:value-of select="@Type"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
		   <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each> )
	   {
		   <xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		   <xsl:value-of select="$Name"/> obj;
		   obj = new <xsl:value-of select="$Name"/>();
		   obj.Init( <xsl:text/>
			   <xsl:for-each select="$nolookups//Field">
				   <xsl:value-of select="@Name"/>
				   <xsl:if test="position()!=last()">, </xsl:if>
			   </xsl:for-each> );
		   obj.MarkAsChild();
		   return obj;
	   }
	   </xsl:if>

	   <xsl:variable name="temp2">
	      <xsl:call-template name="parentnonlookupparams">
	         <xsl:with-param name="object" select="."/>
	         <xsl:with-param name="parents" select="$parents" />
	      </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="allnolookups" select="msxsl:node-set($temp2)"/>
	   <!--<xsl:if test="count($allnolookups/*)!=count($parents)">-->
	   <xsl:if test="count($allnolookups//Field)>0 and (count($allnolookups//Field) != count($nolookups//Field))">
	   internal static <xsl:value-of select="$Name"/> New<xsl:value-of select="$Name"/>Child(<xsl:text/>
		   <xsl:for-each select="$allnolookups//Field">
			   <xsl:value-of select="@Type"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
			   <xsl:if test="position()!=last()">, </xsl:if>
		   </xsl:for-each> )
	   {
		   <xsl:apply-templates select="orm:CreateSP" mode="CreatePrivileges"/>
		   <xsl:value-of select="$Name"/> obj;
		   obj = new <xsl:value-of select="$Name"/>();
		   obj.Init( <xsl:text/>
			   <xsl:for-each select="$allnolookups//Field">
				   <xsl:value-of select="@Name"/>
				   <xsl:if test="position()!=last()">, </xsl:if>
			   </xsl:for-each> );
		   obj.MarkAsChild();
		   return obj;
	   }
	   </xsl:if>
      
	   internal static <xsl:value-of select="$Name"/> Get<xsl:value-of select="$Name"/>Child( SafeDataReader dr )
	   {
		   <xsl:apply-templates select="orm:RetrieveSP" mode="RetrievePrivileges"/>
		   <xsl:value-of select="$Name"/> obj = new <xsl:value-of select="$Name"/>();
		   if( obj.AssignFromDataReader( dr ) )
		   {
			   obj.MarkAsChild();
			   return obj;
		   }
		   else
		   {
			   return null;
		   }
	   }
   </xsl:if>

	   #endregion
   </xsl:template>

   <xsl:template name="Constructors">
	   #region Constructors

   <xsl:if test="@ChildOf">
	   <xsl:variable name="object" select="."/>
	   <xsl:variable name="childof" select="@ChildOf"/>
	   <xsl:variable name="parents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
	   <xsl:if test="count($parents)!=0">
	   protected gen<xsl:value-of select="$Name"/>( <xsl:text/>
		   <xsl:for-each select="$parents">
			   <xsl:value-of select="@SingularName"/><xsl:text> </xsl:text><xsl:value-of select="@SingularName"/>
			   <xsl:if test="position()!=last()">, </xsl:if>
		   </xsl:for-each> ) : base()
	   {
		   <xsl:for-each select="$parents/orm:ParentKeyField">
			   <xsl:variable name="ordinal" select="@Ordinal"/>
			   <xsl:variable name="name" select="@Name"/>
			   <xsl:for-each select="../orm:ChildKeyField[@Ordinal=$ordinal]"> 
				   <xsl:call-template name="PropertyOrSet">
					   <xsl:with-param name="object" select="$object"/>
					   <xsl:with-param name="val" select="concat(../@SingularName,'.',$name)"/>
				   </xsl:call-template> 
			   </xsl:for-each>
		   </xsl:for-each>
	   }
	   </xsl:if>
   </xsl:if> 

	   protected gen<xsl:value-of select="$Name"/>()
	   {
		   // Scope Prevents Direct Instantiation
		   // Must be protected, not private, to avoid children needing constructors
		   Init(); // Sets Defaults
	   }

	   #endregion
   </xsl:template>

   <xsl:template name="Criteria">
	   #region Criteria
   <xsl:if test="string-length(@Inherits)=0">
	   <!-- You have to escape any less than signs that you want in the output -->
	   [Serializable()]
	   protected class Criteria : CSLA.Server.CriteriaBase
	   {
		   <xsl:call-template name="PrimaryKeyDeclarations">
			   <xsl:with-param name="prefix" select="'m'" />
		   </xsl:call-template>
   		 
		   public Criteria( <xsl:call-template name="PrimaryKeyArguments"/> ) : base( typeof( <xsl:value-of select="$Name"/> ) )
		   {
		   <xsl:for-each select="$properties[@IsPrimaryKey='true']">
			   m<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>;<xsl:text/>
		   </xsl:for-each>
		   }
   			
		   <xsl:for-each select="$properties[@IsPrimaryKey='true']">
			   <xsl:call-template name="Property">
				   <xsl:with-param name="incriteria" select="'true'"/>
			   </xsl:call-template>
		   </xsl:for-each>
   			
		   public void AddParameters( IDbCommand cmd )
		   {
			   System.Data.SqlClient.SqlParameter p;
			   <xsl:apply-templates select="$properties[@IsPrimaryKey='true']" mode="ParameterAssignment"/>
		   }
   		
	   }
   </xsl:if>
	   #endregion

   </xsl:template>

   <xsl:template name="DataAccess">
	   #region Data Access

	   private void IApplyEdit()
	   {
		   base.ApplyEdit();
	   }

	   private void ICancelEdit()
	   {
		   base.CancelEdit();
	   }

   <xsl:if test="$Root='true' and string-length(@Inherits)=0">
	   <xsl:if test="orm:RetrieveSP/orm:Privilege">
		   <xsl:call-template name="DataPortalFetch"/>
	   </xsl:if>
   	
	   <xsl:if test="orm:UpdateSP/orm:Privilege">
	   // called by DataPortal to delete/add/update data into the database<xsl:text/>
	   <xsl:if test="orm:UpdateSP/@TransactionType='Enterprise'">
	   [Transactions()]<xsl:text/>
	   </xsl:if>
	   protected override void DataPortal_Update()
	   {
		   // save data into db
	   <xsl:for-each select="orm:UpdateSP">
		   <xsl:call-template name="StartDP"/>
	   </xsl:for-each>
		   <xsl:value-of select="net:InsertNLIndent()"/>if( this.IsDeleted )<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	// we're being deleted<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	if( !this.IsNew )<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	{<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		// we're not new, so get rid of our data<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		cm.CommandText = "<xsl:value-of select="orm:DeleteSP/@Name" />";<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		AddDeleteParameters(cm);<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		cm.ExecuteNonQuery();<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	}<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	// reset our status to be a new object<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	MarkNew();<xsl:text/>

		   <xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>else<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>{<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	// we're not being deleted, so insert or update<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	if( this.IsNew )<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	{<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		// we're new, so insert<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		cm.CommandText = "<xsl:value-of select="orm:CreateSP/@Name" />";<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		AddCreateParameters(cm);<xsl:text/>

		   <xsl:value-of select="net:InsertNLIndent()"/>	}<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	else<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	{<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		// we're not new, so update<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		cm.CommandText = "<xsl:value-of select="orm:UpdateSP/@Name" />";<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>		AddUpdateParameters( cm );<xsl:text/>

		   <xsl:value-of select="net:InsertNLIndent()"/>	}<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	cm.ExecuteNonQuery();<xsl:text/>

		   <xsl:value-of select="net:InsertNLIndent()"/>	// make sure we're marked as an old object<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	GetOutputParameters( cm );<xsl:text/>
		   <xsl:value-of select="net:InsertNLIndent()"/>	MarkOld();<xsl:text/>

		   <xsl:value-of select="net:InsertNLIndent()"/>}<xsl:text/>
		   <xsl:apply-templates select="orm:UpdateSP" mode="CloseDP"/>
		   <!-- Moved the child update into a sub 11/29/03. Inherited objects don't know what children 
		      are supported, and if some were excluded from the parent this creates a bug. So, I'm going 
		      to have the derived classes call the base class to save children -->
		   UpdateChildren();
	   }						
	   </xsl:if>
   	
	   <xsl:if test="string-length(@Inherits)=0">
	   protected void UpdateChildren()
	   {
		   <xsl:for-each select="orm:ChildCollection">
			   m<xsl:value-of select="@Name"/>.Update( <xsl:text/>
				   <xsl:if test="orm:UpdateSP/@TransactionType='ADONET'">tr, </xsl:if>
				   <xsl:text/>(<xsl:value-of select="$Name"/>)this);
		   </xsl:for-each>
	   }
      </xsl:if>

	   <xsl:if test="orm:DeleteSP/orm:Privilege">
	   <xsl:if test="@orm:DeleteSP/TransactionType='Enterprise'">
	   [Transactions()]<xsl:text/>
	   </xsl:if>
	   protected override void DataPortal_Delete( object Criteria )
	   {
		   <xsl:apply-templates select="orm:DeleteSP" mode="StartDPWithCrit" />
		   <xsl:value-of select="net:InsertNLIndent()"/>cm.CommandText = "<xsl:value-of select="orm:DeleteSP/@Name"/> ";
		   <xsl:value-of select="net:InsertNLIndent()"/>cm.ExecuteNonQuery();
		   <xsl:apply-templates select="orm:DeleteSP" mode="CloseDP"/>
	   }
	   </xsl:if>

   </xsl:if>
   	  
   <xsl:if test="@Child='true'">
      <xsl:if test="string-length(@Inherits)=0">
	   protected void Fetch( SafeDataReader dr )
	   {
		   this.AssignFromDataReader( dr );
		   MarkOld();
	   }
      </xsl:if>

	   internal virtual void Update( <xsl:text/>
		   <xsl:if test="@ChildOf">
			   <xsl:value-of select="@ChildOf"/><xsl:text> </xsl:text><xsl:value-of select="@ChildOf"/>
		   </xsl:if> )
	   {
		   if( !this.IsDirty )
			   return;

		   // do the update
		   // Assume the transaction is managed by the parent - this method is only used in the child
		   SqlConnection cn = new SqlConnection( DB( "<xsl:value-of select="@MapDataStructure"/>" ) );
		   cn.Open();
   	   
		   try 
		   {
			   SqlCommand cm = new SqlCommand();
			   cm.Connection = cn;
			   cm.CommandType = CommandType.StoredProcedure;
			   if( this.IsDeleted )
			   {
				   // we're being deleted<xsl:text/>
				   if( !this.IsNew )
				   {
					   // we're not new, so get rid of our data<xsl:text/>
					   cm.CommandText = "<xsl:value-of select="orm:DeleteSP/@Name" />";
					   AddDeleteParameters( cm );
					   cm.ExecuteNonQuery();
				   }
				   // reset our status to be a new object
				   MarkNew();
			   }
			   else
			   {
				   // we're not being deleted, so insert or update<xsl:text/>
				   if( this.IsNew )
				   {
					   // we're new, so insert
					   cm.CommandText = "<xsl:value-of select="orm:CreateSP/@Name" />";
					   AddCreateParameters( cm );
				   }
				   else
				   {
					   // we're not new, so update<xsl:text/>
					   cm.CommandText = "<xsl:value-of select="orm:UpdateSP/@Name" />";
					   AddUpdateParameters( cm );
				   }
				   cm.ExecuteNonQuery();

				   // make sure we're marked as an old object
				   MarkOld();

			   }
		   }
		   finally
		   {
			   cn.Close();
		   }

		   UpdateChildren();
		   <!--' update child objects	<xsl:text/>
		   <xsl:for-each select="orm:ChildCollection">
		   m<xsl:value-of select="@Name"/>.Update(<xsl:text/>
			   <xsl:if test="orm:UpdateSP/@TransactionType='ADONET'">tr, </xsl:if>
 			   <xsl:text/>CType(this, <xsl:value-of select="$Name"/>))
		   </xsl:for-each> -->
	   }
   </xsl:if>

	   #endregion

   </xsl:template>

   <xsl:template name="InternalMethodsAndProperties">
	   <xsl:variable name="object" select="."/>
	   <xsl:variable name="childof" select="@ChildOf"/>
	   <xsl:variable name="parents" select="orm:ParentObject[string-length($childof)=0 or @SingularName!=$childof]"/>
	   #region Internal Methods and Properties

      <xsl:if test="string-length($childof)=0">
	   protected <xsl:text/>
	   <xsl:choose>
		   <xsl:when test="string-length($childof)=0">virtual </xsl:when>
		   <xsl:otherwise>override </xsl:otherwise>
	   </xsl:choose>
	   <xsl:text/>void Init()
	   {
	   <xsl:for-each select="$properties">
	      <xsl:choose>
	         <xsl:when test="@Default">
		         <xsl:choose>
		         <xsl:when test="@IsPrimaryKey='true'">
		   Set<xsl:value-of select="@Name"/>(<xsl:text/>
		         <xsl:value-of select="@Default"/>);</xsl:when>
		         <xsl:otherwise>
		   <!--m<xsl:value-of select="@Name"/><xsl:call-template name="SetMethod"/>
		         <xsl:text/> = <xsl:value-of select="@Default"/>;<xsl:text/>-->
         m<xsl:value-of select="@Name"/>
		         <xsl:text/> = <xsl:value-of select="@Default"/>;<xsl:text/>
		         </xsl:otherwise>
		         </xsl:choose>
	         </xsl:when>
 		      <xsl:when test="not(starts-with(@NETType,'System.'))">
  		   m<xsl:value-of select="@Name"/> = new <xsl:value-of select="@NETType" />();<xsl:text/>
	   	   </xsl:when>
	      </xsl:choose>
	   </xsl:for-each>
	   <xsl:for-each select="$properties[string-length(@Default) > 0]">
	   </xsl:for-each>
	   }
      </xsl:if>

      <xsl:if test="count($parents)!=0">
      protected virtual void Init( 
	   <xsl:for-each select="$parents">
		   <xsl:value-of select="@SingularName"/><xsl:text> </xsl:text><xsl:value-of select="@SingularName"/>
		   <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each> )
	   {
		   <xsl:for-each select="$parents/orm:ChildKeyField">
			   <xsl:variable name="ordinal" select="@Ordinal"/>
			   <xsl:call-template name="PropertyOrSet">
				   <xsl:with-param name="object" select="$object"/>
				   <xsl:with-param name="val" select="concat(../@SingularName,'.',../orm:ParentKeyField[@Ordinal=$ordinal]/@Name)"/>
			   </xsl:call-template> 
		   </xsl:for-each>
	   }
      </xsl:if>
    
	   <xsl:variable name="temp">
	      <xsl:call-template name="parentnonlookupparams">
	         <xsl:with-param name="object" select="."/>
	         <xsl:with-param name="parents" select="$parents" />
	      </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="nolookups" select="msxsl:node-set($temp)"/>
	   <xsl:if test="count($nolookups/BaseObject/*)>0">
      protected virtual void Init(<xsl:text/>
	   <xsl:for-each select="$nolookups//Field">
		   <xsl:value-of select="@Type"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
		   <xsl:if test="position()!=last()">, </xsl:if>
	   </xsl:for-each> )
	   {
		   <xsl:for-each select="$nolookups//Field">
			   <xsl:call-template name="PropertyOrSet">
				   <xsl:with-param name="object" select="$object"/>
				   <xsl:with-param name="val" select="@Name"/>
			   </xsl:call-template> 
		   </xsl:for-each>
	   }
      </xsl:if>

   	
	   private bool AssignFromDataReader( SafeDataReader dr )
	   {
		   if( dr.Read() )
		   {
			   <!--<xsl:apply-templates select="orm:RetrieveSP/orm:SPRecordSet[1]/orm:Column" mode="RetrieveFromReader"/>-->
			   <xsl:apply-templates select="$properties" mode="RetrieveFromReader"/>
			   mIsLoaded = true;
			   MarkOld();
			   return true;
		   }
		   return false;
	   }
   	
	   private void AddCreateParameters( IDbCommand cmd )
	   {
		   System.Data.SqlClient.SqlParameter p;
		   <xsl:apply-templates select="orm:CreateSP/orm:Parameter" mode="ParameterAssignment">
			   <xsl:with-param name="isobject" select="'true'"/>
		   </xsl:apply-templates>
	   }
   	
	   private void GetOutputParameters( IDbCommand cmd )
	   {
		   System.Data.SqlClient.SqlParameter p;
		   <xsl:for-each select="orm:CreateSP/orm:Parameter">
			   <xsl:variable name="paramname" select="@Name"/>
			   <xsl:for-each select="../../orm:Property[@Name=$paramname]" >
				   <xsl:if test="@IsAutoIncrement='true'">
					   p = (System.Data.SqlClient.SqlParameter)cmd.Parameters[ "@<xsl:value-of select="@Name"/>" ];

					   this.Set<xsl:value-of select="@Name"/>((<xsl:value-of select="@NETType"/>)p.Value);
				   </xsl:if>
			   </xsl:for-each>
		   </xsl:for-each>
	   }

	   <xsl:if test="orm:RetrieveSP">
	   private void AddRetrieveParameters( IDbCommand cmd )
	   {
		   System.Data.SqlClient.SqlParameter p;
		   <xsl:apply-templates select="orm:RetrieveSP/orm:Parameter" mode="ParameterAssignment">
			   <xsl:with-param name="isobject" select="'true'"/>
		   </xsl:apply-templates>
	   }
	   </xsl:if>

	   private void AddUpdateParameters( IDbCommand cmd )
	   {
		   System.Data.SqlClient.SqlParameter p;
		   <xsl:apply-templates select="orm:UpdateSP/orm:Parameter" mode="ParameterAssignment">
			   <xsl:with-param name="isobject" select="'true'"/>
		   </xsl:apply-templates>
	   }

	   private void AddDeleteParameters( IDbCommand cmd )
	   {
		   System.Data.SqlClient.SqlParameter p;
		   <xsl:apply-templates select="orm:DeleteSP/orm:Parameter" mode="ParameterAssignment">
			   <xsl:with-param name="isobject" select="'true'"/>
		   </xsl:apply-templates>
	   }

   #endregion 
   </xsl:template>

 <!-- End of main template list -->

</xsl:stylesheet> 
  
