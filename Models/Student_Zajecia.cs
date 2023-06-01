using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    public class Student_Zajecia
    {

        [Key]
        public int Student_Zajecia_ID { get; set; }
        [Display(Name = "Zajęcia")]
        public Zajecia? zajecia { get; set; }
        [Display(Name = "Student")]
        public Student? student { get; set; }
    }
}
