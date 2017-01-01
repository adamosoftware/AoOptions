using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamOneilSoftware
{
	public class FormPosition
	{
		public Point Location { get; set; }
		public Size Size { get; set; }
		public FormWindowState WindowState { get; set; }

		public static FormPosition FromForm(Form form)
		{
			return new FormPosition() { Location = form.Location, Size = form.Size, WindowState = form.WindowState };
		}

	}
}
