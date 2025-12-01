Readme proyecto .NET8.

Instalar antes de ejecutar: 
- .NET SDK 8 

Posicionarse sobre la carpeta AseguradoraApp o ir a la biblioteca de clases Repositorio para descargar las dependencias:
- dotnet add package Microsoft.Data.SqlClient
-paquete Nugets Microsoft.Data.SqlClient

configuracion del archivo appesettings.json:
modificar "ConnectionStrings": {
  "DefaultConnection":
  "Server=localhost;Database=SegurVidaDB;Trusted_Connection=True;Encrypt=False"
}
segun la conexion y credenciales a su base de datos Sql Server

ejecutar la aplicacion:
dotnet run