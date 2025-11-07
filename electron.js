import { app, BrowserWindow, ipcMain } from "electron";
import path from "path";
import { fileURLToPath } from "url";
import dotenv from "dotenv";
import { initDB, getDB } from "./src/backend/config/db.js";
import { predecirVentasAvanzado, getDashboardData } from "./src/backend/services/prediccion.service.js";

dotenv.config();

const __filename = fileURLToPath(import.meta.url);
const _dirname = path.dirname(_filename);
let mainWindow;

async function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1280,
    height: 720,
    backgroundColor: "#0f172a",
    title: "MiniMarket Sistema",
    webPreferences: {
      preload: path.join(__dirname, "src", "preload.js")
    }
  });

  await initDB();
  mainWindow.loadFile(path.join(__dirname, "src", "ui", "login.html"));
}

app.whenReady().then(() => {
  createWindow();

  app.on("activate", function () {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on("window-all-closed", function () {
  if (process.platform !== "darwin") app.quit();
});

/* ============ IPC handlers =========== */

// login
ipcMain.handle("auth:login", async (event, { username, password }) => {
  const db = await getDB();
  const user = await db.get(
    "SELECT id, username, rol FROM usuarios WHERE username = ? AND password = ? AND activo = 1",
    [username, password]
  );
  if (!user) {
    return { ok: false, message: "Usuario o contraseña inválidos" };
  }
  return { ok: true, user };
});

// dashboard
ipcMain.handle("reportes:dashboard", async () => {
  return await getDashboardData();
});

// predicción avanzada
ipcMain.handle("reportes:prediccion", async () => {
  return await predecirVentasAvanzado();
});
