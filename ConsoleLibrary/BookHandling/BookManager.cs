using ConsoleLibrary.Interfaces;
using ConsoleLibrary.UI;

namespace ConsoleLibrary.BookHandling;

/// <summary>
/// Manages the library's book collection, providing operations to add, remove,
/// search, display, borrow, and return books.
/// </summary>
public class BookManager : IBookManager
{
    private readonly List<BookInfo> _library = new();
    private readonly IBookRepository _repository;

    public BookManager(IBookRepository repository)
    {
        _repository = repository;
        _library = _repository.Load();
    }

    /// <summary>
    /// Adds a new book to the library, assigning it the next available shelf ID.
    /// </summary>
    /// <param name="bookInfo">The book information to add.</param>
    public void AddBook(BookInfo bookInfo)
    {
        bookInfo.ShelfId = _library.Count > 0 ? _library.Max(b => b.ShelfId) + 1 : 0;
        _library.Add(bookInfo);
        _repository.Save(_library);
    }

    /// <summary>
    /// Removes a book from the library by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to remove.</param>
    public void RemoveBook(int id)
    {
        var book = _library.FirstOrDefault(b => b.ShelfId == id);
        if (book is not null)
        {
            _library.Remove(book);
            _repository.Save(_library);
        }
    }

    /// <summary>
    /// Searches for books matching the given author name or title and displays their details.
    /// </summary>
    /// <param name="authorOrTitle">The author name or title to search for.</param>
    public List<BookInfo> ShowBookInfo(string authorOrTitle) => _library
            .Where(book => book.Author == authorOrTitle || book.Title == authorOrTitle)
            .ToList();

    /// <summary>
    /// Displays information about all books currently in the library.
    /// </summary>
    public List<BookInfo> ShowAllBooks() => _library;

    /// <summary>
    /// Marks a book as borrowed if it is currently available.
    /// </summary>
    /// <param name="id">The shelf ID of the book to borrow.</param>
    public void BorrowBook(int id)
    {
        var bookInfo = _library.FirstOrDefault(b => b.ShelfId == id);
        if (bookInfo is not null)
        {
            if (bookInfo.Status == BookStatus.Available)
            {
                bookInfo.Status = BookStatus.Borrowed;
                ConsoleHelper.PrintSuccess($"Book has been borrowed {bookInfo.Author} {bookInfo.Title}");
                _repository.Save(_library);
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
        var bookInfo = _library.FirstOrDefault(b => b.ShelfId == id);
        if (bookInfo is not null)
        {
            bookInfo.Status = BookStatus.Available;
            ConsoleHelper.PrintSuccess($"Book has been return into the book-shell: {bookInfo.Author} {bookInfo.Title}");
            _repository.Save(_library);
        }
    }
}
