using System;

namespace POC.DbSwitcher.Query
{
    public class SharedConstants
    {
        public const string UniqueIdParameterName = "UniqueId";
        public const string Query = "SELECT \"Id\", \"Summary\", \"CreatedDate\", \"UniqueId\", \"IsTrue\" FROM edfi.\"DbSwitcher\" WHERE \"UniqueId\" = @UniqueId;";
        public static Guid UniqueIdOne = new Guid("37E95CA5-E27F-470E-A4E6-0FE5819D2782");       
    }
}
