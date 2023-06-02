using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    /// <summary>
    /// Class representation of Lectures on University
    /// </summary>
    public class Zajecia
    {

        /// <summary>
        /// Id_budynku with auto incrementation 
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        public int Id_zajec { get; set; }
         
        /// <summary>
        /// Name = "Nazwa zajęć"
        /// </summary>
        [Display(Name = "Nazwa zajęć")]
        public String Nazwa { get; set; }
        /// <summary>
        /// Name = "Budynek"
        /// </summary>
        [Display(Name = "Budynek")]
        public Budynek? Budynek { get; set; }        
        public ICollection<Student_Zajecia> ?student_Zajecia { get; set; }
        public ICollection<Pracownik> ?Pracownicy { get; set; }
    }
}
