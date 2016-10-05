<?xml version="1.0" encoding="UTF-8" ?>

<!-- Summary:  !-->
<!-- Created: !-->
<!-- TODO: !-->

<!--Required header !-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema">
<xsl:output method="text" encoding="UTF-8" indent="yes"/>
<xsl:preserve-space elements="*" />

<xsl:template match="/">
	<xsl:for-each select="/Family/Person/Person/Person[@Name='Anne']">
      &lt;br/> &lt;br/> ancestor	  "ancestor::*" &lt;br/> 
      <xsl:for-each select="ancestor::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> ancestor-or-self	 "ancestor-or-self::*"  &lt;br/> 
      <xsl:for-each select="ancestor-or-self::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> attribute	 "attribute::*"  &lt;br/> 
      <xsl:for-each select="attribute::*">
         <xsl:value-of select="."/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> child	  "child::*" &lt;br/> 
      <xsl:for-each select="child::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> descendant	 "descendant::*"  &lt;br/> 
      <xsl:for-each select="descendant::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> descendant-or-self	  "descendant-or-self::*" &lt;br/> 
      <xsl:for-each select="descendant-or-self::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> following	  "following::*" &lt;br/> 
      <xsl:for-each select="following::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> following-sibling "following-sibling::*" &lt;br/> 
      <xsl:for-each select="following-sibling::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> namespace	  "namespace::*" &lt;br/> 
      <xsl:for-each select="namespace::*">
         <xsl:value-of select="."/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> parent "parent::*" &lt;br/> 
      <xsl:for-each select="parent::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> preceding "preceding::*" &lt;br/> 
      <xsl:for-each select="preceding::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> preceding-sibling "preceding-sibling::*" &lt;br/> 
      <xsl:for-each select="preceding-sibling::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> self "self::*" &lt;br/> 
      <xsl:for-each select="self::*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> double slash	shortcut //*"  &lt;br/> 
      <xsl:for-each select="//*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> child	shortcut	 "*"  &lt;br/> 
      <xsl:for-each select="*">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> parent	shortcut ".." &lt;br/> 
      <xsl:for-each select="..">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> self shortcut "." &lt;br/> 
      <xsl:for-each select=".">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> node test "child::Person" &lt;br/> 
      <xsl:for-each select="child::Person">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> grandchild "/Family/Person/Person/Person" &lt;br/> 
      <xsl:for-each select="/Family/Person/Person/Person">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> women "//*[@Sex='Female']" &lt;br/> 
      <xsl:for-each select="//*[@Sex='Female']">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> women with names longer than five letters "//*[@Sex='Female'][string-length(@Name)>5]"&lt;br/> 
      <xsl:for-each select="//*[@Sex='Female'][string-length(@Name)>5]">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> women with names longer than five letters 2 "//*[@Sex='Female' and string-length(@Name)>5]" &lt;br/> 
      <xsl:for-each select="//*[@Sex='Female' and string-length(@Name)>5]">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>

      &lt;br/> &lt;br/> female line grandchildren of George "/Family/Person[starts-with(@Name,'George')]/
                  Person[@Sex='Female']/Person[@Sex='Female']"&lt;br/> 
      <xsl:for-each select="/Family/Person[starts-with(@Name,'George')]/
                  Person[@Sex='Female']/Person[@Sex='Female']">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> union "//Family|Person"&lt;br/> 
      <xsl:for-each select="//Family|Person">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
      &lt;br/> &lt;br/> test "Person[@Name=’George VI’]//Person[@Name=Charles]"&lt;br/> 
      <xsl:for-each select="//Person[@Name='George VI']//Person[@Name='Charles']">
         <xsl:value-of select="@Name"/>,  
      </xsl:for-each>
      
	</xsl:for-each>
   
</xsl:template>

</xsl:stylesheet> 
  