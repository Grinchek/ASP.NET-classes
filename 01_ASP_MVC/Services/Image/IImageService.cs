namespace _01_ASP_MVC.Services.Image
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(IFormFile file, string path);
        bool DeleteFile(string path);
    }
}
