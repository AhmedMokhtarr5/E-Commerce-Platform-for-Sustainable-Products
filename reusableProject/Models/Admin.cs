using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Admin
{
    [Key]
    public int AdminId { get; set; }
    public string ?Username { get; set; }
    public string ?Password { get; set; }
}
