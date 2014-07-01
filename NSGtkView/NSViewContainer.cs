using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Gtk;

namespace NSGtkViewDemo
{
	public class NSViewContainer : Bin
	{
		//List<Widget> children = new List<Widget> ();

		public NSViewContainer ()
		{
			WidgetFlags |= Gtk.WidgetFlags.NoWindow;
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);
			if (nsviewChild != null)
				nsviewChild.SetFrameSize (new System.Drawing.SizeF (allocation.Width, allocation.Height));
		}

		public void AddNSChild (NSView nsview)
		{
			nsviewChild = nsview;
			NSView.AddSubview (nsview);
		}

		/*
		protected override void OnAdded (Widget widget)
		{
			widget.Parent = this;
			children.Add (widget);
		}

		protected override void OnRemoved (Widget widget)
		{
			children.Remove (widget);
		}

		protected override void ForAll (bool include_internals, Callback cb)
		{
			foreach (Widget w in children)
				cb (w);
		}
		*/

		/// <summary>
		/// Creates an NSView that embeds the provided GTK widget.
		/// This view can be added to any NSView inside this container
		/// </summary>
		public NSView CreateEmbedView (Gtk.Widget widget)
		{
			embedView = new GtkEmbed (this, widget);

			widget.FocusInEvent += (o, args) => {
				NSView.Window.MakeFirstResponder (embedView);
			};

			return embedView;
		}

		/// <summary>
		/// The root NSView for this widget
		/// </summary>
		public NSView NSView {
			get { return MainClass.GetView (GdkWindow); }
		}

		public NSView NSChild {
			get { return nsviewChild; }
		}

		private NSView embedView;
		private NSView nsviewChild;
	}
}