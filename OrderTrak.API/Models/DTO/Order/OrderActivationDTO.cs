﻿ namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderActivationDTO
    {
        public Guid FormID { get; set; }
        public Guid? StatusID { get; set; }
        public string? OrderNote { get; set; }
    }
}
