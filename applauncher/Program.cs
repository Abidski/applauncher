using System.Runtime.InteropServices;
namespace applauncher;

public class Program
{
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);    
    static int Main(string[] args)
    {
        
        return 1;
    }
    
}