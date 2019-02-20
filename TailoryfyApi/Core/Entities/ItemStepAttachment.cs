using Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ItemStepAttachment : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Image { get; set; }

        [ForeignKey("ItemStepId")]
        public ItemStep ItemStep { get; set; }

        public int ItemStepId { get; set; }
    }
}
