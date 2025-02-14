# 🏢 ArandaProduct  

Este proyecto es una API RESTful en **.NET 8** para la gestión de productos y categorías.  
Permite realizar operaciones **CRUD** (*Crear, Leer, Actualizar y Eliminar*) sobre productos y categorías.  

---

## 📌 Requisitos previos  
Antes de clonar el repositorio, asegúrate de tener instalados los siguientes programas:  

📌 [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
🛢️ [SQL Server 2014 o superior](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)  
🖥️ [Visual Studio 2022](https://visualstudio.microsoft.com/) con la carga de trabajo de **desarrollo en .NET**  
🔗 [Git](https://git-scm.com/)  
💽 [Postman](https://www.postman.com/) *(opcional, para probar la API)*  

---

## 🛄 Base de Datos  

El archivo `DBProducts.sql` contiene el script necesario para crear la base de datos y las tablas requeridas.  

🔹 **Pasos para ejecutar el script:**  

1️⃣ Abre **SQL Server Management Studio (SSMS)**.  

2️⃣ Conéctate a tu servidor de **SQL Server**.  

3️⃣ Abre el archivo `DBProducts.sql`.  

4️⃣ **Ejecuta** el script para crear la base de datos y sus tablas.  

💡 **Importante**: Asegúrate de que la base de datos se ha creado correctamente antes de configurar la API.  

---

## ⚙️ Configuración del entorno  

1️⃣ **Clona el repositorio**:  

```sh
git clone https://github.com/TU_USUARIO/ArandaProduct.git
```

2️⃣ **Configura la conexión a la base de datos** en el archivo `appsettings.json`, dentro del proyecto `ArandaProduct.WebApi`.  

🔹 Busca la sección `"ConnectionStrings"` y edita el valor de la cadena de conexión:  

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=ArandaProductDB;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
}
```

---

## 🚀 Ejecutando la API  

🔹 **Desde Visual Studio**  

1️⃣ Abre el proyecto en **Visual Studio 2022**.  

2️⃣ Selecciona `ArandaProduct.WebApi` como proyecto de inicio.  

3️⃣ Presiona **F5** para ejecutar la API.  

🔹 **Desde la terminal con .NET CLI**  

```sh
cd ArandaProduct.WebApi
dotnet run
```

Esto iniciará la API en `https://localhost:5001` o `http://localhost:5000`.  

---

## 🔥 Probando la API  

### 📄 1️⃣ Swagger UI  
Si la API está corriendo, puedes abrir en tu navegador:  
🔗 [`https://localhost:5001/swagger`](https://localhost:5001/swagger)  
Desde ahí puedes hacer pruebas con los endpoints de forma interactiva.  

### 🚀 2️⃣ Postman / Thunder Client  
Puedes enviar solicitudes manualmente desde **Postman** o **Thunder Client** en VS Code.  

Ejemplo de una solicitud `GET`:  

```sh
GET https://localhost:5001/api/products
```

Ejemplo de una solicitud `POST`:  

```json
POST https://localhost:5001/api/products
Content-Type: application/json

{
  "name": "Laptop Gamer",
  "category": "Electrónica",
  "price": 1500
}
```

---

