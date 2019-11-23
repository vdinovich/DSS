USE [master]
GO
/****** Object:  Database [DSS]    Script Date: 11/23/2019 4:47:41 PM ******/
CREATE DATABASE [DSS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DSS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DSS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DSS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DSS_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DSS].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DSS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DSS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DSS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DSS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DSS] SET ARITHABORT OFF 
GO
ALTER DATABASE [DSS] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DSS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DSS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DSS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DSS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DSS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DSS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DSS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DSS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DSS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DSS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DSS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DSS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DSS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DSS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DSS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DSS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DSS] SET RECOVERY FULL 
GO
ALTER DATABASE [DSS] SET  MULTI_USER 
GO
ALTER DATABASE [DSS] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DSS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DSS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DSS] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
USE [DSS]
GO
/****** Object:  Table [dbo].[Care_Community]    Script Date: 11/23/2019 4:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Care_Community](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Community_Risks]    Script Date: 11/23/2019 4:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Community_Risks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Type_Of_Risk] [nvarchar](50) NULL,
	[Descriptions] [nvarchar](max) NULL,
	[Potential_Risk] [nvarchar](max) NULL,
	[MOH_Visit] [nvarchar](max) NULL,
	[Risk_Legal_Action] [nvarchar](max) NULL,
	[Hot_Alert] [nvarchar](max) NULL,
	[Status_Update] [nvarchar](max) NULL,
	[Resolved] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Critical_Incidents]    Script Date: 11/23/2019 4:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Critical_Incidents](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[CI_Form_Number] [nvarchar](50) NULL,
	[CI_Category_Type] [nvarchar](max) NULL,
	[Location] [nvarchar](50) NULL,
	[Brief_Description] [nvarchar](max) NULL,
	[MOH_Notified] [nchar](3) NULL,
	[Police_Notified] [nchar](3) NULL,
	[POAS_Notified] [nchar](3) NULL,
	[Care_Plan_Updated] [nchar](3) NULL,
	[Quality_Improvement_Actions] [nvarchar](max) NULL,
	[MOHLTC_Follow_Up] [nvarchar](max) NULL,
	[CIS_Initiated] [nvarchar](50) NULL,
	[Follow_Up_Amendments] [nvarchar](50) NULL,
	[Risk_Locked] [nvarchar](50) NULL,
	[File_Complete] [nvarchar](50) NULL,
 CONSTRAINT [PK_Critical_Incidents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labour_Relations]    Script Date: 11/23/2019 4:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Labour_Relations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Union] [nvarchar](50) NULL,
	[Category] [nvarchar](50) NULL,
	[Details] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Accruals] [varchar](3) NULL,
	[Outcome] [nvarchar](50) NULL,
	[Lessons_Learned] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Positions]    Script Date: 11/23/2019 4:47:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Care_Community] ON 

INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (1, N'Algoma Manor
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (2, N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (3, N'Astoria Retirement Residence')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (4, N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (5, N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (6, N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (7, N'Bradford Valley Care Community
')
SET IDENTITY_INSERT [dbo].[Care_Community] OFF
SET IDENTITY_INSERT [dbo].[Community_Risks] ON 

INSERT [dbo].[Community_Risks] ([Id], [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES (1, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'asfafa', N'sdgbasfbadzfbadF', N'SDVSDVSDV', N'ADSVSDV', N'SDVVSV ', N'SDVSDVSDV DFXC', N'waedvWESDvw', N'WAEDGSRFbeSF')
SET IDENTITY_INSERT [dbo].[Community_Risks] OFF
SET IDENTITY_INSERT [dbo].[Critical_Incidents] ON 

INSERT [dbo].[Critical_Incidents] ([id], [Date], [CI_Form_Number], [CI_Category_Type], [Location], [Brief_Description], [MOH_Notified], [Police_Notified], [POAS_Notified], [Care_Plan_Updated], [Quality_Improvement_Actions], [MOHLTC_Follow_Up], [CIS_Initiated], [Follow_Up_Amendments], [Risk_Locked], [File_Complete]) VALUES (1, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'we1111111', N'sfas3/5', N'Fox Ridge', N'a fallen patient', N'yes', N'no ', N'no ', N'yes', N'n/a', N'yes', N'yes', N'yes', N'yes', N'yes')
INSERT [dbo].[Critical_Incidents] ([id], [Date], [CI_Form_Number], [CI_Category_Type], [Location], [Brief_Description], [MOH_Notified], [Police_Notified], [POAS_Notified], [Care_Plan_Updated], [Quality_Improvement_Actions], [MOHLTC_Follow_Up], [CIS_Initiated], [Follow_Up_Amendments], [Risk_Locked], [File_Complete]) VALUES (2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'231123NPR', N'cd', N'Altamont', N'patient complaint', N'yes', N'yes', N'yes', N'yes', N'testing', N'yes', N'yes', N'yes', N'yes', N'yes')
SET IDENTITY_INSERT [dbo].[Critical_Incidents] OFF
SET IDENTITY_INSERT [dbo].[Labour_Relations] ON 

INSERT [dbo].[Labour_Relations] ([Id], [Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES (1, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CUPE', N'u765', N'Laptop', N'paid222', N'no', N'yes', N'none')
SET IDENTITY_INSERT [dbo].[Labour_Relations] OFF
SET IDENTITY_INSERT [dbo].[Positions] ON 

INSERT [dbo].[Positions] ([Id], [Name]) VALUES (1, N'Executive Director')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (2, N'Office Manager')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (3, N'Registered Nurse')
SET IDENTITY_INSERT [dbo].[Positions] OFF
USE [master]
GO
ALTER DATABASE [DSS] SET  READ_WRITE 
GO
