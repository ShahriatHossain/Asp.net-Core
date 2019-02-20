using Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class OrderItemStep : BaseModel
    {
        [ForeignKey("ItemStepId")]
        public ItemStep ItemStep { get; set; }

        public int ItemStepId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("ItemStepAttachmentId")]
        public ItemStepAttachment ItemStepAttachment { get; set; }

        public int ItemStepAttachmentId { get; set; }
    }
}
