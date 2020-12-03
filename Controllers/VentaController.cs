using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCOREM3_DatabaseFirst.DataAccess;
using NETCOREM3_DatabaseFirst.Models;
using Newtonsoft.Json;

namespace NETCOREM3_DatabaseFirst.Controllers
{
    public class VentaController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ListadoCliente = DACustomer.Listado();
            ViewBag.ListadoProducto = DAProduct.Listado();
            return View();
        }

        public IActionResult ListadoProducto()
        {
            List<OrderItem> listado;
            var productos = HttpContext.Session.GetString("listaProducto");
            if (productos == null)
                listado = new List<OrderItem>();
            else
                listado = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            ViewBag.ProductosAgregados = listado;
            ViewBag.ListadoProducto = DAProduct.Listado();

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarOrden(int customerID
            , DateTime orderDate, string orderNumber)
        {
            Order cabecera = new Order();
            cabecera.CustomerId = customerID;
            cabecera.OrderDate = orderDate;
            cabecera.OrderNumber = orderNumber;

            List<OrderItem> detalle = new List<OrderItem>();
            var productos = HttpContext.Session.GetString("listaProducto");
            detalle = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            bool exito = await DAOrder.Insertar(cabecera, detalle);
            return Json(exito);
        }

        [HttpPost]
        public IActionResult AgregarProducto(int productID,
                                              decimal unitPrice,
                                              int quantity)
        {
            List<OrderItem> listado;
            var productos = HttpContext.Session.GetString("listaProducto");
            if (productos == null)
                listado = new List<OrderItem>();
            else
                listado = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            if (listado.Where(x => x.ProductId == productID).Count() > 0)
                return Json("DUP");

            OrderItem detalle = new OrderItem();
            detalle.ProductId = productID;
            detalle.Quantity = quantity;
            detalle.UnitPrice = unitPrice;

            listado.Add(detalle);

            HttpContext.Session.SetString("listaProducto", JsonConvert.SerializeObject(listado));

            return Json("OK");


        }



    }
}
