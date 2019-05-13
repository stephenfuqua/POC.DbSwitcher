CREATE TABLE edfi."Another" (
	"Id" SERIAL NOT NULL,
	"Field" CHAR(1) NOT NULL,
	"CreatedDate" DATETIME NOT NULL DEFAULT getdate(),
	CONSTRAINT PK_Another PRIMARY KEY ("Id")
);