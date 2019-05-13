using System;
using System.Collections.Generic;

namespace POC.DbSwitcher.Console.Query
{
    public interface IQuery
    {
        IEnumerable<Models.DbSwitcher> Get(Guid uniqeId);
    }
}
