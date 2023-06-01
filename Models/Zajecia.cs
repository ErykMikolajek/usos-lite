using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Zajecia
    {
        [Key]
        [Display(Name = "Id")]
        public int Id_zajec { get; set; }
        [Display(Name = "Nazwa zajęć")]
        public String Nazwa { get; set; }
        [Display(Name = "Budynek")]
        public Budynek? Budynek { get; set; }        
        public ICollection<Student_Zajecia> ?student_Zajecia { get; set; }
        public ICollection<Pracownik> ?Pracownicy { get; set; }
    }
}
