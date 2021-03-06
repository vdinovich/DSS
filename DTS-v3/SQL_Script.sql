USE [master]
GO
/****** Object:  Database [DSS]    Script Date: 4/19/2020 9:04:27 AM ******/
CREATE DATABASE [DSS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DSS', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DSS.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DSS_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DSS_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DSS] SET COMPATIBILITY_LEVEL = 140
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
ALTER DATABASE [DSS] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DSS] SET QUERY_STORE = OFF
GO
USE [DSS]
GO
/****** Object:  Table [dbo].[Care_Community]    Script Date: 4/19/2020 9:04:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Care_Community](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Community_Risks]    Script Date: 4/19/2020 9:04:29 AM ******/
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
/****** Object:  Table [dbo].[Critical_Incidents]    Script Date: 4/19/2020 9:04:29 AM ******/
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
/****** Object:  Table [dbo].[Employees]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [nvarchar](50) NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[Gender] [nvarchar](50) NULL,
	[Salary] [int] NULL,
	[Position] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Good_News]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Good_News](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description_Complim] [nvarchar](max) NULL,
	[Respect] [nchar](10) NULL,
	[Passion] [nchar](10) NULL,
	[Teamwork] [nchar](10) NULL,
	[Responsibility] [nchar](10) NULL,
	[Growth] [nchar](10) NULL,
	[Compliment] [nvarchar](max) NULL,
	[Spot_Awards] [nvarchar](max) NULL,
	[External_Awards] [nvarchar](max) NULL,
	[Awards_Received] [nchar](10) NULL,
	[Community_Inititives] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labour_Relations]    Script Date: 4/19/2020 9:04:29 AM ******/
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
/****** Object:  Table [dbo].[Not_WSIBs]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Not_WSIBs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_of_Incident] [datetime] NULL,
	[Employee_Initials] [nvarchar](50) NULL,
	[Position] [nvarchar](50) NULL,
	[Time_of_Incident] [nvarchar](50) NULL,
	[Shift] [nvarchar](50) NULL,
	[Home_Area] [nvarchar](50) NULL,
	[Injury_Related] [nvarchar](50) NULL,
	[Type_of_Injury] [nvarchar](50) NULL,
	[Details_of_Incident] [nvarchar](50) NULL,
 CONSTRAINT [PK_Not_WSIBs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Outbreaks]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Outbreaks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_Declared] [datetime] NULL,
	[Date_Concluded] [datetime] NULL,
	[Type_of_Outbreak] [nvarchar](max) NULL,
	[Total_Days_Closed] [int] NULL,
	[Total_Residents_Affected] [int] NULL,
	[Total_Staff_Affected] [int] NULL,
	[Strain_Identified] [nvarchar](max) NULL,
	[Deaths_Due] [int] NULL,
	[CI_Report_Submitted] [nvarchar](50) NULL,
	[Notify_MOL] [nvarchar](50) NULL,
	[Credit_for_Lost_Days] [int] NULL,
	[LHIN_Letter_Received] [nvarchar](50) NULL,
	[PH_Letter_Received] [nvarchar](50) NULL,
	[Tracking_Sheet_Completed] [nvarchar](50) NULL,
	[Docs_Submitted_Finance] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Positions]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Positions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sign_in_Main]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sign_in_Main](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[care_community_centre] [nvarchar](50) NULL,
	[user_name] [nvarchar](50) NULL,
	[position] [nvarchar](50) NULL,
	[current_date] [datetime] NULL,
	[week] [int] NULL,
	[date_entred] [datetime] NULL,
 CONSTRAINT [PK_Sign_in_Main] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [nvarchar](50) NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Position] [int] NULL,
	[Care_Community] [int] NULL,
	[Date] [datetime] NULL,
	[Week] [nvarchar](50) NULL,
	[User_Name] [nvarchar](50) NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Date_Register] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visits_Agency]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visits_Agency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[date_of_visit] [datetime] NULL,
	[agency] [nvarchar](50) NULL,
	[findings_number] [int] NULL,
	[findings_details] [text] NULL,
	[corrective_actions] [text] NULL,
	[report_posted] [nchar](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visits_Others]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visits_Others](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_of_Visit] [datetime] NULL,
	[Agency] [nvarchar](max) NULL,
	[Number_of_Findings] [int] NULL,
	[Details_of_Findings] [text] NULL,
	[Corrective_Actions] [text] NULL,
	[Report_Posted] [nvarchar](50) NULL,
 CONSTRAINT [PK_Visits_Others] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WSIBs]    Script Date: 4/19/2020 9:04:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WSIBs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_Accident] [datetime] NULL,
	[Employee_Initials] [nvarchar](50) NULL,
	[Accident_Cause] [nvarchar](50) NULL,
	[Date_Duties] [datetime] NULL,
	[Date_Regular] [datetime] NULL,
	[Lost_Days] [int] NULL,
	[Modified_Days_Not_Shadowed] [int] NULL,
	[Modified_Days_Shadowed] [int] NULL,
	[Form_7] [nchar](10) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Care_Community] ON 

INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (1, N'Algoma Manor
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (2, N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (3, N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (4, N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (5, N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (6, N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (7, N'Bradford Valley Care Community
')
INSERT [dbo].[Care_Community] ([Id], [Name]) VALUES (16, N'A New Community')
SET IDENTITY_INSERT [dbo].[Care_Community] OFF
SET IDENTITY_INSERT [dbo].[Community_Risks] ON 

INSERT [dbo].[Community_Risks] ([Id], [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES (3, CAST(N'1999-10-10T00:00:00.000' AS DateTime), N'big', N'exception to the rule', N'yes', N'yes', N'yes', N'hot', N'2Pac', N'WAEDGSRFbeSF')
INSERT [dbo].[Community_Risks] ([Id], [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES (5, CAST(N'2019-12-12T00:00:00.000' AS DateTime), N'actual - not specified ', N'anything', N'non', N'no', N'no', N'no', N'no', N'yes')
SET IDENTITY_INSERT [dbo].[Community_Risks] OFF
SET IDENTITY_INSERT [dbo].[Critical_Incidents] ON 

INSERT [dbo].[Critical_Incidents] ([id], [Date], [CI_Form_Number], [CI_Category_Type], [Location], [Brief_Description], [MOH_Notified], [Police_Notified], [POAS_Notified], [Care_Plan_Updated], [Quality_Improvement_Actions], [MOHLTC_Follow_Up], [CIS_Initiated], [Follow_Up_Amendments], [Risk_Locked], [File_Complete]) VALUES (3, CAST(N'2019-01-02T00:00:00.000' AS DateTime), N'000001/19', N'Alleged/Actual Abuse/Assault', N'Hallway', N'Employee while passing nourishments alleged to call a resident derogatory name', N'Yes', N'No ', N'Yes', N'No ', N'Employee placed on Adm. leave/initiating re-education re- resident abuse', NULL, N'Yes', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Critical_Incidents] OFF
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([Id], [First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES (1, N'Donald', N'Trump', 73, N'male', 100000000, N'President')
INSERT [dbo].[Employees] ([Id], [First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES (2, N'2Pac', N'Shakur', 49, N'male', 15000, N'Rap Musician')
SET IDENTITY_INSERT [dbo].[Employees] OFF
SET IDENTITY_INSERT [dbo].[Good_News] ON 

INSERT [dbo].[Good_News] ([Id], [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES (1, N'hey, hey, hey', N'x         ', N'x         ', NULL, N'x         ', N'x         ', N'very much', N'yes', NULL, N'yes       ', N'lots of')
INSERT [dbo].[Good_News] ([Id], [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES (3, N'gdsdgbdfbfdbd', N'x         ', N'X         ', N'X         ', N'x         ', N'X         ', N'tupac', N'wu tanfg', NULL, N'NO        ', N'yes')
SET IDENTITY_INSERT [dbo].[Good_News] OFF
SET IDENTITY_INSERT [dbo].[Labour_Relations] ON 

INSERT [dbo].[Labour_Relations] ([Id], [Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES (1, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CUPE', N'u765', N'Laptop', N'paid222', N'no', N'yes', N'none')
INSERT [dbo].[Labour_Relations] ([Id], [Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES (4, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CUPE2', N'u765', N'union memberhip', N'paid', N'no', N'none', N'many')
INSERT [dbo].[Labour_Relations] ([Id], [Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES (5, CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CCC', N'u765', N'union memberhip', N'yes3', N'yes', N'done', N'12 peraon')
SET IDENTITY_INSERT [dbo].[Labour_Relations] OFF
SET IDENTITY_INSERT [dbo].[Not_WSIBs] ON 

INSERT [dbo].[Not_WSIBs] ([Id], [Date_of_Incident], [Employee_Initials], [Position], [Time_of_Incident], [Shift], [Home_Area], [Injury_Related], [Type_of_Injury], [Details_of_Incident]) VALUES (1, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'DJT', N'President', N'3am', N'2nd', N'Done', N'no', N'n/a', N'n/a')
SET IDENTITY_INSERT [dbo].[Not_WSIBs] OFF
SET IDENTITY_INSERT [dbo].[Outbreaks] ON 

INSERT [dbo].[Outbreaks] ([Id], [Date_Declared], [Date_Concluded], [Type_of_Outbreak], [Total_Days_Closed], [Total_Residents_Affected], [Total_Staff_Affected], [Strain_Identified], [Deaths_Due], [CI_Report_Submitted], [Notify_MOL], [Credit_for_Lost_Days], [LHIN_Letter_Received], [PH_Letter_Received], [Tracking_Sheet_Completed], [Docs_Submitted_Finance]) VALUES (7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'n/a', 11, 12, 1, N'yes', 1, N'yes', N'yes', 12, NULL, NULL, N'yes', N'no')
SET IDENTITY_INSERT [dbo].[Outbreaks] OFF
SET IDENTITY_INSERT [dbo].[Positions] ON 

INSERT [dbo].[Positions] ([Id], [Name]) VALUES (1, N'Executive Director')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (2, N'Registered Nurse')
INSERT [dbo].[Positions] ([Id], [Name]) VALUES (3, N'Office Manager')
SET IDENTITY_INSERT [dbo].[Positions] OFF
SET IDENTITY_INSERT [dbo].[Sign_in_Main] ON 

INSERT [dbo].[Sign_in_Main] ([id], [care_community_centre], [user_name], [position], [current_date], [week], [date_entred]) VALUES (1, N'Astoria Retirement Residence', N'Aser Aspire', N'Office Manager', CAST(N'1988-12-12T00:00:00.000' AS DateTime), 33, CAST(N'2019-12-13T07:25:10.203' AS DateTime))
INSERT [dbo].[Sign_in_Main] ([id], [care_community_centre], [user_name], [position], [current_date], [week], [date_entred]) VALUES (2, N'Astoria Retirement Residence', N'Tramp2', N'Registered Nurse', CAST(N'1996-12-12T00:00:00.000' AS DateTime), 4, CAST(N'2019-12-13T07:29:08.123' AS DateTime))
INSERT [dbo].[Sign_in_Main] ([id], [care_community_centre], [user_name], [position], [current_date], [week], [date_entred]) VALUES (3, N'Bloomington Cove Care Community
', N'Man1', N'Registered Nurse', CAST(N'1999-12-12T00:00:00.000' AS DateTime), 12, CAST(N'2020-01-04T10:26:51.517' AS DateTime))
SET IDENTITY_INSERT [dbo].[Sign_in_Main] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [First_Name], [Last_Name], [Position], [Care_Community], [Date], [Week], [User_Name], [Login], [Password], [Date_Register]) VALUES (2, N'Jay', N'Leno', 1, 1, CAST(N'2019-10-04T00:00:00.000' AS DateTime), N'43', N'Jay Leno', N'jleno', N'12345', CAST(N'2019-12-02T10:09:46.257' AS DateTime))
INSERT [dbo].[Users] ([Id], [First_Name], [Last_Name], [Position], [Care_Community], [Date], [Week], [User_Name], [Login], [Password], [Date_Register]) VALUES (4, N'Jacob', N'First', 2, 2, CAST(N'1999-11-11T00:00:00.000' AS DateTime), N'22', N'JFirst', N'Admin', N'9999', CAST(N'2019-11-11T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
SET IDENTITY_INSERT [dbo].[Visits_Agency] ON 

INSERT [dbo].[Visits_Agency] ([Id], [date_of_visit], [agency], [findings_number], [findings_details], [corrective_actions], [report_posted]) VALUES (1, CAST(N'2019-12-10T00:00:00.000' AS DateTime), N'Public Health', 1, N'Provided with Green Pass', N'n/A', N'YES')
SET IDENTITY_INSERT [dbo].[Visits_Agency] OFF
SET IDENTITY_INSERT [dbo].[Visits_Others] ON 

INSERT [dbo].[Visits_Others] ([Id], [Date_of_Visit], [Agency], [Number_of_Findings], [Details_of_Findings], [Corrective_Actions], [Report_Posted]) VALUES (6, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'Public Health', 5, N'n/a', N'n/a', N'yes')
INSERT [dbo].[Visits_Others] ([Id], [Date_of_Visit], [Agency], [Number_of_Findings], [Details_of_Findings], [Corrective_Actions], [Report_Posted]) VALUES (8, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'Public Health', 4, N'n/a', N'n/a', N'yes')
INSERT [dbo].[Visits_Others] ([Id], [Date_of_Visit], [Agency], [Number_of_Findings], [Details_of_Findings], [Corrective_Actions], [Report_Posted]) VALUES (9, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'Public Health', 3, N'n/a', N'n/a', N'yes')
INSERT [dbo].[Visits_Others] ([Id], [Date_of_Visit], [Agency], [Number_of_Findings], [Details_of_Findings], [Corrective_Actions], [Report_Posted]) VALUES (11, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'Public Health Ontario', 222, N'n/a updated', N'22', N'yes')
SET IDENTITY_INSERT [dbo].[Visits_Others] OFF
SET IDENTITY_INSERT [dbo].[WSIBs] ON 

INSERT [dbo].[WSIBs] ([Id], [Date_Accident], [Employee_Initials], [Accident_Cause], [Date_Duties], [Date_Regular], [Lost_Days], [Modified_Days_Not_Shadowed], [Modified_Days_Shadowed], [Form_7]) VALUES (2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'DT', N'n/a', CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), -9, 3, 44, N'yes       ')
SET IDENTITY_INSERT [dbo].[WSIBs] OFF
USE [master]
GO
ALTER DATABASE [DSS] SET  READ_WRITE 
GO
