
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/28/2012 21:50:47
-- Generated from EDMX file: C:\Users\leif\Documents\Visual Studio 2010\Projects\SBBArkiv\SBBArkiv\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SBBArchive];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MusicPartSheetMusic_MusicPart]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SheetMusicParts] DROP CONSTRAINT [FK_MusicPartSheetMusic_MusicPart];
GO
IF OBJECT_ID(N'[dbo].[FK_SheetMusic_MusicPartSheetMusic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SheetMusicParts] DROP CONSTRAINT [FK_SheetMusic_MusicPartSheetMusic];
GO
IF OBJECT_ID(N'[dbo].[FK_UserStemme_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserStemme] DROP CONSTRAINT [FK_UserStemme_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserStemme_Stemme]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserStemme] DROP CONSTRAINT [FK_UserStemme_Stemme];
GO
IF OBJECT_ID(N'[dbo].[FK_SheetMusicCategorySheetMusic]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SheetMusic] DROP CONSTRAINT [FK_SheetMusicCategorySheetMusic];
GO
IF OBJECT_ID(N'[dbo].[FK_UserGroupUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserGroupUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SheetMusic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SheetMusic];
GO
IF OBJECT_ID(N'[dbo].[MusicParts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MusicParts];
GO
IF OBJECT_ID(N'[dbo].[SheetMusicParts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SheetMusicParts];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[SheetMusicCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SheetMusicCategories];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[UserStemme]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserStemme];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SheetMusic'
CREATE TABLE [dbo].[SheetMusic] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Composer] nvarchar(max)  NULL,
    [Arranger] nvarchar(max)  NULL,
    [SheetMusicCategoryId] int  NULL,
    [SoleSellingAgent] nvarchar(max)  NULL,
    [MissingParts] nvarchar(max)  NULL,
    [HasBeenScanned] bit  NOT NULL,
    [ArchiveFileName] nvarchar(max)  NULL
);
GO

-- Creating table 'MusicParts'
CREATE TABLE [dbo].[MusicParts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PartName] nvarchar(max)  NOT NULL,
    [Aliases] nvarchar(max)  NULL
);
GO

-- Creating table 'SheetMusicParts'
CREATE TABLE [dbo].[SheetMusicParts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MusicPartId] int  NOT NULL,
    [SheetMusicId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NULL,
    [Inactive] bit  NOT NULL,
    [UserGroupId] int  NOT NULL
);
GO

-- Creating table 'SheetMusicCategories'
CREATE TABLE [dbo].[SheetMusicCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Inactive] bit  NOT NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserMusicPart'
CREATE TABLE [dbo].[UserMusicPart] (
    [Users_Id] int  NOT NULL,
    [MusicParts_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'SheetMusic'
ALTER TABLE [dbo].[SheetMusic]
ADD CONSTRAINT [PK_SheetMusic]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MusicParts'
ALTER TABLE [dbo].[MusicParts]
ADD CONSTRAINT [PK_MusicParts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SheetMusicParts'
ALTER TABLE [dbo].[SheetMusicParts]
ADD CONSTRAINT [PK_SheetMusicParts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SheetMusicCategories'
ALTER TABLE [dbo].[SheetMusicCategories]
ADD CONSTRAINT [PK_SheetMusicCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Users_Id], [MusicParts_Id] in table 'UserMusicPart'
ALTER TABLE [dbo].[UserMusicPart]
ADD CONSTRAINT [PK_UserMusicPart]
    PRIMARY KEY NONCLUSTERED ([Users_Id], [MusicParts_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MusicPartId] in table 'SheetMusicParts'
ALTER TABLE [dbo].[SheetMusicParts]
ADD CONSTRAINT [FK_MusicPartSheetMusic_MusicPart]
    FOREIGN KEY ([MusicPartId])
    REFERENCES [dbo].[MusicParts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MusicPartSheetMusic_MusicPart'
CREATE INDEX [IX_FK_MusicPartSheetMusic_MusicPart]
ON [dbo].[SheetMusicParts]
    ([MusicPartId]);
GO

-- Creating foreign key on [SheetMusicId] in table 'SheetMusicParts'
ALTER TABLE [dbo].[SheetMusicParts]
ADD CONSTRAINT [FK_SheetMusic_MusicPartSheetMusic]
    FOREIGN KEY ([SheetMusicId])
    REFERENCES [dbo].[SheetMusic]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SheetMusic_MusicPartSheetMusic'
CREATE INDEX [IX_FK_SheetMusic_MusicPartSheetMusic]
ON [dbo].[SheetMusicParts]
    ([SheetMusicId]);
GO

-- Creating foreign key on [Users_Id] in table 'UserMusicPart'
ALTER TABLE [dbo].[UserMusicPart]
ADD CONSTRAINT [FK_UserMusicPart_User]
    FOREIGN KEY ([Users_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MusicParts_Id] in table 'UserMusicPart'
ALTER TABLE [dbo].[UserMusicPart]
ADD CONSTRAINT [FK_UserMusicPart_MusicPart]
    FOREIGN KEY ([MusicParts_Id])
    REFERENCES [dbo].[MusicParts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMusicPart_MusicPart'
CREATE INDEX [IX_FK_UserMusicPart_MusicPart]
ON [dbo].[UserMusicPart]
    ([MusicParts_Id]);
GO

-- Creating foreign key on [SheetMusicCategoryId] in table 'SheetMusic'
ALTER TABLE [dbo].[SheetMusic]
ADD CONSTRAINT [FK_SheetMusicCategorySheetMusic]
    FOREIGN KEY ([SheetMusicCategoryId])
    REFERENCES [dbo].[SheetMusicCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SheetMusicCategorySheetMusic'
CREATE INDEX [IX_FK_SheetMusicCategorySheetMusic]
ON [dbo].[SheetMusic]
    ([SheetMusicCategoryId]);
GO

-- Creating foreign key on [UserGroupId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserGroupUser]
    FOREIGN KEY ([UserGroupId])
    REFERENCES [dbo].[UserGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGroupUser'
CREATE INDEX [IX_FK_UserGroupUser]
ON [dbo].[Users]
    ([UserGroupId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------