using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const int BoxKg = 5;

    public int RemainingKg { get; private set; }
    public int FullBoxes { get; private set; }
    public int LooseKg { get; private set; }

    public Surplus Surplus { get; } = new(
        "Cereza Picota",
        "Huerta Los Castaños",
        "Cabezuela del Valle",
        DateTime.Today.AddDays(-2),
        343,
        120,
        3.20m,
        "<b>Recoger antes de las 14:00</b>");

    public void OnGet()
    {
        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;
        FullBoxes = RemainingKg / BoxKg;
        LooseKg = RemainingKg % BoxKg;
    }
}