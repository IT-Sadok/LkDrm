# 📚 ConsoleLibrary

A console-based library management application built with **.NET 10 (C#)**. It allows users to manage a collection of books — adding, searching, borrowing, and returning them — with data persisted to a local JSON file.

---

## 🚀 How It Works

When the application starts, it loads the existing library from a `library.json` file (or creates an empty one if it doesn't exist yet). The user is then presented with an interactive text-based menu in the console.

After every operation that modifies the library (add, borrow, return), the changes are automatically saved back to the JSON file.

### Application Flow

```
Start
  └── Load library from library.json
		└── Show Main Menu
			  ├── 1. Add new book       → Input title, author, year → Save
			  ├── 2. Book information   → Search by author or title → Display
			  ├── 3. List of books      → Display all books
			  ├── 4. Borrow book        → Input shelf ID → Mark as Borrowed
			  ├── 5. Return book        → Input shelf ID → Mark as Available
			  └── 6. Exit              → Close application
```

---

## 🗂️ Project Structure

```
ConsoleLibrary/
│
├── Program.cs                     # Entry point, main menu loop
│
├── BookHandling/
│   ├── BookInfo.cs                # Book model (title, author, year, status, ID)
│   ├── BookManager.cs             # Core logic for managing books
│   └── BookStatus.cs             # Enum: Available / Borrowed
│
├── Interfaces/
│   ├── IBookManager.cs            # Interface for BookManager
│   └── IJSONManager.cs            # Interface for JSONManager
│
├── JsonHandling/
│   └── JSONManager.cs             # Load/save library data to JSON
│
└── UI/
	└── ConsoleHelper.cs           # Console output helpers (colors, prompts)
```

---

## 📦 Models

### `BookInfo`

Represents a single book in the library.

| Property   | Type         | Description                          |
|------------|--------------|--------------------------------------|
| `UniqId`   | `Guid`       | Unique identifier for the book       |
| `Title`    | `string`     | Title of the book                    |
| `Author`   | `string`     | Author of the book                   |
| `Published`| `int`        | Year the book was published          |
| `Status`   | `BookStatus` | Current availability status          |

### `BookStatus` (Enum)

| Value       | Description                        |
|-------------|------------------------------------|
| `Available` | The book is on the shelf           |
| `Borrowed`  | The book has been borrowed         |

---

## ⚙️ Key Operations

### Add a Book
- Prompts for title, author, and publication year
- Assigns a shelf ID and a unique `Guid`
- Sets initial status to `Available`

### Book Information
- Searches the library by **author name** or **title** (exact match)
- Displays all matching books with full details

### List All Books
- Prints every book in the library with shelf ID, author, title, status, year, and unique ID

### Borrow a Book
- Requires the shelf ID
- Only succeeds if the book's status is `Available`
- Changes status to `Borrowed`

### Return a Book
- Requires the shelf ID
- Changes status back to `Available`

---

## 💾 Data Persistence

The library is stored in a `library.json` file in the application's working directory.

- **Format:** JSON (human-readable, indented)
- **Enum serialization:** Stored as strings (e.g., `"Available"`, `"Borrowed"`)
- **Auto-save:** After every mutating operation in the main menu loop

Example `library.json`:

```json
{
  "0": {
	"uniqId": "a1b2c3d4-...",
	"title": "Clean Code",
	"author": "Robert C. Martin",
	"published": 2008,
	"status": "Available"
  }
}
```

---

## 🖥️ Technologies Used

| Technology              | Purpose                                         |
|-------------------------|-------------------------------------------------|
| **.NET 10**             | Target framework                                |
| **C# 13**               | Primary programming language                    |
| `System.Text.Json`      | JSON serialization and deserialization          |
| `JsonStringEnumConverter` | Serialize enums as readable strings in JSON   |
| `Dictionary<int, BookInfo>` | In-memory storage structure for the library |
| `Guid`                  | Unique book identification                      |
| `Console` (System)      | User interface via colored console output       |

---

## 🎨 Console UI Colors

The `ConsoleHelper` class uses colored output to improve readability:

| Method            | Color    | Usage                        |
|-------------------|----------|------------------------------|
| `PrintHeader`     | Cyan     | Section titles and headers   |
| `PrintSuccess`    | Green    | Successful operation messages|
| `PrintError`      | Red      | Error and warning messages   |
| `PrintInfo`       | Yellow   | Informational messages       |
| `PrintMenuOption` | White    | Menu items                   |

---

## 🔧 Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## ▶️ Running the App

```powershell
dotnet run --project ConsoleLibrary
```
