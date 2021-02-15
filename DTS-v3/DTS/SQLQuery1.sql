
CREATE TABLE [dbo].[Care_Community](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [text] NOT NULL
)
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
	[Resolved] [nvarchar](max) NULL
) 
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
	[File_Complete] [nvarchar](50) NULL
) 
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [nvarchar](50) NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Age] [int] NULL,
	[Gender] [nvarchar](50) NULL,
	[Salary] [int] NULL,
	[Position] [nvarchar](50) NULL
)
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
	[Community_Inititives] [nvarchar](max) NULL
) 
CREATE TABLE [dbo].[Labour_Relations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Union] [nvarchar](50) NULL,
	[Category] [nvarchar](50) NULL,
	[Details] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[Accruals] [varchar](3) NULL,
	[Outcome] [nvarchar](50) NULL,
	[Lessons_Learned] [nvarchar](max) NULL
)
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
	[Details_of_Incident] [nvarchar](50) NULL
) 
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
) 
CREATE TABLE [dbo].[Positions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL
)
CREATE TABLE [dbo].[Sign_in_Main](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[care_community_centre] [nvarchar](50) NULL,
	[user_name] [nvarchar](50) NULL,
	[position] [nvarchar](50) NULL,
	[current_date] [datetime] NULL,
	[week] [int] NULL,
	[date_entred] [datetime] NULL
)
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
	[Date_Register] [datetime] NULL
) 
CREATE TABLE [dbo].[Visits_Agency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[date_of_visit] [datetime] NULL,
	[agency] [nvarchar](50) NULL,
	[findings_number] [int] NULL,
	[findings_details] [text] NULL,
	[corrective_actions] [text] NULL,
	[report_posted] [nchar](3) NULL
) 
CREATE TABLE [dbo].[Visits_Others](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_of_Visit] [datetime] NULL,
	[Agency] [nvarchar](max) NULL,
	[Number_of_Findings] [int] NULL,
	[Details_of_Findings] [text] NULL,
	[Corrective_Actions] [text] NULL,
	[Report_Posted] [nvarchar](50) NULL,
	[LHIN_Letter_Received] [nvarchar](50) NULL,
	[PH_Letter_Received] [nvarchar](50) NULL
) 
CREATE TABLE [dbo].[WSIBs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date_Accident] [datetime] NULL,
	[Employee_Initials] [nvarchar](50) NULL,
	[Accident_Cause] [nvarchar](50) NULL,
	[Date_Duties] [datetime] NULL,
	[Date_Regular] [datetime] NULL,
	[Lost_Days] [int] NULL,
	[Modified_Days_Not_Shadowed] [int] NULL,
	[Modified_Days_Shadowed] [int] NULL)

INSERT [dbo].[Care_Community] ( [Name]) VALUES (N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ([Name]) VALUES (N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES  (N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ([Name]) VALUES ( N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES (N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bradford Valley Care Community
')
INSERT [dbo].[Care_Community] ([Name]) VALUES ( N'A New Community')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES (N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bradford Valley Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'A New Community')
INSERT [dbo].[Care_Community] ( [Name]) VALUES (N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bradford Valley Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'A New Community')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Altamont Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Astoria Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Barnswallow Place Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES (N'Bearbrook Retirement Residence
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bloomington Cove Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'Bradford Valley Care Community
')
INSERT [dbo].[Care_Community] ( [Name]) VALUES ( N'A New Community')


INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'1999-10-10T00:00:00.000' AS DateTime), N'big', N'exception to the rule', N'yes', N'yes', N'yes', N'hot', N'2Pac', N'WAEDGSRFbeSF')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'2019-12-12T00:00:00.000' AS DateTime), N'actual - not specified ', N'anything', N'non', N'no', N'no', N'no', N'no', N'yes')
INSERT [dbo].[Community_Risks] ([Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'1999-10-10T00:00:00.000' AS DateTime), N'big', N'exception to the rule', N'yes', N'yes', N'yes', N'hot', N'2Pac', N'WAEDGSRFbeSF')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'2019-12-12T00:00:00.000' AS DateTime), N'actual - not specified ', N'anything', N'non', N'no', N'no', N'no', N'no', N'yes')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'1999-10-10T00:00:00.000' AS DateTime), N'big', N'exception to the rule', N'yes', N'yes', N'yes', N'hot', N'2Pac', N'WAEDGSRFbeSF')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES (CAST(N'2019-12-12T00:00:00.000' AS DateTime), N'actual - not specified ', N'anything', N'non', N'no', N'no', N'no', N'no', N'yes')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES (CAST(N'1999-10-10T00:00:00.000' AS DateTime), N'big', N'exception to the rule', N'yes', N'yes', N'yes', N'hot', N'2Pac', N'WAEDGSRFbeSF')
INSERT [dbo].[Community_Risks] ( [Date], [Type_Of_Risk], [Descriptions], [Potential_Risk], [MOH_Visit], [Risk_Legal_Action], [Hot_Alert], [Status_Update], [Resolved]) VALUES ( CAST(N'2019-12-12T00:00:00.000' AS DateTime), N'actual - not specified ', N'anything', N'non', N'no', N'no', N'no', N'no', N'yes')
 

INSERT [dbo].[Employees] ([First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES ( N'Donald', N'Trump', 73, N'male', 100000000, N'President')
INSERT [dbo].[Employees] ( [First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES ( N'2Pac', N'Shakur', 49, N'male', 15000, N'Rap Musician')
INSERT [dbo].[Employees] ( [First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES (N'Donald', N'Trump', 73, N'male', 100000000, N'President')
INSERT [dbo].[Employees] ( [First_Name], [Last_Name], [Age], [Gender], [Salary], [Position]) VALUES ( N'2Pac', N'Shakur', 49, N'male', 15000, N'Rap Musician')
 

INSERT [dbo].[Good_News] ( [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES ( N'hey, hey, hey', N'x         ', N'x         ', NULL, N'x         ', N'x         ', N'very much', N'yes', NULL, N'yes       ', N'lots of')
INSERT [dbo].[Good_News] ( [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES ( N'gdsdgbdfbfdbd', N'x         ', N'X         ', N'X         ', N'x         ', N'X         ', N'tupac', N'wu tanfg', NULL, N'NO        ', N'yes')
INSERT [dbo].[Good_News] ( [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES (N'hey, hey, hey', N'x         ', N'x         ', NULL, N'x         ', N'x         ', N'very much', N'yes', NULL, N'yes       ', N'lots of')
INSERT [dbo].[Good_News] ( [Description_Complim], [Respect], [Passion], [Teamwork], [Responsibility], [Growth], [Compliment], [Spot_Awards], [External_Awards], [Awards_Received], [Community_Inititives]) VALUES (N'gdsdgbdfbfdbd', N'x         ', N'X         ', N'X         ', N'x         ', N'X         ', N'tupac', N'wu tanfg', NULL, N'NO        ', N'yes')


INSERT [dbo].[Labour_Relations] ([Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES ( CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CUPE', N'u765', N'Laptop', N'paid222', N'no', N'yes', N'none')
INSERT [dbo].[Labour_Relations] ([Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES ( CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CUPE2', N'u765', N'union memberhip', N'paid', N'no', N'none', N'many')
INSERT [dbo].[Labour_Relations] ([Date], [Union], [Category], [Details], [Status], [Accruals], [Outcome], [Lessons_Learned]) VALUES ( CAST(N'2020-11-11T00:00:00.000' AS DateTime), N'CCC', N'u765', N'union memberhip', N'yes3', N'yes', N'done', N'12 peraon')


INSERT [dbo].[Not_WSIBs] ( [Date_of_Incident], [Employee_Initials], [Position], [Time_of_Incident], [Shift], [Home_Area], [Injury_Related], [Type_of_Injury], [Details_of_Incident]) VALUES ( CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'DJT', N'President', N'3am', N'2nd', N'Done', N'no', N'n/a', N'n/a')


INSERT [dbo].[Outbreaks] ( [Date_Declared], [Date_Concluded], [Type_of_Outbreak], [Total_Days_Closed], [Total_Residents_Affected], [Total_Staff_Affected], [Strain_Identified], [Deaths_Due], [CI_Report_Submitted], [Notify_MOL], [Credit_for_Lost_Days], [LHIN_Letter_Received], [PH_Letter_Received], [Tracking_Sheet_Completed], [Docs_Submitted_Finance]) VALUES (1, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-01T00:00:00.000' AS DateTime), N'n/a', 11, 12, 1, N'yes', 1, N'yes', N'yes', 12, NULL, NULL, N'yes', N'no')

INSERT [dbo].[Positions] ( [Name]) VALUES ( N'Executive Director')
INSERT [dbo].[Positions] ([Name]) VALUES ( N'Registered Nurse')
INSERT [dbo].[Positions] ( [Name]) VALUES ( N'Office Manager')


INSERT [dbo].[Users] ( [First_Name], [Last_Name], [Position], [Care_Community], [Date], [Week], [User_Name], [Login], [Password], [Date_Register]) VALUES (NULL, NULL, 1, 1, CAST(N'2021-11-11T22:22:22.000' AS DateTime), NULL, NULL, N'jleno', N'12345', CAST(N'2021-11-11T23:23:23.000' AS DateTime))

