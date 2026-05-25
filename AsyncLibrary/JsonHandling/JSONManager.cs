using AsyncLibrary.BookHandling;
using AsyncLibrary.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AsyncLibrary.JsonHandling;

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
    public async Task<List<BookInfo>> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<BookInfo>>(await File.ReadAllTextAsync(_filePath), _options);
    }

    /// <summary>
    /// Saves the library to the JSON file, overwriting any existing data.
    /// </summary>
    /// <param name="library">The book collection to save.</param>
    public async Task SaveAsync(List<BookInfo> library) => await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(library, _options));
}
