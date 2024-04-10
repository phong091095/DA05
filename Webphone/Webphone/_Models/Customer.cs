using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webphone._Models;

public partial class Customer
{
    public int CustId { get; set; }

    [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
    public string? CustName { get; set; }

    [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? CustPhone { get; set; }

    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string? CustMail { get; set; }

    [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
    public string? CustAd { get; set; }

    public int? LoginId { get; set; }

    public int? InvoiceId { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual LoginInfo? Login { get; set; }
}
