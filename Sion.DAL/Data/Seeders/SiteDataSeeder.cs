using Microsoft.EntityFrameworkCore;
using Sion.DAL.Context;
using Sion.DAL.Entities;

namespace Sion.DAL.Data.Seeders;

public static class SiteDataSeeder
{
    public static async Task SeedAsync(SionDbContext context)
    {
        // Configuraciones del sitio
        if (!await context.ConfiguracionesSitio.AnyAsync())
        {
            var configuraciones = new List<ConfiguracionSitio>
            {
                new() { Clave = "Hero:Titulo",        Valor = "Transformando vidas en Tres Ríos desde 2008",                                                    Descripcion = "Título principal del hero" },
                new() { Clave = "Hero:Subtitulo",     Valor = "Somos una organización cristiana comprometida con el desarrollo integral de familias vulnerables", Descripcion = "Subtítulo del hero" },
                new() { Clave = "Hero:ImagenFondo",   Valor = "",                                                                                                Descripcion = "Ruta imagen de fondo del hero" },
                new() { Clave = "Contador:Beneficiados", Valor = "700",  Descripcion = "Personas beneficiadas" },
                new() { Clave = "Contador:Directos",     Valor = "200",  Descripcion = "Beneficiados directos" },
                new() { Clave = "Contador:Anios",        Valor = "17",   Descripcion = "Años de trabajo" },
                new() { Clave = "Contador:Programas",    Valor = "10",   Descripcion = "Programas activos" },
                new() { Clave = "CTA:Titulo",         Valor = "Tu donación cambia vidas",                                                                        Descripcion = "Título bloque CTA donación" },
                new() { Clave = "CTA:Subtitulo",      Valor = "Cada aporte nos permite mantener nuestros programas y llegar a más familias en Concepción de Tres Ríos.", Descripcion = "Subtítulo bloque CTA donación" },
            };

            context.ConfiguracionesSitio.AddRange(configuraciones);
        }

        // Secciones del home
        if (!await context.SeccionesHome.AnyAsync())
        {
            var secciones = new List<SeccionHome>
            {
                new() { Titulo = "Una comunidad unida por la fe y el servicio",  Descripcion = "Desde 2008 trabajamos junto a las familias de Concepción de Tres Ríos para generar oportunidades reales de desarrollo. Nuestro enfoque cristocéntrico nos guía a servir con amor, dignidad y compromiso.", Estilo = "ImagenDerecha",   Orden = 1, EstaActiva = true, FechaModificacion = DateTime.UtcNow },
                new() { Titulo = "10 programas para el desarrollo integral",     Descripcion = "Fútbol, taekwondo, tutorías, laboratorio de cómputo, inglés, comedor comunitario, patrocinio de niños, subsidio para madres solteras, equipos médicos y gestión de vivienda.",                              Estilo = "ImagenIzquierda", Orden = 2, EstaActiva = true, FechaModificacion = DateTime.UtcNow },
            };

            context.SeccionesHome.AddRange(secciones);
        }

        await context.SaveChangesAsync();
    }
}