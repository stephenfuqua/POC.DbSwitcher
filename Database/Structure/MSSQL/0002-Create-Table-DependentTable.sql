CREATE TABLE [edfi].[DependentTable] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[DbSwitcherId] INT NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	CONSTRAINT [PK_DependentTable] PRIMARY KEY CLUSTERED ([Id]),
	CONSTRAINT [FK_DbSwitcher_Id] FOREIGN KEY ([DbSwitcherId]) REFERENCES [edfi].[DbSwitcher] ([Id])
) ON [PRIMARY]
GO