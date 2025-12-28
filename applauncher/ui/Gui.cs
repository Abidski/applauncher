using System.Diagnostics;
using applauncher.backend;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace applauncher.ui;

internal static class Gui
{
    private const int screenWidth = 800;
    private const int screenHeight = 400;
    private const int fontSize = 20;
    private const int textPadding = 10;
    public static IntPtr windowHandle;
    private static readonly List<Applications.App> Apps = Applications.GetDesktopApps();


    public static void InitializeWindowAsHidden()
    {
        SetConfigFlags(ConfigFlags.UndecoratedWindow | ConfigFlags.TransparentWindow | ConfigFlags.HiddenWindow);
        InitWindow(screenWidth, screenHeight, "launcher");

        unsafe
        {
            windowHandle = (IntPtr)GetWindowHandle();
        }
    }

    public static void RunGuiHidden()
    {
        SetTargetFPS(1);
        BeginDrawing();
        EndDrawing();
    }

    public static void RunGuiActive()
    {
        BeginDrawing();
        SetWindow();
        DrawPrograms(Apps);
        EndDrawing();
    }


    public static void DrawPrograms(List<Applications.App> apps)
    {
        var yPos = 0;
        foreach (var app in apps)
        {
            var textWidth = MeasureText(app.ToString(), fontSize);
            var rectangle = new Rectangle(0, yPos, textWidth, fontSize);
            //DrawRectangle(0, yPos, textWidth, fontSize, Color.Black);
            if (CheckCollisionPointRec(GetMousePosition(), rectangle))
            {
                DrawText(app.ToString(), 0, yPos, fontSize, Color.Pink);
                if (IsMouseButtonPressed(MouseButton.Left) && CheckCollisionPointRec(GetMousePosition(), rectangle))
                {
                    var lastIndexOfDirectory = app.Path.LastIndexOf('\\');
                    var workingDirectory = app.Path.Substring(0, lastIndexOfDirectory);
                    Process.Start(new ProcessStartInfo
                        {
                            FileName = app.Path,
                            WorkingDirectory = workingDirectory,
                            UseShellExecute = true
                        }
                    );
                    CloseWindow();
                }
            }
            else
            {
                DrawText(app.ToString(), 0, yPos, fontSize, Color.RayWhite);
            }

            yPos += fontSize + textPadding;
        }
    }

    public static void SetWindow()
    {
        var currentMonitor = GetCurrentMonitor();
        var middleY = GetMonitorHeight(currentMonitor) / 2 - 200;
        var middleX = GetMonitorWidth(currentMonitor) / 2 - 200;
        SetWindowPosition(middleX, middleY);
        ClearBackground(Color.Blank);
    }

    public static float GetScale()
    {
        var currentMonitor = GetCurrentMonitor();
        var scale = screenHeight / (float)GetMonitorHeight(currentMonitor);
        return scale;
    }

    public static void SetInputBox()
    {
    }
}