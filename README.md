# Proyecto SION — Sitio Web Institucional con Panel Administrativo

Sitio web institucional full-stack desarrollado para una organización sin fines de lucro costarricense, como parte del Trabajo Comunal Universitario (TCU) de la Universidad Americana (UAM). El proyecto cubre el ciclo completo: análisis de requisitos, diseño de arquitectura, desarrollo, pruebas, despliegue en producción y capacitación al cliente.

---

## Descripción

SION es una plataforma web que permite a la organización gestionar su presencia en línea de forma autónoma, sin necesidad de conocimientos técnicos. Incluye un panel administrativo (CMS simplificado), galería fotográfica, formulario de contacto y módulo de donaciones integrado con PayPal.

## Sitio en Producción

**[https://sioncr.org](https://sioncr.org)** — Sistema desplegado y en uso activo por la organización.
---

## Stack Tecnológico

### Backend
- **C# / ASP.NET Core 8** — Razor Pages
- **Arquitectura en 3 capas**: Presentation Layer (PL), Business Logic Layer (BLL), Data Access Layer (DAL)
- **Patrones**: Repository Pattern, Dependency Injection
- **ORM**: Entity Framework Core (Code-First con migraciones)
- **Base de datos**: SQL Server Express 2022
- **Autenticación**: ASP.NET Core Identity

### Frontend
- **Bootstrap 5** — Diseño responsive
- **JavaScript ES6**
- **HTML5 / CSS3**

### Integraciones
- **PayPal .NET SDK** — Módulo de donaciones con webhook IPN, dashboard con filtros y exportación CSV
- **reCAPTCHA v3** — Protección de formularios
- **SMTP** — Notificaciones por correo electrónico

### Infraestructura & DevOps
- **IIS + Windows Server VPS** — Servidor de producción
- **Let's Encrypt** — Certificado SSL
- **Git / GitHub** — Control de versiones
- **Azure DevOps** — Gestión del proyecto con metodología Scrum (Épicas, Features, Tareas)

---

## Funcionalidades Principales

- **Panel Administrativo (CMS)**: Edición de secciones del sitio, gestión de imágenes y control de contenido por personal no técnico
- **Galería fotográfica**: Carga y organización de imágenes desde el panel
- **Módulo de donaciones**: Integración con PayPal, historial de transacciones, filtros y exportación a CSV
- **Formulario de contacto**: Con validación y protección reCAPTCHA v3
- **Autenticación segura**: Login con ASP.NET Core Identity, control de roles

---

## Arquitectura

```
proyecto-sion/
├── PL/               # Presentation Layer — Razor Pages, controladores, vistas
├── BLL/              # Business Logic Layer — Servicios y lógica de negocio
├── DAL/              # Data Access Layer — Repositorios, DbContext, migraciones
└── README.md
```

El diseño en 3 capas permite separar responsabilidades claramente: la capa de presentación no accede directamente a la base de datos, sino que lo hace a través de la capa de negocio, que a su vez utiliza los repositorios de la capa de datos.

---

## Despliegue en Producción

El sistema fue desplegado en un VPS con Windows Server utilizando IIS como servidor web. La configuración incluye:

- Dominio personalizado con SSL (Let's Encrypt)
- SQL Server Express 2022 como motor de base de datos
- Backups automáticos configurados
- Capacitación formal entregada al cliente con documentación de usuario

---

## Gestión del Proyecto

El proyecto fue ejecutado de forma independiente aplicando metodología **Scrum** en Azure DevOps:

- Organización en Épicas → Features → Tareas
- Sprints planificados con entregables definidos
- Entrega formal con período de garantía post-implementación

---

## Contexto Académico

Desarrollado como **Trabajo Comunal Universitario (TCU)** en la Universidad Americana (UAM), Costa Rica. El TCU es un requisito académico que consiste en aplicar conocimientos técnicos en beneficio de una comunidad o organización sin fines de lucro.

---

## Autor

**Aharón David Guzmán Guzmán**  
Ingeniería en Sistemas — Universidad Americana (UAM)  
[LinkedIn](https://www.linkedin.com/in/aharón-guzmán-81948136b) · [GitHub](https://github.com/Aharon-Guzman)
