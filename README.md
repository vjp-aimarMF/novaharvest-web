# Proyecto NovaHarvest — UT3

NovaHarvest es una aplicación web inspirada en el proyecto PIATEX, en el que participa el IES Valle del Jerte (Plasencia). Su objetivo es ayudar a aprovechar los excedentes agrícolas del Valle del Jerte, conectando a productores y consumidores y reduciendo el desperdicio de alimentos.

## 1. Arranque

### Con Docker

Ejecuta los comandos desde la raíz del repositorio, donde está `compose.yaml`.

1. Construir las imágenes e iniciar los servicios:

   ```bash
   docker compose up -d --build
   ```

2. Abrir la aplicación en el navegador:

   <http://localhost:8080/>

3. Para detener y eliminar los contenedores:

   ```bash
   docker compose down
   ```

### Sin Docker

Desde la carpeta backend/NovaHarvest.Web/, compila el backend:

```bash
dotnet build
```

Después, ejecuta la aplicación:

```bash
dotnet run
```

Abre en el navegador la dirección que muestra la consola. Para detener la aplicación, pulsa `Ctrl+C` en la terminal donde se está ejecutando.

## 2. Arquitectura

NovaHarvest utiliza nginx como proxy inverso y ASP.NET Core sobre Kestrel para generar las páginas en el servidor. Docker Compose inicia los servicios `web` y `backend` y permite que se comuniquen entre sí.

```text
Navegador
   |
   | HTTP: http://localhost:8080
   v
web — nginx (puerto 80 del contenedor)
   |
   | proxy_pass http://backend:8080
   | reenvía la petición y la cabecera Host
   v
backend — ASP.NET Core / Kestrel (puerto 8080 del contenedor)
   |
   +-- Razor Pages: páginas generadas en el servidor
   |
   +-- wwwroot/css/site.css
   |
   +-- wwwroot/img/logo.png
```

| Servicio | Función | Puertos |
|---|---|---|
| `web` | nginx actúa como proxy inverso y reenvía las peticiones al backend. | Publica el puerto `8080` del equipo anfitrión hacia el puerto `80` del contenedor. |
| `backend` | Ejecuta la aplicación NovaHarvest con ASP.NET Core sobre Kestrel. | Escucha en el puerto `8080` dentro del contenedor y no publica un puerto al exterior. |

El recorrido de una petición es el siguiente:

1. El navegador solicita una página o un recurso a `http://localhost:8080/`.
2. nginx recibe la petición en `web` y la reenvía a `backend:8080`, conservando la cabecera `Host`.
3. ASP.NET Core procesa la petición. `Program.cs` configura la cultura `es-ES`, registra las rutas de Razor Pages y los recursos estáticos.
4. El backend devuelve la respuesta a nginx y nginx la entrega al navegador.

El `Dockerfile` del backend utiliza dos etapas: la imagen del SDK de .NET para publicar la aplicación y la imagen de ASP.NET Core para ejecutarla. El archivo `backend/.dockerignore` excluye `bin/` y `obj/`; estos directorios también están excluidos de Git mediante `.gitignore`.

## 3. Páginas

Las páginas se generan en el servidor mediante Razor Pages. Las rutas y los archivos principales son:

| URL | Archivo `.cshtml` | `PageModel` | Función |
|---|---|---|---|
| `/` | `Pages/Index.cshtml` | No tiene un archivo `PageModel` propio | Presenta NovaHarvest y muestra la fecha actual en formato largo. |
| `/about` | `Pages/About.cshtml` | No tiene un archivo `PageModel` propio | Presenta el proyecto, la versión `0.2.0` y la información de contacto. |
| `/status` | `Pages/Status.cshtml` | `StatusModel`, en `Pages/Status.cshtml.cs` | Muestra la fecha y hora del servidor, el nombre de la máquina y el contador de visitas. |
| `/surplus` | `Pages/Surplus.cshtml` | `SurplusModel`, en `Pages/Surplus.cshtml.cs` | Muestra el excedente destacado, sus precios, los kilos disponibles, las cajas completas, los kilos sueltos, las fechas y el estado. |
| `/ticket` | `Pages/Reservations/Ticket.cshtml` | `TicketModel`, en `Pages/Reservations/Ticket.cshtml.cs` | Muestra el justificante de reserva en un documento HTML independiente, sin utilizar el diseño común. |

### Ficheros compartidos de `Pages/`

- **`Pages/_ViewImports.cshtml`**: contiene las directivas comunes `@using NovaHarvest.Web.Models` y `@namespace NovaHarvest.Web.Pages`, para disponer de los modelos y establecer el espacio de nombres de las páginas.
- **`Pages/_ViewStart.cshtml`**: establece `_Layout.cshtml` como diseño predeterminado para las páginas que lo utilizan.
- **`Pages/Shared/_Layout.cshtml`**: contiene la estructura HTML compartida, la cabecera, el logotipo, el menú, la zona principal y el pie de página. El título de cada página se obtiene mediante `ViewData["Title"]`. El pie muestra `NovaHarvest · IES Valle del Jerte`, el año actual y el entorno de ejecución.

La página `/ticket` establece `Layout = null` porque utiliza su propio documento HTML. Su ruta es `/ticket`; la ubicación del archivo dentro de `Pages/Reservations/` no añade `/reservations` a la URL.

### Modelos utilizados por las páginas

- **`Models/Surplus.cs`** define los datos del excedente: producto, productor, localidad, fecha de cosecha, kilos disponibles, kilos reservados, precio por kilo y nota.
- **`Pages/Surplus.cshtml.cs` (`SurplusModel`)** prepara los valores que muestra `/surplus`, incluidos los kilos restantes, las cajas completas y los kilos sueltos, el precio rebajado, el valor del excedente, la fecha límite y los días de consumo, su estado, la nota alternativa y el indicador de disponibilidad.
- **`Models/SiteStats.cs`** contiene el contador compartido de visitas.
- **`Pages/Status.cshtml.cs` (`StatusModel`)** prepara la hora del servidor, el nombre de la máquina y el contador que muestra `/status`.
- **`Pages/Reservations/Ticket.cshtml.cs` (`TicketModel`)** prepara el título, el número de reserva, el producto, los kilos, el lugar de recogida y la fecha y hora de generación del justificante.

En las páginas Razor se muestra la información que prepara el `PageModel`; los cálculos y decisiones del excedente se mantienen en `SurplusModel`.
