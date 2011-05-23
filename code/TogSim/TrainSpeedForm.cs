using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Noea.TogSim.Model;

namespace Noea.TogSim.Gui.GDI
{
    public partial class TrainSpeedForm : Form
    {
        ITrainSet _train;

        public TrainSpeedForm(ITrainSet train)
        {
            InitializeComponent();
            _train = train;
        }

        private void TrainSpeedForm_Load(object sender, EventArgs e)
        {
            tbSpeed.Text = "" + _train.RequestedSpeed;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                _train.RequestedSpeed = Convert.ToDouble(tbSpeed.Text);
                this.Close();
            }
            catch
            {
            }
        }
    }
}