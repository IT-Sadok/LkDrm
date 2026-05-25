using AsyncLibrary.BookHandling;
using AsyncLibrary.Interfaces;
using AsyncLibrary.UI;

namespace AsyncLibrary.TestCase;

/// <summary>
/// Test harness for evaluating concurrent operations on an <see cref="IBookManager"/> implementation by executing
/// random book management operations with configurable task count and concurrency limits.
/// </summary>
public class BookManagerConcurrencyTest
{
    private static readonly string[] Authors =
    [
        "George Orwell", "J.K. Rowling", "Stephen King", "Agatha Christie", "Mark Twain",
        "Ernest Hemingway", "Leo Tolstoy", "Jane Austen", "F. Scott Fitzgerald", "H.G. Wells"
    ];

    private static readonly string[] Titles =
    [
        "The Last Horizon", "Shadows of Tomorrow", "Echoes in the Dark", "The Silent Storm",
        "Whispers of the Past", "Beyond the Edge", "The Forgotten World", "A Thousand Lies",
        "The Iron Crown", "Voices in the Wind"
    ];

    private readonly IBookManager _manager;

    public BookManagerConcurrencyTest(IBookManager manager)
    {
        _manager = manager;
    }

    /// <summary>
    /// Prompts the user for task count and SemaphoreSlim concurrency,
    /// then runs all tasks concurrently, each picking a random library operation.
    /// </summary>
    public async Task ExecuteTestAsync()
    {
        var taskCountStr = await ConsoleHelper.ReadInputAsync("Enter number of tasks: ");
        if (!int.TryParse(taskCountStr, out var taskCount) || taskCount <= 0)
        {
            ConsoleHelper.PrintError("Invalid task count.");
            return;
        }

        var concurrencyStr = await ConsoleHelper.ReadInputAsync("Enter SemaphoreSlim concurrency limit: ");
        if (!int.TryParse(concurrencyStr, out var concurrency) || concurrency <= 0)
        {
            ConsoleHelper.PrintError("Invalid concurrency limit.");
            return;
        }

        var semaphore = new SemaphoreSlim(concurrency, concurrency);
        var random = new Random();

        ConsoleHelper.PrintHeader($"Starting {taskCount} tasks with concurrency limit {concurrency}...");

        for (int i = 1; i <= taskCount; i++)
        {
            var tasks = Enumerable.Range(1, taskCount).Select(i => RunRandomOperationAsync(i, semaphore, random));
            await Task.WhenAll(tasks);
        }

        ConsoleHelper.PrintSuccess("All tasks completed.");
    }

    /// <summary>
    /// Executes a randomly selected library operation (add, remove, search, show all, borrow, or return) with
    /// semaphore-based concurrency control.
    /// </summary>
    /// <param name="taskId">The identifier for the executing task, used for logging output.</param>
    /// <param name="semaphore">The semaphore used to control concurrent access to library operations.</param>
    /// <param name="random">The random number generator used to select operations and data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task RunRandomOperationAsync(int taskId, SemaphoreSlim semaphore, Random random)
    {
        await semaphore.WaitAsync();
        try
        {
            // Randomly pick one of 6 operations: 0=Add, 1=Remove, 2=Search, 3=ShowAll, 4=Borrow, 5=Return
            var operation = random.Next(0, 6);

            switch (operation)
            {
                case 0:
                    var newBook = new BookInfo
                    {
                        Id = Guid.NewGuid(),
                        Title = Titles[random.Next(Titles.Length)],
                        Author = Authors[random.Next(Authors.Length)],
                        Published = random.Next(1900, 2025),
                        Status = BookStatus.Available
                    };
                    await _manager.AddBookAsync(newBook);
                    ConsoleHelper.PrintInfo($"[Task {taskId}] ADD \"{newBook.Title}\" by {newBook.Author} ({newBook.Published})");
                    break;

                case 1:
                    var allForRemove = await _manager.ShowAllBooksAsync();
                    if (allForRemove.Count > 0)
                    {
                        var target = allForRemove[random.Next(allForRemove.Count)];
                        await _manager.RemoveBookAsync(target.ShelfId);
                        ConsoleHelper.PrintInfo($"[Task {taskId}] REMOVE ShelfId {target.ShelfId} \"{target.Title}\"");
                    }
                    else
                    {
                        ConsoleHelper.PrintInfo($"[Task {taskId}] REMOVE library is empty, skipped.");
                    }
                    break;

                case 2:
                    var searchTerm = random.Next(2) == 0
                        ? Authors[random.Next(Authors.Length)]
                        : Titles[random.Next(Titles.Length)];
                    var found = await _manager.ShowBookInfoAsync(searchTerm);
                    ConsoleHelper.PrintInfo($"[Task {taskId}] SEARCH \"{searchTerm}\" {found.Count} result(s)");
                    break;

                case 3:
                    var allBooks = await _manager.ShowAllBooksAsync();
                    ConsoleHelper.PrintInfo($"[Task {taskId}] SHOW ALL {allBooks.Count} book(s) in library");
                    break;

                case 4:
                    var allForBorrow = await _manager.ShowAllBooksAsync();
                    var available = allForBorrow.Where(b => b.Status == BookStatus.Available).ToList();
                    if (available.Count > 0)
                    {
                        var pick = available[random.Next(available.Count)];
                        await _manager.BorrowBookAsync(pick.ShelfId);
                        ConsoleHelper.PrintInfo($"[Task {taskId}] BORROW ShelfId {pick.ShelfId} \"{pick.Title}\"");
                    }
                    else
                    {
                        ConsoleHelper.PrintInfo($"[Task {taskId}] BORROW no available books, skipped.");
                    }
                    break;

                case 5:
                    var allForReturn = await _manager.ShowAllBooksAsync();
                    var borrowed = allForReturn.Where(b => b.Status == BookStatus.Borrowed).ToList();
                    if (borrowed.Count > 0)
                    {
                        var pick = borrowed[random.Next(borrowed.Count)];
                        await _manager.ReturnBookAsync(pick.ShelfId);
                        ConsoleHelper.PrintInfo($"[Task {taskId}] RETURN ShelfId {pick.ShelfId} \"{pick.Title}\"");
                    }
                    else
                    {
                        ConsoleHelper.PrintInfo($"[Task {taskId}] RETURN no borrowed books, skipped.");
                    }
                    break;
            }
        }
        finally
        {
            semaphore.Release();
        }
    }
}
