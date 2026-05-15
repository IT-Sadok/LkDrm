using ConsoleLibrary.Interfaces;
using ConsoleLibrary.UI;

namespace ConsoleLibrary.BookHandling;

/// <summary>
/// Manages the library's book collection, providing operations to add, remove,
/// search, display, borrow, and return books.
/// </summary>
public class BookManager : IBookManager
{
    private Dictionary<int, BookInfo> _library = [];

    /// <summary>
    /// Gets or sets the dictionary representing the library's book collection,
    /// where the key is the shelf ID and the value is the book information.
    /// </summary>
    public Dictionary<int, BookInfo> Library
    {
        get { return _library; }
        set { _library = value; }
    }

    /// <summary>
    /// Gets or sets the current shelf ID counter used when adding new books.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Adds a new book to the library, assigning it the next available shelf ID.
    /// </summary>
    /// <param name="bookInfo">The book information to add.</param>
    public void AddBook(BookInfo bookInfo)
    {
        // Assign an ID based on the current number of books
        Id = Library.Keys.Count > 0 ? Library.Keys.Count + 1 : 0;
        Library.TryAdd(Id, bookInfo);
    }

    /// <summary>
    /// Removes a book from the library by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to remove.</param>
    public void RemoveBook(int id) => Library.Remove(id);

    /// <summary>
    /// Searches for books matching the given author name or title and displays their details.
    /// </summary>
    /// <param name="authorOrTitle">The author name or title to search for.</param>
    public void ShowBookInfo(string authorOrTitle)
    {
        if (string.IsNullOrEmpty(authorOrTitle))
        {
            ConsoleHelper.PrintError("Enter please author or title....");
        }

        var bookInfo = Library.Values.Where(book => book.Author == authorOrTitle || book.Title == authorOrTitle);
        if (bookInfo != null)
        {
            foreach (var books in bookInfo)
            {
                ConsoleHelper.PrintInfo($"\nAuthor: {books.Author} \nTitle: {books.Title} \nStatus: {books.Status} \nPublished: {books.Published} \nUniqBookId: {books.UniqId}");
            }
        }
        else
        {
            ConsoleHelper.PrintError($"There is no books or authors with title or author {authorOrTitle}");
        }
    }

    /// <summary>
    /// Displays information about all books currently in the library.
    /// </summary>
    public void ShowAllBooks()
    {
        if (Library.Count > 0)
        {
            foreach (var book in Library)
            {
                ConsoleHelper.PrintInfo($"\nBookShell: {book.Key} \nAuthor: {book.Value.Author} \nTitle: {book.Value.Title} \nStatus: {book.Value.Status} \nPublished: {book.Value.Published} \nUniqBookId: {book.Value.UniqId}");
            }
        }
        else
        {
            ConsoleHelper.PrintError("The library is empty");
        }
    }

    /// <summary>
    /// Marks a book as borrowed if it is currently available.
    /// </summary>
    /// <param name="id">The shelf ID of the book to borrow.</param>
    public void BorrowBook(int id)
    {
        if (Library.TryGetValue(id, out BookInfo? bookInfo))
        {
            if (bookInfo.Status == BookStatus.Available)
            {
                // Mark the book as borrowed
                bookInfo.Status = BookStatus.Borrowed;
                ConsoleHelper.PrintSuccess($"Book has been borrowed {bookInfo.Author} {bookInfo.Title}");
            }
            else
            {
                ConsoleHelper.PrintError($"The book has been borrowed: {bookInfo.Author} {bookInfo.Title}");
            }
        }
        else
        {
            ConsoleHelper.PrintError($"There is no book with ID: {id}");
        }
    }

    /// <summary>
    /// Marks a borrowed book as available again when it is returned.
    /// </summary>
    /// <param name="id">The shelf ID of the book to return.</param>
    public void ReturnBook(int id)
    {
        if (Library.TryGetValue(id, out BookInfo? bookInfo))
        {
            bookInfo.Status = BookStatus.Available;
            ConsoleHelper.PrintSuccess($"Book has been return into the book-shell: {bookInfo.Author} {bookInfo.Title}");
        }
    }
}
