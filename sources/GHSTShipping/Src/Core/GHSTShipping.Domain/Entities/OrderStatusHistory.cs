using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GHSTShipping.Domain.Entities
{
    public class OrderStatusHistory
    {
        /*Id UNIQUEIDENTIFIER PRIMARY KEY,
        OrderId UNIQUEIDENTIFIER NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        ChangeDate DATETIME NOT NULL,
        ChangedBy UNIQUEIDENTIFIER, -- This can store the user who changed the status
        Notes NVARCHAR(MAX), -- Optional notes for the status change
        FOREIGN KEY(OrderId) REFERENCES Orders(Id) -- Assuming Orders is your main order table*/
    }
}
