using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace GHSTShipping.Application.Helpers
{
    public static class JsonFileHelper
    {
        public static async Task SaveDeliveryAddress<T>(List<T> list, string folderName = "metadata", string fileName = "districts.json")
        {
            // Ensure the folder exists
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            // Construct the full file path within the metadata folder
            string filePath = Path.Combine(folderName, fileName);

            // Options to format JSON with indentation (optional)
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            // Serialize the list to a JSON string
            string jsonString = JsonSerializer.Serialize(list, options);

            // Write the JSON string to the file
            await File.WriteAllTextAsync(filePath, jsonString);
        }

        public static async Task<List<T>> GetDeliveryAddressAsync<T>(string folderName = "metadata", string fileName = "districts.json")
        {
            // Construct the full file path within the metadata folder
            string filePath = Path.Combine(folderName, fileName);

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            // Read the JSON string from the file
            string jsonString = await File.ReadAllTextAsync(filePath);

            // Deserialize the JSON string back into a list of objects
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}
