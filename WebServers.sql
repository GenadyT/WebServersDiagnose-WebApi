USE [master]
GO
/****** Object:  Database [WebServers]    Script Date: 8/30/2024 9:51:07 PM ******/
CREATE DATABASE [WebServers]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebServers', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WebServers.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebServers_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\WebServers_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [WebServers] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebServers].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebServers] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebServers] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebServers] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebServers] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebServers] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebServers] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebServers] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebServers] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebServers] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebServers] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebServers] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebServers] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebServers] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebServers] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebServers] SET  ENABLE_BROKER 
GO
ALTER DATABASE [WebServers] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebServers] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebServers] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebServers] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebServers] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebServers] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebServers] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebServers] SET RECOVERY FULL 
GO
ALTER DATABASE [WebServers] SET  MULTI_USER 
GO
ALTER DATABASE [WebServers] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebServers] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebServers] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebServers] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebServers] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebServers] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebServers', N'ON'
GO
ALTER DATABASE [WebServers] SET QUERY_STORE = ON
GO
ALTER DATABASE [WebServers] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [WebServers]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetServerMonitoringStatus]    Script Date: 8/30/2024 9:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[fnGetServerMonitoringStatus]
(
	@ServerID int,	
	@HttpResponseCode int,
	@HttpResponseLatency int
)
RETURNS INT

AS
BEGIN

	DECLARE @@RecordsCount As INT; 
	DECLARE @@MonitoringStatus As INT;
	SET @@MonitoringStatus = 1;

	DECLARE @SeekingHealthID int;
	IF (@HttpResponseCode >= 200 AND @HttpResponseCode < 300) AND (@HttpResponseLatency < 60000)
		SET @SeekingHealthID = 2
	ELSE IF (@HttpResponseCode < 200) OR (@HttpResponseCode >= 300) OR (@HttpResponseLatency >= 60000)
		SET @SeekingHealthID = 3
	ELSE
		SET @SeekingHealthID = 1


	IF @SeekingHealthID = 2
	BEGIN
		SELECT @@RecordsCount = COUNT(*)
		FROM 
			(
				SELECT TOP 4 *
				FROM tblMonitorHistory mh
				WHERE mh.[ServerID] = @ServerID
				ORDER BY mh.[MonitorTime] DESC
			) t
		WHERE 
			(t.HttpResponseCode >= 200 AND t.HttpResponseCode < 300) AND
			(t.HttpResponseLatency < 60)

		IF @@RecordsCount = 4	SET @@MonitoringStatus = 2;
	END

	IF @SeekingHealthID = 3 
	BEGIN
		SELECT @@RecordsCount = COUNT(*)
		FROM 
			(
				SELECT TOP 2 *
				FROM tblMonitorHistory mh
				WHERE mh.[ServerID] = @ServerID
				ORDER BY mh.[MonitorTime] DESC
			) t
		WHERE 
			(t.HttpResponseCode < 200) OR (t.HttpResponseCode >= 300) OR (t.HttpResponseLatency >= 60)

		IF @@RecordsCount = 2	SET @@MonitoringStatus = 3;
	END
	
	RETURN @@MonitoringStatus;
END
GO
/****** Object:  Table [dbo].[tblMonitorHistory]    Script Date: 8/30/2024 9:51:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMonitorHistory](
	[ServerID] [int] NOT NULL,
	[MonitorTime] [smalldatetime] NOT NULL,
	[HttpResponseCode] [int] NOT NULL,
	[HttpResponseLatency] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblServerHealth]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblServerHealth](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_tblHealth] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblWebServers]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblWebServers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
	[HttpURL] [nvarchar](250) NOT NULL,
	[HealthID] [int] NOT NULL,
 CONSTRAINT [PK_tblWebServers_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblMonitorHistory]    Script Date: 8/30/2024 9:51:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_tblMonitorHistory] ON [dbo].[tblMonitorHistory]
(
	[ServerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblWebServers] ADD  CONSTRAINT [DF_tblWebServers_HealthID]  DEFAULT ((1)) FOR [HealthID]
GO
/****** Object:  StoredProcedure [dbo].[spDeleteServer]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spDeleteServer] 
	@ID int,
	@Name nvarchar(25),
	@HttpURL nvarchar(250)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE FROM [dbo].[tblWebServers]
	WHERE 
		([ID] = @ID) OR
		([Name] = @Name) OR
		([HttpURL] = @HttpURL) 
		   
	SELECT @@ROWCOUNT;
	
END
GO
/****** Object:  StoredProcedure [dbo].[spGetServer]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetServer] 
	@ServerID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * 
	FROM tblWebServers
	WHERE ID = @ServerID;

	SELECT TOP 10 * 
	FROM tblMonitorHistory
	WHERE ServerID = @ServerID
	ORDER BY [MonitorTime] DESC;

END
GO
/****** Object:  StoredProcedure [dbo].[spGetServerHistory]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetServerHistory]
	@ServerID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT *
	FROM tblMonitorHistory
	WHERE ServerID = @ServerID;

END
GO
/****** Object:  StoredProcedure [dbo].[spGetServerInfo]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetServerInfo]
	@ServerID as int

AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;

--	Declare @@aaa As int; 

--	SELECT @@aaa = COUNT(*)
--	FROM 
--		(
--			SELECT TOP 5 mh.[ServerID], mh.[HttpResponseCode], mh.[HttpResponseLatency]
--			FROM tblMonitorHistory mh
--			WHERE mh.[ServerID] = @ServerID
--			ORDER BY mh.[MonitorTime] DESC
--		) t
--	WHERE (t.HttpResponseCode >= 200 AND t.HttpResponseCode < 300)

    
--	/*SELECT ws2.*
--	FROM tblWebServers ws2
--	WHERE ws2.[Name] = @Name */

--END
GO
/****** Object:  StoredProcedure [dbo].[spInsertMonitorHistory]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spInsertMonitorHistory]
	@ServerID int,
	@MonitorTime smalldatetime,
	@HttpResponseCode int,
	@HttpResponseLatency int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [dbo].[tblMonitorHistory]
           ([ServerID]
           ,[MonitorTime]
           ,[HttpResponseCode]
           ,[HttpResponseLatency])
     VALUES
           (@ServerID
           ,@MonitorTime
           ,@HttpResponseCode
           ,@HttpResponseLatency)


	UPDATE [dbo].[tblWebServers]
    SET [HealthID] = [dbo].[fnGetServerMonitoringStatus](@ServerID, @HttpResponseCode, @HttpResponseLatency)
	WHERE ID = @ServerID;

END
GO
/****** Object:  StoredProcedure [dbo].[spInsertServer]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spInsertServer] 
	@Name nvarchar(25),
	@HttpURL nvarchar(250)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [dbo].[tblWebServers]
           ([Name], [HttpURL], [HealthID])
     VALUES
           (@Name, @HttpURL, 0)
		   
	SELECT @@IDENTITY;
	
END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateServer]    Script Date: 8/30/2024 9:51:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateServer] 
	@ID int,
	@Name nvarchar(25),
	@HttpURL nvarchar(250)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE [dbo].[tblWebServers]
	SET [Name] = @Name
		,[HttpURL] = @HttpURL
	WHERE ID = @ID
		   
	SELECT @@ROWCOUNT;
	
END
GO
USE [master]
GO
ALTER DATABASE [WebServers] SET  READ_WRITE 
GO
