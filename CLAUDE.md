@AGENTS.md

## Específico de Claude
- En sesiones en la nube, `api.nuget.org` puede estar bloqueado: no se pueden restaurar paquetes ni correr las pruebas en local. Compilar lo que no dependa de paquetes y usar el CI de GitHub Actions como verificación; decir claramente qué se ejecutó y dónde.
- Crear PR con la API REST de GitHub (`gh api repos/eduardothomas/ponte-pilas-app/pulls ...`).
