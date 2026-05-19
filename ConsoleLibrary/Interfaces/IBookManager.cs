using ConsoleLibrary.BookHandling;

namespace ConsoleLibrary.Interfaces;

/// <summary>
/// Defines the contract for managing a library's book collection.
/// </summary>
public interface IBookManager
{
    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    /// <param name="bookInfo">The book information to add.</param>
    void AddBook(BookInfo bookInfo);

    /// <summary>
    /// Removes a book from the library by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to remove.</param>
    void RemoveBook(int id);

    /// <summary>
    /// Searches for and displays books matching the given author name or title.
    /// </summary>
    /// <param name="authorOrTitle">The author name or title to search for.</param>
    void ShowBookInfo(string authorOrTitle);

    /// <summary>
    /// Displays all books currently in the library.
    /// </summary>
    void ShowAllBooks();

    /// <summary>
    /// Marks a book as borrowed by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to borrow.</param>
    void BorrowBook(int id);

    /// <summary>
    /// Marks a borrowed book as returned and available.
    /// </summary>
    /// <param name="id">The shelf ID of the book to return.</param>
    void ReturnBook(int id);
}
