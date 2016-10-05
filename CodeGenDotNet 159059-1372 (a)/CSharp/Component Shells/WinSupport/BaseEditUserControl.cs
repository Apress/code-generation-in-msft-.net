// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Base class for WinForms editing user controls


using System;

namespace KADGen.WinSupport
{
	public class BaseEditUserControl : System.Windows.Forms.UserControl
	{

		// Implements IEditUserControl

		public delegate void DataChangedHandler(
			object sender,
			EventArgs e );
		public event DataChangedHandler DataChanged;

		#region Class Level Declaration
		private BaseSelectUserControl mSelectUserControl;
		protected BusinessObjectSupport.IBusinessObject mBusinessObject;
		protected int mMargin = 5;
		protected bool mbIsDirty;
		protected string mCaption;
		// protected mMinimumSize System.Drawing.Size
		#endregion

		#region  Windows Form Designer generated code 

		public BaseEditUserControl() : base()
		{

			//This call is required by the Windows Form Designer.
			InitializeComponent();

			//Add any initialization after the InitializeComponent() call

		}

		//Form overrides dispose to clean up the component list.
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Required by the Windows Form Designer
		private System.ComponentModel.IContainer components;

		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		protected internal System.Windows.Forms.ErrorProvider errProvider;
		protected internal System.Windows.Forms.ToolTip toolTip;
		[System.Diagnostics.DebuggerStepThrough()] private void InitializeComponent()
		{
			this.errProvider = new System.Windows.Forms.ErrorProvider();
			this.toolTip = new System.Windows.Forms.ToolTip();
			//
			//errProvider
			//
			this.errProvider.ContainerControl = this;
			//
			//toolTip
			//
			//
			//BaseEditUserControl
			//
			this.Name = "BaseEditUserControl";

		}

		#endregion

		#region Public Properties and Methods
		public BaseSelectUserControl SelectUserControl
		{
			get
			{
				return mSelectUserControl;
			}
		}

		public virtual CSLA.ReadOnlyCollectionBase GetList() 
		{
			throw new System.ApplicationException("The GetList method must be overridden");
		}

		public void VisitControls()
		{
			VisitControls(this);
		}
		public void VisitControls( System.Windows.Forms.Control ctl )
		{
			//Visit every control on the form to set error provider
			//By doing this as a complete first step , all error flags are set
			foreach( System.Windows.Forms.Control childCtl in ctl.Controls )
			{
				childCtl.Focus();
				if( childCtl.Controls.Count > 0 )
				{
					VisitControls(childCtl);
				}
			}
		}

		public bool IsFormValid() 
		{
			return IsFormValid(this , this.errProvider);
		}
		public bool IsFormValid(
			System.Windows.Forms.Control ctl ,
			System.Windows.Forms.ErrorProvider errProv ) 
		{
			bool bValid = true;
			foreach( System.Windows.Forms.Control childCtl in ctl.Controls )
			{
				//childCtrl.Focus()
				if( errProv.GetError(childCtl) != "" )
				{
					bValid = false;
					break;
				}
				if( childCtl.Controls.Count > 0 )
				{
					bValid = IsFormValid(childCtl , errProv);
					if( ! bValid )
					{
						break;
					}
				}
			}
			return bValid;
		}

		#endregion

		#region Protected Properties and Methods
		protected virtual void ResizeUC()
		{
			int maxHeight = 0;
			foreach( System.Windows.Forms.Control ctl in this.Controls )
			{
				if( ctl.Visible )
				{
					if( ctl.Bottom > maxHeight )
					{
						maxHeight = ctl.Bottom;
					}
				}
			}
			this.ClientSize = new System.Drawing.Size(this.ClientSize.Width , maxHeight + mMargin);
		}

		protected virtual void OnDataChanged( object sender, EventArgs e )
		{
			DataChanged(sender , e);
		}

		protected void RemoveControl(
			System.Windows.Forms.Control ctl,
			string sFieldName )
		{
			int height = 0;
			if( sFieldName.Trim().Length > 0 )
			{
				if( ctl.Name.Length > 3 )
				{
					if( ctl.Name.Substring(3) == sFieldName )
					{
						// remove later controls and exit               
						ctl.Visible = false;
						if( ctl.Name.Substring(0 , 3).ToLower() != "lbl" )
						{
							height = ctl.Height + ((IEditUserControl) this).VerticalMargin;
							foreach( System.Windows.Forms.Control ctlOther in ctl.Parent.Controls )
							{
								if( ctlOther.Top > ctl.Bottom )
								{
									ctlOther.Top -= height;
								}
							}
						}
					}
				}
				foreach( System.Windows.Forms.Control ctlChild in ctl.Controls )
				{
					RemoveControl(ctlChild , sFieldName);
				}
			}
		}

		protected virtual void BindField(
			System.Windows.Forms.Control control ,
			string propertyName ,
			object dataSource ,
			string  dataMember,
			System.Type netType )
		{

			System.Windows.Forms.Binding bd;

			for( int index = control.DataBindings.Count - 1; index >=0 ; index-- )
			{
				bd = control.DataBindings[index];
				if( bd.PropertyName == propertyName )
				{
					control.DataBindings.Remove(bd);
				}
			}
			control.DataBindings.Add(propertyName , dataSource , dataMember);

			BindEvents(control , propertyName , dataSource , dataMember , netType);

		}

		protected virtual void BindEvents(
			System.Windows.Forms.Control control,
			string controlProperty ,
			object source ,
			string sourceProperty ,
			System.Type netType )
		{
			if( netType == typeof(System.Decimal) )
			{
				WinSupport.Utility.BindEvents(control , controlProperty , source , sourceProperty ,
					new System.Windows.Forms.ConvertEventHandler(FormatCurrency) , 
					new System.Windows.Forms.ConvertEventHandler(ParseCurrency));
			}
		}


		#region Format Event Handlers
		protected virtual void FormatCurrency(
			object sender ,
			System.Windows.Forms.ConvertEventArgs e )
		{
			e.Value = ((System.Decimal) e.Value).ToString("C");
		}

		protected virtual void ParseCurrency(
			object sender ,
			System.Windows.Forms.ConvertEventArgs e )
		{
			string val = e.Value.ToString();
			val = val.Replace("$" , "");
			e.Value = System.Decimal.Parse(val);
		}

		#endregion

		public string Caption
		{
			get
			{
				return mCaption;
			}
		}

		#endregion

	}
}