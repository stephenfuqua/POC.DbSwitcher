with src as (
	select * 
	from  (values ('One', '2019-03-18 9:56:01', '27E95CA5-E27F-470E-A4E6-0FE5819D2781', false),
			('Two', '2019-03-18 9:56:02', '37E95CA5-E27F-470E-A4E6-0FE5819D2782', true)
	) as v ("Summary", "CreatedDate", "UniqueId", "IsTrue")
), upd as (
	update edfi."DbSwitcher"
	set "UniqueId" = src."UniqueId"::uuid,
		"IsTrue" = src."IsTrue"
	from src
	where "DbSwitcher"."Summary" = src."Summary"
	and "DbSwitcher"."CreatedDate" = src."CreatedDate"::timestamp
	returning "DbSwitcher"."Id"
)
insert into edfi."DbSwitcher" ("Summary", "CreatedDate", "UniqueId", "IsTrue")
select "Summary", "CreatedDate"::timestamp, "UniqueId"::uuid, "IsTrue"
from src
where not exists (
	select 1 
	from edfi."DbSwitcher" 
	where "Summary" = src."Summary"
	and "CreatedDate" = src."CreatedDate"::timestamp
);



