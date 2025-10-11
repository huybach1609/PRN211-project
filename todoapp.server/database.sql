﻿USE [master]
GO
Drop database [PRN231_project]
GO
Create database [PRN231_project]
GO
ALTER DATABASE [PRN231_project] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PRN231_project].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PRN231_project] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PRN231_project] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PRN231_project] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PRN231_project] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PRN231_project] SET ARITHABORT OFF 
GO
ALTER DATABASE [PRN231_project] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PRN231_project] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PRN231_project] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PRN231_project] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PRN231_project] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PRN231_project] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PRN231_project] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PRN231_project] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PRN231_project] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PRN231_project] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PRN231_project] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PRN231_project] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PRN231_project] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PRN231_project] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PRN231_project] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PRN231_project] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PRN231_project] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PRN231_project] SET RECOVERY FULL 
GO
ALTER DATABASE [PRN231_project] SET  MULTI_USER 
GO
ALTER DATABASE [PRN231_project] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PRN231_project] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PRN231_project] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PRN231_project] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PRN231_project] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PRN231_project] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PRN231_project', N'ON'
GO
ALTER DATABASE [PRN231_project] SET QUERY_STORE = OFF
GO
USE [PRN231_project]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[FullName] [nvarchar](400) NULL,
	[Email] [nvarchar](max) NULL,
	[Roll] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[List]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[List](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[AccountID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StickyNote]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StickyNote](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [StickyNote_pk] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubTask]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubTask](
	[Id] [int] IDENTITY(1,1)NOT NULL,
	[TaskId] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagsTask]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagsTask](
	[TagsId] [int] NOT NULL,
	[TaskId] [int] NOT NULL,
	[status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TagsId] ASC,
	[TaskId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 3/20/2024 8:52:47 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DueDate] [date] NULL,
	[Status] [bit] NULL,
	[ListId] [int] NULL,
	[CreateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (1, N'huybach', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Lê Huy Bách ', N'bachlh03@gmail.com', 1)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (2, N'admin', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'admin a', N'bach@gmail.com', 0)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (3, N'huulong', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Tran Huu Long', N'hulong02@gmail.com', 1)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (4, N'kiencuong', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Nguyễn Trung Kiên Cường', N'kc@gmail.com', 1)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (5, N'trungkien', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Nguyễn Trung Kiên', N'trungkien@gmial.com', 1)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (6, N'truongan', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Vũ Trường An ', N'truongan@gmail.com', 1)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [FullName], [Email], [Roll]) VALUES (7, N'Cuong', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Tri', N'cuong@gmail.com', 1)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[List] ON 

INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (2, N'Default', 2)
INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (5, N'Document 3', 1)
INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (12, N'Document', 1)
INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (13, N'Message', 1)
INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (14, N'OutDate', 1)
INSERT [dbo].[List] ([Id], [Name], [AccountID]) VALUES (15, N'New List Note', 7)
SET IDENTITY_INSERT [dbo].[List] OFF
GO
SET IDENTITY_INSERT [dbo].[StickyNote] ON 

INSERT [dbo].[StickyNote] ([Id], [AccountId], [Name], [Details]) VALUES (2, 1, N'Content Strategy 123', N'Would need time to get insights (goals, personals, budget, audits), but after, it would be good to focus on assembling my team (start with SEO specialist, then perhaps an email marketer?). Also need to brainstorm on tooling.')
INSERT [dbo].[StickyNote] ([Id], [AccountId], [Name], [Details]) VALUES (4, 1, N'Banner', N'Notes from the workshop: 
- Sizing matters 
- Choose distinctive imagery 
- The landing page must match the display ad')
INSERT [dbo].[StickyNote] ([Id], [AccountId], [Name], [Details]) VALUES (9, 1, N'Password Facebook', N'asdfasdf')
INSERT [dbo].[StickyNote] ([Id], [AccountId], [Name], [Details]) VALUES (11, 1, N'Hello', N'Number code: 234
')
INSERT [dbo].[StickyNote] ([Id], [AccountId], [Name], [Details]) VALUES (14, 1, N'nothing here', N'123df wqr

')
SET IDENTITY_INSERT [dbo].[StickyNote] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1, N'school')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (2, N'home')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (3, N'work')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (6, N'deadline')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (8, N'new')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (9, N'old')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (10, N'out')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (1, 21, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (1, 25, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (2, 21, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (3, 21, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (3, 26, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (8, 26, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (9, 24, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (9, 26, 1)
INSERT [dbo].[TagsTask] ([TagsId], [TaskId], [status]) VALUES (10, 26, 1)
GO
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([Id], [Name], [Description], [DueDate], [Status], [ListId], [CreateDate]) VALUES (21, N'Listen to and follow ‘Matter of Opinion’', N'<h3 id="link-8b154a0" class="css-1vs5pxi e1gnsphs0"><strong class="css-8qgvsz ebyp5n10"><em class="css-2fg4z9 e1gzwzxm0">Listen to and follow &lsquo;Matter of Opinion&rsquo;</em></strong><br><strong class="css-8qgvsz ebyp5n10"><a class="css-yywogo" title="" href="https://podcasts.apple.com/us/podcast/matter-of-opinion/id1438024613" target="_blank" rel="noopener noreferrer"><em class="css-2fg4z9 e1gzwzxm0">Apple Podcasts</em></a></strong><strong class="css-8qgvsz ebyp5n10"><em class="css-2fg4z9 e1gzwzxm0">&nbsp;|&nbsp;</em></strong><strong class="css-8qgvsz ebyp5n10"><a class="css-yywogo" title="" href="https://open.spotify.com/show/6bmhSFLKtApYClEuSH8q42?si=b1efc95656564f2d" target="_blank" rel="noopener noreferrer"><em class="css-2fg4z9 e1gzwzxm0">Spotify</em></a></strong><strong class="css-8qgvsz ebyp5n10"><em class="css-2fg4z9 e1gzwzxm0">&nbsp;|&nbsp;</em></strong><strong class="css-8qgvsz ebyp5n10"><a class="css-yywogo" title="" href="https://music.amazon.com/podcasts/b42495b5-3d35-424f-8dcb-2b402b49f9ea/matter-of-opinion" target="_blank" rel="noopener noreferrer"><em class="css-2fg4z9 e1gzwzxm0">Amazon Music</em></a></strong></h3>
<hr class="css-7ad88g e1mu4ftr0">
<p class="css-at9mc1 evys1bk0">Could Donald Trump&rsquo;s promise to be a dictator on day one come true?</p>
<p class="css-at9mc1 evys1bk0">On this episode of &ldquo;Matter of Opinion,&rdquo; the hosts debate which policies could be most consequential in a potential second Trump term and whether a proposal set out by conservative allies could provide the tools to execute his vision.</p>
<p class="css-at9mc1 evys1bk0">And Michelle Cottle shares her passion for a trend that can only be achieved with lots of volume.</p>
<p class="css-at9mc1 evys1bk0"><em class="css-2fg4z9 e1gzwzxm0">(A full transcript of this audio essay will be available within 24 hours of publication in the audio player above.)</em></p>
<p class="css-at9mc1 evys1bk0">&nbsp;</p>', CAST(N'2024-03-28' AS Date), 1, 5, CAST(N'2024-03-08T18:47:25.833' AS DateTime))
INSERT [dbo].[Task] ([Id], [Name], [Description], [DueDate], [Status], [ListId], [CreateDate]) VALUES (24, N'Document ', N'<h3 id="link-3f64e4bc" class="css-xkf25q e1h9rw200" data-testid="headline">Reminder: Trump&rsquo;s Last Year in Office Was a National Nightmare</h3>
<p class="css-xkf25q e1h9rw200" data-testid="headline">One of the amazing political achievements of Republicans in this election cycle has been their ability, at least so far, to send Donald Trump&rsquo;s last year in office down the memory hole. Voters are supposed to remember the good economy of January 2020, with its combination of low unemployment and low inflation, while forgetting about the plague year that followed.</p>', CAST(N'2024-03-01' AS Date), 0, 5, CAST(N'2024-03-08T23:21:38.460' AS DateTime))
INSERT [dbo].[Task] ([Id], [Name], [Description], [DueDate], [Status], [ListId], [CreateDate]) VALUES (25, N'qwertwrtwert', N'<p>qwertyuiop</p>', CAST(N'2024-03-09' AS Date), 0, 5, CAST(N'2024-03-09T00:29:36.977' AS DateTime))
INSERT [dbo].[Task] ([Id], [Name], [Description], [DueDate], [Status], [ListId], [CreateDate]) VALUES (26, N'First note', N'<p>asdfasdf</p>', CAST(N'2024-03-28' AS Date), 0, 15, CAST(N'2024-03-20T07:44:44.507' AS DateTime))
SET IDENTITY_INSERT [dbo].[Task] OFF
GO
ALTER TABLE [dbo].[SubTask] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[TagsTask] ADD  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[Task] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[List]  WITH CHECK ADD FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[StickyNote]  WITH CHECK ADD FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[SubTask]  WITH CHECK ADD  CONSTRAINT [SubTask_Task_Id_fk] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubTask] CHECK CONSTRAINT [SubTask_Task_Id_fk]
GO
ALTER TABLE [dbo].[TagsTask]  WITH CHECK ADD  CONSTRAINT [FK__TagsTask__TagsId__440B1D61] FOREIGN KEY([TagsId])
REFERENCES [dbo].[Tags] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TagsTask] CHECK CONSTRAINT [FK__TagsTask__TagsId__440B1D61]
GO
ALTER TABLE [dbo].[TagsTask]  WITH CHECK ADD  CONSTRAINT [FK__TagsTask__TaskId__44FF419A] FOREIGN KEY([TaskId])
REFERENCES [dbo].[Task] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TagsTask] CHECK CONSTRAINT [FK__TagsTask__TaskId__44FF419A]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [Task_List_Id_fk] FOREIGN KEY([ListId])
REFERENCES [dbo].[List] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [Task_List_Id_fk]
GO
USE [master]
GO
ALTER DATABASE [PRN231_project] SET  READ_WRITE 
GO