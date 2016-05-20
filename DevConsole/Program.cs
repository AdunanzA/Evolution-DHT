using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestNode
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("messages.log"));
            Trace.AutoFlush = true;
            Trace.Indent();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}