using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Imię")]
        public String Imie { get; set; }
        [Display(Name = "Nazwisko")]
        public String Nazwisko { get; set; }

        [Display(Name = "Data rozpoczecia studiow")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataZatrudnienia { get; set; }
        public ICollection<Zajecia> ?Zajecia { get; set; }
    }
}