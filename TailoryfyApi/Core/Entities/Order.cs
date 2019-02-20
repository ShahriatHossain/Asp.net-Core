using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Order : BaseModel
    {
        [Required]
        [MaxLength(450)]
        public string CustomerId { get; set; }

        [MaxLength(50)]
        public string OrderCode { get; set; }

        public DateTime CreatedDate { get; set; }

        [MaxLength(50)]
        public string OrderStatus { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public int? ReceiverId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
