using System.Windows.Forms;

namespace MoviePoster.Utilities
{
    class FullScreen
    {
        Form1 TargetForm;
        FormWindowState PreviousWindowState;

        public FullScreen(Form1 tf)
        {
            TargetForm = tf;
            TargetForm.KeyPreview = true;
            TargetForm.KeyDown += TargetForm_KeyDown;
        }

        private void TargetForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                Toggle();
            }

            if (e.KeyData == Keys.T)
            {
                TargetForm.ToggleMenu();
            }

            if (e.KeyData == Keys.S)
            {
                TargetForm.ShowSettings();
            }
        }

        private void TargetForm_T(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.S)
            {
                TargetForm.ToggleMenu();
            }
        }

        public void Toggle()
        {
            if (TargetForm.WindowState == FormWindowState.Maximized)
            {
                Leave();
            }
            else
            {
                Enter();
            }
        }

        public void Enter()
        {
            if (TargetForm.WindowState != FormWindowState.Maximized)
            {
                PreviousWindowState = TargetForm.WindowState;
                TargetForm.WindowState = FormWindowState.Normal;
                TargetForm.FormBorderStyle = FormBorderStyle.None;
                TargetForm.WindowState = FormWindowState.Maximized;
            }
        }

        public void Leave()
        {
            TargetForm.FormBorderStyle = FormBorderStyle.Sizable;
            TargetForm.WindowState = PreviousWindowState;
        }
    }
}