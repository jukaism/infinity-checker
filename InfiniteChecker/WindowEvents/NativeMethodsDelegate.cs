using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteChecker.WindowEvents
{
    internal static class NativeMethodsDelegate
    {
        public delegate void WinEventDelegate(
            System.IntPtr hWinEventHook,
            uint eventType,
            System.IntPtr hwnd,
            long idObject,
            long idChild,
            uint dwEventThread,
            uint dwmsEventTime
            );
    }
}
