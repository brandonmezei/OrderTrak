﻿namespace OrderTrak.API.Models.DTO.Order
{
    public class OrderSearchDTO : SearchQueryDTO
    {
        public bool ShowCancel { get; set; }
        public bool ShowShipped { get; set; }
        public bool PickingOnly { get; set; }
        public bool ShipReadyOnly { get; set; }
    }
}
