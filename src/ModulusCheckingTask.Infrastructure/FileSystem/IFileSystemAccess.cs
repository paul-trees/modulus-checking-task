namespace ModulusCheckingTask.Infrastructure.FileSystem
{
    public interface IFileSystemAccess
    {
        string[] ReadAllLines(string path);
    }
}
