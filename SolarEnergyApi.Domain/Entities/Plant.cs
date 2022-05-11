using System.ComponentModel.DataAnnotations;

namespace SolarEnergyApi.Domain.Entities
{
    public class Plant
    {
        public Plant(string nickname, string place, string brand, string model, bool active)
        {
            Nickname = nickname;
            Place = place;
            Brand = brand;
            Model = model;
            Active = active;
            Generations = new List<Generation>();
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo apelido é obrigatório")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "O campo local é obrigatório")]
        public string Place { get; set; }

        [Required(ErrorMessage = "O campo marca é obrigatório")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "O campo modelo é obrigatório")]
        public string Model { get; set; }

        [Required(ErrorMessage = "O campo ativo é obrigatório")]
        public bool Active { get; set; }
        public List<Generation> Generations { get; set; }

        public void Update(
            string nickname,
            string place,
            string brand,
            string model,
            bool active
        )
        {
            Nickname = nickname;
            Place = place;
            Brand = brand;
            Model = model;
            Active = active;
        }
    }
}
