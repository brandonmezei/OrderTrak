﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class INV_StockStatus : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string StockStatus { get; set; } = string.Empty;

        public virtual ICollection<INV_Stock> INV_Stock { get; set; } = [];
    }
}
