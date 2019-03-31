CREATE TABLE edfi."DependentTable" (
	"Id" SERIAL NOT NULL,
	"DbSwitcherId" INT NOT NULL,
	"CreatedDate" TIMESTAMP NOT NULL,
	CONSTRAINT "PK_DependentTable" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_DbSwitcher_Id" FOREIGN KEY ("DbSwitcherId") REFERENCES edfi."DbSwitcher" ("Id")
);