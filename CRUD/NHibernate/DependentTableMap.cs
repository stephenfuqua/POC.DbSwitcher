using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace POC.DbSwitcher.CRUD.NHibernate
{
    public class DependentTableMap : ClassMapping<Models.DependentTable>
    {
        public DependentTableMap()
        {
            Schema("edfi");
            Table("DependentTable");
            Id(x => x.Id, m => m.Generator(Generators.Native));
            Property(x => x.CreatedDate);
            Property(x => x.DbSwitcherId);
        }
    }
}
