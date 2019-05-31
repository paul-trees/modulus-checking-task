using System.Linq;
using Microsoft.Extensions.Logging;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Infrastructure.FileSystem;
using ModulusCheckingTask.Infrastructure.Mappers;
using ModulusCheckingTask.Infrastructure.Seeders;
using NSubstitute;
using Xunit;

namespace ModulusCheckingTasp.Infrastructure.UnitTests.Seeders
{
    public class ModulusCheckingEntitySeederTests
    {
        #region Fields

        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly IModulusWeightRepository _modulusWeightRepository;
        private readonly IModulusWeightEntityMapper _modulusWeightEntityMapper;

        private readonly ModulusCheckingEntitySeeder _sut;

        #endregion

        #region Constructor

        public ModulusCheckingEntitySeederTests()
        {
            _fileSystemAccess = Substitute.For<IFileSystemAccess>();
            _modulusWeightRepository = Substitute.For<IModulusWeightRepository>();
            _modulusWeightEntityMapper = Substitute.For<IModulusWeightEntityMapper>();

            _sut = new ModulusCheckingEntitySeeder(Substitute.For<ILogger<IModulusCheckingEntitySeeder>>(), _fileSystemAccess,
                _modulusWeightRepository, _modulusWeightEntityMapper);
        }

        #endregion

        #region Tests

        [Fact]
        public void Execute_ReturnsImmediatelyAsModulusWeightsAlreadyLoaded()
        {
            // Arrange
            _modulusWeightRepository.Count().Returns(10);

            // Act
            _sut.Execute();

            // Assert
            _fileSystemAccess.DidNotReceiveWithAnyArgs().ReadAllLines(default(string));
        }

        [Fact]
        public void Execute_LoadsFileAndSavesToDatabase()
        {
            // Arrange
            var lineArray = new[] {"1", "2", "3", "4", "5"};
            _fileSystemAccess.ReadAllLines(Arg.Any<string>()).Returns(lineArray);

            // Act
            _sut.Execute();

            // Assert
            _fileSystemAccess.Received(1).ReadAllLines(Arg.Is<string>(s => s.EndsWith(@"Resources\valacdos.txt")));
            _modulusWeightEntityMapper.Received(5).Create(Arg.Is<string>(s => lineArray.Contains(s)));
            _modulusWeightRepository.Received(1).SaveChanges();
        }

        #endregion
    }
}
