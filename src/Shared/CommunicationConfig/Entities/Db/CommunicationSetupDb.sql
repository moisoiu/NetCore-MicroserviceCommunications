USE [master]
GO
/****** Object:  Database [Configuration]    Script Date: 21.03.2020 10:40:20 ******/
CREATE DATABASE [Configuration]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Configuration', FILENAME = N'/var/opt/mssql/data/Configuration.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Configuration_log', FILENAME = N'/var/opt/mssql/data/Configuration_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Configuration] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Configuration].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Configuration] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Configuration] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Configuration] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Configuration] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Configuration] SET ARITHABORT OFF 
GO
ALTER DATABASE [Configuration] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Configuration] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Configuration] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Configuration] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Configuration] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Configuration] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Configuration] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Configuration] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Configuration] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Configuration] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Configuration] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Configuration] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Configuration] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Configuration] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Configuration] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Configuration] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Configuration] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Configuration] SET RECOVERY FULL 
GO
ALTER DATABASE [Configuration] SET  MULTI_USER 
GO
ALTER DATABASE [Configuration] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Configuration] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Configuration] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Configuration] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Configuration] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Configuration', N'ON'
GO
ALTER DATABASE [Configuration] SET QUERY_STORE = OFF
GO
USE [Configuration]
GO
/****** Object:  Table [dbo].[Communication]    Script Date: 21.03.2020 10:40:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Communication](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CommunicationModeName] [nvarchar](20) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Communication] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Communication] ON 
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (1, N'AkkaNet', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (2, N'AzureEventBus', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (3, N'AzureServiceBus', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (4, N'Dapr', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (5, N'gRPC', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (6, N'Kafka', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (7, N'NServiceBus', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (8, N'Refit', 0)
GO
INSERT [dbo].[Communication] ([Id], [CommunicationModeName], [IsActive]) VALUES (9, N'SagaWorkFlow', 0)
GO
SET IDENTITY_INSERT [dbo].[Communication] OFF
GO
ALTER TABLE [dbo].[Communication] ADD  CONSTRAINT [DF_Communication_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
USE [master]
GO
ALTER DATABASE [Configuration] SET  READ_WRITE 
GO
