using AsyncLibrary.BookHandling;
using AsyncLibrary.TestCase;
using AsyncLibrary.JsonHandling;
using AsyncLibrary.UI;

var repository = new JSONManager();
var manager = new BookManager(repository);
await manager.InitializeAsync();

ConsoleHelper.PrintHeader("=== Async Library ===");

bool running = true;
while (running)
{
    ConsoleHelper.PrintMenuOption("1. Add book");
    ConsoleHelper.PrintMenuOption("2. Remove book");
    ConsoleHelper.PrintMenuOption("3. Search book");
    ConsoleHelper.PrintMenuOption("4. Show all books");
    ConsoleHelper.PrintMenuOption("5. Borrow book");
    ConsoleHelper.PrintMenuOption("6. Return book");
    ConsoleHelper.PrintMenuOption("7. Test");
    ConsoleHelper.PrintMenuOption("0. Exit");

    var choice = await ConsoleHelper.ReadInputAsync("Choose option: ");

    switch (choice.Trim())
    {
        case "1":
            var title = await ConsoleHelper.ReadInputAsync("Title: ");
            var author = await ConsoleHelper.ReadInputAsync("Author: ");
            var publishedStr = await ConsoleHelper.ReadInputAsync("Published year: ");
            if (int.TryParse(publishedStr, out var year))
            {
                await manager.AddBookAsync(new BookInfo { Id = Guid.NewGuid(), Title = title, Author = author, Published = year, Status = BookStatus.Available });
                ConsoleHelper.PrintSuccess("Book added.");
            }
            else
            {
                ConsoleHelper.PrintError("Invalid year.");
            }
            break;

        case "2":
            var removeIdStr = await ConsoleHelper.ReadInputAsync("Shelf ID to remove: ");
            if (int.TryParse(removeIdStr, out var removeId))
            {
                await manager.RemoveBookAsync(removeId);
            }
            else
            {
                ConsoleHelper.PrintError("Invalid ID.");
            }
            break;

        case "3":
            var query = await ConsoleHelper.ReadInputAsync("Author or title: ");
            var results = await manager.ShowBookInfoAsync(query);
            if (results.Count == 0)
            {
                ConsoleHelper.PrintError("No books found.");
            }
            foreach (var b in results)
            {
                ConsoleHelper.PrintInfo($"[{b.ShelfId}] {b.Title} by {b.Author} ({b.Published}) - {b.Status}");
            }
            break;

        case "4":
            var all = await manager.ShowAllBooksAsync();
            if (all.Count == 0)
            {
                ConsoleHelper.PrintError("Library is empty.");
            }
            foreach (var b in all)
            {
                ConsoleHelper.PrintInfo($"[{b.ShelfId}] {b.Title} by {b.Author} ({b.Published}) - {b.Status}");
            }
            break;

        case "5":
            var borrowIdStr = await ConsoleHelper.ReadInputAsync("Shelf ID to borrow: ");
            if (int.TryParse(borrowIdStr, out var borrowId))
            {
                await manager.BorrowBookAsync(borrowId);
            }
            else
            {
                ConsoleHelper.PrintError("Invalid ID.");
            }
            break;

        case "6":
            var returnIdStr = await ConsoleHelper.ReadInputAsync("Shelf ID to return: ");
            if (int.TryParse(returnIdStr, out var returnId))
            {
                await manager.ReturnBookAsync(returnId);
            }
            else
            {
                ConsoleHelper.PrintError("Invalid ID.");
            }
            break;
            
        case "7":
            var testApp = new BookManagerConcurrencyTest(manager);
            await testApp.ExecuteTestAsync();
            break;

        case "0":
            running = false;
            break;

        default:
            ConsoleHelper.PrintError("Unknown option.");
            break;
    }
}