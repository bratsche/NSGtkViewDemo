using System;
using System.Collections.Generic;
using Gtk;

namespace NSGtkViewDemo
{
	public class GtkEmbedContainer : Container
	{
		public Widget GtkView { get; set; }
		List<Widget> children = new List<Widget> ();

		public GtkEmbedContainer ()
		{
			// Mono.TextEditor.GtkWorkarounds.FixContainerLeak (this);
		}

		public override GLib.GType ChildType ()
		{
			return Widget.GType;
		}

		public void AddChild (Widget widget)
		{
			widget.Parent = this;
			children.Add (widget);
		}

		public void RemoveChild (Widget widget)
		{
			widget.Unparent ();
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