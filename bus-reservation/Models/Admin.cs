using System;
using System.Collections.Generic;

namespace bus_reservation.Models;

public partial class Admin
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
