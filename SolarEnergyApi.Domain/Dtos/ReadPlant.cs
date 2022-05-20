using SolarEnergyApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarEnergyApi.Domain.Dtos
{
    public class ReadPlant
    {
        public ReadPlant(Plant plant)
        {
            Id = plant.Id;
            Nickname = plant.Nickname;
            Place = plant.Place;
            Brand = plant.Brand;
            Model = plant.Model;
            Active = plant.Active;
        }
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Place { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool Active { get; set; }
    }
}
