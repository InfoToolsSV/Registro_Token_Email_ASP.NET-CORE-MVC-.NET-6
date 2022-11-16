SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[Apellido] [varchar](50) NULL,
	[Fecha_Nacimiento] [date] NULL,
	[Correo] [varchar](50) NULL,
	[Usuario] [varchar](50) NULL,
	[Pass] [varchar](50) NULL,
	[Token] [varchar](max) NULL,
	[Estado] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_registrar]
@Nombre VARCHAR(50),
@Apellido VARCHAR(50),
@Fecha_Nacimiento date,
@Correo VARCHAR(50),
@Usuario VARCHAR(50),
@Pass VARCHAR(50),
@Token VARCHAR(max),
@Estado bit
AS BEGIN
INSERT INTO Usuarios VALUES(@Nombre, @Apellido, @Fecha_Nacimiento, @Correo, @Usuario, @Pass, @Token, @Estado)
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_validar]
@Token VARCHAR(max)
AS BEGIN
DECLARE @Correo VARCHAR(100)
SET @Correo =(SELECT Correo from Usuarios where Token=@Token)
Update Usuarios set Estado=1 where Token=@Token
Update Usuarios set Token=null where Correo=@Correo
END
GO
