using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteChecker.WindowEvents
{
    public class WindowEventArgs
    {
        public System.IntPtr Hwnd;
        public WindowEventType WindowEventType;
    }
}
