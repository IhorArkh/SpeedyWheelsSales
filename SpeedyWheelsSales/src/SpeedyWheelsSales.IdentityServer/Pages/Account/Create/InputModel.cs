// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace SpeedyWheelsSales.IdentityServer.Pages.Account.Create;

public class InputModel
{
    [Required]
    [MaxLength(50, ErrorMessage = "Username must be at most 50 characters long")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
    public string? Username { get; set; }

    [Required]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters long")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
    public string? Name { get; set; }

    [Required]
    [RegularExpression(@"^\d{10,14}$", ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).{8,}$", ErrorMessage =
        "Password must be at least 8 characters long, contain 1 capital letter and 1 special character.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }

    public string? ReturnUrl { get; set; }

    public string? Button { get; set; }
}