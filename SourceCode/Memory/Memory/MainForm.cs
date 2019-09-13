using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Memory
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnIDisposable_Click(object sender, EventArgs e)
        {
            // 1. effect of the "using" statement <-> try/finally 
            using (DisposableWrapper dw1 = new DisposableWrapper(100))
            {
                Debug.WriteLine("do something with DisposableWrapper...");
            }

            DisposableWrapper dw2 = new DisposableWrapper(200);
            try
            {
                Debug.WriteLine("do something with DisposableWrapper...");
            }
            finally
            {
                if (dw2 != null)
                    dw2.Dispose();
            }
            // TODO: look at the generated code in decompiler

            // 2. without the "using" statement
            DisposableWrapper never = new DisposableWrapper(300);
            GC.Collect();   // the finalizer is not called here!

            // difference between RELEASE and DEBUG optimizations
            GC.WaitForPendingFinalizers();  // should be called here (but not in DEBUG)

            GC.Collect();  // (even here in DEBUG!)
            // but should be finalized sometime before the application exits...
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            MirrorWindow window = new MirrorWindow();
            window.Init(tbSource);
            window.Show();
        }

        private void btnFullCollect_Click(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
