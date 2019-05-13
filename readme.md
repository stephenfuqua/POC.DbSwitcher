# POC.DbSwitcher

A simple experiment around using targeting multiple database technologies with
the same application, namely: Microsoft SQL Server and PostgreSQL. Database
object creation is managed with the help of DbUp. Then basic CRUD tests are
performed using both Entity Framework Core and NHibernate 5. Through config
settings, either one or the other database server is targeted - that is, at
runtime, the target is either SQL Server or Postgres, not both.

Annoyance: due to differences in case sensitivity between not only the database
server software, but also the ORM tools, and the desire to maintain PascalCase
class and property names with a minimum of mapping, all names in PostgreSQL are
being created with quotations - thus forcing that they always be addressed with
quotations.

No attempt has been made to optimize the ORMs in this proof-of-concept experiment.

Sample connection strings:

* SQL Server: `server=localhost;Database=poc-dbswitcher;Username=yourUser;Password=yourPassword`
* PostgreSQL: `Host=localhost;Database=poc-dbswitcher;Username=yourUser;Password=yourPassword`