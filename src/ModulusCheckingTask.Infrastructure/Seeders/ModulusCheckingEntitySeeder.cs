using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Infrastructure.FileSystem;
using ModulusCheckingTask.Infrastructure.Mappers;

namespace ModulusCheckingTask.Infrastructure.Seeders
{
    public class ModulusCheckingEntitySeeder : IModulusCheckingEntitySeeder
    {
        #region Fields

        private readonly ILogger<IModulusCheckingEntitySeeder> _logger;
        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly IModulusWeightRepository _modulusWeightRepository;
        private readonly IModulusWeightEntityMapper _modulusWeightEntityMapper;

        #endregion

        #region Constructor

        public ModulusCheckingEntitySeeder(ILogger<IModulusCheckingEntitySeeder> logger, IFileSystemAccess fileSystemAccess, IModulusWeightRepository modulusWeightRepository, 
            IModulusWeightEntityMapper modulusWeightEntityMapper)
        {
            _logger = logger;
            _fileSystemAccess = fileSystemAccess;
            _modulusWeightRepository = modulusWeightRepository;
            _modulusWeightEntityMapper = modulusWeightEntityMapper;
        }

        #endregion

        #region IModulusCheckingEntitySeeder

        public void Execute()
        {
            if (_modulusWeightRepository.Count() > 0) return;

            var fullDataFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Resources\valacdos.txt");
            _logger.LogDebug($"Attempting to read file '{fullDataFilePath}'...");
            var fileContent = _fileSystemAccess.ReadAllLines(fullDataFilePath);

            _logger.LogDebug($"File read successfully ({fileContent.Length} lines). Attempting to transform and load data...");
            foreach (var line in fileContent)
            {                
                _modulusWeightRepository.Insert(_modulusWeightEntityMapper.Create(line));
            }

            _logger.LogDebug("All lines processed. Attempting to save the entities to the database...");
            _modulusWeightRepository.SaveChanges();
        }

        #endregion
    }
}
