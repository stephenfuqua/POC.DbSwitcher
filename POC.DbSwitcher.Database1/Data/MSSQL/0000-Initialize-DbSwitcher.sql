MERGE [edfi].[DbSwitcher] as target
USING (
	VALUES
		('One', '2019-03-18 9:56:01', '27E95CA5-E27F-470E-A4E6-0FE5819D2781', 0),
		('Two', '2019-03-18 9:56:02', '37E95CA5-E27F-470E-A4E6-0FE5819D2782', 1)
) as source ([Summary], [CreatedDate], [UniqueId], [IsTrue])
ON (target.[Summary] = source.[Summary] AND target.[CreatedDate] = source.[CreatedDate])
WHEN MATCHED THEN 
	UPDATE SET [UniqueId] = source.[UniqueId], [IsTrue] = source.[IsTrue]
WHEN NOT MATCHED THEN
	INSERT ([Summary], [CreatedDate], [UniqueId], [IsTrue])
	VALUES (source.[Summary], source.[CreatedDate], source.[UniqueId], source.[IsTrue]);
GO