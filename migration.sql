IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ConfiguracionesSitio] (
    [Id] int NOT NULL IDENTITY,
    [Clave] nvarchar(max) NOT NULL,
    [Valor] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NULL,
    CONSTRAINT [PK_ConfiguracionesSitio] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Donaciones] (
    [Id] int NOT NULL IDENTITY,
    [TransaccionPaypalId] nvarchar(max) NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Moneda] nvarchar(max) NOT NULL,
    [NombreDonante] nvarchar(max) NULL,
    [EmailDonante] nvarchar(max) NULL,
    [EsRecurrente] bit NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    [Estado] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Donaciones] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ImagenesGaleria] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [RutaOriginal] nvarchar(max) NOT NULL,
    [RutaThumbnail] nvarchar(max) NOT NULL,
    [RutaWebP] nvarchar(max) NOT NULL,
    [EstaActiva] bit NOT NULL,
    [FechaSubida] datetime2 NOT NULL,
    CONSTRAINT [PK_ImagenesGaleria] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [LogsAuditoria] (
    [Id] int NOT NULL IDENTITY,
    [Accion] nvarchar(max) NOT NULL,
    [EntidadAfectada] nvarchar(max) NOT NULL,
    [UsuarioEmail] nvarchar(max) NOT NULL,
    [FechaHora] datetime2 NOT NULL,
    [Detalle] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_LogsAuditoria] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [SeccionesHome] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    [RutaImagen] nvarchar(max) NULL,
    [Estilo] nvarchar(max) NOT NULL,
    [Orden] int NOT NULL,
    [EstaActiva] bit NOT NULL,
    [FechaModificacion] datetime2 NOT NULL,
    CONSTRAINT [PK_SeccionesHome] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260519045749_InitialCreate', N'8.0.27');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ConfiguracionesSitio]') AND [c].[name] = N'Valor');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ConfiguracionesSitio] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [ConfiguracionesSitio] ALTER COLUMN [Valor] nvarchar(1000) NOT NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ConfiguracionesSitio]') AND [c].[name] = N'Descripcion');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ConfiguracionesSitio] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [ConfiguracionesSitio] ALTER COLUMN [Descripcion] nvarchar(500) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ConfiguracionesSitio]') AND [c].[name] = N'Clave');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ConfiguracionesSitio] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [ConfiguracionesSitio] ALTER COLUMN [Clave] nvarchar(100) NOT NULL;
GO

CREATE UNIQUE INDEX [IX_ConfiguracionesSitio_Clave] ON [ConfiguracionesSitio] ([Clave]);
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SeccionesHome]') AND [c].[name] = N'Titulo');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [SeccionesHome] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [SeccionesHome] ALTER COLUMN [Titulo] nvarchar(150) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SeccionesHome]') AND [c].[name] = N'RutaImagen');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [SeccionesHome] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [SeccionesHome] ALTER COLUMN [RutaImagen] nvarchar(500) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SeccionesHome]') AND [c].[name] = N'Estilo');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [SeccionesHome] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [SeccionesHome] ALTER COLUMN [Estilo] nvarchar(50) NOT NULL;
GO

CREATE INDEX [IX_SeccionesHome_Orden] ON [SeccionesHome] ([Orden]);
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ImagenesGaleria]') AND [c].[name] = N'Titulo');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [ImagenesGaleria] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [ImagenesGaleria] ALTER COLUMN [Titulo] nvarchar(200) NOT NULL;
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ImagenesGaleria]') AND [c].[name] = N'RutaOriginal');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [ImagenesGaleria] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [ImagenesGaleria] ALTER COLUMN [RutaOriginal] nvarchar(500) NOT NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ImagenesGaleria]') AND [c].[name] = N'RutaThumbnail');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [ImagenesGaleria] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [ImagenesGaleria] ALTER COLUMN [RutaThumbnail] nvarchar(500) NOT NULL;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ImagenesGaleria]') AND [c].[name] = N'RutaWebP');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [ImagenesGaleria] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [ImagenesGaleria] ALTER COLUMN [RutaWebP] nvarchar(500) NOT NULL;
GO

CREATE INDEX [IX_ImagenesGaleria_EstaActiva] ON [ImagenesGaleria] ([EstaActiva]);
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Donaciones]') AND [c].[name] = N'TransaccionPaypalId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Donaciones] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Donaciones] ALTER COLUMN [TransaccionPaypalId] nvarchar(100) NOT NULL;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Donaciones]') AND [c].[name] = N'Estado');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Donaciones] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Donaciones] ALTER COLUMN [Estado] nvarchar(50) NOT NULL;
GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Donaciones]') AND [c].[name] = N'NombreDonante');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Donaciones] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [Donaciones] ALTER COLUMN [NombreDonante] nvarchar(200) NULL;
GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Donaciones]') AND [c].[name] = N'EmailDonante');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Donaciones] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Donaciones] ALTER COLUMN [EmailDonante] nvarchar(254) NULL;
GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Donaciones]') AND [c].[name] = N'Moneda');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Donaciones] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Donaciones] ALTER COLUMN [Moneda] nvarchar(10) NOT NULL;
GO

CREATE UNIQUE INDEX [IX_Donaciones_TransaccionPaypalId] ON [Donaciones] ([TransaccionPaypalId]);
GO

CREATE INDEX [IX_Donaciones_FechaRegistro] ON [Donaciones] ([FechaRegistro]);
GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogsAuditoria]') AND [c].[name] = N'Accion');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [LogsAuditoria] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [LogsAuditoria] ALTER COLUMN [Accion] nvarchar(100) NOT NULL;
GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogsAuditoria]') AND [c].[name] = N'EntidadAfectada');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [LogsAuditoria] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [LogsAuditoria] ALTER COLUMN [EntidadAfectada] nvarchar(100) NOT NULL;
GO

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[LogsAuditoria]') AND [c].[name] = N'UsuarioEmail');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [LogsAuditoria] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [LogsAuditoria] ALTER COLUMN [UsuarioEmail] nvarchar(254) NOT NULL;
GO

CREATE INDEX [IX_LogsAuditoria_FechaHora] ON [LogsAuditoria] ([FechaHora]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260519120000_AddConstraintsAndIndexes', N'8.0.27');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260524004807_AddIdentityTables', N'8.0.27');
GO

COMMIT;
GO

