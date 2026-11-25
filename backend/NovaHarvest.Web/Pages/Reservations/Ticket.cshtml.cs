using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NovaHarvest.Web.Pages.Reservations;

public class TicketModel : PageModel
{
    public string ReservationNumber { get; private set; } = "";
    public string Product { get; private set; } = "";
    public int Kilograms { get; private set; }
    public string PickupLocation { get; private set; } = "";
    public DateTime GeneratedAt { get; private set; }

    public void OnGet()
    {
        ReservationNumber = "RES-0001";
        Product = "Cereza Picota";
        Kilograms = 20;
        PickupLocation = "Cooperativa del Valle del Jerte";
        GeneratedAt = DateTime.Now;
    }
}