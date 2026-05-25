namespace AsyncLibrary.UI;

/// <summary>
/// Provides helper methods for displaying formatted messages and reading input in the console application.
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Writes the specified message to the console in cyan to display it as a header.
    /// </summary>
    /// <param name="message">The message to display as a header. Cannot be null.</param>
    public static void PrintHeader(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the specified message to the console in green to indicate a successful operation.
    /// </summary>
    /// <param name="message">The message to display in the console output. If null or empty, no text is displayed.</param>
    public static void PrintSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the specified error message to the console in red text.
    /// </summary>
    /// <param name="message">The error message to display. Cannot be null.</param>
    public static void PrintError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the specified message to the console in yellow text.
    /// </summary>
    /// <param name="message">The message to display in the console output. Cannot be null.</param>
    public static void PrintInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the specified menu option message to the console in white text.
    /// </summary>
    /// <param name="message">The text of the menu option to display. If null or empty, no output is written.</param>
    public static void PrintMenuOption(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Displays the specified prompt in the console and asynchronously reads a line of input from the user.
    /// </summary>
    /// <param name="prompt">The message to display to the user before reading input. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the line of input entered by the user, or an empty string if the input is null.</returns>
    public static async Task<string> ReadInputAsync(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(prompt);
        Console.ResetColor();
        return await Console.In.ReadLineAsync() ?? string.Empty;
    }
}