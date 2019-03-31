using POC.DbSwitcher.CRUD.EntityFramework;
using POC.DbSwitcher.CRUD.NHibernate;
using System;
using System.Linq;
using System.Transactions;

namespace POC.DbSwitcher.CRUD
{
    public class CrudTester
    {

        private readonly PocDbSwitcherContext _dbContext;
        private readonly PocDbSwitcherRepository _nhibernateRepository;

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

            _nhibernateRepository = new PocDbSwitcherRepository(databaseType, connectionString);
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
            AttemptToEnterDuplicateKey();
            MissingForeignKey();
            DeleteForeignKeyReference();
            UpdateFailsDueToMissingForeignKey();

            void DeleteForeignKeyReference()
            {
                Console.WriteLine("Attempt to delete parent record in foreign key relationship...");

                try
                {
                    using (var _ = new TransactionScope())
                    {
                        var parent = new Models.DbSwitcher
                        {
                            CreatedDate = DateTime.Now,
                            IsTrue = true,
                            Summary = "First",
                            UniqueId = Guid.NewGuid()
                        };
                        _dbContext.DbSwitchers.Add(parent);
                        _dbContext.SaveChanges();

                        var child = new Models.DependentTable
                        {
                            CreatedDate = DateTime.Now,
                            DbSwitcherId = parent.Id
                        };

                        _dbContext.DependentTables.Add(child);
                        _dbContext.SaveChanges();

                        _dbContext.Remove(parent);
                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void MissingForeignKey()
            {
                Console.WriteLine("Attempt to create dependent record when foreign key reference does not exist...");
                try
                {
                    var child = new Models.DependentTable
                    {
                        CreatedDate = DateTime.Now,
                        DbSwitcherId = 1234567
                    };
                    _dbContext.DependentTables.Add(child);
                    _dbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void UpdateFailsDueToMissingForeignKey()
            {
                Console.WriteLine("Attempt to change a foreign key to reference record that does not exist...");

                try
                {
                    using (var _ = new TransactionScope())
                    {
                        var parent = new Models.DbSwitcher
                        {
                            CreatedDate = DateTime.Now,
                            IsTrue = true,
                            Summary = "First",
                            UniqueId = Guid.NewGuid()
                        };
                        _dbContext.DbSwitchers.Add(parent);
                        _dbContext.SaveChanges();

                        var child = new Models.DependentTable
                        {
                            CreatedDate = DateTime.Now,
                            DbSwitcherId = parent.Id
                        };

                        _dbContext.DependentTables.Add(child);
                        _dbContext.SaveChanges();

                        child.DbSwitcherId = 123124634;
                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void AttemptToEnterDuplicateKey()
            {
                Console.WriteLine("Attempting Duplicate Key Test...");

                var first = new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "First",
                    UniqueId = Guid.NewGuid()
                };
                _dbContext.DbSwitchers.Add(first);

                var second = new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "Second",
                    UniqueId = first.UniqueId
                };
                _dbContext.DbSwitchers.Add(second);

                try
                {
                    using (var _ = new TransactionScope())
                    {
                        _dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

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
        private void WriteExceptionMessage(Exception ex)
        {
            Console.Error.WriteLine($"Exception type is {ex.GetType()}");
            Console.Error.WriteLine(ex.Message);

            if (ex.InnerException != null)
            {
                Console.Error.WriteLine("---- Inner exception ----");
                WriteExceptionMessage(ex.InnerException);
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
            AttemptToEnterDuplicateKey();
            MissingForeignKey();
            DeleteForeignKeyReference();
            UpdateFailsDueToMissingForeignKey();

            void DeleteForeignKeyReference()
            {
                Console.WriteLine("Attempt to delete parent record in foreign key relationship...");

                try
                {
                    _nhibernateRepository.RunMultipeStepsInsideTransaction((session, transaction) =>
                    {
                        var parent = new Models.DbSwitcher
                        {
                            CreatedDate = DateTime.Now,
                            IsTrue = true,
                            Summary = "First",
                            UniqueId = Guid.NewGuid()
                        };

                        parent.Id = (int)session.Save(parent);
                        session.Flush();

                        var child = new Models.DependentTable
                        {
                            CreatedDate = DateTime.Now,
                            DbSwitcherId = parent.Id
                        };
                        child.Id = (int)session.Save(child);
                        session.Flush();

                        session.Delete(parent);
                        session.Flush();
                    });

                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void MissingForeignKey()
            {
                Console.WriteLine("Attempt to create dependent record when foreign key reference does not exist...");
                try
                {
                    var child = new Models.DependentTable
                    {
                        CreatedDate = DateTime.Now,
                        DbSwitcherId = 1234567
                    };
                    _nhibernateRepository.Create(child);

                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void UpdateFailsDueToMissingForeignKey()
            {
                Console.WriteLine("Attempt to change a foreign key to reference record that does not exist...");

                try
                {

                    _nhibernateRepository.RunMultipeStepsInsideTransaction((session, transaction) =>
                    {
                        var parent = new Models.DbSwitcher
                        {
                            CreatedDate = DateTime.Now,
                            IsTrue = true,
                            Summary = "First",
                            UniqueId = Guid.NewGuid()
                        };

                        parent.Id = (int)session.Save(parent);
                        session.Flush();

                        var child = new Models.DependentTable
                        {
                            CreatedDate = DateTime.Now,
                            DbSwitcherId = parent.Id
                        };
                        child.Id = (int)session.Save(child);
                        session.Flush();

                        child.DbSwitcherId = 123234235;
                        session.Update(child);
                        session.Flush();
                    });

                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }

            void AttemptToEnterDuplicateKey()
            {
                Console.WriteLine("Attempting Duplicate Key Test...");

                var first = new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "First",
                    UniqueId = Guid.NewGuid()
                };

                var second = new Models.DbSwitcher
                {
                    CreatedDate = DateTime.Now,
                    IsTrue = true,
                    Summary = "Second",
                    UniqueId = first.UniqueId
                };
                _dbContext.DbSwitchers.Add(second);

                try
                {
                    _nhibernateRepository.Create<Models.DbSwitcher>(new Models.DbSwitcher[] { first, second });
                }
                catch (Exception ex)
                {
                    WriteExceptionMessage(ex);
                }
            }
            void DisplayAllValues()
            {
                foreach (var dbSwitcher in _nhibernateRepository.FindAll<Models.DbSwitcher>())
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
