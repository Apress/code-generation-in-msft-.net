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
  Summary: Generates the plumbing class for business object collections !-->

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
<xsl:param name="CollectionName"/>
<xsl:param name="filename"/>
<xsl:param name="database"/>
<xsl:param name="gendatetime"/>
<xsl:param name="Root"/>

<xsl:template match="/">
	<xsl:apply-templates select="//orm:Object[@CollectionName=$CollectionName]" 
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
   public class gen<xsl:value-of select="$CollectionName"/> : BusinessCollectionBase
   {
	   protected bool mIsLoaded;
	   <xsl:call-template name="BusinessPropertiesAndMethods" />
	   <xsl:call-template name="Contains" />
	   <xsl:call-template name="SharedMethods" />
	   <xsl:call-template name="Constructors" />
	   <xsl:call-template name="Criteria" />
	   <xsl:call-template name="DataAccess" />
   }
}
   </xsl:template>


   <xsl:template name="BusinessPropertiesAndMethods">
   #region Business Properties and Methods 
      <xsl:variable name="object" select="." />
      <xsl:variable name="childof" select="@ChildOf" />
	   <xsl:variable name="otherparents" select="orm:ParentObject[@SingularName!=$childof]"/>
	   <xsl:variable name="parents" select="orm:ParentObject"/>
	   public <xsl:value-of select="$Name"/> this[ int Index ]
	   {
		   get
		   {
			   return (<xsl:value-of select="$Name"/> ) List[ Index ];
		   }
	   }
   	
	   public <xsl:value-of select="$Name"/> GetItem( <xsl:text/>
	         <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
	               <xsl:value-of select="@NETType"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
	               <xsl:if test="position()!=last()">, </xsl:if>
	         </xsl:for-each> )
	   {
	      <xsl:call-template name="FindInList"/>
	      return null;
	   }

	   public bool IsLoaded
	   {
		   get
	   	   {
	      		   return mIsLoaded;
		   }
	   }   
     
   <xsl:if test="$Root='true'">
	   public void Add()
	   {
		   List.Add( <xsl:value-of select="$Name"/>.New<xsl:text/>
				   <xsl:value-of select="$Name"/>() );
	   }	
   </xsl:if>

	   public virtual void Remove( <xsl:value-of select="@Name" /><xsl:text> </xsl:text><xsl:value-of select="@Name" /> )
	   {
		   List.Remove( <xsl:value-of select="@Name" /> );
	   }
   	
	   <xsl:if test="@CollectionChild='true'">
	   <xsl:variable name="parent" select="." />
	   <!-- This creates a temporary XML tree, uses nodeset to change it to a tree, then iterates in the code that builds the methods -->
	   <xsl:variable name="temp">
         <xsl:call-template name="otherparentparams">
            <xsl:with-param name="otherparents" select="$otherparents"/>
            <xsl:with-param name="object" select="$object"/>
         </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="tempparams" select="msxsl:node-set($temp)"/>
	   <xsl:variable name="temp2">
         <xsl:call-template name="parentnonlookupparams">
            <xsl:with-param name="parents" select="$otherparents"/>
            <xsl:with-param name="object" select="$object"/>
         </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="tempparamsnolookup" select="msxsl:node-set($temp2)"/>
	   <xsl:variable name="temp3">
         <xsl:call-template name="parentnonlookupparams">
            <xsl:with-param name="parents" select="$parents"/>
            <xsl:with-param name="object" select="$object"/>
         </xsl:call-template>
	   </xsl:variable>
	   <xsl:variable name="tempallnolookup" select="msxsl:node-set($temp3)"/>
	   <xsl:if test="$tempparams/*">
	   public virtual <xsl:value-of select="$object/@Name"/> NewItem(
	      <xsl:for-each select="$tempparams//Field">
		      <xsl:value-of select="@ObjectType"/><xsl:text> </xsl:text><xsl:value-of select="@ObjectName"/>
	         <xsl:if test="last()!=position()">, 
	         </xsl:if>
      	      </xsl:for-each> )
         { 
	      <xsl:value-of select="$object/@Name"/> obj;
	      obj = <xsl:value-of select="$object/@Name" />.New<xsl:value-of select="$object/@Name" />Child( <xsl:text/>
	      <xsl:for-each select="$tempparams//Field" >
	         <xsl:value-of select="@ObjectName"/>
	         <xsl:if test="last()!=position()">, </xsl:if> 
	      </xsl:for-each>);
	      if( MakeNewItem( obj ) )
	      { 
		      return obj;
	      }
	      else
	      {
	         return null;
	      }
	   }
	   </xsl:if>
   	
	   //nolookup
	   <xsl:apply-templates select="$tempparamsnolookup" mode="NewItemContents"/>
      
	   //nolookup
	   <xsl:if test="count($tempallnolookup//Field) != count($tempparamsnolookup//Field)">
	      <xsl:apply-templates select="$tempallnolookup" mode="NewItemContents"/>
	   </xsl:if>
   	
	   public <xsl:value-of select="$Name"/> NewItem()
	   {	
	      <xsl:value-of select="$Name"/> obj; 
	      obj = <xsl:value-of select="$Name"/>.NewItemChild();
	      return obj;
	   }	
      
	   protected virtual bool MakeNewItem( 
	      <xsl:value-of select="@Name"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/> )
	   {
		   if( !Contains( <xsl:value-of select="@Name"/> ) )
		   {
			   List.Add( <xsl:value-of select="@Name"/> );
			   return true;
		   }
		   else
		   {
			   throw new Exception( "<xsl:value-of select="@Name"/> already assigned" );
		   }
	   }

      <!-- Added to support scenario 
      Steps:
         1. Create a new address object
         2. Use that object to open and populate the edit form
         3. User edits, including setting values of composit PK, other than app id
         4. User saves
         5. See if collection contains the object
         6. If yes, delete existing and add object
         7. If no, add object
      If you don't use this scenario, don't include this method. -->
	   public void AddItem( <xsl:value-of select="@Name"/> bObj )
	   {
		   MakeNewItem( bObj );
	   }
      
	   public bool Contains(
	      <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	         <xsl:text/><xsl:value-of select="@NETType"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
	         <xsl:if test="position()!=last()">,
	         </xsl:if>
	      </xsl:for-each> )
	   {
		   return (GetItem( <xsl:text/>
	      <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	         <xsl:text/><xsl:value-of select="@Name"/>
	         <xsl:if test="position()!=last()">, </xsl:if>
	      </xsl:for-each>) != null );
	   }	

	   public bool ContainsDeleted(
	      <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
	         <xsl:text/><xsl:value-of select="@NETType"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/> 
	         <xsl:if test="position()!=last()">, </xsl:if>
      	      </xsl:for-each> )
	   {
         <xsl:call-template name="FindInList">
            <xsl:with-param name="listname" select="'deletedList'"/>
            <xsl:with-param name="return" select="'true'"/>
         </xsl:call-template>
         return false;
	   }
	   </xsl:if>

   #endregion
   </xsl:template>

   <xsl:template match="BaseObject" mode="NewItemContents">
	   <xsl:if test="count(//Field)!=0">
	   public virtual <xsl:value-of select="/BaseObject/@Name"/> NewItem( 
		   <xsl:for-each select="//Field">
			   <xsl:value-of select="@Type"/><xsl:text> </xsl:text><xsl:value-of select="@Name"/>
			   <xsl:if test="last()!=position()">, 
			   </xsl:if>
		   </xsl:for-each> )
	   {
		   <xsl:value-of select="/BaseObject/@Name"/> obj;
		   obj = <xsl:value-of select="/BaseObject/@Name" />.New<xsl:value-of select="/BaseObject/@Name" />Child(<xsl:text/>
		   <xsl:for-each select="//Field" >
			   <xsl:value-of select="@Name"/>
			   <xsl:if test="last()!=position()">, </xsl:if> 
		   </xsl:for-each>);
		   if( MakeNewItem( obj ) )
		   {
			   return obj;
		   }
		   else
		   {
		      return null;
		   }
	   }
	   </xsl:if>
   </xsl:template>

   <xsl:template name="Contains">

	   #region Contains

	   public bool Contains(<xsl:value-of select="$Name"/> Item)
	   {
		   foreach( <xsl:value-of select="$Name"/> child in List )
		   {
			   if( child.Equals( Item ) )
			   {
				   return true;
			   }
		   }
		   return false;
	   }

	   public bool ContainsDeleted( <xsl:value-of select="$Name"/> Item )
	   {
		   foreach( <xsl:value-of select="$Name"/> child in deletedList )
		   {
			   if( child.Equals( Item ) )
			   {
				   return true;
			   }
		   }
		   return false;
	   }

	   #endregion
   </xsl:template>


   <xsl:template name="SharedMethods">
	   #region Shared Methods
   <xsl:if test="@CollectionRoot='true'">
	   public static <xsl:value-of select="$CollectionName"/> New<xsl:value-of select="$CollectionName"/>()
	   {
		   return new <xsl:value-of select="$CollectionName"/>();
	   }
   	
	   public static <xsl:value-of select="$CollectionName"/> Get<xsl:value-of select="$CollectionName"/>( <xsl:call-template name="PrimaryKeyArguments"/> )
	   {
		   return (<xsl:value-of select="$CollectionName"/>)DataPortal.Fetch( new Criteria( <xsl:call-template name="PrimaryKeyList"/> ) );
	   }
   	
	   public static void DeleteCollection( <xsl:call-template name="PrimaryKeyArguments"/> )
	   {
		   DataPortal.Delete( new Criteria( <xsl:call-template name="PrimaryKeyList"/> ) );
	   }
   </xsl:if> 

   <xsl:if test="@CollectionChild='true'">
	   internal static <xsl:value-of select="$CollectionName"/> New<xsl:value-of select="$CollectionName"/>Child()
	   {
		   return new <xsl:value-of select="$CollectionName"/>();
	   }

	   internal static <xsl:value-of select="$CollectionName"/> Get<xsl:value-of select="$CollectionName"/>Child( SafeDataReader dr )
	   {
		   <xsl:value-of select="$CollectionName"/> col = new <xsl:value-of select="$CollectionName"/>();
		   col.Fetch( dr );
		   return col;
	   }
   </xsl:if>
	   #endregion
   </xsl:template>

   <xsl:template name="Constructors">
	   #region Constructors

	   protected gen<xsl:value-of select="$CollectionName"/>()
	   {
		   // disallow direct creation

		   // mark us as a child collection
		   MarkAsChild();
	   }

	   #endregion
   </xsl:template>

   <xsl:template name="Criteria">
   <xsl:if test="@CollectionRoot='true'">
	   #region Criteria
	   <!-- You have to escape any less than signs that you want in the output -->
	   [Serializable()]
	   public class Criteria : CSLA.Server.CriteriaBase
	   {
	   <xsl:call-template name="PrimaryKeyDeclarations">
		   <xsl:with-param name="prefix" select="'m'" />
	   </xsl:call-template>
   	 
		   public Criteria( <xsl:call-template name="PrimaryKeyArguments"/> ) : base(typeof( <xsl:value-of select="$CollectionName"/> ))
		   {
			   <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
				   this.<xsl:value-of select="@Name"/> = <xsl:value-of select="@Name"/>;<xsl:text/>
			   </xsl:for-each>
		   }
   		
	   <xsl:for-each select="orm:Property[@IsPrimaryKey='true']">
		   public <xsl:value-of select="@NETType" /><xsl:text> </xsl:text><xsl:value-of select="@Name" />
		   {
			   get
			   {
				   return m<xsl:value-of select="@Name" />;
			   }
			   set
			   {
				   m<xsl:value-of select="@Name" /> = value;
			   }
		   }
      </xsl:for-each>
   		
		   public void AddParameters( IDbCommand cmd )
		   {
			   System.Data.SqlClient.SqlParameter p;
			   <xsl:apply-templates select="orm:Property[@IsPrimaryKey='true']" mode="ParameterAssignment"/>
		   }
	   }

	   #endregion
   </xsl:if>
   </xsl:template>

   <xsl:template name="DataAccess">

	   #region Data Access

   <xsl:if test="@CollectionRoot='true'">
	   <xsl:call-template name="DataPortalFetch">
		   <xsl:with-param name="iscollection" select="'true'"/>
	   </xsl:call-template>
   	
	   protected override void DataPortal_Update()
	   {
		   // Loop through each deleted child object and call its Update() method
		   foreach( <xsl:value-of select="$Name"/> child in deletedList )
		   {
			   child.Update( <xsl:if test="@ChildOf">null</xsl:if> );
		   }
   		
		   // Then clear the list of deleted objects because they are truly gone now
		   deletedList.Clear();
   		
		   // Loop through each non-deleted child object and call its Update() method
		   foreach( <xsl:value-of select="$Name"/> child in List )
		   {
			   child.Update( <xsl:if test="@ChildOf">null</xsl:if> );
		   }
	   }
   	
	   <!-- This code is in Rocky's book, but is apparently not needed 
	   protected Overrides Sub DataPortal_Delete(ByVal Criteria As Object)
		   Dim crit As Criteria = CType(Criteria, Criteria)
		   ' TODO: Delete child object data that matches the criteria (the comment without implementation is from Rockys book)
	   End Sub -->
   </xsl:if> 

   <xsl:if test="@CollectionChild='true'">
	   // called to load data from the database
	   private void Fetch( SafeDataReader dr )
	   {
		   <xsl:value-of select="$Name"/> obj;
		   obj = <xsl:value-of select="$Name"/>.Get<xsl:value-of select="$Name"/>Child( dr );
		   while( obj != null )
		   {
			   List.Add( obj );
			   obj = <xsl:value-of select="$Name"/>.Get<xsl:value-of select="$Name"/>Child( dr );
		   }
		   mIsLoaded = true;
	   }

	   // called by Project to delete/add/update data into the database
	   internal void Update( <xsl:text/>
		   <xsl:if test="@ChildOf">
			   <xsl:value-of select="@ChildOf"/><xsl:text> </xsl:text><xsl:value-of select="@ChildOf"/>
		   </xsl:if> )
	   {	
		   // update (thus deleting) any deleted child objects
		   foreach( <xsl:value-of select="@Name"/> obj in deletedList )
		   {
			   obj.Update( <xsl:if test="@ChildOf"><xsl:value-of select="@ChildOf"/></xsl:if> );
		   }
		   // now that they are deleted, remove them from memory too
		   deletedList.Clear();

		   // add/update any current child objects
		   foreach( <xsl:value-of select="@Name"/> obj in List )
		   {
			   obj.Update( <xsl:if test="@ChildOf"><xsl:value-of select="@ChildOf"/></xsl:if> );
		   }
	   }
   </xsl:if> 
	   #endregion

   </xsl:template>

   <xsl:template name="StuffForChild">
	   public <xsl:value-of select="$Name"/> this[ string ResourceID ]
	   {
		   get
		   {
			   ProjectResource r;

			   foreach( r in List )
			   {
				   if( r.ResourceID == ResourceID )
				   {
					   return r;
				   }
			   }
			   return null;
		   }
	   }

	   public Sub Remove( string ResourceID )
	   {
		   foreach( ProjectResource r in List )
		   {
			   if( r.ResourceID = ResourceID )
			   {
				   Remove( r );
				   break;	
			   }
		   }
	   }
    
   </xsl:template>

   <xsl:template name="FindInList">
	   <xsl:param name="listname" select="'List'"/>
	   <xsl:param name="return" select="'r'"/>
	   foreach( <xsl:value-of select="@Name"/> r in <xsl:value-of select="$listname"/> )
	   {
		   if( <xsl:text/>
			   <xsl:for-each select=".//orm:Property[@IsPrimaryKey='true']">
				   <xsl:text/>(<xsl:call-template name="Comparison">
					   <xsl:with-param name="first" select="concat('r.',@Name)"/>
					   <xsl:with-param name="second" select="@Name"/>
					   <xsl:with-param name="operand" select="'='"/>
					   <xsl:with-param name="type" select="@NETType"/>
				   </xsl:call-template> 
				   <xsl:if test="position()!=last()">) &amp;&amp; </xsl:if>
			   </xsl:for-each> ))
		   {
			   return <xsl:value-of select="$return"/>;
		   }
	   }
   </xsl:template>

</xsl:stylesheet> 
  
