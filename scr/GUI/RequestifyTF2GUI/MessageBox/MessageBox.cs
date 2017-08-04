using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RequestifyTF2GUI.MessageBox
{
    class MessageBox
    {
        public void Show(string message,string title, Sounds sound=Sounds.None)
        {
            var msgbox = new RequestifyTF2Forms.MessageBox
            {
                MessageText = message,
                Text = title,
                Color = "#F44336"
            };


          //  msgbox.WindowState = FormWindowState.Minimized;
            msgbox.Show();
            msgbox.BringToFront();
            msgbox.Activate();
            msgbox.Focus();
         //   msgbox.WindowState = FormWindowState.Normal;

            switch (sound)
            {
                case Sounds.None:
                    break;
                case Sounds.Asterik:
                    SystemSounds.Asterisk.Play();
                    break;
                case Sounds.Beep:
                    SystemSounds.Beep.Play();
                    break;
                case Sounds.Exclamation:
                    SystemSounds.Exclamation.Play();
                    break;
                case Sounds.Hand:
                    SystemSounds.Hand.Play();
                    break;
                case Sounds.Question:
                    SystemSounds.Question.Play();
                    break;
              
            }
           



        }

        internal enum Sounds
        {
            Asterik,
            Beep,
            Exclamation,
            Hand,
            Question,
            None
        }

    }
}
