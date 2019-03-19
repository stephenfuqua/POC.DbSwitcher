using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace POC.DbSwitcher.CRUD.NHibernate
{

    public class DbSwitcherMap : ClassMapping<Models.DbSwitcher>
    {
        public DbSwitcherMap()
        {
            Schema("edfi");
            Table("DbSwitcher");
            Id(x => x.Id, m => m.Generator(Generators.Native));
            Property(x => x.CreatedDate);
            Property(x => x.IsTrue);
            Property(x => x.Summary);
            Property(x => x.UniqueId);
        }
    }
}
