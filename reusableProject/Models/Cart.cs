using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


public class Cart
{
    [Key]
    public int CartId { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public decimal TotalPrice { get; set; }
    DateTime CreatedAt { get; set; }

    // Navigation Property
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

