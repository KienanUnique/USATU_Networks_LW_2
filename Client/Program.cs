using System;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        private const int CountOfClientsFormsToOpen = 3;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormClient());
            var forms = new Form[CountOfClientsFormsToOpen];
            for (int i = 0; i < CountOfClientsFormsToOpen; i++)
            {
                forms[i] = new FormClient();
            }
            Application.Run(new MultiFormContext(forms));
        }
    }
}