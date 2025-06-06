﻿using System.ComponentModel.DataAnnotations;

namespace OrderTrak.API.Models.DTO.Receiving
{
    public class ReceivingCreateDTO
    {
        [Required(ErrorMessage = "Tracking Number is required.")]
        public string TrackingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Carrier is required.")]
        public string Carrier { get; set; } = string.Empty;
    }
}
