-- Crear la base de datos
CREATE DATABASE ProductDB;
GO

-- Usar la base de datos recién creada
USE ProductDB;
GO

-- Crea la tabla Categories
CREATE TABLE Categories (
    IdCategory INT IDENTITY(1,1) PRIMARY KEY,  -- Clave primaria autoincrementada
    NameCategory NVARCHAR(100) NOT NULL UNIQUE, -- Nombre único de la categoría
    CreatedCategory DATETIME DEFAULT GETDATE() -- Fecha de creación (auditoría) (corregido el nombre)
);
GO

-- Crear la tabla Products
CREATE TABLE Products (
    IdProduct INT IDENTITY(1,1) PRIMARY KEY,  -- Clave primaria autoincrementada
    NameProduct NVARCHAR(100) NOT NULL,        -- Nombre del producto (ahora NOT NULL)
    DescriptionProduct NVARCHAR(500) NULL,     -- Descripción del producto
    IdCategory INT NOT NULL,                   -- Relación con la tabla Categorías
    ImageProduct VARBINARY(MAX) NULL,	         -- Imagen del producto (opcional)
    CreatedAt DATETIME DEFAULT GETDATE(),      -- Fecha de creación (auditoría)
    FOREIGN KEY (IdCategory) REFERENCES Categories(IdCategory) -- Clave foránea
);
GO

-- Crear un índice en NameProduct para mejorar la búsqueda
CREATE INDEX IX_Products_Name ON Products(NameProduct);
GO

-- Corregir el índice en IdCategory (antes CategoryProduct, que no existe)
CREATE INDEX IX_Products_Category ON Products(IdCategory);
GO

-- Insert Categories
INSERT INTO Categories (NameCategory) VALUES ('Electrónica');
INSERT INTO Categories (NameCategory) VALUES ('Hogar');
INSERT INTO Categories (NameCategory) VALUES ('Ropa');
INSERT INTO Categories (NameCategory) VALUES ('Deportes');
INSERT INTO Categories (NameCategory) VALUES ('Automotriz');
GO

-- Insert Categories
INSERT INTO Products (NameProduct, DescriptionProduct, IdCategory)  
VALUES ('Laptop Gamer', 'Laptop con procesador Intel i7, 16GB RAM y SSD de 512GB', 1);  
INSERT INTO Products (NameProduct, DescriptionProduct, IdCategory)  
VALUES ('Bicicleta de montaña', 'Bicicleta todo terreno con suspensión delantera', 4);
GO  