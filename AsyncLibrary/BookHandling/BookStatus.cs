namespace AsyncLibrary.BookHandling;

/// <summary>
/// Represents the availability status of a book in the library.
/// </summary>
public enum BookStatus
{
    /// <summary>
    /// The book is available for borrowing.
    /// </summary>
    Available,

    /// <summary>
    /// The book has been borrowed and is currently not available.
    /// </summary>
    Borrowed,
}