using POC.DbSwitcher.CRUD.EntityFramework;
using System;
using System.Linq;

namespace POC.DbSwitcher.CRUD
{
    public class CrudTester
    {

        private readonly PocDbSwitcherContext _dbContext;

        public CrudTester(DatabaseType databaseType, string connectionString)
        {
            switch (databaseType)
            {
                case DatabaseType.Postgres:
                    _dbContext = new PgSqlContext(connectionString);
                    break;
                case DatabaseType.SqlServer:
                    _dbContext = new MsSqlContext(connectionString);
                    break;
                default:
                    throw new InvalidOperationException($"Database type {databaseType} is not supported.");
            }
            
        }

        public void RunTests()
        {
            DisplayAllValues();
            var entry = CreateNewEntry();
            DisplayAllValues();
            UpdateEntry(entry);
            DisplayAllValues();
            DeleteEntry(entry);
            DisplayAllValues();

            void DisplayAllValues()
            {
                foreach (var dbSwitcher in _dbContext.DbSwitchers.Where(x => x.Id > -1).ToList())
                {
                    Console.WriteLine(dbSwitcher.ToString());
                }
            }

            Models.DbSwitcher CreateNewEntry()
            {
                Console.WriteLine("CreateNewEntry...");

                var item = _dbContext.DbSwitchers.Add(new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "New Entry",
                    UniqueId = Guid.NewGuid()
                }).Entity;

                _dbContext.SaveChanges();

                return item;
            }

            void UpdateEntry(Models.DbSwitcher entity)
            {
                Console.WriteLine("UpdateEntry...");

                entity.IsTrue = false;
                entity.IsTrue2 = true;
                _dbContext.SaveChanges();
            }

            void DeleteEntry(Models.DbSwitcher entity)
            {
                Console.WriteLine("DeleteEntry...");

                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
            }
        }
    }
}
