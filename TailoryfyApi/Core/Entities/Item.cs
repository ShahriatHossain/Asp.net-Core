using Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Item : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [MaxLength(150)]
        public string Image { get; set; }

        [MaxLength(50)]
        public string ItemCode { get; set; }

        public ICollection<ItemStep> ItemSteps { get; set; }
            = new List<ItemStep>();
    }
}
