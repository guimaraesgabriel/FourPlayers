using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class JogosCategorias
    {
        [Key]
        public int Id { get; set; }

        //FK
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categorias Categorias { get; set; }


        public int JogoId { get; set; }

        [ForeignKey("JogoId")]
        public virtual Jogos Jogos { get; set; }
    }
}