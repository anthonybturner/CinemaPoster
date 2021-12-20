using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CinemaPosterApp
{
    public partial class Settings : Form
    {
        private CinemaForm mainForm;
        public Settings(CinemaForm f)
        {
            InitializeComponent();
            mainForm = f;
            numericUpDown1.Value = mainForm.MAX_MOVIES;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
            {
                mainForm.MAX_MOVIES = ((int)numericUpDown1.Value);
                mainForm.RestartPosters();
            }
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
