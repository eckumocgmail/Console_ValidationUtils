

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntities
{


    /// <summary>
    /// Выоплненая продажа, объединяет 
    ///     1 покупателя, 
    ///     1 продавца,
    /// одно время и место.
    /// 
    /// </summary>
    public class SaleItem
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public ProductItem Product { get; set; }

        [Required]
        public int ProductID { get; set; }
        public int ProductCount { get; set; }

        


        [Required]
        [Column( TypeName = "datetime" )]
        public DateTime DateTime { get; set; }

    }
}
