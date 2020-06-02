using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class JogosImagens
    {
        [Key]
        public int Id;

        [MaxLength(255)]
        public string URL{ get; set; }


        //FK
        public int JogoId { get; set; }

        [ForeignKey("JogoId")]
        public virtual Jogos Jogos { get; set; }
    }
}