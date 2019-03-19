using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Driver;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using POC.DbSwitcher.CRUD.Models;


namespace POC.DbSwitcher.CRUD.NHibernate
{
    public class PocDbSwitcherRepository<T>
        where T: class, IHaveIdProperty
    {
        private readonly ISessionFactory _sessionFactory;

        public PocDbSwitcherRepository(DatabaseType databaseType, string connectionString)
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(x =>
            {
                x.ConnectionString = connectionString;

                // There is undoubtedly a better way to handle this. But it is 
                // good enough for the POC
                switch (databaseType)
                {
                    case DatabaseType.Postgres:
                        x.Driver<NpgsqlDriver>();
                        x.Dialect<PostgreSQL83Dialect>();
                        cfg.SetNamingStrategy(new QuotedNamingStrategy());
                        //x.LogFormattedSql = true;
                        x.LogSqlInConsole = true;
                        break;
                    case DatabaseType.SqlServer:
                        x.Driver<SqlClientDriver>();
                        x.Dialect<MsSql2012Dialect>();
                        break;
                    default:
                        throw new InvalidOperationException($"Database type {databaseType} not supported.");
                }
            });
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            cfg.AddDeserializedMapping(CreateMapping(), null);

            _sessionFactory = cfg.BuildSessionFactory();
        }

        public IEnumerable<T> FindAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<T>().ToList();
            }
        }

        public T Create(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                entity.Id = (int)session.Save(entity);
                transaction.Commit();

                return entity;
            }
        }

        public void Update(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Update(entity);
                transaction.Commit();
            }
        }

        public void Delete(T entity)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(entity);
                transaction.Commit();
            }
        }

        private static HbmMapping CreateMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(new List<Type> { typeof(DbSwitcherMap) });
            
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }

}
