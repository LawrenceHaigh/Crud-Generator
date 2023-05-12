using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Enums
{
    public enum DataTypes
    {
        //Int
        bigint, //Int64
        @int, //Int32       
        tinyint, //Int16
        smallint, //Int16

        //Float
        @float,

        //Decimal(28-29 digits) OR Double(~15-17 digits) depending on float point
        @decimal,        
        numeric,
        money, //Decimal only 
        smallmoney,//Decimal only

        //Char if length = 1 else string
        @char,
        nchar,

        //String
        ntext,
        nvarchar,
        varchar,
        text,

        //DateTime
        date,
        datetime,
        datetime2,
        smalldatetime,

        //TimeSpan
        datetimeoffset,
        time,
        timestamp,

        //Bool
        bit,

        //Guid
        uniqueidentifier,

        //Other
        //TODO: Will come back to these
        binary,
        geography,
        geometry,
        hierarchyid,
        image,
        real,
        sql_variant,
        varbinary,        
        xml
    }
}
