using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace NumMayor6011464
{
    [Table("resul")]
    public class Resul
    {

        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("numero1")]
        public string? Numero1 { get; set; }

        [Column("numero2")]
        public string? Numero2 { get; set; }

        [Column("suma")]
        public string? Suma { get; set; }
    }
}