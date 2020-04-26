﻿USE [master]
GO
/****** Object:  Database [User]    Script Date: 12.04.2020 18:29:17 ******/
CREATE DATABASE [User]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'User', FILENAME = N'/var/opt/mssql/data/User.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'User_log', FILENAME = N'/var/opt/mssql/data/User_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [User] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [User].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [User] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [User] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [User] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [User] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [User] SET ARITHABORT OFF 
GO
ALTER DATABASE [User] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [User] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [User] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [User] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [User] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [User] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [User] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [User] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [User] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [User] SET  DISABLE_BROKER 
GO
ALTER DATABASE [User] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [User] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [User] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [User] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [User] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [User] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [User] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [User] SET RECOVERY FULL 
GO
ALTER DATABASE [User] SET  MULTI_USER 
GO
ALTER DATABASE [User] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [User] SET DB_CHAINING OFF 
GO
ALTER DATABASE [User] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [User] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [User] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'User', N'ON'
GO
ALTER DATABASE [User] SET QUERY_STORE = OFF
GO
USE [User]
GO
/****** Object:  Table [dbo].[User]    Script Date: 12.04.2020 18:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](25) NOT NULL,
	[LastName] [nvarchar](25) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Account] [nvarchar](25) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Salt] [nvarchar](100) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [User] SET  READ_WRITE 
GO