using System;
using System.Collections.Generic;

namespace EventAssociation.Infrastructure.SqlliteDmPersistence.Models;

public partial class VeaGuest
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PictureUrl { get; set; }
}
