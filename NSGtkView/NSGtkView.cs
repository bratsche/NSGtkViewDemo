using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace NSGtkViewDemo
{
	[StructLayout (LayoutKind.Sequential)]
	struct NativeEventButtonStruct {
		public Gdk.EventType type;
		public IntPtr window;
		public sbyte send_event;
		public uint time;
		public double x;
		public double y;
		public IntPtr axes;
		public uint state;
		public uint button;
		public IntPtr device;
		public double x_root;
		public double y_root;
	}

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

		public NSView Toplevel { get; set; }

		public GtkEmbedContainer GtkParent {
			get { return parent; }
			set {
				parent = value;
				if (widget != null) {
					parent.Add (widget);
//					widget.Show ();
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

		/*
		public override void MouseMoved (NSEvent nsevt)
		{
			Console.WriteLine ("MoveMoved");
		}
		*/

//		public override void MouseDown (NSEvent nsevt)
//		{
//			Console.WriteLine ("MouseDown");
//
//			/*
//			var evtStruct = new NativeEventButtonStruct {
//				type = Gdk.EventType.ButtonPress,
//				send_event = 1,
//				window = Widget.GdkWindow.Handle,
//				x = nsevt.AbsoluteX,
//				y = nsevt.AbsoluteY,
//				button = (uint)nsevt.ButtonNumber,
//				device = IntPtr.Zero
//			};
//
//			IntPtr ptr = GLib.Marshaller.StructureToPtrAlloc (evtStruct);
//			try {
//				Gdk.EventButton evt = new Gdk.EventButton (ptr);
//				Gdk.EventHelper.Put (evt);
//			} finally {
//				Marshal.FreeHGlobal (ptr);
//			}
//			*/
//		}

//		public override void MouseUp (NSEvent nsevt)
//		{
//			Console.WriteLine ("MouseUp");
//		}

		public override bool AcceptsFirstResponder ()
		{
			//Console.WriteLine ("AcceptsFirstResponder");
			return false;
		}

		public override bool BecomeFirstResponder ()
		{
			//Console.WriteLine ("BecomeFirstResponder");
			return false;
		}

		public override NSView HitTest (PointF pt)
		{
			if (Frame.Contains (pt)) {
				Console.WriteLine ("HitTest [{0},{1}]", pt.X, pt.Y);
				return this;
			} else {
				Console.WriteLine ("Does NOT contain pt"); 
				return base.HitTest (pt);
			}
		}
	}
}
