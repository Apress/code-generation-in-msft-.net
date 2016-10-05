// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Tools to facilitate working with navigator approach to XML DOM
//  Refactor: Since Whidbey will prefer the navigator approach, this class needs updating for transfer into Whidbey.


using System;

namespace KADGen.Utility
{
	public class NodeItHelpers
	{
		public static System.Xml.XPath.XPathNodeIterator GetNodeIt( System.Xml.XPath.XPathNavigator xnav, string xPath, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XPath.XPathExpression xpathExpr = xnav.Compile( xPath );
			if( nsmgr != null )
			{
				xpathExpr.SetContext( nsmgr );
			}
			return xnav.Select( xpathExpr );
		}

		public static System.Xml.XPath.XPathNodeIterator GetSortedNodeIt( System.Xml.XPath.XPathNavigator xnav, string xPath, System.Xml.XmlNamespaceManager nsmgr, string sortXpath, bool isAscending, bool isCaseSensitive, bool isNumber )
		{
			System.Xml.XPath.XPathExpression xpathExpr = xnav.Compile( xPath );
			System.Xml.XPath.XmlSortOrder sortOrder;
			System.Xml.XPath.XmlCaseOrder caseOrder;
			System.Xml.XPath.XmlDataType datatype;
			if( isAscending )
			{
				sortOrder = System.Xml.XPath.XmlSortOrder.Ascending;
			}
			else
			{
				sortOrder = System.Xml.XPath.XmlSortOrder.Descending;
			}
			if( isCaseSensitive )
			{
				caseOrder = System.Xml.XPath.XmlCaseOrder.UpperFirst;
			}
			else
			{
				caseOrder = System.Xml.XPath.XmlCaseOrder.None;
			}
			if( isNumber )
			{
				datatype = System.Xml.XPath.XmlDataType.Number;
			}
			else
			{
				datatype = System.Xml.XPath.XmlDataType.Text;
			}
			xpathExpr.AddSort( sortXpath, sortOrder, caseOrder, "", datatype );
			if( nsmgr != null )
			{
				xpathExpr.SetContext( nsmgr );
			}
			return xnav.Select( xpathExpr );
		}

		public static string GetChildAttribute( System.Xml.XPath.XPathNavigator xnav, string childName, string attributeName, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XPath.XPathNodeIterator nodeit = GetNodeIt( xnav, childName, nsmgr );
			nodeit.MoveNext();
			return ((string)(nodeit.Current.GetAttribute(attributeName, "" )));
		}
	}
}
