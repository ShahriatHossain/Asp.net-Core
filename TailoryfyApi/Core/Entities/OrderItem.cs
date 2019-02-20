using Core.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class OrderItem : BaseModel
    {
        public DateTime? ActualDate { get; set; }

        public DateTime? EstimationDate { get; set; }

        public decimal? SleevesLength { get; set; }

        public decimal? ShoulderWidth { get; set; }

        public decimal? ChestAround { get; set; }

        public decimal? Stomach { get; set; }

        public decimal? Waist { get; set; }

        public decimal? BreastPoint { get; set; }

        public decimal? Hips { get; set; }

        public decimal? BicepAround { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }

        public int ItemId { get; set; }
    }
}
