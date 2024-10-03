using System;
using System.Collections.Generic;

namespace bus_reservation.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public string RouteName { get; set; } = null!;

    public string StartingPlace { get; set; } = null!;

    public string DestinationPlace { get; set; } = null!;

    public virtual ICollection<Bus> Buses { get; set; } = new List<Bus>();
}
