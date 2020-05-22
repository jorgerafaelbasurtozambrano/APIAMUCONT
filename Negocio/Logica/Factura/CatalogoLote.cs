using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Factura
{
    public class CatalogoLote
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        //CatalogoAsignarProductoLote GestionAsignarProductoLote = new CatalogoAsignarProductoLote();
        List<Lote> ListaLote;
        public object IngresarLote(Lote Lote)
        {
            object objeto = new object();
            try
            {
                Lote LoteData = CargarTodosLosLotes().Where(p => p.Codigo == Lote.Codigo).FirstOrDefault();
                if (LoteData == null)
                {
                    int id = 0;
                    id = int.Parse(ConexionBD.sp_CrearLote(Lote.Codigo, Lote.Capacidad, Lote.FechaExpiracion).Select(e => e.Value.ToString()).First());
                    if (id!=0)
                    {
                        LoteData = CargarTodosLosLotes().Where(p => Seguridad.DesEncriptar(p.IdLote) == id.ToString()).FirstOrDefault();
                        return LoteData;
                    }
                    //objeto = Seguridad.Encriptar(ConexionBD.sp_CrearLote(Lote.Codigo, Lote.Capacidad, Lote.FechaExpiracion).Select(e => e.Value.ToString()).First());
                }
                else
                {
                    return "El lote " + LoteData.Codigo + " ya existe";
                    //ConexionBD.sp_AumentarLote(int.Parse(Seguridad.DesEncriptar(LoteData.IdLote)), Lote.Capacidad);
                    //objeto = LoteData;
                }
                return objeto;
            }
            catch (Exception)
            {
                return "false";
            }
        }
        public List<Lote> CargarTodosLosLotes()
        {
            List<Lote> DatoTodosLosLotes = new List<Lote>();
            foreach (var item in ConexionBD.sp_MostrarTodosLosLotes())
            {
                DatoTodosLosLotes.Add(new Lote()
                {
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    Codigo = item.Codigo,
                    Capacidad = item.Capacidad,
                    Estado = item.Estado,
                    FechaExpiracion = item.FechaExpiracion,
                    LoteUtilizado = item.LoteUtilizado,
                });
            }
            return DatoTodosLosLotes;
        }
        public void CargarDatos(int IdCabecera, int IdRelacionLogica,string PerteneceKit)
        {
            ListaLote = new List<Lote>();
            foreach (var item in ConexionBD.sp_ConsultarLote(IdCabecera,IdRelacionLogica,Convert.ToBoolean(PerteneceKit)))
            {
                ListaLote.Add(new Lote()
                {
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    Codigo = item.Codigo,
                    Capacidad = item.Capacidad,
                    Estado = item.Estado,
                    FechaExpiracion = item.FechaExpiracion,
                    AsignarProductoLote = new AsignarProductoLote() {
                        IdAsignarProductoLote = Seguridad.Encriptar(item.IdAsignarProductoLote.ToString()),
                        ValorUnitario = item.ValorUnitario,
                    },
                });
            }
        }
        public List<Lote> ListarLotes(int IdCabecera, int IdRelacionLogica, string PerteneceKit)
        {
            CargarDatos(IdCabecera,IdRelacionLogica,PerteneceKit);
            return ListaLote;
        }
    }
}
