using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace NSGtkViewDemo
{
	public partial class NSGtkView : MonoMac.AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public NSGtkView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		public NSGtkView (RectangleF frame)
		{
			Initialize ();
			Frame = frame;
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public NSGtkView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		private Gtk.Widget widget = null;
		private GtkEmbedContainer parent = null;

		public GtkEmbedContainer GtkParent {
			get { return parent; }
			set {
				parent = value;
				if (widget != null) {
					parent.Add (widget);
					widget.Show ();
				}
			}
		}

		public Gtk.Widget Widget {
			get { return widget; }
			set {
				widget = value;
				if (parent != null) {
					parent.Add (widget);
				}
			}
		}

		void UpdateAllocation () {
			var height = widget.Screen.Height;
			RectangleF rect = Frame;
			var allocation = new Gdk.Rectangle {
				X = (int)rect.Left,
				Y = (int)rect.Top,
				Width = (int)rect.Width,
				Height = (int)rect.Height
			};

			widget.SizeAllocate (allocation);
		}

		public override void ViewWillDraw ()
		{
			UpdateAllocation ();
		}
	}
}
