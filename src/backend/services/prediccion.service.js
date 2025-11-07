import { getDB } from "../config/db.js";

// regresión lineal simple y = a*x + b
function regresionLineal(puntos) {
  const n = puntos.length;
  const sumX = puntos.reduce((acc, p) => acc + p.x, 0);
  const sumY = puntos.reduce((acc, p) => acc + p.y, 0);
  const sumXY = puntos.reduce((acc, p) => acc + p.x * p.y, 0);
  const sumX2 = puntos.reduce((acc, p) => acc + p.x * p.x, 0);

  const denom = n * sumX2 - sumX * sumX;
  if (denom === 0) {
    return { a: 0, b: sumY / n };
  }
  const a = (n * sumXY - sumX * sumY) / denom;
  const b = (sumY - a * sumX) / n;
  return { a, b };
}

export async function predecirVentasAvanzado() {
  const db = await getDB();

  // ventas por día últimos 30
  const rows = await db.all(`
    SELECT DATE(fecha) as fecha, SUM(total) as total_dia
    FROM ventas
    WHERE DATE(fecha) >= DATE('now', '-30 day')
    GROUP BY DATE(fecha)
    ORDER BY fecha ASC
  `);

  if (rows.length === 0) {
    return { proyeccion: 0, base: [], productosAltaRotacion: [] };
  }

  const puntos = rows.map((r, idx) => ({
    x: idx,
    y: Number(r.total_dia)
  }));

  const { a, b } = regresionLineal(puntos);
  const nextX = puntos.length;
  const proyeccion = a * nextX + b;

  // productos de alta rotación últimos 15 días
  const rotacion = await db.all(`
    SELECT vd.producto_id, p.nombre, SUM(vd.cantidad) as total_vendida
    FROM venta_detalle vd
    JOIN productos p ON p.id = vd.producto_id
    JOIN ventas v ON v.id = vd.venta_id
    WHERE DATE(v.fecha) >= DATE('now', '-15 day')
    GROUP BY vd.producto_id, p.nombre
    ORDER BY total_vendida DESC
    LIMIT 10
  `);

  return {
    proyeccion: Math.max(0, Math.round(proyeccion)),
    base: rows,
    tendencia: { a, b },
    productosAltaRotacion: rotacion
  };
}

export async function getDashboardData() {
  const db = await getDB();
  const hoy = await db.get(
    `SELECT SUM(total) as total_hoy FROM ventas WHERE DATE(fecha) = DATE('now')`
  );
  const stockBajo = await db.all(
    `SELECT * FROM productos WHERE stock < 5 ORDER BY stock ASC`
  );
  return {
    ventasHoy: hoy?.total_hoy || 0,
    stockBajo
  };
}
