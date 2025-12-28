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

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern int GetMessage(out TagMsg lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [STAThread]
    private static int Main(string[] args)
    {
        Run();
        return 1;
    }

    private static void Run()
    {
        Gui.InitializeWindowAsHidden();
        var messagePump = new Thread(() => MessagePump());
        messagePump.Start();
        while (true)
            if (showLauncher)
                Gui.RunGuiActive();
            else
                Gui.RunGuiHidden();
    }

    private static void MessagePump()
    {
        TagMsg message = default;
        Console.WriteLine("Starting messagePumpThread");
        var status = RegisterHotKey(IntPtr.Zero, 0, ALT, Q);
        while (GetMessage(out message, IntPtr.Zero, 0, 0) != 0 && !showLauncher)
            if (message.message == 786)
                showLauncher = !showLauncher;
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