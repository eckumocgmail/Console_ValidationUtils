using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;

public static class ControllerExtensions
{
    public static IActionResult Comp( 
        this Controller controller, object model )
        => controller.PartialView(model.GetType().GetTypeName(), model);
}