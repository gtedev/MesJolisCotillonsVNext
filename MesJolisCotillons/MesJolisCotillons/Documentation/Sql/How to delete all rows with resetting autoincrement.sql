begin transaction 

	delete dbo.Command;
	DBCC CHECKIDENT (Command, RESEED, 0)

--commit; 
rollback;

