// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
// ====================================================================
//  Summary: Main file for managing the Generation process

using System;

namespace KADGen.CodeGenerationSupport
{
	public struct OutputInfo
	{
		public DateTime StartTime;	// Fields aren't displayed by default in the grid
		public DateTime EndTime;
		private string mSection;
		private string mDirectiveName;
		private string mDirectiveType;
		private string mSelectFile;
		private string mInputFile;
		private string mTemplate;
		private string mStatus;
		private string mOutputRule;
		private int mCountSelected;
		private int mCountCreated;
		private int mCountOutput;

		public string Section
		{
			get
			{
				return mSection;
			}
			set
			{
				mSection = value;
			}
		}

		public string DirectiveName
		{
			get
			{
				return mDirectiveName;
			}
			set
			{
				mDirectiveName = value;
			}
		}

		public string DirectiveType
		{
			get
			{
				return mDirectiveType;
			}
			set
			{
				mDirectiveType = value;
			}
		}

		public string SelectFile
		{
			get
			{
				return mSelectFile;
			}
			set
			{
				mSelectFile = value;
			}
		}

		public string InputFile
		{
			get
			{
				return mInputFile;
			}
			set
			{
				mInputFile = value;
			}
		}

		public string Template
		{
			get
			{
				return mTemplate;
			}
			set
			{
				mTemplate = value;
			}
		}

		public string Status
		{
			get
			{
				return mStatus;
			}
			set
			{
				mStatus = value;
			}
		}

		public string OutputRule
		{
			get
			{
				return mOutputRule;
			}
			set
			{
				mOutputRule = value;
			}
		}

		public int CountSelected
		{
			get
			{
				return mCountSelected;
			}
			set
			{
				mCountSelected = value;
			}
		}

		public int CountCreated
		{
			get
			{
				return mCountCreated;
			}
			set
			{
				mCountCreated = value;
			}
		}

		public int CountOutput
		{
			get
			{
				return mCountOutput;
			}
			set
			{
				mCountOutput = value;
			}
		}

		public TimeSpan Elapsed
		{
			get
			{
				return EndTime.Subtract( StartTime );
			}
		}

	}
	/// <summary>
	/// 
	/// </summary>
	public class Generation
	{
		#region Class level declarations
		private delegate System.IO.Stream GenerateDelegate( string outputFileName, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlNode node, System.Xml.XmlNode nodeSelect );
		//  Const vbcrlf As string = Microsoft.VisualBasic.ControlChars.CrLf
		private System.Collections.ArrayList mLogEntries;
		private System.Collections.ArrayList mOutput;
		private OutputInfo mCurrentOutput;
		// The following fields are used to avoid repeatedly loading XML files
		private System.Xml.XPath.XPathNavigator mNavMetadata;
		private CodeGenerationSupport.Param[] mParams;
		private CodeGenerationSupport.ExtObject[] mExtObjects;
		private System.Xml.Xsl.XslTransform mXSLTTransform;
		private System.Data.SqlClient.SqlConnection mConnection;
		// The following fields avoid repeated reflection look ups 
		private System.Reflection.MethodInfo mMethodInfo;
		private static System.Xml.XmlNode mNodeFilePaths;
		private System.Xml.XmlNode mNodeFilters;
		private string mDocPath;
		private ProjectSettings mProjectSettings = new ProjectSettings();
		private LocalSettings mLocalSettings = new LocalSettings();
		private Utility.SourceBase mSourceControl;
		private Utility.IProgressCallback mCallBack;
		private System.Xml.XmlDocument mXsdDoc;
		private int mCntDone;

		//private Const vbcr As string = Microsoft.VisualBasic.ControlChars.Cr
		//private Const vblf As string = Microsoft.VisualBasic.ControlChars.Lf

		#endregion

      #region public and Friend Methods and Properties
      internal static  System.Xml.XmlNode GetFilePathNode() 
      {
         return mNodeFilePaths;
      }

                                                                          
      public System.Collections.ArrayList RunGeneration( System.Xml.XmlDocument xmlDoc, System.Xml.XmlDocument xsdDoc, Utility.IProgressCallback callBack, ref int cntDone, ref System.Collections.ArrayList output, string xmlDocFileName )
		{
			// The sections just help order things
			// XPathNavigators must be used to support sorting

			mLogEntries = new System.Collections.ArrayList();

			System.Xml.XPath.XPathNavigator xpathNav = xmlDoc.CreateNavigator();
			//System.Xml.XPath.XPathExpression xpathExpr3
			System.Xml.XPath.XPathNodeIterator nodeItSection;
			System.Xml.XPath.XPathNodeIterator nodeItDirective;
			// System.Xml.XmlNode xAttr
			System.Xml.XmlNamespaceManager nsmgr = GetNameSpaceManager( xmlDoc, "kg" );
			int cntNodes = CountNodes( xmlDoc, nsmgr );
			int percentDone;
			string currentDirName;
			string sectionName;

			// Push current directory info 
			currentDirName = Environment.CurrentDirectory;
			Environment.CurrentDirectory = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( xmlDocFileName ) );

			mCallBack = callBack;
			mXsdDoc = xsdDoc;
			mCntDone = cntDone;
			mOutput = output;

			SetLocalSettings( xmlDoc, nsmgr );
			SetProjectSettings( xmlDoc, nsmgr );
			SetupSourceSafe();

			nsmgr.AddNamespace( "xs", "http://www.w3.org/2001/XMLSchema" );

			mNodeFilePaths = xmlDoc.SelectSingleNode( "kg:GenerationScript/kg:FilePaths", nsmgr );
			mNodeFilters = xmlDoc.SelectSingleNode( "kg:GenerationScript/kg:Filters", nsmgr );
			mDocPath = System.IO.Path.GetDirectoryName( xmlDocFileName );

			nodeItSection = Utility.NodeItHelpers.GetNodeIt( xpathNav, "//kg:Section[kg:Standard/@Checked='true']", nsmgr );
			while( nodeItSection.MoveNext() )
			{
				sectionName = Utility.NodeItHelpers.GetChildAttribute( nodeItSection.Current, "kg:Standard", "Name", nsmgr );
				nodeItDirective = Utility.NodeItHelpers.GetSortedNodeIt( nodeItSection.Current, "./*", nsmgr, "Ordinal", true, false, true );
				while( nodeItDirective.MoveNext() )
				{
					mCurrentOutput = new OutputInfo();
					mCurrentOutput.Section = sectionName;
					mCurrentOutput.DirectiveName = Utility.NodeItHelpers.GetChildAttribute( nodeItDirective.Current, "kg:Standard", "Name", nsmgr );
					mCurrentOutput.DirectiveType = nodeItDirective.Current.LocalName;
					mCurrentOutput.CountSelected = nodeItSection.Count;
					if( Utility.NodeItHelpers.GetChildAttribute(nodeItDirective.Current, "kg:Standard", "Checked", nsmgr ) != "false" )
					{
						mCntDone += 1;
						System.Windows.Forms.Application.DoEvents();
						if( callBack.GetCancel() )
						{
							if( System.Windows.Forms.MessageBox.Show( "Do you really want to cancel process?", "Confirm Cancel", System.Windows.Forms.MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.Yes )
							{
								break;
							}
							else
							{
								callBack.ResetCancel();
							}
						}
						if( cntNodes == 0 )
						{
							callBack.UpdateProgress( 100 );
						}
						else
						{
							percentDone = ((int)100 * mCntDone / cntNodes );
							if( percentDone > 100 )
							{
								percentDone = 100;
							}
							callBack.UpdateProgress( percentDone );
						}
						callBack.UpdateCurrentNode( ((System.Xml.IHasXmlNode)(nodeItDirective.Current)).GetNode() );
						// This is also updated in Multipas
						callBack.UpdateCurrentFile( "" );
						RunGenerationNode( nodeItDirective.Current, nsmgr, xsdDoc );
					}
					else
					{
						mCurrentOutput.Status = "! Checked";
					}
					mOutput.Add( this.mCurrentOutput );
				}
			}

			cntDone = mCntDone;

			// Pop current directory
			Environment.CurrentDirectory = currentDirName;
			return mLogEntries;
		}
		#endregion

		#region protected and internal Methods and Properties -empty
		#endregion

		#region protected Event Response Methods -empty
		#endregion

		#region private Methods and Properties

		private void SetLocalSettings( System.Xml.XmlDocument xmlDoc, System.Xml.XmlNamespaceManager nsmgr )
		{
			string sLocalFile;
			System.Xml.XmlNode elem;
			System.Xml.XmlDocument xmlLocalDoc = new System.Xml.XmlDocument();
			elem = xmlDoc.SelectSingleNode( "kg:GenerationScript", nsmgr );
			nsmgr.AddNamespace( "kl", "http://kadgen.com/KADGenLocalSettings.xsd" );
			sLocalFile = Utility.Tools.GetAttributeOrEmpty( elem, "LocalSettings" );
			if( System.IO.File.Exists( sLocalFile ) )
			{
				xmlLocalDoc.Load( sLocalFile );
				mLocalSettings.Node = xmlLocalDoc.SelectSingleNode( "//kl:LocalSettings", nsmgr );
			}
		}

		private void SetProjectSettings( System.Xml.XmlDocument xmlDoc, System.Xml.XmlNamespaceManager nsmgr )
		{
			string sProjectFile;
			System.Xml.XmlNode elem;
			System.Xml.XmlDocument xmlLocalDoc = new System.Xml.XmlDocument();
			elem = xmlDoc.SelectSingleNode( "kg:GenerationScript", nsmgr );
			sProjectFile = Utility.Tools.GetAttributeOrEmpty( elem, "ProjectSettings" );
			if( System.IO.File.Exists( sProjectFile ) )
			{
				xmlLocalDoc.Load( sProjectFile );
				nsmgr.AddNamespace( "kp", "http://kadgen.com/KADGenProjectSettings.xsd" );
				mProjectSettings.Node = xmlLocalDoc.SelectSingleNode( "//kp:ProjectSettings", nsmgr );
			}
		}

		private void SetupSourceSafe()
		{
			try
			{
				// NOTE: change the following for alternate sourcecontrol tools
				try
				{
					// I am opening this via reflection so people that don't have SourceSafe
					// can run this tool
					System.Reflection.Assembly asm;
					string path = System.Windows.Forms.Application.ExecutablePath;
					path = System.IO.Path.GetDirectoryName( path );
					path = System.IO.Path.Combine( path, "SourceSafeTools.dll" );
					asm = System.Reflection.Assembly.LoadFile( path );
					if( asm != null )
					{
						mSourceControl = ((Utility.SourceBase)(asm.CreateInstance( "KADGen.SourceSafeTools.SourceSafe" )));
					}
					//Simple instatiation blows a compile error where people don't have sourcesafe
					//If you Then 've got SourceSafe, reference the SourceSafeTools directory and 
					//uncomment the following
					//mSourceControl = new SourceSafeTools.SourceSafe( "" )
				}
				catch( System.Exception ex )
				{
					System.Diagnostics.Debug.WriteLine( ex );
					// TODO: Determine whether missing source control is an error
					// It probably won't be an error here, as you might not have any
					// files marked for Checkout. OTOH, this is an earlier location to 
					// get an exception.
				}

			}
			catch
			{
				System.Diagnostics.Debug.WriteLine( "No SourceSafe" );
			}
		}

		private void RunGenerationNode( System.Xml.XPath.XPathNavigator nav, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlDocument xsdDoc )
		{
			try
			{
				System.Xml.XmlNode node = ((System.Xml.IHasXmlNode)nav).GetNode();
				System.Xml.XmlNode nodeXSD = Utility.xmlHelpers.GetSchemaForNode( node.LocalName, xsdDoc );
				System.Reflection.MethodInfo methodInfo = null;
				if( node.LocalName != "Standard" )
				{
					methodInfo = Utility.Tools.GetMethodInfo( nodeXSD, typeof(Generation ), nsmgr, mLocalSettings.GetBasePath( ), mDocPath, mNodeFilePaths );

				}
				if( methodInfo != null )
				{
					methodInfo.Invoke( this, new object[]{ node, nsmgr } );
				}
			}
			catch( System.Exception ex )
			{
				System.Diagnostics.Debug.WriteLine( ex.ToString() );
				throw;
			}
		}

		private void XSLTProcess( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{

			string basePath = mLocalSettings.GetBasePath();
			string outputFile = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:SinglePass", "OutputFile", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			string inputXML = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:XSLTFiles", "InputFileName", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			string XSLTFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:XSLTFiles", "XSLTFileName", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			ExtObject[] extObjects = GetExtObjects( node, nsmgr );
			System.IO.Stream stream;
			bool madeFile = false;
			int countOutput = 0;
			string outputType = Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "OutputFileType", nsmgr );
			CurrentOutputStart( "None", inputXML, XSLTFileName, 1, this.GetOutType(node ).ToString());
			try
			{
				if( ShouldRun(outputFile, node ) )
				{
					if( SSCheckOutFile(node, basePath, outputFile ) )
					{
						try
						{
							stream = CodeGenerationSupport.XSLTSupport.GenerateViaXSLT( XSLTFileName, inputXML, outputType, extObjects );
							madeFile = MarkWriteAndClose( stream, node, outputFile );
						}
						finally
						{
							SSCheckInFile( node, basePath, outputFile, madeFile );
						}
					}
				}
				if( madeFile )
				{
					countOutput = 1;
				}
				CurrentOutputEnd( "Done", 1, countOutput );

			}
			catch
			{
				mCurrentOutput.Status = "Error";
				throw;
			}
		}

		private void RunProcess( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string basePath = mLocalSettings.GetBasePath();
			string outputFile = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:SinglePass", "OutputFile", nsmgr ), basePath, mDocPath, mNodeFilePaths );
			System.IO.Stream stream;
			object[] objs;
			bool madeFile = false;
			int countOutput = 0;
         this.SetProcess(node, nsmgr);
         objs = this.SetProcessParameters(node, nsmgr);
			CurrentOutputStart( "None", mMethodInfo.Name, this.GetProcessName( node, nsmgr ), 1, this.GetOutType( node ).ToString() );
			try
			{
				SetProcess( node, nsmgr );
				objs = SetProcessParameters( node, nsmgr );
				if( ShouldRun( outputFile, node ) )
				{
					if( SSCheckOutFile( node, basePath, outputFile ) )
					{
						try
						{
							stream = ((System.IO.Stream)(mMethodInfo.Invoke( this, objs )));
							SSCheckOutFile( node, basePath, outputFile );
							madeFile = MarkWriteAndClose( stream, node, outputFile );
						}
						finally
						{
							SSCheckInFile( node, basePath, outputFile, madeFile );
						}
					}
				}
				if( madeFile )
				{
					countOutput = 1;
				}
				CurrentOutputEnd( "Done", 1, countOutput );
			}
			catch
			{
				CurrentOutputEnd( "Error", 1, countOutput );
				throw;
			}
		}

		private void CreateMetadata( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string basePath = mLocalSettings.GetBasePath();
			string server = Utility.Tools.GetAttributeOrEmpty( node, "Server" );
			string mappingFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "MappingFileName" ), basePath, mDocPath, mNodeFilePaths );
			string outputFile = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:SinglePass", "OutputFile", nsmgr ), basePath, mDocPath, mNodeFilePaths );
			string databaseName = Utility.Tools.GetAttributeOrEmpty( node, "Database" );
         databaseName = Utility.Tools.FixPath(databaseName, basePath, mDocPath, mNodeFilePaths);
			Metadata.SQLExtractMetaData SQLServerMetaData = new Metadata.SQLExtractMetaData( server );
			System.Xml.XmlDocument xmlDoc;
			string skipStoredProcs = Utility.Tools.GetAttributeOrEmpty( node, "SkipStoredProcs" );
			string selectPatterns = Utility.Tools.GetAttributeOrEmpty( node, "SelectPatterns" );
			string setSelectPatterns = Utility.Tools.GetAttributeOrEmpty( node, "SetSelectPatterns" );
			string removePrefix = Utility.Tools.GetAttributeOrEmpty( node, "RemovePrefix" );
			string lookupPrefix = Utility.Tools.GetAttributeOrEmpty( node, "LookupPrefix" );
			CurrentOutputStart( "None", "N/A", "N/A", 1, this.GetOutType( node ).ToString() );
			try
			{
				if( ShouldRun( outputFile, node ) )
				{
					if( SSCheckOutFile( node, basePath, outputFile ) )
					{
						try
						{
							xmlDoc = SQLServerMetaData.CreateMetaData( ( skipStoredProcs == "true" ), selectPatterns, setSelectPatterns, removePrefix, lookupPrefix, databaseName );
                     Utility.Tools.MakePathIfNeeded(outputFile);
							xmlDoc.Save( outputFile );
						}
						finally
						{
							SSCheckInFile( node, basePath, outputFile, true );
						}
					}
				}

				CurrentOutputEnd( "Done", 1, 1 );
			}
			catch
			{
				CurrentOutputEnd( "Error", 1, 0 );
				throw;
			}
		}

		private void MergeMetadata( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string baseXMLFileName = null;
			string mergeXMLFileName = "";
			string outputXMLFileName;
			string basePath = mLocalSettings.GetBasePath();
			CurrentOutputStart( "None", baseXMLFileName, mergeXMLFileName, 1, this.GetOutType(node ).ToString());
			try
			{
				// TODO: Add overwriting protection
				baseXMLFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty(node, "BaseXMLFileName" ), basePath, mDocPath, mNodeFilePaths);
				mergeXMLFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty(node, "MergingXMLFileName" ), basePath, mDocPath, mNodeFilePaths);
				outputXMLFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty(node, "OutputXMLFileName" ), basePath, mDocPath, mNodeFilePaths);
				// TODO: Consider adding check out for these files, but generally they would not be checked in and out as they are transitory
				Metadata.MergeFreeForm.Merge( baseXMLFileName, mergeXMLFileName, outputXMLFileName );
				CurrentOutputEnd( "Done", 1, 1 );
			}
			catch
			{
				CurrentOutputEnd( "Error", 1, 0 );
				throw;
			}
		}

		private void XSLTGeneration( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string basePath = mLocalSettings.GetBasePath();
			string metadataFileName = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:XSLTFiles", "InputFileName", nsmgr ), basePath, mDocPath, mNodeFilePaths );
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			string xsltFile = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:XSLTFiles", "XSLTFileName", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			CurrentOutputStart( xsltFile );
			try
			{
				xmlDoc.Load( metadataFileName );
				KADGen.LibraryInterop.Singletons.XMLDoc = xmlDoc;
				KADGen.LibraryInterop.Singletons.NsMgr = KADGen.Utility.xmlHelpers.BuildNamespacesManagerForDoc( xmlDoc );
				mNavMetadata = xmlDoc.CreateNavigator();

				mParams = CodeGenerationSupport.XSLTSupport.GetXSLTParams( xsltFile, basePath );
				mExtObjects = GetExtObjects( node, nsmgr );
				mXSLTTransform = new System.Xml.Xsl.XslTransform();
				mXSLTTransform.Load( xsltFile );
				MultiPass( node, nsmgr, new GenerateDelegate( XSLTGenerationCallBack ) );
				mNavMetadata = null;
				mParams = null;
				mXSLTTransform = null;
				CurrentOutputEnd( "Done" );
			}
			catch
			{
				CurrentOutputEnd( "Error" );
				throw;
			}
		}


		private void BruteForceGeneration( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			CurrentOutputStart( this.GetProcessName( node, nsmgr ));
			try
			{
				SetProcess( node, nsmgr );
				MultiPass( node, nsmgr, new GenerateDelegate( BruteForceGenerationCallBack ) );
				mMethodInfo = null;
				mParams = null;
				CurrentOutputEnd( "Done" );
			}
			catch
			{
				CurrentOutputEnd( "Error" );
				throw;
			}
		}

		private void CodeDOMGeneration( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			CurrentOutputStart( this.GetProcessName( node, nsmgr ));
			try
			{
				SetProcess( node, nsmgr );
				MultiPass( node, nsmgr, new GenerateDelegate( CodeDOMGenerationCallBack ) );
				mMethodInfo = null;
				mParams = null;
				CurrentOutputEnd( "Done" );
			}
			catch
			{
				CurrentOutputEnd( "Error" );
				throw;
			}
		}

		private void RunSQLScripts( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
         string basePath  = mLocalSettings.GetBasePath();
			string serverName = Utility.Tools.GetAttributeOrEmpty( node, "Server" );
			string databaseName = Utility.Tools.GetAttributeOrEmpty( node, "Database" );
         databaseName = Utility.Tools.FixPath(databaseName, basePath, mDocPath, mNodeFilePaths);
			CurrentOutputStart( serverName + ":" + databaseName );
			try
			{
				mConnection = new System.Data.SqlClient.SqlConnection( "workstation id=" + serverName + ";packet size=4096;integrated security=SSPI;data source=" + serverName + ";persist security info=false;initial catalog=" + databaseName );
				MultiPass( node, nsmgr, new GenerateDelegate( RunSQLScriptsCallBack ) );
				CurrentOutputEnd( "Done" );
			}
			catch
			{
				CurrentOutputEnd( "Error" );
				throw;
			}
			finally
			{
				mConnection.Close();
			}
		}

		private void CopyFiles( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string sourceFileName = Utility.Tools.GetAttributeOrEmpty( node, "SourceFileName" );
			string targetFileName = Utility.Tools.GetAttributeOrEmpty( node, "TargetFileName" );
         sourceFileName = Utility.Tools.FixPath(sourceFileName, "", "", mNodeFilePaths);
         targetFileName = Utility.Tools.FixPath(targetFileName, "", "", mNodeFilePaths);
			CurrentOutputStart( "None", sourceFileName, "None", 1, "" );
			// TODO: COnsider Source Control adn whether to allow overwriting files
         if( sourceFileName.Trim().Length > 0 & targetFileName.Trim().Length > 0 )
         {
            if( System.IO.File.Exists(sourceFileName) )
            {
               System.IO.File.Copy(sourceFileName, targetFileName);
            }
            else if( System.IO.Directory.Exists(sourceFileName) )
            {
               System.IO.DirectoryInfo dirTarget = System.IO.Directory.CreateDirectory(targetFileName);
               System.IO.DirectoryInfo dirSource = new System.IO.DirectoryInfo(sourceFileName);
               CopyContainedFiles(dirSource, dirTarget);
            }
            else
            {
               throw new System.Exception("File " + sourceFileName + " Not Found");
            }
			}
			CurrentOutputEnd( "Done", 1, 1 );
		}

		private void NestedScript( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XmlDocument xmlDoc;
			string xmlFileName = null;
			// TODO: Incorporate nested scripts in scrollbar, logging and output
			CurrentOutputStart( "None", xmlFileName, "None", -1, "" );
			xmlFileName = Utility.Tools.GetAttributeOrEmpty( node, "ScriptName" );
			xmlFileName = Utility.Tools.FixPath( xmlFileName, "", "", mNodeFilePaths );
			xmlDoc = Utility.xmlHelpers.LoadFile( xmlFileName, mXsdDoc );
			RunGeneration( xmlDoc, mXsdDoc, mCallBack, ref mCntDone, ref mOutput, xmlFileName );
			CurrentOutputEnd( "Done", 1, 1 );
			//System.Windows.Forms.MessageBox.Show( "Nested Script not yet supported" )
		}

		private void MultiPass( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr, GenerateDelegate delegGenerate )
		{
			string basePath = mLocalSettings.GetBasePath();
			System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
			System.Xml.XmlNodeList nodeList;
			string outputFileName;
			//System.Xml.XmlNode attr;
			System.IO.Stream stream;

			string outputDir = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "OutputDir", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			string outputFilePattern = Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "OutputFilePattern", nsmgr );
			string selectXPath = Utility.Tools.FixFilter( Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "SelectXPath", nsmgr ), mNodeFilters);
			string selectNSPrefix = Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "SelectNSPrefix", nsmgr );
			string selectNamespace = Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "SelectNamespace", nsmgr );
			string selectFile = Utility.Tools.FixPath( Utility.Tools.GetAttributeOrEmpty( node, "kg:MultiPass", "SelectFile", nsmgr ), basePath, mDocPath, mNodeFilePaths);
			bool madeFile;
			if( selectFile.Trim().Length > 0 )
			{
				if( this.mCurrentOutput.InputFile != null && this.mCurrentOutput.InputFile.Trim().Length == 0 )
				{
					this.mCurrentOutput.InputFile = selectFile;
				}
				xmlDoc.Load( selectFile );
				System.Xml.XmlNamespaceManager nsmgrMeta = Utility.Tools.BuildNamespaceManager( xmlDoc, node, "kg:MultiPass", nsmgr, "Select" );
				nsmgrMeta.AddNamespace( "ffu", "http://kadgen/FreeFormForUI.xsd" );
				nsmgrMeta.AddNamespace( "ui", "http://kadgen.com/UserInterface.xsd" );
				nodeList = xmlDoc.SelectNodes( selectXPath, nsmgrMeta );
				this.mCurrentOutput.CountSelected = nodeList.Count;
				foreach(System.Xml.XmlNode nodeSelect in nodeList )
				{
					madeFile = false;
					outputFileName = System.IO.Path.Combine( outputDir, ParseFilenamePattern(outputFilePattern, nodeSelect, nsmgrMeta ));
					mCallBack.UpdateCurrentFile( outputFileName );
					Utility.Tools.MakePathIfNeeded( outputFileName );
					if( ShouldRun(outputFileName, node ) )
					{
						if( SSCheckOutFile(node, basePath, outputFileName ) )
						{
							try
							{
								stream = delegGenerate( outputFileName, nsmgr, node, nodeSelect );
								if( stream != null )
								{
									madeFile = MarkWriteAndClose( stream, node, outputFileName );
								}
								this.mCurrentOutput.CountCreated += 1;
								if( madeFile )
								{
									this.mCurrentOutput.CountOutput += 1;
								}
							}
							finally
							{
								SSCheckInFile( node, basePath, outputFileName, madeFile );
							}
						}
					}
				}
			}
		}


		private System.IO.Stream XSLTGenerationCallBack( string outputFileName, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlNode node, System.Xml.XmlNode nodeSelect )
		{
			SetParameters( nodeSelect, outputFileName );
			string outputType = Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "OutputFileType", nsmgr );
			return CodeGenerationSupport.XSLTSupport.GenerateViaXSLT( mXSLTTransform, mNavMetadata, outputType, mExtObjects, mParams );
		}

		private System.IO.Stream BruteForceGenerationCallBack( string outputFileName, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlNode node, System.Xml.XmlNode nodeSelect )
		{
			SetParameters( nodeSelect, outputFileName );
			return ((System.IO.Stream)(mMethodInfo.Invoke(this, BuildParamArray())));
		}

		private System.IO.Stream CodeDOMGenerationCallBack( string outputFileName, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlNode node, System.Xml.XmlNode nodeSelect )
		{
			Utility.OutputType outType = ((Utility.OutputType)(System.Enum.Parse( typeof(Utility.OutputType), Utility.Tools.GetAttributeOrEmpty( node, "TargetLanguage" ))));
			System.CodeDom.CodeCompileUnit compileUnit;
			SetParameters( nodeSelect, outputFileName );
			object[] @params = this.BuildParamArray();
			if( @params == null )
			{
				compileUnit = ((System.CodeDom.CodeCompileUnit)(mMethodInfo.Invoke( this, @params )));
			}
			else
			{
				compileUnit = ((System.CodeDom.CodeCompileUnit)(mMethodInfo.Invoke( this, BuildParamArray() )));
			}
			return CodeGenerationSupport.CodeDOMSupport.GenerateViaCodeDOM( outType, compileUnit );
		}

		private System.IO.Stream RunSQLScriptsCallBack( string outputFileName, System.Xml.XmlNamespaceManager nsmgr, System.Xml.XmlNode node, System.Xml.XmlNode nodeSelect )
		{
			RunScript( node, outputFileName );
			return null;
		}

		private bool ShouldRun( string outputfile, System.Xml.XmlNode node )
		{
			Utility.OutputType outType;
			Utility.FileChanged fileChanged;
			if( node.Name == "kg:RunSQLScripts" )
			{
				// Should Run is handled later
				return true;
			}
			else
			{
				System.Xml.XmlNamespaceManager nsmgr = GetNameSpaceManager( node.OwnerDocument, "kg" );
				System.Xml.XmlNode nodeStandard = node.SelectSingleNode( "kg:OutputRules", nsmgr );
				outType = this.GetOutType( node );
				fileChanged = Utility.HashTools.IsFileChanged( outputfile, mProjectSettings.GetCommentStart( outType ), mProjectSettings.GetCommentEnd( outType ), mProjectSettings.GetHeaderMarker( ), mProjectSettings.GetHashMarker( ) );
				return ShouldRun( this.GetGenType( node ), fileChanged, outputfile, false );
			}
		}

		private bool ShouldRun( Utility.GenType genType, Utility.FileChanged fileChanged, string name, bool isRunSQLScript )
		{
			string sMode = "";
			if( isRunSQLScript )
			{
				sMode = "( Running SQL Script ) ";
			}
			//If ! isRunSQLScript & '         ( System.IO.File.Exists(name ) && ( System.IO.File.GetAttributes(name ) & '                   System.IO.FileAttributes.ReadOnly) != 0) Then
			//   LogError( name + " cannot output because file's ReadOnly - probably because of SourceSafe issues", name )
			//Else
			switch( genType )
			{
				case Utility.GenType.Overwrite:
					return true;
				case Utility.GenType.Always:
					if( fileChanged == Utility.FileChanged.Unknown )
						LogError( "ERROR: " + sMode + "Autogenerated hash could not be recovered. Generation of " + name + " aborted", name );
					if( fileChanged == Utility.FileChanged.Unknown || fileChanged == Utility.FileChanged.Changed )
						LogError( "ERROR: " + sMode + "Autogenerated file was manually edited. Generation of " + name + " aborted", name );
					else
						return true;
					break;
				/*switch( fileChanged )
				{
					case Utility.FileChanged.Unknown:
						LogError( "ERROR: " + sMode + "Autogenerated hash could not be recovered. Generation of " + name + " aborted", name );
						break;
					case Utility.FileChanged.Unknown:
					case Utility.FileChanged.Changed:
						LogError( "ERROR: " + sMode + "Autogenerated file was manually edited. Generation of " + name + " aborted", name );
						break;
					default:
						return true;
				}*/

				case Utility.GenType.None:
					// I have no idea why we'd be here
					break;
				case Utility.GenType.Once:
					switch( fileChanged )
					{
						case Utility.FileChanged.FileDoesntExist:
							return true;
						default:
							LogInfo( "Information Only: " + sMode + name + " not generated because it already existed. This could be a normal condition.", name );
							break;
					}
					break;
				case Utility.GenType.UntilEdited:
					switch( fileChanged )
					{
						case Utility.FileChanged.FileDoesntExist:
						case Utility.FileChanged.NotChanged:
							return true;
						case Utility.FileChanged.Unknown:
							LogError( "ERROR: " + sMode + "Autogenerated hash could not be recovered. Generation of " + name + " aborted", name );
							break;
						case Utility.FileChanged.Changed:
							LogInfo( "Information Only: " + sMode + name + " not generated because it existed and had been edited. This could be a normal condition.", name );
							break;
					}
					break;
			}
			return false;
		}

		private System.Xml.XmlNamespaceManager GetNameSpaceManager( System.Xml.XmlDocument xmlDoc, string prefix )
		{
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( xmlDoc.NameTable );
			string ns = "";
			switch( prefix )
			{
				case "kg":
					ns = "http://kadgen.com/KADGenDriving.xsd";
					break;
				case "dbs":
					ns = "http://kadgen/DatabaseStructure";
					break;
			}

			if( ns != null )
			{
				nsmgr.AddNamespace( prefix, ns );
				return nsmgr;
			}
			return null;
		}

		private Utility.OutputType GetOutType( System.Xml.XmlNode node )
		{
			string outTypeString;
			System.Xml.XmlNamespaceManager nsmgr = GetNameSpaceManager( node.OwnerDocument, "kg" );
			System.Xml.XmlNode nodeStandard = node.SelectSingleNode( "kg:OutputRules", nsmgr );
			outTypeString = Utility.Tools.GetAttributeOrEmpty( nodeStandard, "OutputFileType" );
			if( outTypeString.Trim().Length == 0 )
			{
				return Utility.OutputType.None;
			}
			else
			{
				return ((Utility.OutputType)(System.Enum.Parse(typeof(Utility.OutputType ), outTypeString)));
			}
		}

		private string GetApplyHash( System.Xml.XmlNode node )
		{
			System.Xml.XmlNamespaceManager nsmgr = Utility.Tools.BuildNamespaceManager( node, "kg", false );
			return Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "HashOutput", nsmgr );
		}

		private Utility.GenType GetGenType( System.Xml.XmlNode node )
		{
			string GenTypeString;
			System.Xml.XmlNamespaceManager nsmgr = GetNameSpaceManager( node.OwnerDocument, "kg" );
			System.Xml.XmlNode nodeStandard = node.SelectSingleNode( "kg:OutputRules", nsmgr );
			GenTypeString = Utility.Tools.GetAttributeOrEmpty( nodeStandard, "OutputGenType" );
			return ((Utility.GenType)(System.Enum.Parse( typeof(Utility.GenType), GenTypeString )));
		}

		private bool MarkWriteAndClose( System.IO.Stream stream, System.Xml.XmlNode node, string outputFile )
		{
			System.IO.StreamWriter writer;
			System.IO.StreamReader reader;
			Utility.OutputType outType = this.GetOutType( node );
			Utility.GenType genType = this.GetGenType( node );
			string applyHash = this.GetApplyHash( node );
			bool madeFile = stream.Length > 0;
			if( madeFile )
			{
				if( applyHash == "true" )
				{
					stream = Utility.HashTools.ApplyHash( stream, mProjectSettings.GetCommentText(genType ), mProjectSettings.GetCommentStart( outType ), mProjectSettings.GetCommentEnd( outType ), mProjectSettings.GetHeaderMarker(), mProjectSettings.GetHashMarker() );
				}
            Utility.Tools.MakePathIfNeeded(outputFile);
				writer = new System.IO.StreamWriter( outputFile );
				reader = new System.IO.StreamReader( stream );
				stream.Seek( 0, System.IO.SeekOrigin.Begin );
				writer.Write( reader.ReadToEnd() );
				writer.Flush();
				writer.Close();
				reader.Close();
			}
			stream.Close();
			return madeFile;
		}

		protected virtual bool SSCheckOutFile( System.Xml.XmlNode node, string workingPath, string file )
		{

			Utility.SourceBase.ItemStatus status;
			string checkoutProject = GetSSCheckOutProject( node );
			bool ret;
			if( GetSSCheckOut(node ) )
			{
				if( (checkoutProject != null ) && checkoutProject.Trim().Length > 0 )
				{
					checkoutProject = Utility.Tools.FixPath( checkoutProject, mLocalSettings.GetBasePath( ), mDocPath, mNodeFilePaths );
					if( mSourceControl == null )
					{
						// TODO: Figure out whether this is an error
						return true;
					}
					else
					{
						status = mSourceControl.CheckOut( file, workingPath, checkoutProject );
						ret = ( status == Utility.SourceBase.ItemStatus.CheckedOutToMe | status == Utility.SourceBase.ItemStatus.DoesntExist );
						if( ! ret )
						{
							LogError( "Couldn't check file out of SourceSafe", file );
						}
						return ret;
					}
				}
			}
			else
			{
				return true;
			}
			return false;
		}

		protected virtual void SSCheckInFile( System.Xml.XmlNode node, string workingPath, string file, bool addFile )
		{
			Utility.SourceBase.ItemStatus status;
			string checkoutProject = GetSSCheckOutProject( node );
			if( (checkoutProject != null ) && checkoutProject.Trim().Length > 0 )
			{
				checkoutProject = Utility.Tools.FixPath( checkoutProject, mLocalSettings.GetBasePath(), mDocPath, mNodeFilePaths );
				if( mSourceControl == null )
				{
					// TODO: Figure out whether this is an error
				}
				else
				{
					status = mSourceControl.CheckIn( file, workingPath, checkoutProject );
					if( status == Utility.SourceBase.ItemStatus.DoesntExist )
					{
						if( addFile )
						{
							SSAddFile( node, workingPath, checkoutProject, file );
						}
					}
				}
			}
		}

		protected virtual void SSAddFile( System.Xml.XmlNode node, string workingPath, string checkoutProject, string file )
		{
				;
			if( checkoutProject.Trim().Length > 0 )
			{
				if( mSourceControl == null )
				{
					// TODO: Figure out whether this is an error
				}
				else
				{
					mSourceControl.AddFile( file, workingPath, checkoutProject );
				}
			}

		}

		protected virtual string GetSSCheckOutProject( System.Xml.XmlNode node )
		{
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( node.OwnerDocument.NameTable );
			nsmgr.AddNamespace( "kg", "http://kadgen.com/KADGenDriving.xsd" );

			if( Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "CheckOut", nsmgr ) == "true" )
			{
				return Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "CheckOutProject", nsmgr );
			}
			return "";
		}

		protected virtual bool GetSSCheckOut( System.Xml.XmlNode node )
		{
			System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager( node.OwnerDocument.NameTable );
			nsmgr.AddNamespace( "kg", "http://kadgen.com/KADGenDriving.xsd" );

			return Utility.Tools.GetAttributeOrEmpty( node, "kg:OutputRules", "CheckOut", nsmgr ) == "true";
		}

		private void LogError( string message, string source )
		{
			mLogEntries.Add( new Utility.LogEntry( Utility.LogEntry.logLevel.SeriousError, message, source ) );
		}
		private void LogCritical( string message, string source )
		{
			mLogEntries.Add( new Utility.LogEntry( Utility.LogEntry.logLevel.CriticalError, message, source ) );
		}
		private void LogInfo( string message, string source )
		{
			mLogEntries.Add( new Utility.LogEntry( Utility.LogEntry.logLevel.InfoOnly, message, source ) );
		}
		private void LogWarning( string message, string source )
		{
			mLogEntries.Add( new Utility.LogEntry( Utility.LogEntry.logLevel.Warning, message, source ) );
		}

		private int CountNodes( System.Xml.XmlDocument xmlDoc, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Xml.XmlNodeList nodelist;
			nodelist = xmlDoc.SelectNodes( "//kg:Section[kg:Standard/@Checked='true']/*[kg:Standard/@Checked='true']", nsmgr );
			return nodelist.Count + 1;
		}

		private void SetProcess( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Reflection.ParameterInfo[] paramInfos;
			mMethodInfo = Utility.Tools.GetMethodInfo( node, typeof(Generation ), nsmgr, "kg:Process", mLocalSettings.GetBasePath( ), mDocPath, mNodeFilePaths );
			if( mMethodInfo != null )
			{
				paramInfos = mMethodInfo.GetParameters();
				mParams = new CodeGenerationSupport.Param[paramInfos.GetLength( 0 )];
				for( int i=0; i<=paramInfos.GetUpperBound( 0 ); i++ )
				{
					mParams[i] = new CodeGenerationSupport.Param( paramInfos[i].Name );
				}
			}
		}
 
		private void SetParameters( System.Xml.XmlNode nodeSelect, string outputFileName )
		{
			if( mParams != null )
			{
				for( int i = 0; i <= mParams.GetUpperBound( 0 ); i++ )
				{
					System.Xml.XmlNamespaceManager nsmgr;
					System.Xml.XmlNode nodeTemp;
					switch( mParams[i].Name.ToLower() )
					{
						case "gendatetime":
							mParams[i].Value = System.DateTime.Now.ToString();
							break;
						case "filename":
							mParams[i].Value = outputFileName;
							break;
						case "database":
							nsmgr = new System.Xml.XmlNamespaceManager( nodeSelect.OwnerDocument.NameTable );
							nsmgr.AddNamespace( "dbs", nodeSelect.NamespaceURI );
							nodeTemp = nodeSelect.SelectSingleNode( "ancestor::dbs:DataStructure", nsmgr );
							mParams[i].Value = Utility.Tools.GetAttributeOrEmpty( nodeTemp, "Name" );
							break;
						case "nodeselect":
							mParams[i].Value = nodeSelect;
							break;
						default:
							System.Xml.XmlNode nodeAttr;
							nsmgr = new System.Xml.XmlNamespaceManager( nodeSelect.OwnerDocument.NameTable );
							nsmgr.AddNamespace( "dbs", nodeSelect.NamespaceURI );
							nodeAttr = nodeSelect.Attributes.GetNamedItem( mParams[i].Name);
							if( nodeAttr == null )
							{
								nodeTemp = nodeSelect.SelectSingleNode( "ancestor::dbs:" + mParams[i].Name, nsmgr);
								if( nodeTemp == null )
								{
									System.Diagnostics.Debug.WriteLine( "Could not find xpath:" );
								}
								else
								{
									mParams[i].Value = Utility.Tools.GetAttributeOrEmpty( nodeTemp, "Name" );
								}
							}
							else
							{
								mParams[i].Value = nodeAttr.Value;
							}
							break;
					}
				}
			}
		}

		// CHANGE: SetProcessParameters was very complex, so I rewrote it changing from commas to obscure delmiters 10/11/2003
		private object[] SetProcessParameters( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			// TODO: Rewrite to a recursive algorithm
			object[] ret;
			string paramString = Utility.Tools.GetAttributeOrEmpty( node, "kg:Parameter", "Parameter", nsmgr );
			ret = new object[mParams.GetLength( 0 )];
			string[] @params = paramString.Split( '|' );
			//string[] arrayvals;
			int iPos = 0;
			// If the directive node is passed, it must be the first parameter
			if( mParams[0].Name.ToLower() == "nodedirective" )
			{
				ret[0] = node;
				iPos += 1;
			}

			for( int i = 0; i <= @params.GetUpperBound( 0 ); i++ )
			{
				@params[i] = @params[i].Trim();
				ret[iPos] = Utility.Tools.FixPath( @params[i].Trim(), mLocalSettings.GetBasePath( ), mDocPath, mNodeFilePaths );
				iPos += 1;
			}
			return ret;
		}


		private object[] BuildParamArray()
		{
			object[] ret = {};
			if( mParams != null )
			{
				ret = new object[mParams.GetLength( 0 )];
				for( int i = 0; i <= mParams.GetUpperBound( 0 ); i++ )
				{
					ret[i] = mParams[i].Value;
				}
			}
			return ret;
		}

		private CodeGenerationSupport.ExtObject[] GetExtObjects( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			System.Type type = Utility.Tools.GetSpecifiedType( node, this.GetType(), nsmgr, "kg:XSLTFiles", mLocalSettings.GetBasePath( ), mDocPath, mNodeFilePaths );
			if( type != null )
			{
				ExtObject[] extObjects;
				string nSpace = Utility.Tools.GetAttributeOrEmpty( node, "kg:XSLTFiles", "NamespaceURI", nsmgr );
				if( nSpace.TrimEnd().Length > 0 )
				{
					extObjects = new ExtObject[1];
					extObjects[0].NameSpaceURI = nSpace;
					extObjects[0].value = type;
					return extObjects;
				}
			}
			return null;
		}

		private string ParseFilenamePattern( string fileName, System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			string xPath;
			int iStart;
			int iEnd;
			string sEnd;
			System.Xml.XmlNode fragNode;
			while( fileName.IndexOf( "<" ) >= 0 )
			{
				iStart = fileName.IndexOf( "<" );
				iEnd = fileName.IndexOf( ">" );
				xPath = fileName.Substring( iStart + 1, iEnd - iStart - 1 );
				if( xPath.IndexOf( "<" ) >= 0 )
				{
					xPath = ParseFilenamePattern( xPath, node, nsmgr );
				}
				// *Changed to pass nsmgr 7/27/03 KD
				//System.Xml.XmlNamespaceManager nsmgr = GetNameSpaceManager( node.OwnerDocument, "dbs" )
				fragNode = node.SelectSingleNode( xPath, nsmgr );
				sEnd = fileName.Substring( iEnd + 1 );
				fileName = fileName.Substring( 0, iStart );
				if( fragNode != null )
				{
					fileName += fragNode.Value;
				}
				fileName += sEnd;
			}
			return fileName;
		}

		public void RunScript( System.Xml.XmlNode node, string filename )
		{
			System.Data.SqlClient.SqlTransaction trans;
			System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
			System.IO.StreamReader reader = null;
			Utility.FileChanged fileChanged;
			Utility.OutputType outType;
			Utility.GenType genType;
			string sProcOldCode;
			string sProcNewCode;
			string sProcName;
			//string sServerName;
			int iProcStatementPos;
			outType = this.GetOutType( node );
			genType = this.GetGenType( node );
			if( System.IO.File.Exists( filename ) )
			{
				try
				{
					reader = new System.IO.StreamReader( filename );
					string[] statements = System.Text.RegularExpressions.Regex.Split( reader.ReadToEnd(), "\\sgo\\s", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.IgnoreCase );
					iProcStatementPos = GetProcStatementPosition( statements );
					sProcName = RetrieveStoredProcName( statements, iProcStatementPos );
					sProcOldCode = RetrieveStoredProcContents( sProcName, node );
					if( sProcOldCode == null )
					{
						fileChanged = Utility.FileChanged.FileDoesntExist;
					}
					else
					{
						fileChanged = Utility.HashTools.IsTextChanged( sProcOldCode, mProjectSettings.GetCommentStart(outType ), mProjectSettings.GetCommentEnd( outType ), mProjectSettings.GetHeaderMarker( ), mProjectSettings.GetHashMarker( ));
					}
					if( ShouldRun( genType, fileChanged, System.IO.Path.GetFileName( filename ), true) )
					{
						try
						{
							mConnection.Open();
							trans = mConnection.BeginTransaction();
							cmd.Connection = mConnection;
							cmd.Transaction = trans;
							cmd.CommandType = System.Data.CommandType.Text;

							for( int i = 0; i <= statements.GetUpperBound(0); i++ )
							{
								if( i == iProcStatementPos )
								{
									sProcNewCode = Utility.HashTools.ApplyHash( statements[i], mProjectSettings.GetCommentText( genType ), mProjectSettings.GetCommentStart( outType ), mProjectSettings.GetCommentEnd( outType ), true, mProjectSettings.GetHeaderMarker(), mProjectSettings.GetHashMarker() );
									cmd.CommandText = sProcNewCode;
								}
								else
								{
									cmd.CommandText = statements[i];
								}
								cmd.ExecuteNonQuery();
							}
							trans.Commit();

						}
						catch( Exception ex )
						{
							Console.WriteLine( ex );
							throw ex;
						}
						finally
						{
							cmd.Connection.Close();
						}
					}
				}
				catch
				{
					throw;
				}
				finally
				{
					reader.Close();
				}
			}

		}

		private int GetProcStatementPosition( string[] statements )
		{
			//string sprocName;
			//string iPos;
			// TODO: Replace this with Regex that avoids finding CreateProcedure in a comment
			//       and in other ways might need to be more sophisticated
			for( int i = 0; i <= statements.GetUpperBound( 0 ); i++ )
			{
				string[] s = System.Text.RegularExpressions.Regex.Split( statements[i], @"\W+create\W+procedure\W+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.IgnoreCase );
				if( s.GetLength(0 ) > 1 )
				{
					return i;
				}
			}
			return 0;
		}

		private string RetrieveStoredProcName( string[] statements, int iProcPos )
		{
			string sprocName;
			//string iPos;
			// TODO: Replace this with Regex that avoids finding CreateProcedure in a comment
			//       and in other ways might need to be more sophisticated

			string[] s = System.Text.RegularExpressions.Regex.Split( statements[iProcPos], @"\W+create\W+procedure\W+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			if( s.GetLength(0 ) > 1 )
			{
				sprocName = s[1].Trim();
				// Separate into individual words
				string[] sWords = System.Text.RegularExpressions.Regex.Split( sprocName, @"\W+\w+\W+", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.CultureInvariant | System.Text.RegularExpressions.RegexOptions.IgnoreCase );
				if( sWords.GetLength(0 ) > 0 )
				{
					return sWords[0];
				}
			}
			return "";
		}

		private string RetrieveStoredProcContents( string procName, System.Xml.XmlNode node )
		{
			string databaseName = Utility.Tools.GetAttributeOrEmpty( node, "Database" );
			string sqlText = "SELECT * FROM INFORMATION_SCHEMA.ROUTINES  WHERE ROUTINE_CATALOG='" + databaseName + "' AND ROUTINE_TYPE='PROCEDURE' AND ROUTINE_NAME='" + procName + "'";
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter( sqlText, mConnection );
			System.Data.DataTable dt = new System.Data.DataTable();
			da.Fill( dt );
			if( dt.Rows.Count > 0 )
			{
				return dt.Rows[0]["ROUTINE_DEFINITION"].ToString();
			}
			return "";
		}

		private void CurrentOutputStart( string selectFile, string inputFile, string template, int countSelected, string outputRule )
		{
			mCurrentOutput.StartTime = DateTime.Now;
			mCurrentOutput.SelectFile = selectFile;
			mCurrentOutput.InputFile = inputFile;
			mCurrentOutput.Template = System.IO.Path.GetFileName( template );
			mCurrentOutput.CountSelected = countSelected;
		}
		private void CurrentOutputStart( string template )
		{
			mCurrentOutput.StartTime = DateTime.Now;
			mCurrentOutput.Template = System.IO.Path.GetFileName( template );
		}

		private void CurrentOutputEnd( string status, int countCreated, int countOutput )
		{
			mCurrentOutput.EndTime = DateTime.Now;
			mCurrentOutput.Status = status;
			mCurrentOutput.CountCreated = countCreated;
			mCurrentOutput.CountOutput = countOutput;
		}
		private void CurrentOutputEnd( string status )
		{
			mCurrentOutput.EndTime = DateTime.Now;
			mCurrentOutput.Status = status;
		}

		private string GetProcessName( System.Xml.XmlNode node, System.Xml.XmlNamespaceManager nsmgr )
		{
			return ( Utility.Tools.GetAttributeOrEmpty( node, "kg:Process", "TypeName", nsmgr ) + ":" + Utility.Tools.GetAttributeOrEmpty( node, "kg:Process", "MethodName", nsmgr ) );
		}

      private void CopyContainedFiles(System.IO.DirectoryInfo sourceDir , System.IO.DirectoryInfo targetDir  )
      {
         System.IO.DirectoryInfo dirTarget;
         System.IO.DirectoryInfo dirSource;
         foreach (System.IO.FileInfo f in sourceDir.GetFiles())
         {
            f.CopyTo(System.IO.Path.Combine(targetDir.FullName, f.Name));
         }
         foreach ( System.IO.DirectoryInfo d in sourceDir.GetDirectories())
         {
            dirTarget = System.IO.Directory.CreateDirectory(System.IO.Path.Combine(targetDir.FullName, d.Name));
            dirSource = new System.IO.DirectoryInfo(d.FullName);
            CopyContainedFiles(dirSource, dirTarget);
         }
      }

		#endregion
	}
}
