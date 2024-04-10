using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Webphone._Models;

public partial class Product
{
    public int ProId { get; set; }
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    public string? ProName { get; set; }
    [Required(ErrorMessage = "Số lượng là bắt buộc.")]
    [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải là số nguyên dương.")]
    public int? Quantity { get; set; }

    [Required(ErrorMessage = "Thương hiệu sản phẩm là bắt buộc.")]
    public string ProBrand { get; set; }

    [Required(ErrorMessage = "Loại sản phẩm là bắt buộc.")]
    public string ProType { get; set; }

    [Required(ErrorMessage = "RAM là bắt buộc.")]
    public string Ram { get; set; }

    [Required(ErrorMessage = "CPU là bắt buộc.")]
    public string Cpu { get; set; }

    [Required(ErrorMessage = "Hệ điều hành là bắt buộc.")]
    public string Hdh { get; set; }

    [Required(ErrorMessage = "Thông số camera là bắt buộc.")]
    public string Camera { get; set; }

    [Required(ErrorMessage = "Màu sắc là bắt buộc.")]
    public string Color { get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải là số dương.")]
    public decimal? ProPrice { get; set; }

    public byte[]? ProImg { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
