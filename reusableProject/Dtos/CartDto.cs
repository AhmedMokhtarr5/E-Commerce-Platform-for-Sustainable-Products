﻿namespace reusableProject.Dtos
{
    public class CartDto
    {
       
            public int UserId { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<CartItemDto> CartItems { get; set; }  // Include CartItems if needed
        
    }
}
