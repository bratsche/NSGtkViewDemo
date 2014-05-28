using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace NSGtkView
{
	public partial class NSGtkView : MonoMac.AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public NSGtkView (IntPtr handle) : base (handle)
		{
			Initialize ();
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

		private Gtk.Widget widget;
		private Gtk.Container parent;

		public Gtk.Container Parent {
			get { return parent; }
			set {
				parent = value;
				if (widget != null) {
					parent.Add (widget);
				}
			}
		}

		Gtk.Widget Widget {
			get { return widget; }
			set {
				widget = value;
				if (Parent != null) {
					Parent.Add (widget);
				}

				widget.Realized += (sender, e) => {
					UpdateAllocation ();
					widget.QueueDraw ();
				};
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
