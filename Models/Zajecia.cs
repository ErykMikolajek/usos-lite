using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Zajecia
    {
        [Key]
        [Display(Name = "Id")]
        public int Id_zajec { get; set; }
        [Display(Name = "Nazwa zespołu")]
        public String Nazwa { get; set; }
        public Budynek? Budynek { get; set; }
        public ICollection<Student> ?Studenci { get; set; }
        public ICollection<Pracownik> ?Pracownicy { get; set; }
    }
}
