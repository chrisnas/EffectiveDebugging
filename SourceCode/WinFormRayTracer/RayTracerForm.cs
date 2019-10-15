using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormRayTracer
{
    public delegate void Action<T, U, V>(T t, U u, V v);

    public partial class RayTracerForm : Form
    {
        Bitmap bitmap;
        PictureBox pictureBox;
        const int width = 600;
        const int height = 600;


        public RayTracerForm()
        {
            InitializeComponent();

            bitmap = new Bitmap(width, height);

            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = bitmap;

            ClientSize = new System.Drawing.Size(width, height + 24);
            Controls.Add(pictureBox);
            Text = "Ray Tracer";
            Load += RayTracerForm_Load;
        }

        private void RayTracerForm_Load(object sender, System.EventArgs e)
        {
            // just to be able to see the scene rendering live
            this.Show();

            RayTracer rayTracer = new RayTracer(width, height, (int x, int y, System.Drawing.Color color) =>
            {
                bitmap.SetPixel(x, y, color);
                if (x == 0) pictureBox.Refresh();
            });

            //var clock = Stopwatch.StartNew();
            rayTracer.Render(rayTracer.DefaultScene);
            pictureBox.Invalidate();
            //MessageBox.Show($"Rendering in {clock.ElapsedMilliseconds} ms", "");
        }
    }
}
