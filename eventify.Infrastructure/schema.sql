CREATE TABLE [Clubs] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [MapsLink] nvarchar(max) NOT NULL,
    [IsVerified] bit NOT NULL,
    CONSTRAINT [PK_Clubs] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Members] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Members] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Events] (
    [Id] int NOT NULL IDENTITY,
    [Title_Value] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [DateRange_Start] datetime2 NOT NULL,
    [DateRange_End] datetime2 NOT NULL,
    [ClubId] int NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [MemberId] int NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Events_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Events_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id])
);
GO


CREATE TABLE [NewsFeedItems] (
    [Id] int NOT NULL IDENTITY,
    [MemberId] int NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    [RelatedEntityId] int NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    CONSTRAINT [PK_NewsFeedItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NewsFeedItems_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [BookingInvitations] (
    [Id] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [MemberId] int NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_BookingInvitations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BookingInvitations_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BookingInvitations_Members_MemberId] FOREIGN KEY ([MemberId]) REFERENCES [Members] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [TimeTableSlots] (
    [Id] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [ArtistAccountId] int NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [EndTime] datetime2 NOT NULL,
    CONSTRAINT [PK_TimeTableSlots] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TimeTableSlots_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [RecordedPerformances] (
    [Id] int NOT NULL IDENTITY,
    [EventId] int NOT NULL,
    [TimeTableSlotId] int NOT NULL,
    [RecordingLink] nvarchar(max) NOT NULL,
    [Timestamp] datetime2 NOT NULL,
    [Type] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RecordedPerformances] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RecordedPerformances_TimeTableSlots_TimeTableSlotId] FOREIGN KEY ([TimeTableSlotId]) REFERENCES [TimeTableSlots] ([Id]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_BookingInvitations_EventId] ON [BookingInvitations] ([EventId]);
GO


CREATE INDEX [IX_BookingInvitations_MemberId] ON [BookingInvitations] ([MemberId]);
GO


CREATE INDEX [IX_Events_ClubId] ON [Events] ([ClubId]);
GO


CREATE INDEX [IX_Events_MemberId] ON [Events] ([MemberId]);
GO


CREATE INDEX [IX_NewsFeedItems_MemberId] ON [NewsFeedItems] ([MemberId]);
GO


CREATE UNIQUE INDEX [IX_RecordedPerformances_TimeTableSlotId] ON [RecordedPerformances] ([TimeTableSlotId]);
GO


CREATE INDEX [IX_TimeTableSlots_EventId] ON [TimeTableSlots] ([EventId]);
GO


