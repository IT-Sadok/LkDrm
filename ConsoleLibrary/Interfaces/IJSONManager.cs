using ConsoleLibrary.BookHandling;

namespace ConsoleLibrary.Interfaces;

/// <summary>
/// Defines the contract for loading and saving the library data in JSON format.
/// </summary>
public interface IJSONManager
{
    /// <summary>
    /// Loads the library collection from the JSON file.
    /// </summary>
    /// <returns>A dictionary of books keyed by their shelf ID, or an empty dictionary if no file exists.</returns>
    Dictionary<int, BookInfo> Load();

    /// <summary>
    /// Saves the library collection to the JSON file.
    /// </summary>
    /// <param name="library">The dictionary of books to persist.</param>
    void Save(Dictionary<int, BookInfo> library);
}
