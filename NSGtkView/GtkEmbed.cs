using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Gtk;

namespace NSGtkViewDemo
{
	public class GtkEmbed : NSView
	{
		WidgetWithNativeWindow cw;
		NSViewContainer container;

		public GtkEmbed (NSViewContainer container, Gtk.Widget w)
		{
			this.container = container;
			cw = new WidgetWithNativeWindow (this);
			cw.Add (w);
			container.Add (cw);
			cw.Show ();
		}

		public override RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;
				UpdateAllocation ();
			}
		}

		[Export ("isGtkView")]
		public bool isGtkView ()
		{
			return true;
		}

		void UpdateAllocation ()
		{
			if (container.GdkWindow == null || cw.GdkWindow == null)
				return;

			var gw = MainClass.GetView (cw.GdkWindow);
			gw.Frame = new RectangleF (0, 0, Frame.Width, Frame.Height);
			var rect = GetRelativeAllocation (MainClass.GetView (container.GdkWindow), gw);

			var allocation = new Gdk.Rectangle {
				X = (int)rect.Left,
				Y = (int)rect.Top,
				Width = (int)rect.Width,
				Height = (int)rect.Height
			};

			cw.SizeAllocate (allocation);
		}

		RectangleF GetRelativeAllocation (NSView ancestor, NSView child)
		{
			if (child == null)
				return RectangleF.Empty;
			if (child.Superview == ancestor)
				return child.Frame;
			var f = GetRelativeAllocation (ancestor, child.Superview);
			var cframe = child.Frame;
			return new RectangleF (cframe.X + f.X, cframe.Y + f.Y, cframe.Width, cframe.Height);
		}
	}
}