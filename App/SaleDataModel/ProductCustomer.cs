using DataCommon;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntities
{
    public class ProductCustomer<TItem> where TItem : SaleItem
    {
        [Key]
        public int ID { get; set; }


        /// <summary>
        /// Идентификатор учетной записи в Бд
        /// </summary>
        public int AccountId { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [PhoneNumber]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public IEnumerable<ProductCustomer<TItem>> CustomerOrders { get; set; }





    }
}
