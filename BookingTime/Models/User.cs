using System;
using System.Collections.Generic;

namespace BookingTime.Models;

public partial class User
{
    public long Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public bool? IsVerified { get; set; }

    public string? VerificationToken { get; set; }

    public DateTime? TokenExpireTime { get; set; }
}
