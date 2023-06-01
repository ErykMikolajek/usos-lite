using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Dane
    {
        [Key]
        public int Id { get; set; }
        public String Tekst { get; set; }

    }
}