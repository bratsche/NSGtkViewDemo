using System;
using System.Collections.Generic;
using Gtk;

namespace NSGtkViewDemo
{
	public class GtkEmbedContainer : Container
	{
		Widget gtknsview = null;

		public Widget GtkView {
			get { return gtknsview; }
			set {
				gtknsview = value;
				gtknsview.Parent = this;
			}
		}
		List<Widget> children = new List<Widget> ();

		public GtkEmbedContainer ()
		{
			//WidgetFlags |= Gtk.WidgetFlags.NoWindow;
			// Mono.TextEditor.GtkWorkarounds.FixContainerLeak (this);
		}

		protected override void OnRealized ()
		{
			WidgetFlags |= WidgetFlags.Realized;

			var attributes = new Gdk.WindowAttr {
				WindowType = Gdk.WindowType.Child,
				Wclass = Gdk.WindowClass.InputOutput,
				EventMask = (int)Gdk.EventMask.AllEventsMask,
			};

			GdkWindow = new Gdk.Window (GtkView.GdkWindow, attributes, 0);
			GdkWindow.UserData = Handle;
			GdkWindow.Background = Style.Background (State);
			Style.Attach (GdkWindow);
		}

		public override GLib.GType ChildType ()
		{
			return Widget.GType;
		}

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
			cb (GtkView);

			foreach (Widget w in children)
				cb (w);
		}

		protected override void OnSizeAllocated (Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated (allocation);

			if (GtkView != null && GtkView.Visible) {
				var childRequisition = GtkView.ChildRequisition;
				var childAllocation = new Gdk.Rectangle {
					X = allocation.X,
					Y = allocation.Y,
					Width = (int)(allocation.Width - BorderWidth * 2),
					Height = (int)(allocation.Height - BorderWidth * 2)
				};

				GtkView.SizeAllocate (childAllocation);
			}
		}
	}
}