using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteChecker.WindowEvents
{
    internal enum WS_EX : int
    {
        WS_EX_LAYERED = 0x00080000,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_TOOLWINDOW = 0x00000080
    }
    internal enum WINEVENT : uint
    {
        WINEVENT_OUTOFCONTEXT = 0x0000,
        WINEVENT_SKIPOWNTHREAD = 0x0001,
        WINEVENT_SKIPOWNPROCESS = 0x0002,
        WINEVENT_INCONTEXT = 0x0004,
    }
    internal enum OBJID : long
    {
        OBJID_WINDOW = 0x00000000,
        OBJID_SYSMENU = 0xFFFFFFFF,
        OBJID_TITLEBAR = 0xFFFFFFFE,
        OBJID_MENU = 0xFFFFFFFD,
        OBJID_CLIENT = 0xFFFFFFFC,
        OBJID_VSCROLL = 0xFFFFFFFB,
        OBJID_HSCROLL = 0xFFFFFFFA,
        OBJID_SIZEGRIP = 0xFFFFFFF9,
        OBJID_CARET = 0xFFFFFFF8,
        OBJID_CURSOR = 0xFFFFFFF7,
        OBJID_ALERT = 0xFFFFFFF6,
        OBJID_SOUND = 0xFFFFFFF5,
    }
    public enum HookWindowEventType
    {
        Foreground = 1,
        MoveSizeStart = 2,
        MoveSizeEnd = 4,
        MinimizeStart = 8,
        MinimizeEnd = 16,
        Create = 32,
        Destroy = 64,
        Show = 128,
        Hide = 256,
        LocationChange = 512,
        NameChange = 1024
    }
    internal enum HOOK_EVENT : uint
    {
        EVENT_MIN = 0x00000001,
        EVENT_SYSTEM_FOREGROUND = 0x00000003,
        EVENT_SYSTEM_MOVESIZESTART = 0x0000000a,
        EVENT_SYSTEM_MOVESIZEEND = 0x0000000b,
        //EVENT_SYSTEM_DIALOGEND = 0x00000011,
        EVENT_SYSTEM_MINIMIZESTART = 0x00000016,
        EVENT_SYSTEM_MINIMIZEEND = 0x00000017,
        EVENT_OBJECT_CREATE = 0x00008000,
        EVENT_OBJECT_DESTROY = 0x00008001,
        EVENT_OBJECT_SHOW = 0x00008002,
        EVENT_OBJECT_HIDE = 0x00008003,
        EVENT_OBJECT_LOCATIONCHANGE = 0x0000800b,
        EVENT_OBJECT_NAMECHANGE = 0x0000800c,
        EVENT_MAX = 0x7fffffff,
    }
    internal enum GetAncestorFlags
    {
        GetParent = 1,
        GetRoot = 2,
        GetRootOwner = 3
    }
    internal enum GWL : int
    {
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20
    }
    internal enum DWMWINDOWATTRIBUTE : uint
    {
        NCRenderingEnabled = 1,
        NCRenderingPolicy,
        TransitionsForceDisabled,
        AllowNCPaint,
        CaptionButtonBounds,
        NonClientRtlLayout,
        ForceIconicRepresentation,
        Flip3DPolicy,
        ExtendedFrameBounds,
        HasIconicBitmap,
        DisallowPeek,
        ExcludedFromPeek,
        Cloak,
        Cloaked,
        FreezeRepresentation
    }
}
