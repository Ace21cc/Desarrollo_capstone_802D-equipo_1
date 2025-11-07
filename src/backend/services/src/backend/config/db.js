import sqlite3 from "sqlite3";
import { open } from "sqlite";
import fs from "fs";
import path from "path";
import { fileURLToPath } from "url";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const dataDir = path.join(__dirname, "..", "..", "..", "data");
const dbPath = path.join(dataDir, "minimarket.db");

let dbInstance = null;

export async function getDB() {
  if (!dbInstance) {
    dbInstance = await open({
      filename: dbPath,
      driver: sqlite3.Database
    });
  }
  return dbInstance;
}

export async function initDB() {
  if (!fs.existsSync(dataDir)) {
    fs.mkdirSync(dataDir, { recursive: true });
  }

  const db = await getDB();

  await db.exec(`
    CREATE TABLE IF NOT EXISTS productos (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      nombre TEXT NOT NULL,
      codigo TEXT,
      precio REAL NOT NULL DEFAULT 0,
      stock INTEGER NOT NULL DEFAULT 0
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS ventas (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      fecha TEXT NOT NULL,
      total REAL NOT NULL
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS venta_detalle (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      venta_id INTEGER NOT NULL,
      producto_id INTEGER NOT NULL,
      cantidad INTEGER NOT NULL,
      precio_unitario REAL NOT NULL,
      FOREIGN KEY (venta_id) REFERENCES ventas(id),
      FOREIGN KEY (producto_id) REFERENCES productos(id)
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS clientes (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      nombre TEXT NOT NULL,
      telefono TEXT
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS cuentas_cobrar (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      cliente_id INTEGER NOT NULL,
      venta_id INTEGER,
      monto REAL NOT NULL,
      pagado INTEGER DEFAULT 0,
      fecha TEXT NOT NULL,
      FOREIGN KEY (cliente_id) REFERENCES clientes(id),
      FOREIGN KEY (venta_id) REFERENCES ventas(id)
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS proveedores (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      nombre TEXT NOT NULL,
      telefono TEXT
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS cuentas_pagar (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      proveedor_id INTEGER NOT NULL,
      descripcion TEXT,
      monto REAL NOT NULL,
      pagado INTEGER DEFAULT 0,
      fecha TEXT NOT NULL,
      FOREIGN KEY (proveedor_id) REFERENCES proveedores(id)
    );
  `);

  await db.exec(`
    CREATE TABLE IF NOT EXISTS usuarios (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      username TEXT UNIQUE NOT NULL,
      password TEXT NOT NULL,
      rol TEXT NOT NULL,
      activo INTEGER DEFAULT 1
    );
  `);

  // usuario admin y trabajador por defecto
  const admin = await db.get("SELECT * FROM usuarios WHERE username = 'admin'");
  if (!admin) {
    await db.run(
      "INSERT INTO usuarios (username, password, rol) VALUES (?,?,?)",
      ["admin", "1234", "admin"]
    );
    await db.run(
      "INSERT INTO usuarios (username, password, rol) VALUES (?,?,?)",
      ["trabajador", "1234", "trabajador"]
    );
  }

  // productos demo
  const prod = await db.get("SELECT COUNT(*) as c FROM productos");
  if (prod.c === 0) {
    await db.run("INSERT INTO productos (nombre, codigo, precio, stock) VALUES ('Pan', 'P001', 1000, 50)");
    await db.run("INSERT INTO productos (nombre, codigo, precio, stock) VALUES ('Leche', 'L001', 1200, 30)");
    await db.run("INSERT INTO productos (nombre, codigo, precio, stock) VALUES ('Arroz', 'A001', 1500, 40)");
  }
}
