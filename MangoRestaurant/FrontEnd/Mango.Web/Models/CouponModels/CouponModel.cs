﻿namespace Mango.Web.Models.CouponModels;

public class CouponModel
{
    public int CouponId { get; set; }

    public string CouponCode { get; set; }

    public double DiscountAmount { get; set; }
}