// ====================================================================
//  Copyright ©2004, Kathleen Dollard, All Rights Reserved.
// ====================================================================
//   I'm distributing this code so you'll be able to use it to see code
//   generation in action and I hope it will be useful and you'll enjoy 
//   using it. This code is provided "AS IS" without warranty, either 
//   expressed or implied, including implied warranties of merchantability 
//   and/or fitness for a particular purpose. 
//  ====================================================================
//  Summary: Home for utility methods

using System;

namespace KADGen.WinSupport
{
	public class Utility
	{
		public const int border = 6;
		public const int margin = 4;
		public const string vbcrlf = Microsoft.VisualBasic.ControlChars.CrLf;

		public static string SubstringBefore(
			string s,
			string sBefore)
		{
			int iPos = s.IndexOf(sBefore);
			if( iPos == -1 )
			{
				return s;
			}
			else
			{
				return s.Substring(0 , iPos);
			}
		}

		public static System.Windows.Forms.Form ParentForm(
			System.Windows.Forms.Control ctl)
		{
			while( ! (ctl is System.Windows.Forms.Form ))
			{
				ctl = ctl.Parent;
			}
			if( ctl != null )
			{
				return ( System.Windows.Forms.Form ) ctl;
			}
			else
			{
				return null;
			}
		}

		public static System.Windows.Forms.Form ParentForm(
			System.Windows.Forms.MenuItem ctl)
		{
			return ctl.Parent.GetMainMenu().GetForm();
		}

		public static  System.Windows.Forms.Form ParentForm(
			object sender)
		{     
			if( sender is System.Windows.Forms.MenuItem )
			{
				return Utility.ParentForm((System.Windows.Forms.MenuItem) sender );
			}
			else
			{
				return Utility.ParentForm(( System.Windows.Forms.Control ) sender);
			}
		}


		public static int Position(
			System.Drawing.Graphics graphics,
			System.Windows.Forms.Control ctl,
			int iTop)
		{         
			const char c  = 'K';
			System.Drawing.SizeF sizef;
			System.Drawing.SizeF sizefText;
			System.Drawing.SizeF sizefSpace;
			System.Drawing.SizeF sizeFNew;
			int chars = 0;
			int cnt = 0;
			bool noResize = false;
			bool multiline = false;
			int iLines = 0;
			int iChars = 0;

			ctl.Top = iTop;

			if( ctl is System.Windows.Forms.DataGrid )
			{
				noResize = true;
			}
			else if ( ctl is System.Windows.Forms.TextBox )
			{
				chars = (( System.Windows.Forms.TextBox ) ctl).MaxLength;
				multiline = (( System.Windows.Forms.TextBox ) ctl).Multiline;
			}
			else if ( ctl is System.Windows.Forms.ComboBox )
			{
				chars = (( System.Windows.Forms.ComboBox ) ctl).MaxLength;
			}

			if( chars == 0 )
			{
				cnt = 1;
			}
			else
			{
				cnt = chars;
			}

			if( ! noResize )
			{
				sizefText = graphics.MeasureString(ctl.Text , ctl.Font);
				sizef = graphics.MeasureString(new string(c , cnt) , ctl.Font);
				sizefSpace = new System.Drawing.SizeF(ctl.Parent.Size.Width - ctl.Left - border ,
					ctl.Parent.Size.Height - ctl.Top - border);

				if( sizefText.Width > sizef.Width )
				{
					sizef.Width = sizefText.Width;
				}

				if( chars > 32000 )
				{
					sizeFNew = new System.Drawing.SizeF(ctl.Parent.Width - ctl.Left , sizef.Height + 6);
				}
				else
				{
					sizeFNew = sizef;
				}

				if( multiline )
				{
					sizeFNew = new System.Drawing.SizeF(sizeFNew.Width , sizefSpace.Height);
				}

				ctl.Size = new System.Drawing.Size(System.Convert.ToInt32( sizeFNew.Width ) , System.Convert.ToInt32( sizeFNew.Height ));
			}
			iTop = ctl.Bottom + margin;
			return iTop;

		}

		public static int Position(
			System.Drawing.Graphics graphics,
			System.Windows.Forms.Label lbl,
			System.Windows.Forms.Control ctl,
			int labelWidth,
			int iTop)
		{          
			int ret ;
			lbl.Width = labelWidth;
			ctl.Left = lbl.Right + margin;
			ret = Position(graphics , ctl , iTop);
			Position(graphics , lbl , iTop);
			return ret;
		}

		public static bool IsEmpty( object val ) 
		{
			if ( val is short )
			{
				return (short)val  == 0;
			}
			else if ( val is int )
			{
				return (int)val == 0;
			}
			else if ( val is long )
			{
				return (long)val  == 0;
			}
			else if  ( val is double )
			{
				return (double)val  == 0;
			}
			else if ( val is float )
			{
				return (float)val  == 0;
			}
			else if ( val is string )
			{
				return (string) val == "";
			}
			else if ( val is Guid )
			{
				return ((Guid) val == Guid.Empty);
			}
			else if ( val is DateTime )
			{
				return ( DateTime ) val  == System.DateTime.MinValue;
			}
			else if ( val is Byte )
			{
				return ((Byte) val == 0);
			}
			else
			{
				return false;
			}
		}


		public static void BindField(
			System.Windows.Forms.Control control,
			string propertyName,
			object dataSource,
			string dataMember)
		{
			System.Windows.Forms.Binding bd ;

			for( int index = control.DataBindings.Count - 1 ; index >= 0 ; index-- )
			{
				bd = control.DataBindings[index];
				if( bd.PropertyName == propertyName )
				{
					control.DataBindings.Remove(bd);
				}
			}
			control.DataBindings.Add(propertyName , dataSource , dataMember);
		}

		public static void BindEvents(
			System.Windows.Forms.Control control,
			string controlProperty,
			object source,
			string sourceProperty,
			System.Windows.Forms.ConvertEventHandler delegFormat,
			System.Windows.Forms.ConvertEventHandler delegParse)
		{
			System.Windows.Forms.Binding binding = control.DataBindings[controlProperty];

			if( binding != null )
			{
				control.DataBindings.Remove(binding);
			}

			binding = new System.Windows.Forms.Binding(controlProperty , source , sourceProperty);

			if( delegFormat != null )
			{
				binding.Format += new System.Windows.Forms.ConvertEventHandler(delegFormat);
			}

			if( delegParse != null )
			{
				binding.Parse += new System.Windows.Forms.ConvertEventHandler(delegParse);
			}

			control.DataBindings.Add(binding);

		}

		public static object InvokeSharedMethod(
			System.Type type,
			string method,
			params object[] parms)
		{
			while( type.GetMember(method).GetLength(0) == 0 )
			{
				type = type.BaseType;
				if( type == typeof(object) )
				{
					type = null;
					break;
				}
			}
			return type.InvokeMember(method ,
				System.Reflection.BindingFlags.InvokeMethod ,
				null , null , parms);
		}

		public static object InvokeInstanceMethod(
			System.Type type,
			string method,
			object obj,
			params object[] parms)
		{
			return type.InvokeMember(method ,
				System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod ,
				null , obj , parms);
		}

		public static object InvokeSharedPropertyGet(
			System.Type type,
			string method,
			params object[] parms)
			// Static properties are a bit tricky to retrieve. 
			// This is an explicit walk of the hierarchy looking 
			// for the method, assuming if its found the signature
			// is right. KAD 1/22/04
	{
			while( type.GetMember(method).GetLength(0) == 0 )
			{
				type = type.BaseType;
				if( type == typeof(Object) )
				{
					type = null;
					break;
				}
			}
			return type.InvokeMember(method ,
				System.Reflection.BindingFlags.GetProperty ,
				null , null , parms);
		}

		public static object CreateInstance(
			System.Type type,
			params object[] parms)
		{
			return Activator.CreateInstance(type , parms);
		}

		public static int GetWidth(
			string stringToMeasure,
			System.Drawing.Font font,
			System.Drawing.Graphics graphics) 
		{
			return System.Convert.ToInt32(graphics.MeasureString(stringToMeasure , font).Width);
		}

	}
}