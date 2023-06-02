using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    /// <summary>
    /// Class representation of Student 
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Id of student with auto incrementation 
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name 
        /// </summary>
        [Display(Name = "Imię")]
        public String Imie { get; set; }
        /// <summary>
        /// Surname 
        /// </summary>
        [Display(Name = "Nazwisko")]
        public String Nazwisko { get; set; }
        /// <summary>
        /// DataOfStudiesStart 
        /// </summary>

        [Display(Name = "Data rozpoczecia studiow")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataOfStudiesStart { get; set; }
        public ICollection<Student_Zajecia> ?student_Zajecia { get; set; }
    }
}