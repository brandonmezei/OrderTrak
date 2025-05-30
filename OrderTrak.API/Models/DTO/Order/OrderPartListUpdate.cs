﻿using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderPartListUpdate
    {
        [Required]
        public Guid FormID { get; set; }
        public Guid? POID { get; set; }
        public Guid? StockGroupID { get; set; }
        public string? SerialNumber { get; set; }
        public int Quantity { get; set; }
    }
}
