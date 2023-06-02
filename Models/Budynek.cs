using System.ComponentModel.DataAnnotations;

namespace MvcPracownik.Models
{
    /// <summary>
    /// Class representation of Building 
    /// </summary>


    public class Budynek
    {
        /// <summary>
        /// Id_budynku with auto incrementation 
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        public int Id_budynku { get; set; }

        /// <summary>
        /// Name = "Nazwa budynku"
        /// </summary>
        [Display(Name = "Nazwa budynku")]
        public String Nazwa { get; set; }
        public ICollection<Zajecia> ?Zajecia { get; set; }
    }
}
