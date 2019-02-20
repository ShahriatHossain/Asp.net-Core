using Core.Entities;
using System.Collections.Generic;

namespace Core.Dtos
{
    public class ItemDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string ItemCode { get; set; }

        public ICollection<ItemStep> ItemSteps { get; set; }
            = new List<ItemStep>();
    }
}
