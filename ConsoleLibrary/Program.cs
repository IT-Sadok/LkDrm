using ConsoleLibrary.BookHandling;
using ConsoleLibrary.JsonHandling;
using ConsoleLibrary.UI;

Console.Title = "Library";

var bookRepository = new JSONManager();
var bookManager = new BookManager(bookRepository);

while (true)
{
    ConsoleHelper.PrintHeader("=== Library: Main menu ===");
    ConsoleHelper.PrintMenuOption("1. [Book] Add new book");
    ConsoleHelper.PrintMenuOption("2. [Book] Book information");
    ConsoleHelper.PrintMenuOption("3. [Book] List of books");
    ConsoleHelper.PrintMenuOption("4. [Operation] Borrow book");
    ConsoleHelper.PrintMenuOption("5. [Operation] Book returning");
    ConsoleHelper.PrintMenuOption("6. Exit");

    var input = ConsoleHelper.ReadInput("\nYour choice: ");

    if (input == "6")
    {
        break;
    }

    switch (input)
    {
        case "1":
            ConsoleHelper.PrintHeader("\n--- Adding new book ---");

            string title = ConsoleHelper.ReadInput("Enter a book name: ");
            string author = ConsoleHelper.ReadInput("Enter author name: ");
            _ = int.TryParse(ConsoleHelper.ReadInput("Enter when book was published: "), out int published);

            BookInfo bookInfo = new()
            {
                Title = title,
                Author = author,
                Published = published,
                Status = BookStatus.Available,
                Id = Guid.NewGuid(),
            };

            bookManager.AddBook(bookInfo);

            ConsoleHelper.PrintSuccess($"The book '{title}' has been saved.");
            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
        case "2":
            ConsoleHelper.PrintHeader("\n--- Book information ---");

            string authorOrTitle = ConsoleHelper.ReadInput("Enter a book author or title: ");
            bookManager.ShowBookInfo(authorOrTitle);

            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
        case "3":
            ConsoleHelper.PrintHeader("\n--- All books ---");

            bookManager.ShowAllBooks();

            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
        case "4":
            ConsoleHelper.PrintHeader("\n--- Borrow book ---");
            int.TryParse(ConsoleHelper.ReadInput("Enter book ID: "), out int borrowBookId);

            bookManager.BorrowBook(borrowBookId);

            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
        case "5":
            ConsoleHelper.PrintHeader("\n--- Return book ---");
            _ = int.TryParse(ConsoleHelper.ReadInput("Enter book ID: "), out int returnBookId);

            bookManager.ReturnBook(returnBookId);

            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
        default:
            ConsoleHelper.PrintError("Unknown command.");
            ConsoleHelper.PrintInfo("Press 'Enter' to continue...");
            Console.ReadLine();

            break;
    }
}