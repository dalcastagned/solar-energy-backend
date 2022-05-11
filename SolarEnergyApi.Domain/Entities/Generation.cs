using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolarEnergyApi.Domain.Entities
{
    public class Generation
    {
        public Generation(DateTime date, double generatePower, int idPlant)
        {
            Date = date;
            GeneratePower = generatePower;
            IdPlant = idPlant;
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "O campo potência gerada é obrigatório")]
        public double GeneratePower { get; set; }

        [JsonIgnore]
        [Required]
        public int IdPlant { get; set; }
    }
}
