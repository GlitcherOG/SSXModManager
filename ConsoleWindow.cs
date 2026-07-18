using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SSXMultiTool.Utilities
{
    public class ConsoleWindow
    {
        static bool Initialised = false;

        public static void GenerateConsole()
        {
            if (!Initialised)
            {
                if (!AttachConsole(-1))
                    AllocConsole();

                // Reconnect Console.Out
                var stdout = Console.OpenStandardOutput();
                var writer = new StreamWriter(stdout)
                {
                    AutoFlush = true
                };
                Console.SetOut(writer);

                // Reconnect Console.Error too
                var stderr = Console.OpenStandardError();
                var errorWriter = new StreamWriter(stderr)
                {
                    AutoFlush = true
                };
                Console.SetError(errorWriter);

                Initialised = true;
            }
            else
            {
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void CloseConsole()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int FreeConsole();

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
    }
}
