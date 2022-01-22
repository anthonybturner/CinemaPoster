using System.Windows.Forms;

namespace CinemaPosterApp.Utilities
{
    class FullScreen
    {
        CinemaForm TargetForm;
        FormWindowState PreviousWindowState;

        public FullScreen(CinemaForm tf)
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


            if (e.KeyData == Keys.S)
            {
                TargetForm.ShowSettings();
            }

            if (e.KeyData == Keys.N && !TargetForm.NowPlaying.IsPlaying())
            {
               //TargetForm.GetPosterAsync();
            }
        }

        private void TargetForm_T(object sender, KeyEventArgs e)
        {
           
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