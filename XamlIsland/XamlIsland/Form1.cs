using Microsoft.Toolkit.Forms.UI.XamlHost;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XamlIsland
{
    public partial class Form1 : Form
    {
        private WindowsXamlHost inkCanvasHost;
        private WindowsXamlHost inkToolbarHost;
        private Windows.UI.Xaml.Controls.InkCanvas inkCanvas;
        private Windows.UI.Xaml.Controls.InkToolbar inkToolbar;
        public Form1()
        {
            InitializeComponent();

            // InkCanvas
            inkCanvasHost = new WindowsXamlHost();
            inkCanvasHost.InitialTypeName = "Windows.UI.Xaml.Controls.InkCanvas";
            inkCanvasHost.ChildChanged += InkCanvas_ChildChanged;
            // InkToolbar
            inkToolbarHost = new WindowsXamlHost();
            inkToolbarHost.InitialTypeName = "Windows.UI.Xaml.Controls.InkToolbar";
            inkToolbarHost.ChildChanged += InkToolbar_ChildChanged;
            // Layout
            inkToolbarHost.Top = 0;
            inkToolbarHost.Left = 0;
            inkToolbarHost.Height = 50;
            inkToolbarHost.Dock = DockStyle.Top;

            inkCanvasHost.Dock = DockStyle.Fill;
            // Add to Window
            Controls.Add(inkToolbarHost);
            Controls.Add(inkCanvasHost);
        }

        private void InkToolbar_ChildChanged(object sender, EventArgs e)
        {
            inkToolbar = ((WindowsXamlHost)sender).Child as Windows.UI.Xaml.Controls.InkToolbar;
            InitializeUwpControls();
        }

        private void InkCanvas_ChildChanged(object sender, EventArgs e)
        {
            inkCanvas = ((WindowsXamlHost)sender).Child as Windows.UI.Xaml.Controls.InkCanvas;
            InitializeUwpControls();
        }

        private void InitializeUwpControls()
        {
            if (inkToolbar == null || inkCanvas == null)
            {
                return;
            }

            inkToolbar.TargetInkCanvas = inkCanvas;
        }
    }
}
