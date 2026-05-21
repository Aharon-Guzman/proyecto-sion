using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sion.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintsAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── ConfiguracionSitio: longitudes y unique ───────────────────────────
            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "ConfiguracionesSitio",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "ConfiguracionesSitio",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Clave",
                table: "ConfiguracionesSitio",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesSitio_Clave",
                table: "ConfiguracionesSitio",
                column: "Clave",
                unique: true);

            // ── SeccionHome: longitudes e índice de orden ─────────────────────────
            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "SeccionesHome",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RutaImagen",
                table: "SeccionesHome",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Estilo",
                table: "SeccionesHome",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SeccionesHome_Orden",
                table: "SeccionesHome",
                column: "Orden");

            // ── ImagenGaleria: longitudes e índice de estado ──────────────────────
            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "ImagenesGaleria",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RutaOriginal",
                table: "ImagenesGaleria",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RutaThumbnail",
                table: "ImagenesGaleria",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RutaWebP",
                table: "ImagenesGaleria",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ImagenesGaleria_EstaActiva",
                table: "ImagenesGaleria",
                column: "EstaActiva");

            // ── Donacion: longitudes, unique en PayPal ID, índice de fecha ────────
            migrationBuilder.AlterColumn<string>(
                name: "TransaccionPaypalId",
                table: "Donaciones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Donaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NombreDonante",
                table: "Donaciones",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailDonante",
                table: "Donaciones",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Moneda",
                table: "Donaciones",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Donaciones_TransaccionPaypalId",
                table: "Donaciones",
                column: "TransaccionPaypalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donaciones_FechaRegistro",
                table: "Donaciones",
                column: "FechaRegistro");

            // ── LogAuditoria: longitudes e índice de fecha ────────────────────────
            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                table: "LogsAuditoria",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EntidadAfectada",
                table: "LogsAuditoria",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioEmail",
                table: "LogsAuditoria",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_FechaHora",
                table: "LogsAuditoria",
                column: "FechaHora");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar índices
            migrationBuilder.DropIndex(name: "IX_ConfiguracionesSitio_Clave", table: "ConfiguracionesSitio");
            migrationBuilder.DropIndex(name: "IX_SeccionesHome_Orden", table: "SeccionesHome");
            migrationBuilder.DropIndex(name: "IX_ImagenesGaleria_EstaActiva", table: "ImagenesGaleria");
            migrationBuilder.DropIndex(name: "IX_Donaciones_TransaccionPaypalId", table: "Donaciones");
            migrationBuilder.DropIndex(name: "IX_Donaciones_FechaRegistro", table: "Donaciones");
            migrationBuilder.DropIndex(name: "IX_LogsAuditoria_FechaHora", table: "LogsAuditoria");

            // Revertir columnas a nvarchar(max)
            migrationBuilder.AlterColumn<string>(name: "Valor", table: "ConfiguracionesSitio",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(1000)", oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(name: "Descripcion", table: "ConfiguracionesSitio",
                type: "nvarchar(max)", nullable: true,
                oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500, oldNullable: true);

            migrationBuilder.AlterColumn<string>(name: "Clave", table: "ConfiguracionesSitio",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(name: "Titulo", table: "SeccionesHome",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(150)", oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(name: "RutaImagen", table: "SeccionesHome",
                type: "nvarchar(max)", nullable: true,
                oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500, oldNullable: true);

            migrationBuilder.AlterColumn<string>(name: "Estilo", table: "SeccionesHome",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(50)", oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(name: "Titulo", table: "ImagenesGaleria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(200)", oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(name: "RutaOriginal", table: "ImagenesGaleria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(name: "RutaThumbnail", table: "ImagenesGaleria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(name: "RutaWebP", table: "ImagenesGaleria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(500)", oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(name: "TransaccionPaypalId", table: "Donaciones",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(name: "Estado", table: "Donaciones",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(50)", oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(name: "NombreDonante", table: "Donaciones",
                type: "nvarchar(max)", nullable: true,
                oldClrType: typeof(string), oldType: "nvarchar(200)", oldMaxLength: 200, oldNullable: true);

            migrationBuilder.AlterColumn<string>(name: "EmailDonante", table: "Donaciones",
                type: "nvarchar(max)", nullable: true,
                oldClrType: typeof(string), oldType: "nvarchar(254)", oldMaxLength: 254, oldNullable: true);

            migrationBuilder.AlterColumn<string>(name: "Moneda", table: "Donaciones",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(10)", oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(name: "Accion", table: "LogsAuditoria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(name: "EntidadAfectada", table: "LogsAuditoria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(100)", oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(name: "UsuarioEmail", table: "LogsAuditoria",
                type: "nvarchar(max)", nullable: false,
                oldClrType: typeof(string), oldType: "nvarchar(254)", oldMaxLength: 254);
        }
    }
}
