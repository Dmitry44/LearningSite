﻿using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public AppUser User { get; set; } = new();

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; } = new();
    }
}
