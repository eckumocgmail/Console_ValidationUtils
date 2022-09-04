using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Console8_ValidationUtils.ViewModels
{

    
    public class OrderCheckoutViewModel: OrderCheckoutViewModel<object>
    {

    }
    public class OrderCheckoutViewModel<TOrderItem>
    {
        private ConcurrentBag<TOrderItem> Selected { get; set; }
        public OrderCheckoutViewModel()
        {
            this.Selected = new ConcurrentBag<TOrderItem>();
        }


        public string GetOrderItemType() => typeof(TOrderItem).GetTypeName();
        public IEnumerable<TOrderItem> GetSelectedItems() => Selected;        
    }
}
