# AsyncLibrary

A console-based library management application built with **.NET 10**, demonstrating async/await patterns, concurrent access control via `SemaphoreSlim`, and JSON persistence.

---

## Project Structure

```
AsyncLibrary/
├── BookHandling/
│   ├── BookInfo.cs                     # Book model (Id, ShelfId, Title, Author, Published, Status)
│   ├── BookManager.cs                  # Core library operations (add, remove, search, borrow, return)
│   └── BookStatus.cs                   # Enum: Available | Borrowed
├── Interfaces/
│   ├── IBookManager.cs                 # Contract for all book management operations
│   └── IBookRepository.cs              # Contract for load/save persistence
├── JsonHandling/
│   └── JSONManager.cs                  # JSON file persistence (library.json)
├── TestCase/
│   └── BookManagerConcurrencyTest.cs   # Concurrent stress test harness
├── UI/
│   └── ConsoleHelper.cs                # Colored console output + async input
└── Program.cs                          # Entry point and interactive menu
```

---

## Features

### Library Operations
| Option | Operation     | Description                                              |
|--------|---------------|----------------------------------------------------------|
| `1`    | Add book      | Prompts for title, author, year — assigns next ShelfId   |
| `2`    | Remove book   | Removes a book by ShelfId                                |
| `3`    | Search book   | Finds books by author name or title                      |
| `4`    | Show all      | Lists every book with status                             |
| `5`    | Borrow book   | Marks an available book as Borrowed                      |
| `6`    | Return book   | Marks a borrowed book as Available                       |
| `7`    | Test          | Runs the concurrent stress test (see below)              |
| `0`    | Exit          | Exits the application                                    |

### Async I/O
- All library operations are fully `async`/`await` — no blocking calls.
- Console input uses `Console.In.ReadLineAsync()` via `ConsoleHelper.ReadInputAsync()`.

### Concurrency Control
`BookManager` uses a `SemaphoreSlim` (configurable initial and max capacity) to serialize write access to the in-memory library and the JSON file, preventing race conditions under concurrent load.

### JSON Persistence
Books are persisted to `library.json` in the working directory after every mutating operation. Enum values (`Available`/`Borrowed`) are stored as readable strings via `JsonStringEnumConverter`.

---

## Concurrency Stress Test (`Option 7`)

`BookManagerConcurrencyTest` lets you simulate multiple concurrent users hitting the library at once.

**When prompted:**
1. `Enter number of tasks:` — total number of concurrent operations to run
2. `Enter SemaphoreSlim concurrency limit:` — how many tasks may execute simultaneously

Each task randomly picks one of six operations:

| Operation  | Details                                      |
|------------|----------------------------------------------|
| `ADD`      | Random author, title, year (1900–2024)       |
| `REMOVE`   | Random book from current library             |
| `SEARCH`   | Random author or title string                |
| `SHOW ALL` | Reads full library                           |
| `BORROW`   | Random book with `Available` status          |
| `RETURN`   | Random book with `Borrowed` status           |

All tasks are started with `Task.WhenAll` and gated by the `SemaphoreSlim` you configured.

**Example output:**
```
Starting 10 tasks with concurrency limit 3...
[Task 2] ADD "The Iron Crown" by Jane Austen (1984)
[Task 5] BORROW ShelfId 1 "Echoes in the Dark"
[Task 8] SHOW ALL 4 book(s) in library
[Task 1] REMOVE ShelfId 0 "The Last Horizon"
...
All tasks completed.
```

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```
powershell
dotnet run
```

### Build

```
powershell
dotnet build
```

---

## Key Classes

### `BookManager`
```
BookManager(IBookRepository repository, int initialCapacity = 1, int maxCapacity = 1)
```
- Holds the in-memory library (`List<BookInfo>`)
- All public methods are `async` and guarded by `SemaphoreSlim`
- Persists changes via `IBookRepository.SaveAsync()` after every mutation

### `JSONManager`
- Implements `IBookRepository`
- Reads/writes `library.json` using `System.Text.Json`
- Returns an empty list if the file does not exist yet

### `ConsoleHelper`
- Static helper for colored console output (`PrintHeader`, `PrintSuccess`, `PrintError`, `PrintInfo`, `PrintMenuOption`)
- `ReadInputAsync(string prompt)` — async console input

---

## Architecture

```
Program.cs
	│
	├── BookManager (IBookManager)
	│       ├── SemaphoreSlim  ← concurrency gate
	│       └── JSONManager (IBookRepository)  ← persistence
	│
	└── BookManagerConcurrencyTest
			└── IBookManager  ← uses same BookManager instance
```
