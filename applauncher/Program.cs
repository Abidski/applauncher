using System.Drawing;
using System.Runtime.InteropServices;
using applauncher.ui;

namespace applauncher;

public class Program
{
    private const uint ALT = 0x0001;
    private const uint Q = 0x51;
    private const int HOTKEY_PRESSED_CODE = 786;
    private static volatile bool showLauncher;
    private static bool Quit;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern int GetMessage(out TagMsg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    [STAThread]
    private static int Main(string[] args)
    {
        Run();
        return 1;
    }

    private static void Run()
    {
        Gui.InitializeWindowAsActive();
        var hWnd = Gui.windowHandle;
        if (hWnd == IntPtr.Zero)
            Console.WriteLine("Window handle error");
        else
            HideWindow(hWnd);


        var messagePump = new Thread(() => MessagePump(hWnd));
        messagePump.Start();
        while (!Quit)
        {
            Gui.RunGuiActive();
            ToggleLauncher(showLauncher, hWnd);
        }
    }

    private static void MessagePump(IntPtr hWnd)
    {
        TagMsg message = default;
        Console.WriteLine("Starting messagePumpThread");
        var status = RegisterHotKey(IntPtr.Zero, 0, ALT, Q);
        while (GetMessage(out message, IntPtr.Zero, 0, 0) != 0 && !Quit)
            if (message.message == 786)
            {
                showLauncher = !showLauncher;
                Console.WriteLine("HotKetPressed");
            }
    }

    private static void ToggleLauncher(bool show, IntPtr hWnd)
    {
        if (show)
            ShowWindow(hWnd);
        else
            HideWindow(hWnd);
    }


    private static void HideWindow(IntPtr hWnd)
    {
        ShowWindow(hWnd, 0);
    }

    private static void ShowWindow(IntPtr hWnd)
    {
        ShowWindow(hWnd, 5);
        SetForegroundWindow(hWnd);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TagMsg
    {
        public IntPtr windowHandle;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public Point p;
    }
}