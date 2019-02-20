using Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class OrderReceiver : BaseModel
    {
        public string ReceiverId { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public bool? IsConfirmed { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
