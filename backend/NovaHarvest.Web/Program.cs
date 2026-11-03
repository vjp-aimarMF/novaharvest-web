var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();


app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>NovaHarvest - Inicio</title>
    <link rel="stylesheet" href="/css/site.css">
</head>
<body>
    <header>
        <img src="/img/logo.png" alt="Logo de NovaHarvest">
        <h1>NovaHarvest</h1>
        <nav>
            <a href="/">Inicio</a>
            <a href="/about">Acerca de</a>
            <a href="/status">Estado</a>
        </nav>
    </header>

    <main>
        <h1>Inicio</h1>
        <p>NovaHarvest es un proyecto para aprovechar los excedentes agrícolas.</p>
        <p>Conectamos a productores y consumidores del Valle del Jerte.</p>
        <p>Buscamos reducir el desperdicio de alimentos.</p>
        <p>Queremos fomentar un consumo más sostenible.</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · 2026
    </footer>
</body>
</html>
""", "text/html; charset=utf-8"));

app.MapGet("/status", () => Results.Content($"""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>NovaHarvest - Estado del servidor</title>
    <link rel="stylesheet" href="/css/site.css">
</head>
<body>
    <header>
        <img src="/img/logo.png" alt="Logo de NovaHarvest">
        <h1>NovaHarvest</h1>
        <nav>
            <a href="/">Inicio</a>
            <a href="/about">Acerca de</a>
            <a href="/status">Estado</a>
        </nav>
    </header>

    <main>
        <h1>Estado del servidor</h1>
        <p>Fecha y hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>
        <p>Página generada por ASP.NET Core (Kestrel)</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · {DateTime.Now.Year}
    </footer>
</body>
</html>
""", "text/html; charset=utf-8"));

app.MapGet("/about", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>NovaHarvest - Acerca de</title>
    <link rel="stylesheet" href="/css/site.css">
</head>
<body>
    <header>
        <img src="/img/logo.png" alt="Logo de NovaHarvest">
        <h1>NovaHarvest</h1>
        <nav>
            <a href="/">Inicio</a>
            <a href="/about">Acerca de</a>
            <a href="/status">Estado</a>
        </nav>
    </header>

    <main>
        <h1>Acerca de</h1>
        <p>NovaHarvest es un proyecto para aprovechar los excedentes agrícolas.</p>
        <p>Centro educativo: IES Valle del Jerte.</p>
        <p>Autor: Aimar Moreno Fuentes.</p>
        <p>Versión: 0.1.0</p>
    </main>

    <footer>
        NovaHarvest · IES Valle del Jerte · 2026
    </footer>
</body>
</html>
""", "text/html; charset=utf-8"));

app.Run();
