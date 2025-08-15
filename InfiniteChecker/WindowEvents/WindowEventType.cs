using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteChecker.WindowEvents
{
    public enum WindowEventType : int
    {
        None,
        Foreground,
        MoveSizeStart,
        MoveSizeEnd,
        MinimizeStart,
        MinimizeEnd,
        Create,
        Destroy,
        Show,
        Hide,
        LocationChange,
        NameChange,
    }
}
