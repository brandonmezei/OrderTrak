﻿using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Project
{
    public class ProjectUpdateDTO
    {
        [Required]
        public Guid? FormID { get; set; }

        [Required(ErrorMessage = "Project Code is required.")]
        [MaxLength(4)]
        public string? ProjectCode { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        public string? ProjectName { get; set; }

        [Required(ErrorMessage = "Contact Name is required.")]
        public string? ContactName { get; set; }

        [Required(ErrorMessage = "Contact Phone is required.")]
        public string? ContactPhone { get; set; }

        public string? ContactEmail { get; set; }

        public string? StakeHolderEmail { get; set; }
        public string? UDF1 { get; set; }
        public string? UDF2 { get; set; }
        public string? UDF3 { get; set; }
        public string? UDF4 { get; set; }
        public string? UDF5 { get; set; }
        public string? UDF6 { get; set; }
        public string? UDF7 { get; set; }
        public string? UDF8 { get; set; }
        public string? UDF9 { get; set; }
        public string? UDF10 { get; set; }
        public string? OrderUDF1 { get; set; }
        public string? OrderUDF2 { get; set; }
        public string? OrderUDF3 { get; set; }
        public string? OrderUDF4 { get; set; }
        public string? OrderUDF5 { get; set; }
        public string? OrderUDF6 { get; set; }
        public string? OrderUDF7 { get; set; }
        public string? OrderUDF8 { get; set; }
        public string? OrderUDF9 { get; set; }
        public string? OrderUDF10 { get; set; }
    }
}
