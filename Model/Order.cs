using System;
using System.Collections.Generic;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

public partial class Order
{
    public int OrderId { get; set; }

    public int ClientId { get; set; }

    public int ProductId { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product Product { get; set; } = null!;
}
