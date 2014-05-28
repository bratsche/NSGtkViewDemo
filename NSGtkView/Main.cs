using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Gtk;

namespace NSGtkViewDemo
{
	class MainClass
	{
		const string LibGdk = "libgdk-quartz-2.0.dylib";
		const string LibGtk = "libgtk-quartz-2.0.dylib";

		[System.Runtime.InteropServices.DllImport (LibGdk)]
		extern static IntPtr gtk_ns_view_new (IntPtr nsview);

		[System.Runtime.InteropServices.DllImport (LibGtk)]
		extern static IntPtr gdk_quartz_window_get_nswindow (IntPtr gdkwindow);

		public static NSWindow GetWindow (Gtk.Window window)
		{
			var ptr = gdk_quartz_window_get_nswindow (window.GdkWindow.Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return MonoMac.ObjCRuntime.Runtime.GetNSObject (ptr) as NSWindow;
		}

		static Gtk.Widget NSViewToGtkWidget (object view)
		{
			var prop = view.GetType ().GetProperty ("Handle");
			var handle = prop.GetValue (view, null);

			return new Gtk.Widget (gtk_ns_view_new ((IntPtr)handle));
		}

		static void Main (string[] args)
		{
			NSWindow nswindow = null;
			NSSplitView split;

			Application.Init ();

			var window = new Window (WindowType.Toplevel);
			window.Show ();

			nswindow = GetWindow (window);

			split = new NSSplitView (nswindow.Frame);
			var nsbutton = new NSButton (split.Frame);
			nsbutton.Title = "NSButton";
			split.AddSubview (nsbutton);
			Widget gtknsview = NSViewToGtkWidget (split);

			var embed = new GtkEmbedContainer { GtkView = gtknsview };
			window.Add (embed);

			window.Show ();

			window.ShowAll (); 
			Application.Run ();
		}
	}
}
