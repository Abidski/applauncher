using System.Drawing;
using System.Runtime.InteropServices;

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

    private static int Main(string[] args)
    {
        var messagePump = new Thread(() => MessagePump());
        messagePump.Start();

        return 1;
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