using ConsoleLibrary.BookHandling;

namespace ConsoleLibrary.Interfaces;

/// <summary>
/// Defines the contract for loading and saving the library data in JSON format.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Loads the library collection from the JSON file.
    /// </summary>
    /// <returns>A list of books, or an empty list if no file exists.</returns>
    List<BookInfo> Load();

    /// <summary>
    /// Saves the library collection to the JSON file.
    /// </summary>
    /// <param name="library">The list of books to persist.</param>
    void Save(List<BookInfo> library);
}
