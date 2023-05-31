using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Pracownik
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Imię")]
        public String Imie { get; set; }
        [Display(Name = "Nazwisko")]
        public String Nazwisko { get; set; }

        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataZatrudnienia { get; set; }
        public Zajecia? Zajecia { get; set; }

    }
}