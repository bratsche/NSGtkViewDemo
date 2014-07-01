using System;
using System.Drawing;
using Gtk;

namespace NSGtkViewDemo
{
	public class WidgetWithNativeWindow: Gtk.EventBox
	{
		GtkEmbed embedParent;

		public WidgetWithNativeWindow (GtkEmbed embed)
		{
			embedParent = embed;
		}

		protected override void OnRealized ()
		{
			Console.WriteLine ("Realizing");
			WidgetFlags |= WidgetFlags.Realized;

			Console.WriteLine ("WidgetWithNativeWindow: {0}", Allocation);

			Gdk.WindowAttr attributes = new Gdk.WindowAttr ();
			attributes.X = Allocation.X;
			attributes.Y = Allocation.Y;
			attributes.Height = Allocation.Height;
			attributes.Width = Allocation.Width;
			attributes.WindowType = Gdk.WindowType.Child;
			attributes.Wclass = Gdk.WindowClass.InputOutput;
			attributes.Visual = Visual;
			attributes.TypeHint = (Gdk.WindowTypeHint)100; // Custom value which means the this gdk window has to use a native window
			attributes.Colormap = Colormap;
			attributes.EventMask = (int)(Events |
				Gdk.EventMask.ExposureMask |
				Gdk.EventMask.Button1MotionMask |
				Gdk.EventMask.ButtonPressMask |
				Gdk.EventMask.ButtonReleaseMask |
				Gdk.EventMask.KeyPressMask |
				Gdk.EventMask.KeyReleaseMask);

			Gdk.WindowAttributesType attributes_mask =
				Gdk.WindowAttributesType.X |
				Gdk.WindowAttributesType.Y |
				Gdk.WindowAttributesType.Colormap |
				Gdk.WindowAttributesType.Visual;
			GdkWindow = new Gdk.Window (ParentWindow, attributes, (int)attributes_mask);
			GdkWindow.UserData = Handle;

			Style = Style.Attach (GdkWindow);
			Style.SetBackground (GdkWindow, State);
			this.WidgetFlags &= ~WidgetFlags.NoWindow;

			// Remove the gdk window from the original location and move it to the GtkEmbed view that contains it
			var gw = MainClass.GetView (GdkWindow);
			gw.RemoveFromSuperview ();
			embedParent.AddSubview (gw);
			gw.Frame = new RectangleF (0, 0, embedParent.Frame.Width, embedParent.Frame.Height);
		}
	}
}