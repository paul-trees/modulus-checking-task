using System.IO;

namespace ModulusCheckingTask.Infrastructure.FileSystem
{
    public class FileSystemAccess : IFileSystemAccess
    {
        #region IFileSystemAccess

        public string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        #endregion
    }
}
