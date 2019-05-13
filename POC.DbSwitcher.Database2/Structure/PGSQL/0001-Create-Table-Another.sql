CREATE TABLE edfi."Another" (
	"Id" SERIAL NOT NULL,
	"Field" CHAR(1) NOT NULL,
	"CreatedDate" TIMESTAMP NOT NULL DEFAULT current_timestamp,
	CONSTRAINT PK_Another PRIMARY KEY ("Id")
);