using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace POC.DbSwitcher.Console.CRUD.NHibernate
{
    public class DependentTableMap : ClassMapping<Models.DependentTable>
    {
        public DependentTableMap()
        {
            Schema(Models.DependentTable.Constants.SchemaName);
            Table(Models.DependentTable.Constants.TableName);
            Id(x => x.Id, m => m.Generator(Generators.Native));
            Property(x => x.CreatedDate);
            Property(x => x.DbSwitcherId);
        }
    }
}
