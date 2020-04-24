using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Factura
{
    public class CatalogoStock
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        List<Stock> ListaStock;
        public bool IngresarStock(Stock Stock)
        {
            try
            {
                ConexionBD.sp_CrearStock(Stock.Cantidad, int.Parse(Stock.IdAsignarProductoLote));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CargarStock()
        {
            ListaStock = new List<Stock>();
            var ListaAsignarProductoLote = GestionAsignarProductoLote.ListarAsignarProductoLote();
            foreach (var item in ConexionBD.sp_ConsultarStock())
            {
                ListaStock.Add(new Stock()
                {
                    IdStock = Seguridad.Encriptar(item.IdStock.ToString()),
                    Cantidad = item.Cantidad,
                    IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                    FechaActualizacion = item.FechaActualizacion,
                    AsignarProductoLote = ListaAsignarProductoLote.Where(p => Seguridad.DesEncriptar(p.IdAsignarProductoLote) == item.IdAsignarProductoLote.ToString()).FirstOrDefault(),
                    //AsignarProductoLote = GestionAsignarProductoLote.ConsultarDatosAsignarProductoLote(item.IdAsignarProductoLote.ToString()),
                });
            }
        }
        public List<Stock> ListarStock()
        {
            CargarStock();
            List<Stock> ListaUnida = new List<Stock>();
            List<Stock> Lista1 = new List<Stock>();
            List<Stock> Lista2 = new List<Stock>();
            Lista1 = ListaStock.Where(p =>  p.AsignarProductoLote.IdLote!= "" && p.AsignarProductoLote.Lote.Estado == true).ToList();
            Lista2 = ListaStock.Where(p => p.Cantidad > 0 == true && p.AsignarProductoLote.IdLote=="").ToList();
            ListaUnida = Lista1.Union(Lista2).ToList();
            return ListaUnida;
            //return ListaStock.Where(p=> p.AsignarProductoLote.Lote.Estado == true || p.Cantidad >=0).ToList();
        }
    }
}
