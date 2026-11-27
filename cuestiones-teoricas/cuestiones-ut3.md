# Cuestiones UT3 — NovaHarvest

## 1. De `.cshtml` a HTML
Al compilar NovaHarvest, Razor transforma las páginas `.cshtml` en código C# compilado. Cuando se solicita una página, ASP.NET Core ejecuta ese código en el servidor y devuelve HTML al navegador.

![Cuestión 1](/backend/NovaHarvest.Web/wwwroot/img/Captura-cuestion1.png)

`WriteLiteral` escribe el HTML y el texto fijo de la página, como el encabezado «Estado del servidor» y la etiqueta «Fecha y hora del servidor».

Mientras que `Write` escribe el resultado de una expresión C# de Razor, como `@Model.ServerTime.ToString("dd/MM/yyyy HH:mm:ss")`, que muestra la fecha y hora calculadas en el servidor.

## 2. Tecnologías
La línea del precio de nuestra página utiliza Razor: `@Model.DiscountedPrice.ToString("C")`. En PHP se puede escribir `<?= $discountedPrice ?>` y en ASP clásico `<%= discountedPrice %>`. Esas dos sintaxis usan delimitadores; Razor empieza con @ y detecta el final de la expresión por la sintaxis de C#.

En UT2 las páginas se generaban con `MapGet` y cadenas HTML en `Program.cs`. Ahora cada página tiene su propio `.cshtml` y `Program.cs` registra Razor Pages con `MapRazorPages()`.

![Cuestión 2.1](/backend/NovaHarvest.Web/wwwroot/img/Captura-Cuestion2(Old-Program.cs).png)

![Cuestión 2.2](/backend/NovaHarvest.Web/wwwroot/img/Captura-Cuestion2(New-Program.cs).png)

## 3. Etiquetas de código
En `Surplus.cshtml`, `@Model.Surplus.Product` es una expresión implícita y muestra el producto. `@(Model.DiscountedPrice.ToString("C"))` es una expresión explícita. El bloque `@{ ViewData["Title"] = "Excedente destacado"; }` establece el título y no añade texto al cuerpo. `@@novaharvest` muestra una arroba literal y `@* comentario *@` no aparece en el HTML.

`info@novaharvest.es` se interpreta como texto de una dirección de correo; @novaharvest podría interpretarse como código Razor, por eso se usa `@@` para mostrar la arroba literal.

![Cuestión 3.1](/backend/NovaHarvest.Web/wwwroot/img/Captura-Cuestion3.1.png)

![Cuestión 3.2](/backend/NovaHarvest.Web/wwwroot/img/Captura-Cuestion3.2.png)