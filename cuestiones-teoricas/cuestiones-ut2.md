# Cuestiones UT2 - NovaHarvest

## 1. Cliente y servidor

Cuando abro `/status`, el cliente es el navegador, que solicita la página y muestra el HTML recibido. El servidor es NovaHarvest, ejecutado con ASP.NET Core sobre Kestrel. En Docker, nginx recibe la petición y la reenvía al backend. El servidor genera la fecha y hora actuales mediante C#. Cuando se puedan reservar productos, la cantidad disponible debe comprobarse obligatoriamente en el servidor. Así se evita que un usuario modifique la petición desde el navegador y reserve más kilos de los que existen.

## 2. Estático o dinámico

La página de inicio podría ser estática si su contenido no cambiara, aunque actualmente la genero con C#. La página `/about` también podría ser estática porque muestra información del proyecto, el centro, el autor y la versión. En cambio, `/status` es dinámica porque muestra la fecha y hora actuales del servidor. NovaHarvest también necesitará una página para consultar las reservas de un comprador y otra para mostrar los excedentes disponibles. Estas páginas necesitarán información actualizada, por lo que no bastaría con guardar un HTML fijo.

## 3. Ejecución en el servidor

Cuando abro `http://localhost:8080/status`, el navegador envía una petición HTTP a nginx. El servicio `web` recibe la petición en el puerto 80 del contenedor, publicado como el puerto 8080 del equipo. Nginx reenvía la petición al servicio `backend:8080`, pasando la cabecera `Host`. Kestrel recibe la petición y ejecuta la ruta `/status` definida en `Program.cs`. El código C# genera el HTML con la fecha y hora y lo devuelve al navegador a través de nginx. Con CGI, el servidor web ejecutaría un programa externo para procesar la petición. Con un módulo embebido, el código se ejecutaría dentro del propio servidor web, en lugar de utilizar el backend ASP.NET Core separado que tiene NovaHarvest.

## 4. Reverse proxy

En NovaHarvest con Docker, la cabecera `Server` debería mostrar `nginx`, porque nginx es quien responde directamente al navegador. Al ejecutar la aplicación con `dotnet run`, debería aparecer `Kestrel`, ya que la petición llega directamente a ASP.NET Core. Para comprobarlo, puedo ejecutar `curl -I http://localhost:8080/status` con Docker y `curl -I` usando la dirección que indique `dotnet run`. Si detengo el backend con `docker compose stop backend`, las peticiones a `/status` y `/css/site.css` deberían devolver el código HTTP 502, porque nginx no puede contactar con el servidor de aplicaciones. El backend no publica ningún puerto porque solo debe ser accesible desde la red interna de Docker, a través de nginx.

### 4. Cabeceras HTTP y exposición de puertos

Al ejecutar `curl -s -D - -o /dev/null http://localhost:8080/status`, obtenemos una respuesta `HTTP/1.1 200 OK` y la cabecera `Server: nginx/1.27.5`.
Esto demuestra que nginx recibe la petición y devuelve una respuesta correcta.
Con `docker compose ps` comprobamos que nginx publica el puerto 8080 del equipo, mientras que el backend solo utiliza el puerto 8080 dentro de Docker.
Así, el backend no tiene un puerto publicado directamente en el equipo anfitrión y las peticiones pasan por nginx.

## 5. Nginx

En NovaHarvest, nginx no sirve `site.css` ni `logo.png` por su cuenta porque su configuración `web/nginx.conf` reenvía todas las peticiones al backend. ASP.NET Core sirve esos archivos desde la carpeta `wwwroot`. Así, nginx actúa como reverse proxy y el backend se encarga de generar las páginas y servir los archivos estáticos. Si la aplicación se publicara en Internet, nginx podría aportar funciones como terminar conexiones HTTPS y gestionar certificados. También podría limitar peticiones para reducir abusos o distribuirlas entre varios servidores backend.

## 6. Tecnologías

Si la cooperativa pidiera utilizar otra tecnología de servidor vista en clase, tendría que cambiar principalmente el código del backend y su forma de construirse. Por ejemplo, cambiarían `Program.cs`, el archivo del proyecto .NET y el `backend/Dockerfile`, porque estarían relacionados con ASP.NET Core. También podría cambiar la estructura de los archivos del backend y la forma de generar las páginas. En cambio, `compose.yaml` y `web/nginx.conf` podrían mantenerse si el nuevo backend escuchara en el mismo puerto y continuara usando el mismo servicio `backend`. Los archivos de estilos y el logotipo también podrían conservarse si la nueva aplicación los sirviera en las mismas rutas.

## 7. Marcado y código

En `Program.cs`, una página se genera mediante una cadena de texto HTML. Por ejemplo, en la ruta `/status` utilizo `Results.Content` para devolver el HTML con el título, el menú, el contenido y el pie de página. Un problema de esta forma de trabajar es que el código resulta largo y difícil de mantener cuando la página crece. Otro problema es que repetir o modificar etiquetas puede provocar errores en el HTML. Además, si se introduce información de usuarios sin tratarla correctamente, podrían aparecer problemas de seguridad. En las tres páginas se repiten la estructura HTML, el encabezado, el logotipo, el menú de navegación, el enlace al CSS y el pie de página.

## 8. Docker

Con el entorno de NovaHarvest levantado tengo dos servicios, `web` y `backend`, que normalmente corresponden a dos contenedores y dos imágenes construidas para el proyecto. Una imagen es la plantilla con la que se crea un contenedor, mientras que un contenedor es una instancia que está ejecutándose. La imagen final del backend no incluye el SDK de .NET porque solo necesita ejecutar la aplicación publicada, no compilarla. La compilación se realiza en la primera etapa del `backend/Dockerfile`, que utiliza la imagen del SDK de .NET 10. El archivo `backend/.dockerignore` excluye `bin/` y `obj/`. Sin él, esos archivos generados podrían entrar en el contexto de construcción, ocupando espacio y pudiendo interferir con la compilación.

## 9. Herramientas

| Herramienta | Problema que resuelve en NovaHarvest |
|---|---|
| Visual Studio Code | Permite editar `Program.cs`, los archivos HTML y CSS y la configuración de Docker. |
| .NET 10 | Permite crear y ejecutar la aplicación ASP.NET Core. |
| Kestrel | Atiende las peticiones HTTP y ejecuta las rutas del backend. |
| nginx | Actúa como reverse proxy y reenvía las peticiones al backend. |
| Docker | Ejecuta los servicios en contenedores aislados. |
| Docker Compose | Construye y arranca `web` y `backend` con un solo comando. |
| Git | Guarda el historial de cambios mediante commits. |
| Navegador web | Permite probar las páginas, los enlaces, los estilos y las imágenes. |
| Terminal | Permite ejecutar comandos de .NET, Docker, Compose y Git. |
| curl | Permite comprobar las respuestas HTTP y sus cabeceras. |

## 10. Observación

Para comparar la hora, abro `/status` con Docker y después ejecuto NovaHarvest mediante `dotnet run`. En ambos casos, la hora se genera con `DateTime.Now` en el servidor que ejecuta el código. Al ejecutar directamente la aplicación, utiliza la hora local configurada en el equipo. En Docker, utiliza la zona horaria configurada en el contenedor, que puede ser diferente de la del equipo. Por eso, las horas podrían coincidir o mostrar una diferencia. Para comprobarlo, comparo los valores que aparecen en ambas páginas y tengo en cuenta la zona horaria de cada entorno.
