using AsyncLibrary.Interfaces;
using AsyncLibrary.UI;

namespace AsyncLibrary.BookHandling;

/// <summary>
/// Manages the library's book collection, providing operations to add, remove,
/// search, display, borrow, and return books.
/// </summary>
public class BookManager : IBookManager
{
    private readonly List<BookInfo> _library = new();
    private readonly IBookRepository _repository;
    private readonly SemaphoreSlim _semaphore;
    private readonly int _initialCapacity;
    private readonly int _maxCapacity;

    public BookManager(IBookRepository repository, int initialCapacity = 1, int maxCapacity = 1)
    {
        _repository = repository;
        _initialCapacity = initialCapacity;
        _maxCapacity = maxCapacity;
        _semaphore = new SemaphoreSlim(_initialCapacity, _maxCapacity);
    }

    /// <summary>
    /// Initializes the library by loading books from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        var books = await _repository.LoadAsync();
        _library.Clear();
        _library.AddRange(books);
    }

    /// <summary>
    /// Adds a new book to the library, assigning it the next available shelf ID.
    /// </summary>
    /// <param name="bookInfo">The book information to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddBookAsync(BookInfo bookInfo)
    {
        await _semaphore.WaitAsync();
        try
        {
            bookInfo.ShelfId = _library.Count > 0 ? _library.Max(b => b.ShelfId) + 1 : 0;
            _library.Add(bookInfo);
            await _repository.SaveAsync(_library);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Removes a book from the library by its shelf ID.
    /// </summary>
    /// <param name="id">The shelf ID of the book to remove.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task RemoveBookAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var book = _library.FirstOrDefault(b => b.ShelfId == id);
            if (book is not null)
            {
                _library.Remove(book);
                await _repository.SaveAsync(_library);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Searches for books matching the given author name or title and displays their details.
    /// </summary>
    /// <param name="authorOrTitle">The author name or title to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of books matching the search criteria.</returns>
    public async Task<List<BookInfo>> ShowBookInfoAsync(string authorOrTitle)
    {
        await _semaphore.WaitAsync();
        try
        {
            return _library
            .Where(book => book.Author == authorOrTitle || book.Title == authorOrTitle)
            .ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Displays information about all books currently in the library.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of all books in the library.</returns>
    public async Task<List<BookInfo>> ShowAllBooksAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return _library.ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Marks a book as borrowed if it is currently available.
    /// </summary>
    /// <param name="id">The shelf ID of the book to borrow.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task BorrowBookAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var bookInfo = _library.FirstOrDefault(b => b.ShelfId == id);
            if (bookInfo is not null)
            {
                if (bookInfo.Status == BookStatus.Available)
                {
                    bookInfo.Status = BookStatus.Borrowed;
                    ConsoleHelper.PrintSuccess($"Book has been borrowed {bookInfo.Author} {bookInfo.Title}");
                    await _repository.SaveAsync(_library);
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
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Marks a borrowed book as available again when it is returned.
    /// </summary>
    /// <param name="id">The shelf ID of the book to return.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ReturnBookAsync(int id)
    {
        await _semaphore.WaitAsync();
        try
        {
            var bookInfo = _library.FirstOrDefault(b => b.ShelfId == id);
            if (bookInfo is not null)
            {
                bookInfo.Status = BookStatus.Available;
                ConsoleHelper.PrintSuccess($"Book has been return into the book-shell: {bookInfo.Author} {bookInfo.Title}");
                await _repository.SaveAsync(_library);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
