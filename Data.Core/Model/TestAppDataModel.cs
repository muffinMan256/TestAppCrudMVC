using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Core.Model
{
    [Table("Personal")]
    public class Personal
    {
        [Key]
        public long Id { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool Valid { get; set; }
        public byte[]? Image { get; set; }
        public int? Departament { get; set; }
        public Guid UserId { get; set; }
        public string FullName => Nume + " " + Prenume;
    }


}
