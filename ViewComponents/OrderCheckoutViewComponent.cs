using Console8_ValidationUtils.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace Console8_ValidationUtils.ViewComponents
{
    public class OrderCheckoutViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke( OrderCheckoutViewModel model )
            => View(model);
    }
}
