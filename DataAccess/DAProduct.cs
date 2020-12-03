using NETCOREM3_DatabaseFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst.DataAccess
{
    public class DAProduct
    {
        public static IEnumerable<Product> Listado()
        {
            using (var data = new SalesContext())
            {
                return data.Products.ToList();
            }
        }
    }
}
