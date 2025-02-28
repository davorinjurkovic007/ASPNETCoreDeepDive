﻿using System.ComponentModel.DataAnnotations;

namespace Globomantics.Web.Models;

public class CustomerModel
{
    [Required]
    [EmailAddress]
    public required string Email { get; init; }
    [Required]
    public required string Name { get; init; }
    [Required]
    public required string ShippingAddress { get; init; }
    [Required]
    public required string City { get; init; }
    [Required]
    public required string PostalCode { get; init; }
    [Required]
    public required string Country { get; init; }
}
