create function edfi."Split"(string varchar, delimiter varchar(1)) 
returns table ("Id" int, "Token" varchar) as $$
	
	select
		cast(row_number() over (order by null) as int) as "Id",
		sub.* as "Token"
	from (
		select unnest(string_to_array(string, delimiter))
	) sub;

$$ language sql;