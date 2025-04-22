using System;

namespace ZolozCSharpOpenApi.Utils.Log;

public static class Logger
{
    private static void WriteWithColor(string message, ConsoleColor color, bool isError = false)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;

        if (isError)
        {
            Console.Error.Write(message);
        }    
        else
        {
            Console.Out.Write(message);
        }

        Console.ForegroundColor = originalColor;
    }

    private static void WriteLineWithColors((string Text, ConsoleColor Color)[] parts, bool isError = false)
    {
        foreach (var part in parts)
        {
            WriteWithColor(part.Text, part.Color, isError);
        }

        if (isError)
        {
            Console.Error.WriteLine();
        }
        else
        {
            Console.Out.WriteLine();
        }
    }

    private static string GetTimestamp() =>
        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ";

    public static void Info(string message)
    {
        WriteLineWithColors(new[]
        {
            (GetTimestamp(), ConsoleColor.DarkGray),
            ("[INFO] ", ConsoleColor.Cyan),
            (message, ConsoleColor.White)
        });
    }

    public static void Warn(string message)
    {
        WriteLineWithColors(new[]
        {
            (GetTimestamp(), ConsoleColor.DarkGray),
            ("[WARN] ", ConsoleColor.Yellow),
            (message, ConsoleColor.White)
        });
    }

    public static void Error(string message)
    {
        WriteLineWithColors(new[]
        {
            (GetTimestamp(), ConsoleColor.DarkGray),
            ("[ERROR] ", ConsoleColor.Red),
            (message, ConsoleColor.White)
        }, isError: true);
    }

    public static void Fatal(string message)
    {
        WriteLineWithColors(new[]
        {
            (GetTimestamp(), ConsoleColor.DarkGray),
            ("[FATAL] ", ConsoleColor.Magenta),
            (message, ConsoleColor.White)
        }, isError: true);
    }
}
