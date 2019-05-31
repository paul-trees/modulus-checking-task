using System;
using ModulusCheckingTask.Core.Entities;

namespace ModulusCheckingTask.Infrastructure.Mappers
{
    public class ModulusWeightEntityMapper : IModulusWeightEntityMapper
    {
        #region IModulusWeightEntityMapper

        public ModulusWeightEntity Create(string modulusWeightData)
        {
            if (string.IsNullOrEmpty(modulusWeightData)) throw new ArgumentException(nameof(modulusWeightData));

            var array = modulusWeightData.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length < 17 || array.Length > 18)
                throw new ArgumentOutOfRangeException(nameof(modulusWeightData));

            return MapToModulusWeightEntity(array);
        }

        #endregion

        #region Private Methods

        private ModulusWeightEntity MapToModulusWeightEntity(string[] array)
        {
            var entity = new ModulusWeightEntity
            {
                SortCodeRangeStart = int.Parse(array[0]),
                SortCodeRangeEnd = int.Parse(array[1]),
                ModCheck = array[2],
                WeightU = int.Parse(array[3]),
                WeightV = int.Parse(array[4]),
                WeightW = int.Parse(array[5]),
                WeightX = int.Parse(array[6]),
                WeightY = int.Parse(array[7]),
                WeightZ = int.Parse(array[8]),
                WeightA = int.Parse(array[9]),
                WeightB = int.Parse(array[10]),
                WeightC = int.Parse(array[11]),
                WeightD = int.Parse(array[12]),
                WeightE = int.Parse(array[13]),
                WeightF = int.Parse(array[14]),
                WeightG = int.Parse(array[15]),
                WeightH = int.Parse(array[16])
            };

            if (array.Length == 18)
                entity.ExceptionCode = array[17];

            return entity;
        }

        #endregion
    }
}
