using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation_Project.Models
{
    public class LearningMaterial
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Url { get; set; }


        // Navigation Properties
        public ICollection<CourseMaterial> CourseMaterials { get; set; } = new List<CourseMaterial>();
        public ICollection<CompletedMaterial> CompletedMaterials { get; set; } = new List<CompletedMaterial>();
    }
}