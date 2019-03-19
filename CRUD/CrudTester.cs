using POC.DbSwitcher.CRUD.EntityFramework;
using System;
using System.Linq;
using POC.DbSwitcher.CRUD.NHibernate;

namespace POC.DbSwitcher.CRUD
{
    public class CrudTester
    {

        private readonly PocDbSwitcherContext _dbContext;
        private readonly PocDbSwitcherRepository<Models.DbSwitcher> _nhibernateRepository;

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

            _nhibernateRepository = new PocDbSwitcherRepository<Models.DbSwitcher>(databaseType, connectionString);
        }

        public void RunEntityFrameworkTests()
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
                _dbContext.SaveChanges();
            }

            void DeleteEntry(Models.DbSwitcher entity)
            {
                Console.WriteLine("DeleteEntry...");

                _dbContext.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public void RunNHibernateTests()
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
                foreach (var dbSwitcher in _nhibernateRepository.FindAll())
                {
                    Console.WriteLine(dbSwitcher.ToString());
                }
            }

            Models.DbSwitcher CreateNewEntry()
            {
                Console.WriteLine("CreateNewEntry...");

                var item = _nhibernateRepository.Create(new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "New Entry",
                    UniqueId = Guid.NewGuid()
                });

                return item;
            }

            void UpdateEntry(Models.DbSwitcher entity)
            {
                Console.WriteLine("UpdateEntry...");

                entity.IsTrue = false;
                entity.IsTrue = true;
                _nhibernateRepository.Update(entity);
            }

            void DeleteEntry(Models.DbSwitcher entity)
            {
                Console.WriteLine("DeleteEntry...");

                _nhibernateRepository.Delete(entity);
            }
        }
    }
}
