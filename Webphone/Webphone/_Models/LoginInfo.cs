using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
namespace Webphone._Models;

public partial class LoginInfo
{
    public int LoginId { get; set; }
    public string? UserName { get; set; }
    public string? Passw { get; set; } 

    public string? Email { get; set; }

    public int LoginRole { get; set; }

    public bool LoginStatus { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
