using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdSrvr4Demo.Endpoints.Account
{
  public class RegisterViewModel
  {
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    // public string IdNumber { get; set; }
    public string FavouriteTeam { get; set; }

    public SelectList Teams {get; set;}
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    [Required]
    //[RegularExpression(@"^([\+]?27[-]?|[0])?[1-9][0-9]{12}$")]
    public string PhoneNumber { get; set; }
    public string Country { get; set; }
    public string Team { get; set; }
    public string Gender { get; set; }
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }

  }
}
