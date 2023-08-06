namespace LibraryV2.Utilities
{
    public static class FileSystemHelper
    {
        public static string GetDirectoryName()
        {
            return Directory.GetCurrentDirectory();
        }

        public static string GetContentDirectory()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\Content\\");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public static string GetFilePath(string fileName)
        {
            return Path.Combine(GetContentDirectory(), fileName);
        }
    }
}
