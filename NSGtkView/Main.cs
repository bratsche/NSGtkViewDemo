using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Gtk;
using System.Collections.Generic;

namespace NSGtkViewDemo
{
	class MainClass
	{
		const string LibGtk = "libgtk-quartz-2.0.dylib";

		[System.Runtime.InteropServices.DllImport (LibGtk)]
		extern static IntPtr gdk_quartz_window_get_nswindow (IntPtr gdkwindow);

		[System.Runtime.InteropServices.DllImport (LibGtk)]
		extern static IntPtr gdk_quartz_window_get_nsview (IntPtr gdkwindow);

		public static NSWindow GetWindow (Gtk.Window window)
		{
			var ptr = gdk_quartz_window_get_nswindow (window.GdkWindow.Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return MonoMac.ObjCRuntime.Runtime.GetNSObject (ptr) as NSWindow;
		}

		public static NSView GetView (Gdk.Window window)
		{
			var ptr = gdk_quartz_window_get_nsview (window.Handle);
			if (ptr == IntPtr.Zero)
				return null;
			return MonoMac.ObjCRuntime.Runtime.GetNSObject (ptr) as NSView;
		}

		static void Main (string[] args)
		{
			NSApplication.Init ();
			Application.Init ();

			// Create the main window
			var window = new Window (WindowType.Toplevel);
//			window.DefaultWidth = 500;
//			window.DefaultHeight = 500;

			// Add the root container
			var viewContainer = new NSViewContainer ();
			window.Add (viewContainer);
			window.ShowAll ();

			// Add a split view to the container
			NSSplitView sp = new NSSplitView (new RectangleF (0, 0, window.Allocation.Width, window.Allocation.Height));

			// The first element of the split view is a mac button
			sp.AddSubview (new NSButton (new RectangleF (0, 0, 60, 60)) { Title = "Mac Button"} );
			sp.AddSubview (new NSTextField (new RectangleF (0, 0, 60, 60)));
			viewContainer.AddNSChild (sp);

			var btn = new Button ("Gtk Button");
			var entry = new Entry ();

			var vbox = new VBox ();
			vbox.PackStart (btn);
			vbox.PackStart (entry);
			vbox.ShowAll ();

			// Create an embed view for the gtk button, and add it to the split
			var gtkView = viewContainer.CreateEmbedView (vbox);
			sp.AddSubview (gtkView);
			gtkView.SetFrameSize (new SizeF (50, 50));

			Application.Run ();
		}
	}
}