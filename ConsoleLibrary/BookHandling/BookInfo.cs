namespace ConsoleLibrary.BookHandling;

/// <summary>
/// Represents a book in the library with its metadata and availability status.
/// </summary>
public class BookInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the book.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the shelf position of the book in the library.
    /// </summary>
    public int ShelfId { get; set; }

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the author of the book.
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the year the book was published.
    /// </summary>
    public int Published { get; set; }

    /// <summary>
    /// Gets or sets the current availability status of the book.
    /// </summary>
    public BookStatus Status { get; set; }
}
