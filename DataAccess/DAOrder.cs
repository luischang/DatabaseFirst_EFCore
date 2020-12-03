using NETCOREM3_DatabaseFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst.DataAccess
{
    public class DAOrder
    {
        public static IEnumerable<Order> Listado()
        {
            using (var data = new SalesContext())
            {
                return data.Orders.ToList();
            }        
        }

        public static async Task<bool> Insertar(Order cabecera, IEnumerable<OrderItem> detalle)
        {
            bool exito = true;
            try
            {
                using (var data = new SalesContext())
                {
                    await data.Orders.AddAsync(cabecera);//Id no existe
                    await data.SaveChangesAsync(); //Si existe el Id

                    int newOrderID = cabecera.Id;
                    decimal totalAmount=0;
                    foreach (var item in detalle)
                    {
                        item.OrderId = newOrderID;
                        totalAmount = totalAmount + (item.UnitPrice * item.Quantity);
                    }
                    await data.OrderItems.AddRangeAsync(detalle);
                    cabecera.TotalAmount = totalAmount;
                    await data.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                exito = false;
            }
            return exito;        
        }

    }
}
