using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoviePoster
{
    public partial class Settings : Form
    {
        private Form1 mainForm;
        public Settings(Form1 f)
        {
            InitializeComponent();
            mainForm = f;
            numericUpDown1.Value = mainForm.MaxMovies;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
            {
                mainForm.MaxMovies = ((int)numericUpDown1.Value);
                mainForm.RestartPosters();
            }
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
