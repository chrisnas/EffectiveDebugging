using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DebugMe
{
    public partial class SpaceInvader : Form
    {
        private Point _position;

        public SpaceInvader()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            ButtonStart.Visible = false;

            using (var graphics = CreateGraphics())
            {
                graphics.DrawPolygon(Pens.Black, new[] { new PointF(3, 3), new PointF(3, 3.5f), new PointF(4, 3) });
            }
        }

        private void Render()
        {

        }
    }
}
