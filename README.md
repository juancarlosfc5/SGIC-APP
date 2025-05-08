## SGIC APP 

DML

DROP DATABASE NJSIGC;
CREATE DATABASE IF NOT EXISTS NJSIGC;
USE NJSIGC;

-- TIPOS Y TERCEROS
CREATE TABLE tipo_documentos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    descripcion VARCHAR(50)
);

CREATE TABLE tipo_terceros (
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

CREATE TABLE terceros (
    id VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(50),
    apellidos VARCHAR(50),
    email VARCHAR(80) UNIQUE,
    tipo_doc_id INT,
    tipo_tercero_id INT,
    ciudad_id INT,
    FOREIGN KEY (tipo_doc_id) REFERENCES tipo_documentos(id),
    FOREIGN KEY (tipo_tercero_id) REFERENCES tipo_terceros(id),
    FOREIGN KEY (ciudad_id) REFERENCES ciudad(id)
);

CREATE TABLE tercero_telefonos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero VARCHAR(20),
    tipo VARCHAR(20),
    tercero_id VARCHAR(20),
    FOREIGN KEY (tercero_id) REFERENCES terceros(id)
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
    FOREIGN KEY (tercero_id) REFERENCES terceros(id),
    FOREIGN KEY (eps_id) REFERENCES eps(id),
    FOREIGN KEY (arl_id) REFERENCES arl(id)
);

-- CLIENTE
CREATE TABLE clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_id VARCHAR(20),
    nombre VARCHAR (50),
    fecha_nac DATE,
    fecha_ultima_compra DATE,
    FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- PROVEEDOR
CREATE TABLE proveedor (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_id VARCHAR(20),
    dcto DOUBLE,
    dia_pago INT,
    FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- PRODUCTOS
CREATE TABLE productos (
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
CREATE TABLE productos_proveedor (
    producto_id INT,
    tercero_id VARCHAR(20),
    PRIMARY KEY (producto_id, tercero_id),
    FOREIGN KEY (producto_id) REFERENCES productos(id),
    FOREIGN KEY (tercero_id) REFERENCES terceros(id)
);

-- COMPRAS
CREATE TABLE compras (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tercero_prov_id VARCHAR(20),
    fecha DATE,
    tercero_empl_id VARCHAR(20),
    desc_compra TEXT,
    FOREIGN KEY (tercero_prov_id) REFERENCES terceros(id),
    FOREIGN KEY (tercero_empl_id) REFERENCES terceros(id)
);

CREATE TABLE detalle_compra (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    producto_id INT,
    cantidad INT,
    valor DOUBLE,
    compra_id INT,
    FOREIGN KEY (producto_id) REFERENCES productos(id),
    FOREIGN KEY (compra_id) REFERENCES compras(id)
);

-- VENTAS
CREATE TABLE venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE,
    tercero_cli_id VARCHAR(20),
    tercero_emp_id VARCHAR(20),
    forma_pago VARCHAR(50),
    FOREIGN KEY (tercero_cli_id) REFERENCES terceros(id),
    FOREIGN KEY (tercero_emp_id) REFERENCES terceros(id)
);

CREATE TABLE detalle_venta (
    id INT AUTO_INCREMENT PRIMARY KEY,
    factura_id INT,
    producto_id INT,
    cantidad INT,
    valor DOUBLE,
    FOREIGN KEY (factura_id) REFERENCES venta(id),
    FOREIGN KEY (producto_id) REFERENCES productos(id)
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
    FOREIGN KEY (tercero_id) REFERENCES terceros(id)
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
    FOREIGN KEY (producto_id) REFERENCES productos(id)
);
