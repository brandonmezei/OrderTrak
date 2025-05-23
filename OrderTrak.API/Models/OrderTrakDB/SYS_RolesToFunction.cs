﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderTrak.API.Models.OrderTrakDB
{
    public class SYS_RolesToFunction : CommonObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]      
        public int RoleID { get; set; }

        [Required]      
        public int FunctionID { get; set; }

        [Required]
        public bool CanAccess { get; set; }

        [ForeignKey("RoleID")]
        public virtual SYS_Roles SYS_Roles { get; set; } = null!;

        [ForeignKey("FunctionID")]
        public virtual SYS_Function SYS_Function { get; set; } = null!;
    }
}
