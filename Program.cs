using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

static class Program
{
    [STAThreadAttribute]
    static void Main()
    {
        Application.Run(new SchetsEditor());
    }
}