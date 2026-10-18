"""Servidor para capturas visuales: sirve la web publicada (con fallback SPA) y una API falsa con datos de ejemplo.

Uso: python3 servidor.py <carpeta-wwwroot>
Web en http://localhost:5173 y API falsa en http://localhost:5080.
"""
import json
import sys
import threading
import uuid
from datetime import date, timedelta
from functools import partial
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path

HOY = date.today()


def tarea(titulo, dias=None, hora=None, prioridad="Normal", repeticion="Ninguna", completada=False, avisos=(0,)):
    return {
        "id": str(uuid.uuid4()),
        "titulo": titulo,
        "notas": None,
        "fecha": None if dias is None else (HOY + timedelta(days=dias)).isoformat(),
        "hora": hora,
        "prioridad": prioridad,
        "repeticion": repeticion,
        "completada": completada,
        "completadaEn": None,
        "avisosMinutosAntes": list(avisos) if dias is not None else [],
    }


TAREAS = [
    tarea("Renovar la licencia de conducir", -2, "10:00:00", "Alta"),
    tarea("Pagar la tarjeta de crédito", 0, "09:00:00", "Urgente", avisos=(0, 60)),
    tarea("Llamar al seguro del carro", 0, "15:30:00"),
    tarea("Gimnasio", 0, "18:00:00", "Baja", "Semanal"),
    tarea("Comprar regalo de cumpleaños", 1),
    tarea("Revisar propuesta para el cliente", 3, "11:00:00", "Alta"),
    tarea("Pagar alquiler", 14, None, "Alta", "Mensual"),
    tarea("Aprender a hacer pan", None, avisos=()),
    tarea("Ordenar el clóset", None, prioridad="Baja", avisos=()),
    tarea("Enviar facturas", -1, completada=True),
]


class Api(SimpleHTTPRequestHandler):
    def _json(self, codigo, cuerpo):
        datos = json.dumps(cuerpo).encode()
        self.send_response(codigo)
        self.send_header("Content-Type", "application/json")
        self.send_header("Access-Control-Allow-Origin", "*")
        self.send_header("Access-Control-Allow-Headers", "*")
        self.send_header("Access-Control-Allow-Methods", "*")
        self.end_headers()
        self.wfile.write(datos)

    def do_OPTIONS(self):
        self._json(204, {})

    def do_GET(self):
        ruta = self.path.split("?")[0].rstrip("/")
        if ruta == "/api/tareas":
            incluir = "incluirCompletadas=true" in self.path
            return self._json(200, [t for t in TAREAS if incluir or not t["completada"]])
        if ruta.startswith("/api/tareas/"):
            id_ = ruta.split("/")[3]
            return self._json(200, next(t for t in TAREAS if t["id"] == id_))
        self._json(404, {})

    def log_message(self, *args):
        pass


class Web(SimpleHTTPRequestHandler):
    def send_head(self):
        archivo = Path(self.translate_path(self.path))
        if not archivo.exists():
            self.path = "/index.html"
        return super().send_head()

    def guess_type(self, path):
        return "application/wasm" if str(path).endswith(".wasm") else super().guess_type(path)

    def log_message(self, *args):
        pass


if __name__ == "__main__":
    raiz = sys.argv[1]
    threading.Thread(target=ThreadingHTTPServer(("127.0.0.1", 5080), Api).serve_forever, daemon=True).start()
    print("Web en http://localhost:5173, API falsa en http://localhost:5080", flush=True)
    ThreadingHTTPServer(("127.0.0.1", 5173), partial(Web, directory=raiz)).serve_forever()
