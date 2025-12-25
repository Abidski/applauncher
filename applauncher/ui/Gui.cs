using System.Diagnostics;
using System.Net.Mime;
using static Raylib_cs.Raylib;
using  applauncher.backend;
using Raylib_cs;

namespace MyNamespace;
class Gui
{
    private const int screenWidth = 800;
    private const int screenHeight = 400;
    private const int fontSize = 20;
    private const int textPadding = 10;
    
    static int UserInterface(string[] args)
    {
        List<Applications.App> apps = Applications.GetDesktopApps();
        
        SetConfigFlags(ConfigFlags.UndecoratedWindow|ConfigFlags.TransparentWindow);
        InitWindow(screenWidth, screenHeight, "launcher");
        while (!WindowShouldClose())
        {
            BeginDrawing();
            SetWindow();
            DrawPrograms(apps);
            EndDrawing();
        }
        CloseWindow();
        return 0;
    }

    public static void DrawPrograms(List<Applications.App> apps)
    {
        int yPos = 0;
        foreach (var app in apps)
        {
            int textWidth = MeasureText(app.ToString(), fontSize);
            Rectangle rectangle = new Rectangle(0, yPos, textWidth, fontSize);
            //DrawRectangle(0, yPos, textWidth, fontSize, Color.Black);
            if (CheckCollisionPointRec(GetMousePosition(), rectangle))
            {
                DrawText(app.ToString(), 0,yPos,fontSize,Color.Pink);
                if (IsMouseButtonPressed(MouseButton.Left) && CheckCollisionPointRec(GetMousePosition(), rectangle))
                {
                    int lastIndexOfDirectory = app.Path.LastIndexOf('\\');
                    string workingDirectory = app.Path.Substring(0,lastIndexOfDirectory);
                    Process.Start(new ProcessStartInfo
                        {
                        FileName = app.Path,
                        WorkingDirectory = workingDirectory,
                        UseShellExecute= true
                        }
                    );
                    CloseWindow();
                }
            }
            else
            {
                DrawText(app.ToString(), 0,yPos,fontSize,Color.RayWhite);
            }
            yPos += fontSize+textPadding;
        }
    }

    public static void SetWindow()
    {
        int currentMonitor = GetCurrentMonitor();
        int middleY = GetMonitorHeight(currentMonitor)/2 - 200;
        int middleX = GetMonitorWidth(currentMonitor)/2 - 200;
        SetWindowPosition(middleX,middleY);
        ClearBackground(Color.Blank);
    }

    public static float GetScale()
    {
        int currentMonitor = GetCurrentMonitor();
        float scale = (float)screenHeight/ (float)GetMonitorHeight(currentMonitor);
        return scale;

    }

    public void SetInputBox()
    {
    }
}

