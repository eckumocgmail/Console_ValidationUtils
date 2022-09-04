using System.Text;
using System.Text.Json.Serialization;
 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataEntities
{
    public class ProductItem: BaseEntity
    {
        
        public float ProductRate { get; set; } = 0;
        public string ProductName { get; set; }
        public string ProductIndicatorsJson { get; set; }

        [JsonIgnore]
        public IList<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
