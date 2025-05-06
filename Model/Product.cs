using System;
using System.Collections.Generic;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
