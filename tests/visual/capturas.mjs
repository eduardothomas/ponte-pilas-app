// Toma capturas de las pantallas principales en escritorio y móvil. Requiere servidor.py corriendo.
import { chromium } from 'playwright';
import { mkdirSync } from 'node:fs';

const salida = process.argv[2] ?? 'capturas';
mkdirSync(salida, { recursive: true });

const pantallas = [
  { nombre: 'hoy', ruta: '/', esperar: '.pp-seccion' },
  { nombre: 'calendario', ruta: '/calendario', esperar: '.pp-cal' },
  { nombre: 'nueva-tarea', ruta: '/tarea/nueva', esperar: '.pp-form' },
];
const dispositivos = [
  { nombre: 'escritorio', viewport: { width: 1280, height: 900 } },
  { nombre: 'movil', viewport: { width: 390, height: 844 }, isMobile: true, deviceScaleFactor: 2 },
];

const navegador = await chromium.launch();
let errores = 0;
for (const d of dispositivos) {
  const contexto = await navegador.newContext({ viewport: d.viewport, isMobile: d.isMobile, deviceScaleFactor: d.deviceScaleFactor ?? 1, locale: 'es-PA' });
  const pagina = await contexto.newPage();
  pagina.on('pageerror', e => { errores++; console.error(`[${d.nombre}] error de página: ${e.message}`); });
  for (const p of pantallas) {
    await pagina.goto(`http://localhost:5173${p.ruta}`);
    await pagina.waitForSelector(p.esperar, { timeout: 60000 });
    await pagina.waitForTimeout(300);
    await pagina.screenshot({ path: `${salida}/${p.nombre}-${d.nombre}.png`, fullPage: true });
    console.log(`OK ${p.nombre}-${d.nombre}`);
  }
  await contexto.close();
}
await navegador.close();
process.exit(errores > 0 ? 1 : 0);
