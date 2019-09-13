using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Memory
{
    public partial class MirrorWindow : Form
    {
        private byte[] _workload = new byte[1024 * 1024];

        public MirrorWindow()
        {
            InitializeComponent();
        }

        public void Init(TextBox textBox)
        {
            textBox.TextChanged += TextBox_TextChanged;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in ((TextBox)sender).Text.Reverse())
            {
                builder.Append(item);
            }
            tbMirror.Text = builder.ToString();
        }
    }
}
