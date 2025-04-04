﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_Function : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string FunctionName { get; set; } = string.Empty;

        public virtual ICollection<SYS_RolesToFunction> SYS_RolesToFunction { get; set; } = [];
    }
}
