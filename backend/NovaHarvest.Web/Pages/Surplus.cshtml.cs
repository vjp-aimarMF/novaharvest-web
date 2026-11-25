using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class SurplusModel : PageModel
{
    private const int BoxKg = 5;

    private const decimal SurplusDiscount = 0.30m;

    private const int ShelfLifeDays = 7;

    private const int LowStockKg = 50;

    public int RemainingKg { get; private set; }
    public int FullBoxes { get; private set; }
    public int LooseKg { get; private set; }

    public decimal DiscountedPrice { get; private set; }
    public decimal SurplusValue { get; private set; }

    public DateTime ConsumeBefore { get; private set; }
    public int DaysToConsume { get; private set; }

    public string Status { get; private set; } = "";

    public bool IsOutOfStock { get; private set; }

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
        DiscountedPrice = Surplus.PricePerKg * (1 - SurplusDiscount);
        SurplusValue = DiscountedPrice * RemainingKg;
        ConsumeBefore = Surplus.HarvestDate.AddDays(ShelfLifeDays);
        DaysToConsume = (ConsumeBefore - DateTime.Today).Days;
        Status = RemainingKg <= 0
            ? "Agotado"
            : RemainingKg < LowStockKg
                ? "¡Últimos kilos!"
                : "Disponible";
        IsOutOfStock = RemainingKg <= 0;
    }
}