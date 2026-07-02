using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class MenuCategory
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public byte[] Image { get; set; } = Array.Empty<byte>();

    public MenuCategory() { }
}

public class MenuPositionImage
{
    public int Id { get; set; }
    public byte[] Image { get; set; } = [];
    
    //внешний ключ
    public int MenuPositionId { get; set; }
    public MenuPosition? MenuPosition { get; set; }

    public MenuPositionImage() { }
}
public class MenuPosition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    
    //внешний ключ
    public int MenuCategoryId { get; set; }
    public MenuCategory? MenuCategory { get; set; }
    
    public MenuPosition() { }
}

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        // Получаем список всех изображений в папке "images" (в корне проекта)
        string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "images");

        // Получаем все файлы в папке "images", исключая .DS_Store
        string[] imageFiles = Directory.GetFiles(imagesDirectory)
            .Where(file => !file.EndsWith(".DS_Store"))  // Исключаем файл .DS_Store
            .ToArray();

        // Сортировка файлов по числовой части имени файла
        var sortedImageFiles = imageFiles
            .OrderBy(path => int.Parse(Path.GetFileNameWithoutExtension(path)))
            .ToArray();
        foreach (var imageFile in sortedImageFiles)
        {
            Console.WriteLine($"Processing {imageFile}");
        }
        
        int menuPositionId = 1131; // Начальный идентификатор позиции меню
        int counter = 0; // Счётчик для подсчёта количества отправленных фотографий

        foreach (string imagePath in sortedImageFiles)
        {
            byte[] imageBytes;

            // Загружаем изображение
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    // Сохраняем изображение в поток в формате JPEG
                    image.Save(ms, new JpegEncoder()); // Сохраняем в JPEG формат
                    imageBytes = ms.ToArray(); // Получаем массив байтов
                }
            }

            var menuPositionImage = new MenuPositionImage()
            {
                Image = imageBytes,
                MenuPositionId = menuPositionId
            };

            var json = JsonConvert.SerializeObject(menuPositionImage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:8080/api/menu-position-images", content);
                response.EnsureSuccessStatusCode(); // Вызывает исключение, если статус код не 2xx

                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Image {Path.GetFileName(imagePath)} uploaded successfully.");

                counter++; // Увеличиваем счётчик отправленных фотографий

                // Инкрементируем MenuPositionId после 3-х отправленных фотографий
                menuPositionId++;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error uploading {Path.GetFileName(imagePath)}: {e.Message}");
            }
        }
        
    }
}


