## SGIC APP 


DDL

DROP DATABASE NJSIGC;
CREATE DATABASE IF NOT EXISTS NJSIGC;
USE NJSIGC;

-- TIPOS Y TERCEROS
CREATE TABLE tipo_documento (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50)
);

CREATE TABLE tipo_tercero (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50)
);

CREATE TABLE pais (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

CREATE TABLE region (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    pais_id INT,
    FOREIGN KEY (pais_id) REFERENCES pais(id)
);

CREATE TABLE ciudad (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    region_id INT,
    FOREIGN KEY (region_id) REFERENCES region(id)
);

CREATE TABLE tercero (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    apellidos VARCHAR(50),
    email VARCHAR(80) UNIQUE,
    tipo_doc_id INT,
    tipo_tercero_id INT,
    ciudad_id INT,
    FOREIGN KEY (tipo_doc_id) REFERENCES tipo_documento(id),
    FOREIGN KEY (tipo_tercero_id) REFERENCES tipo_tercero(id),
    FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);

CREATE TABLE tercero_telefono (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero VARCHAR(20),
    tipo VARCHAR(20),
    tercero_id VARCHAR(20),
    FOREIGN KEY (tercero_id) REFERENCES tercero(id)
);

-- EMPRESA
CREATE TABLE empresa (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    ciudad_id INT,
    fecha_reg DATE,
    FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);

-- EPS y ARL
CREATE TABLE eps (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

CREATE TABLE arl (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50)
);

-- EMPLEADO
CREATE TABLE empleado (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_id VARCHAR(20),
    fecha_ingreso DATE,
    salario_base DOUBLE,
    eps_id INT,
    arl_id INT,
    FOREIGN KEY (tercero_id) REFERENCES tercero(id),
    FOREIGN KEY (eps_id) REFERENCES eps(id),
    FOREIGN KEY (arl_id) REFERENCES arl(id)
);

-- CLIENTE
CREATE TABLE cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_id VARCHAR(20),
    fecha_nac DATE,
    fecha_ultima_compra DATE,
    FOREIGN KEY (tercero_id) REFERENCES tercero(id)
);

-- PROVEEDOR
CREATE TABLE proveedor (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_id VARCHAR(20),
    dcto DOUBLE,
    dia_pago INT,
    FOREIGN KEY (tercero_id) REFERENCES tercero(id)
);

-- producto
CREATE TABLE producto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    stock INT,
    stock_min INT,
    stock_max INT,
    created_at DATE,
    updated_at DATE,
    barcode VARCHAR(50) UNIQUE
);

-- RELACIÓN PRODUCTO - PROVEEDOR
CREATE TABLE producto_proveedor (
    producto_id INT,
    tercero_id VARCHAR(20),
    PRIMARY KEY (producto_id, tercero_id),
    FOREIGN KEY (producto_id) REFERENCES producto(id),
    FOREIGN KEY (tercero_id) REFERENCES tercero(id)
);

-- COMPRAS
CREATE TABLE compras (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_prov_id VARCHAR(20),
    fecha DATE,
    tercero_empl_id VARCHAR(20),
    desc_compra TEXT,
    FOREIGN KEY (tercero_prov_id) REFERENCES tercero(id),
    FOREIGN KEY (tercero_empl_id) REFERENCES tercero(id)
);

CREATE TABLE detalle_compra (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    producto_id INT,
    cantidad INT,
    valor DOUBLE,
    compra_id INT,
    FOREIGN KEY (producto_id) REFERENCES producto(id),
    FOREIGN KEY (compra_id) REFERENCES compras(id)
);

-- VENTAS
CREATE TABLE venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    tercero_cli_id VARCHAR(20),
    tercero_emp_id VARCHAR(20),
    forma_pago VARCHAR(50),
    FOREIGN KEY (tercero_cli_id) REFERENCES tercero(id),
    FOREIGN KEY (tercero_emp_id) REFERENCES tercero(id)
);

CREATE TABLE detalle_venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    factura_id INT,
    producto_id INT,
    cantidad INT,
    valor DOUBLE,
    FOREIGN KEY (factura_id) REFERENCES venta(id),
    FOREIGN KEY (producto_id) REFERENCES producto(id)
);

-- FACTURACIÓN
CREATE TABLE facturacion (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha_resolucion DATE,
    num_inicio INT,
    num_final INT,
    fact_actual INT
);

-- MOVIMIENTOS DE CAJA
CREATE TABLE tipo_mov_caja (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    tipo VARCHAR(10)
);

CREATE TABLE mov_caja (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    tipo_mov_id INT,
    valor DOUBLE,
    concepto TEXT,
    tercero_id VARCHAR(20),
    FOREIGN KEY (tipo_mov_id) REFERENCES tipo_mov_caja(id),
    FOREIGN KEY (tercero_id) REFERENCES tercero(id)
);

-- PLANES
CREATE TABLE planes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    fecha_ini DATE,
    fecha_fin DATE,
    dcto DOUBLE
);

CREATE TABLE plan_producto (
    plan_id INT,
    producto_id INT,
    PRIMARY KEY (plan_id, producto_id),
    FOREIGN KEY (plan_id) REFERENCES planes(id),
    FOREIGN KEY (producto_id) REFERENCES producto(id)
);


DML 

USE NJSIGC;

-- tipo_documento
INSERT INTO tipo_documento (descripcion) VALUES 
('Cédula de Ciudadanía'), 
('NIT'), 
('Pasaporte');

-- tipo_tercero
INSERT INTO tipo_tercero (descripcion) VALUES 
('Empleado'), 
('Cliente'), 
('Proveedor');

-- pais
INSERT INTO pais (nombre) VALUES 
('Colombia'), 
('Ecuador'), 
('Perú');

-- region
INSERT INTO region (nombre, pais_id) VALUES 
('Antioquia', 1), 
('Pichincha', 2), 
('Lima', 3);

-- ciudad
INSERT INTO ciudad (nombre, region_id) VALUES 
('Medellín', 1), 
('Quito', 2), 
('Lima', 3);

-- tercero
INSERT INTO tercero (id, nombre, apellidos, email, tipo_doc_id, tipo_tercero_id, ciudad_id) VALUES 
('1001', 'Juan', 'Pérez', 'juan.perez@email.com', 1, 1, 1), 
('1002', 'Ana', 'Gómez', 'ana.gomez@email.com', 1, 2, 2), 
('1003', 'Carlos', 'Martínez', 'carlos.martinez@email.com', 2, 3, 3);


-- tercero_telefono
INSERT INTO tercero_telefono (numero, tipo, tercero_id) VALUES 
('3001234567', 'Celular', '1001'), 
('3123456789', 'Fijo', '1002'), 
('3109876543', 'Celular', '1003');

-- empresa
INSERT INTO empresa (id, nombre, ciudad_id, fecha_reg) VALUES 
('E001', 'Empresas S.A.', 1, '2022-01-01'), 
('E002', 'Servicios Ltda.', 2, '2022-05-10'), 
('E003', 'Comercializadora SAS', 3, '2023-03-15');

-- eps
INSERT INTO eps (nombre) VALUES 
('Sura'), 
('Nueva EPS'), 
('Sanitas');

-- arl
INSERT INTO arl (nombre) VALUES 
('Colpatria'), 
('Positiva'), 
('Bolívar');

-- empleado
INSERT INTO empleado (tercero_id, fecha_ingreso, salario_base, eps_id, arl_id) VALUES 
('1001', '2022-01-15', 2500000, 1, 1), 
('1002', '2023-03-01', 2000000, 2, 2), 
('1003', '2024-06-20', 3000000, 3, 3);

-- cliente
INSERT INTO cliente (tercero_id, fecha_nac, fecha_ultima_compra) VALUES 
('1002', '1990-05-15', '2024-11-01'), 
('1003', '1985-10-10', '2025-01-20'), 
('1001', '1988-02-20', '2025-03-05');

-- proveedor
INSERT INTO proveedor (tercero_id, dcto, dia_pago) VALUES 
('1003', 10.5, 15), 
('1001', 5.0, 10), 
('1002', 7.25, 20);

-- producto
INSERT INTO producto (nombre, stock, stock_min, stock_max, created_at, updated_at, barcode) VALUES 
('Laptop Dell', 10, 2, 20, '2024-01-01', '2025-01-01', '123456789001'), 
('Mouse Logitech', 50, 10, 100, '2024-02-10', '2025-01-01', '123456789002'), 
('Teclado HP', 30, 5, 60, '2024-03-05', '2025-01-01', '123456789003');

-- producto_proveedor
INSERT INTO producto_proveedor (producto_id, tercero_id) VALUES 
(1, '1003'), 
(2, '1003'), 
(3, '1002');

-- compras
INSERT INTO compras (tercero_prov_id, fecha, tercero_empl_id, desc_compra) VALUES 
('1003', '2024-01-15', '1001', 'Compra de laptops'), 
('1002', '2024-02-20', '1002', 'Compra de mouse'), 
('1003', '2024-03-05', '1003', 'Compra de teclados');

-- detalle_compra
INSERT INTO detalle_compra (fecha, producto_id, cantidad, valor, compra_id) VALUES 
('2024-01-15', 1, 5, 4000000, 1), 
('2024-02-20', 2, 20, 500000, 2), 
('2024-03-05', 3, 10, 800000, 3);

-- venta
INSERT INTO venta (fecha, tercero_cli_id, tercero_emp_id, forma_pago) VALUES 
('2024-05-01', '1002', '1001', 'Efectivo'), 
('2024-05-02', '1003', '1002', 'Tarjeta'), 
('2024-05-03', '1001', '1003', 'Transferencia');

-- detalle_venta
INSERT INTO detalle_venta (factura_id, producto_id, cantidad, valor) VALUES 
(1, 1, 1, 1000000), 
(2, 2, 2, 80000), 
(3, 3, 3, 300000);

-- facturacion
INSERT INTO facturacion (fecha_resolucion, num_inicio, num_final, fact_actual) VALUES 
('2024-01-01', 1, 1000, 10), 
('2024-06-01', 1001, 2000, 1010), 
('2025-01-01', 2001, 3000, 2020);

-- tipo_mov_caja
INSERT INTO tipo_mov_caja (nombre, tipo) VALUES 
('Ingreso por venta', 'Ingreso'), 
('Pago a proveedor', 'Egreso'), 
('Pago nómina', 'Egreso');

-- mov_caja
INSERT INTO mov_caja (fecha, tipo_mov_id, valor, concepto, tercero_id) VALUES 
('2024-05-01', 1, 1000000, 'Venta realizada', '1002'), 
('2024-05-02', 2, 500000, 'Pago proveedor', '1003'), 
('2024-05-03', 3, 2000000, 'Pago a empleado', '1001');

-- planes
INSERT INTO planes (nombre, fecha_ini, fecha_fin, dcto) VALUES 
('Plan Primavera', '2024-03-01', '2024-05-31', 10.0), 
('Plan Verano', '2024-06-01', '2024-08-31', 15.0), 
('Plan Invierno', '2024-09-01', '2024-12-31', 20.0);

-- plan_producto
INSERT INTO plan_producto (plan_id, producto_id) VALUES 
(1, 1), 
(2, 2), 
(3, 3);
