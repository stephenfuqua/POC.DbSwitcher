-- Using quotation marks is annoying here. Without themm, PG will create edfi.dbswitcher instead,
-- and Entity Framework will fail when it tries to query edfi.DbSwitcher. Another option
-- would be to hardcode the lowercase version of the table name in the DbContext builder
-- only for postgres, and then remove the quotation marks here. Or consider a customization
-- like https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/21#issuecomment-376114368

CREATE TABLE edfi."DbSwitcher" (
	"Id" SERIAL NOT NULL,
	"Summary" VARCHAR(50),
	"CreatedDate" TIMESTAMP NOT NULL,
	"UniqueId" UUID NOT NULL,
	"IsTrue" SMALLINT NOT NULL DEFAULT ((0)),
	CONSTRAINT PK_DbSwitcher PRIMARY KEY ("Id")
);