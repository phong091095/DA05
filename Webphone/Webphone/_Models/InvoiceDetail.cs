using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webphone._Models;

public partial class InvoiceDetail
{
    public int InvoiceDetailId { get; set; }

    [Required(ErrorMessage = "InvoiceId là bắt buộc.")]
    public int? InvoiceId { get; set; }

    [Required(ErrorMessage = "ProductId là bắt buộc.")]
    public int? ProductId { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc.")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải là số nguyên dương.")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Subtotal là bắt buộc.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal phải là số dương.")]
    public decimal? Subtotal { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual Product? Product { get; set; }
}
