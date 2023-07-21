using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesCRUD.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display Order must be in the range of 1 to 100")]
        public int DisplayOrder { get; set; }
    }
}
