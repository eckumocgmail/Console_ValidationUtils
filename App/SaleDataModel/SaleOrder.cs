using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataEntities
{  
    public class SaleOrder<TItem> where TItem : SaleItem
    {
        [Key]
        public int ID { get; set; }
     
        public ProductCustomer<TItem> Customer { get; set; }
        public int CustomerID { get; set; }

        public ProductHolder<TItem> Holder { get; set; }
        public int? HolderID { get; set; }

 
        public ProductTransport Transport { get; set; }
        public int? TransportID { get; set; }


        public DateTime? OrderCreated { get; set; }
        public DateTime? OrderUpdated { get; set; }
        public int UpdateCounter { get; set; } = 0;

        public ICollection<TItem> OrderItems { get; set; }        

        /***
         * 1-зарегистрирован
         * 2-на складе
         * 3-у курьера
         * 4-в постамате
         * 5-добавлен получателю
         * 6-отменён
         */
        public int OrderStatus { get; set; } = 0;
        public string GetStausText()
        {
            switch (OrderStatus)
            {
                case 1: return "зарегистрирован";
                case 2: return "на складе";
                case 3: return "у курьера";
                case 4: return "в постамате";
                case 5: return "добавлен получателю";
                case 6: return "отменён";
                default: return "неправельный стату";
            }
        }
    }
    
}
