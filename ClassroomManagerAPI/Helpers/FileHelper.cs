namespace ClassroomManagerAPI.Helpers
{
    public static class FileHelper
    {
        public static string saveFile (IFormFile file) {
            try
            {
                var fileName = file.FileName;
                var extensionName = Path.GetExtension(fileName);
                while(true)
                {
                    string newFilename = Path.Join(Guid.NewGuid().ToString(),".",extensionName) ;
                    if (File.Exists(newFilename))
                    {
                        continue;
                    }
                    fileName = Path.Combine(Directory.GetCurrentDirectory(),"Public",newFilename);
                    using(var stream = new FileStream(fileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return fileName;
                }
            }catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static FileStream getFile(string fileName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Public", fileName);
            if(!File.Exists(imagePath)) return null;
            return File.OpenRead(imagePath);
        }
    }
}
