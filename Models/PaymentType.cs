﻿namespace consoleshoppen.Models
{
    public class PaymentType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}