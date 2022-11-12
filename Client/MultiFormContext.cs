using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public class MultiFormContext : ApplicationContext
    {
        private int _openForms;
        public MultiFormContext(params Form[] forms)
        {
            _openForms = forms.Length;

            foreach (var form in forms)
            {
                form.FormClosed += (s, args) =>
                {
                    if (Interlocked.Decrement(ref _openForms) == 0)
                        ExitThread();
                };

                form.Show();
            }
        }
    }
}