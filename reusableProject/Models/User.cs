using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int UserId { get; set; }
    public string ?Name { get; set; }
    public string ?Email { get; set; }
    public string? password { get; set; }
    public string? Area { get; set; }
    public string ?Address { get; set; }
    public string ?MobileNumber { get; set; }

    // Navigation Properties
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

