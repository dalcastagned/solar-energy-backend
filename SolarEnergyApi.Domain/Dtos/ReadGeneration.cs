using SolarEnergyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadGeneration
    {
        public ReadGeneration(Generation generation)
        {
            Id = generation.Id;
            Date = generation.Date;
            GeneratePower = generation.GeneratePower;
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double GeneratePower { get; set; }
    }
}
