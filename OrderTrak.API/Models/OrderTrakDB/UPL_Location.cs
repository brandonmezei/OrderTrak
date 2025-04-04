﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class UPL_Location : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("UPL_UOM")]
        public int UOMID { get; set; }


        [Required]
        public string LocationNumber { get; set; } = string.Empty;

        [Required]
        public decimal Height { get; set; } = 0;

        [Required]
        public decimal Width { get; set; } = 0;

        [Required]
        public decimal Depth { get; set; } = 0;

        public virtual UPL_UOM UPL_UOM { get; set; } = null!;

        public virtual ICollection<INV_Stock> INV_Stock { get; set; } = [];

    }
}
