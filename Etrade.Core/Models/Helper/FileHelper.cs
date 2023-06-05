namespace Etrade.Core.Models.Helper
{
    public class FileHelper
    {
        static string environmentPath = Environment.CurrentDirectory + @"\wwwroot\img\";
        public static string Add(IFormFile file)
        {
            var sourcePath = Path.GetTempFileName();
            if (file.Length > 0)
            {
                using (var uploading = new FileStream(sourcePath, FileMode.Create))
                {
                    file.CopyTo(uploading);
                }
            }
            var result = NewPath(file);
            File.Move(sourcePath, environmentPath + result);
            return result;
        }
        public static bool Delete(string imageName)
        {
            string path = environmentPath + imageName;
            try
            {
                File.Delete(path);
            }
            catch (Exception exception)
            {
                return false;
            }
            return true;
        }
        public static string Update(string imageName, IFormFile file)
        {
            string sourcePath = environmentPath + imageName;
            string newImageName = NewPath(file).ToString();
            var result = environmentPath + newImageName;
            if (sourcePath.Length > 0)
            {
                using (var stream = new FileStream(result, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            try
            {
                File.Delete(sourcePath);
            }
            catch { }
            return newImageName;
        }
        private static string NewPath(IFormFile file)
        {
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileExtension = fileInfo.Extension;
            var newPath = Guid.NewGuid().ToString() + fileExtension;
            return newPath;
        }
    }
}
