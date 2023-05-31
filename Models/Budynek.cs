using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Budynek
    {
        [Key]
        [Display(Name = "Id")]
        public int Id_zajec { get; set; }
        [Display(Name = "Nazwa budynku")]
        public String Nazwa { get; set; }
        public ICollection<Zajecia> ?Zajecia { get; set; }
    }
}
