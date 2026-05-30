using AsyncLibrary.BookHandling;

namespace AsyncLibrary.Interfaces;

/// <summary>
/// Defines the contract for managing a library's book collection.
/// </summary>
public interface IBookManager
{
    /// <summary>
    /// Initializes the instance asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task InitializeAsync();

    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    /// <param name="bookInfo">The book information to add.</param>
    Task AddBookAsync(BookInfo bookInfo);

    /// <summary>
    /// Removes a book from the library by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to remove.</param>
    Task RemoveBookAsync(int id);

    /// <summary>
    /// Searches for and displays books matching the given author name or title.
    /// </summary>
    /// <param name="authorOrTitle">The author name or title to search for.</param>
    Task<List<BookInfo>> ShowBookInfoAsync(string authorOrTitle);

    /// <summary>
    /// Displays all books currently in the library.
    /// </summary>
    Task<List<BookInfo>> ShowAllBooksAsync();

    /// <summary>
    /// Marks a book as borrowed by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to borrow.</param>
    Task BorrowBookAsync(int id);

    /// <summary>
    /// Marks a borrowed book as returned and available.
    /// </summary>
    /// <param name="id">The shelf ID of the book to return.</param>
    Task ReturnBookAsync(int id);
}
