using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadMonthGeneration
    {
        public ReadMonthGeneration(string month, double generation)
        {
            Month = month;
            Generation = generation;
        }
        public string Month { get; set; }
        public double Generation { get; set; }
    }
}
