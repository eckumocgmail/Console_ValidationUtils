using System.ComponentModel.DataAnnotations;

namespace DataEntities
{
    /// <summary>
    /// Свеедния о продукции имеющейся в наличии на складе
    /// </summary>
    public class ProductsInStock
    {
        [Key]
        public int ID { get; set; }
        public int HolderID { get; set; }
        public int ProductID { get; set; }
        public int ProductCount { get; set; }
        public float ProductPrice { get; set; }
    }
}
