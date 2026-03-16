# 🏥 Project: Health - Sistema de Gestión Médica y Administrativa

## 📖 Descripción del Proyecto
**Project: Health** es un sistema integral diseñado para administrar un centro de salud y fisioterapia. El objetivo principal es centralizar la gestión operativa, reemplazando soluciones temporales (plantillas de Notion) por un software a medida, escalable y robusto.

El sistema maneja dos flujos de negocio principales:
1. **Atención Médica:** Gestión de pacientes, historias médicas, diagnósticos y evolución clínica (escala de dolor).
2. **Administrativa/Arrendamiento:** Gestión de clientes (fisioterapeutas externos) y control del alquiler de los espacios del centro.

## 🛠️ Stack Tecnológico
La arquitectura del proyecto está dividida en un backend sólido y un frontend multiplataforma:

**Backend & Base de Datos:**
* **C# / .NET 8:** Framework principal para la construcción de la RESTful API.
* **ASP.NET Core Web API:** Para el manejo de endpoints y lógica de negocio.
* **Entity Framework Core (Code-First):** ORM utilizado para el modelado de datos y migraciones.
* **PostgreSQL:** Motor de base de datos relacional elegido por su rendimiento y fiabilidad.

**Frontend (En Planificación):**
* **Flutter / Dart:** Para la construcción de interfaces nativas tanto en escritorio como en dispositivos móviles desde un solo código base.

## 🏗️ Arquitectura de Datos
Se implementó un diseño relacional optimizado, evitando la redundancia de datos y las "tablas Frankenstein". 
* Separación estricta entre entidades físicas (`Paciente`, `Cliente/Arrendatario`) y eventos transaccionales (`HistoriaMedica`, `Alquiler`).
* Uso de `Enums` fuertemente tipados (Ej. Estado Civil, Identidad de Género) para garantizar la integridad de los datos en la base de datos y estandarizar las respuestas de la API.

## 🚀 Estado Actual del Proyecto (Roadmap)

- [x] **Fase 1: MVP en Notion.** Despliegue de una plantilla funcional para cubrir la necesidad inmediata del cliente.
- [x] **Fase 2.1: Setup del Entorno.** Configuración del workspace con .NET SDK, PostgreSQL y extensiones de desarrollo.
- [x] **Fase 2.2: Modelado de Datos.** Creación de las entidades principales (`Paciente`, `Cliente`, `HistoriaMedica`) y configuración del `AppDbContext`.
- [ ] **Fase 2.3: Migraciones y Conexión.** Ejecución de migraciones a PostgreSQL. *(En progreso)*
- [ ] **Fase 2.4: Desarrollo de Controladores.** Creación de operaciones CRUD para las entidades principales.
- [ ] **Fase 3: Desarrollo Frontend.** Inicio de la aplicación en Flutter y consumo de la API.

## 💻 Instalación y Ejecución Local
*(Instrucciones en construcción a la espera de la primera migración de la base de datos)*
