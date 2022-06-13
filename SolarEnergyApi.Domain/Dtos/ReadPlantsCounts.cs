using SolarEnergyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadPlantsCounts
    {
        public ReadPlantsCounts(int activePlants, int inactivePlants)
        {
            ActivePlants = activePlants;
            InactivePlants = inactivePlants;
        }
        public int ActivePlants { get; set; }
        public int InactivePlants { get; set; }
    }
}
