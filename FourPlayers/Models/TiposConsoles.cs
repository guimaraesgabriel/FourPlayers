using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FourPlayers.Models
{
    public class TiposConsoles
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public String Tipo { get; set; }
    }
}