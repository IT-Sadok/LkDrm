using ConsoleLibrary.BookHandling;
using ConsoleLibrary.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleLibrary.JsonHandling;

/// <summary>
/// Handles loading and saving the library's book collection to and from a JSON file.
/// </summary>
public class JSONManager : IBookRepository
{
    private readonly string _filePath = "library.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    /// <summary>
    /// Loads the library from the JSON file. Returns an empty dictionary if the file does not exist.
    /// </summary>
    /// <returns>A dictionary of books keyed by their shelf ID.</returns>
    public List<BookInfo> Load()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<BookInfo>>(File.ReadAllText(_filePath), _options);
    }

    /// <summary>
    /// Saves the library to the JSON file, overwriting any existing data.
    /// </summary>
    /// <param name="library">The book collection to save.</param>
    public void Save(List<BookInfo> library) => File.WriteAllText(_filePath, JsonSerializer.Serialize(library, _options));
}
