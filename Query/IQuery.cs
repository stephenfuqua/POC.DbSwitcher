using System;
using System.Collections.Generic;

namespace POC.DbSwitcher.Query
{
    public interface IQuery
    {
        IEnumerable<Models.DbSwitcher> Get(Guid uniqeId);
    }
}
