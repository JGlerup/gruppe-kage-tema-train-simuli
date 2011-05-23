using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Noea.TogSim.Gui.GDI
{
    public partial class StoppingBox : Form
    {
        public ProgressBar Progress { get { return progressBar1; } }
        public StoppingBox()
        {
            InitializeComponent();
        }
    }
}