using System.Text;
using System.Text.Json.Serialization;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntities
{
    public class ProductImage
    {
        [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }

        [JsonIgnore]
        public ProductItem Product { get; set; } = null;
        public string ContentType { get; set; } = "image/png";

        [JsonIgnore]
        public byte[] ImageData { get; set; } = new byte[0];
    }
}
