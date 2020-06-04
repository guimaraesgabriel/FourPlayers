using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class TiposContas
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }


        //FK


        //ICOLLECTIONS
        public ICollection<JogosContas> JogosContas { get; set; }
    }
}