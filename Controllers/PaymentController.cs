using Microsoft.AspNetCore.Mvc;
using PaymentApp.Services;

namespace PaymentApp.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/process")]
    public async Task<IActionResult> Process(string senderName, decimal amount, string iban)
    {
        var transaction = await _paymentService.ProcessPayment(
            senderName ?? "",
            amount,
            iban ?? ""
        );

        if (transaction.Result == "Accepted")
            TempData["Success"] = $"Payment accepted. Transaction ID: {transaction.TransactionReference}";
        else
            TempData["Error"] = transaction.RejectionReason;

        return Redirect("/");
    }
}