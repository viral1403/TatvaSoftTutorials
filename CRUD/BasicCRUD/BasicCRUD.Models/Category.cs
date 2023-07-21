using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BasicCRUD.Models
{
    public class Category
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage="Display Order must be in the range 1 to 100 only!")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
