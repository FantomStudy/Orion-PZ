//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DB_PZ
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        public int order_id { get; set; }
        public int employee_id { get; set; }
        public System.DateTime order_date { get; set; }
        public int status_id { get; set; }
        public int payment_id { get; set; }
        public Nullable<decimal> cost { get; set; }
        public int cart_id { get; set; }
    
        public virtual Carts Carts { get; set; }
        public virtual Employees Employees { get; set; }
        public virtual Order_status Order_status { get; set; }
        public virtual Payments Payments { get; set; }
    }
}