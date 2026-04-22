using Microsoft.AspNetCore.Mvc;

namespace PaymentApp.Controllers;

public class PaymentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}