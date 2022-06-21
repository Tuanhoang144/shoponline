using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAdmin.Models.PerMisstion
{
    public enum PermisstionType :int
    {
        Admin=1,
        Order=2,
        Product=3,
        User=4,
        SenDo=5,
        Voucher=6,
        New=7,
        Statistical=8,
        Rely = 9,
    }
}