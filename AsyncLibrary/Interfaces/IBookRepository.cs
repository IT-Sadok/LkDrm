using AsyncLibrary.BookHandling;

namespace AsyncLibrary.Interfaces;

/// <summary>
/// Defines the contract for loading and saving the library data.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Loads the library collection from the file.
    /// </summary>
    /// <returns>A list of books, or an empty list if no file exists.</returns>
    Task<List<BookInfo>> LoadAsync();

    /// <summary>
    /// Saves the library collection to the file.
    /// </summary>
    /// <param name="library">The list of books to persist.</param>
    Task SaveAsync(List<BookInfo> library);
}
