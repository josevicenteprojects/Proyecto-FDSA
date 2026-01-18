# Backoffice Viajes Altairis 🏨🌍

Sistema de gestión hotelera moderna, construido con **Clean Architecture** y **Diseño Premium**.

![Dashboard Preview](frontend/public/dashboard-preview.png)

## 🚀 Inicio Rápido (Quick Start)

Para levantar toda la aplicación (Base de Datos + Backend + Frontend) con un solo comando:

```bash
docker-compose up -d --build
```

El sistema estará disponible en:
- **Frontend**: [http://localhost:3000](http://localhost:3000)
- **Backend API**: [http://localhost:5000/swagger](http://localhost:5000/swagger)

## 🛠️ Tecnologías

### Frontend (`/frontend`)
- **Next.js 14** (App Router)
- **Tailwind CSS** + **Shadcn/ui** (Glassmorphism design)
- **Zustand** (State Management)
- **Recharts** (Analítica)

### Backend (`/backend`)
- **.NET 8 Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Clean Architecture** (Domain, Infrastructure, API)

## 📦 Estructura del Proyecto

```
/
├── backend/            # Solución .NET 8
├── frontend/           # Aplicación Next.js
├── docker-compose.yml  # Orquestación de contenedores
└── README.md
```

## ✨ Características Principales

- **Dashboard en Tiempo Real**: KPIs de ocupación, ingresos y tendencias.
- **Gestión de Hoteles**: CRUD completo con soporte de imágenes premium.
- **Motor de Reservas**: Filtros avanzados (Próximas/Pasadas) y ordenamiento.
- **Inventario**: Control de disponibilidad y precios por habitación.
- **Reportes VIP**: Generación simulada de informes PDF/CSV.

## 👥 Autor

Desarrollado para **Viajes Altairis**.
