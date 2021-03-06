﻿using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Gtk;

namespace NSGtkViewDemo
{
	public class NSViewContainer : Bin
	{
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

		/// <summary>
		/// Creates an NSView that embeds the provided GTK widget.
		/// This view can be added to any NSView inside this container
		/// </summary>
		public NSView CreateEmbedView (Gtk.Widget widget)
		{
			widget.SizeRequest ();
			embedView = new GtkEmbed (this, widget);

			WatchForFocus (widget);

			return embedView;
		}

		private void WatchForFocus (Widget widget)
		{
			widget.FocusInEvent += (o, args) => {
				NSView.Window.MakeFirstResponder (embedView);
			};

			if (widget is Container) {
				Container c = (Container)widget;

				foreach (Widget w in c.Children) {
					WatchForFocus (w);
				}
			}
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