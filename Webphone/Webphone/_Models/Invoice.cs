using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webphone._Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    [Required(ErrorMessage = "Ngày hóa đơn là bắt buộc.")]
    [DataType(DataType.Date, ErrorMessage = "Ngày hóa đơn không hợp lệ.")]
    public DateOnly? InvoiceDate { get; set; }

    [Required(ErrorMessage = "Tổng số tiền là bắt buộc.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Tổng số tiền phải là số dương.")]
    public decimal? TotalAmount { get; set; }

    [Required(ErrorMessage = "Trạng thái hóa đơn là bắt buộc.")]
    public string? InvoiceStatus { get; set; }

    [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
    public string? CustName { get; set; }

    [Required(ErrorMessage = "Số điện thoại khách hàng là bắt buộc.")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? CustPhone { get; set; }

    [Required(ErrorMessage = "Địa chỉ khách hàng là bắt buộc.")]
    public string? CustAdd { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
