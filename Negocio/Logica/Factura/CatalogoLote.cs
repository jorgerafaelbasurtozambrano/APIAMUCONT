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
        public Lote IngresarLote(Lote Lote)
        {
            foreach (var item in ConexionBD.sp_CrearLote(Lote.Codigo, Lote.Capacidad, Lote.FechaExpiracion))
            {
                Lote.IdLote = Seguridad.Encriptar(item.IdLote.ToString());
                Lote.Capacidad = item.Capacidad;
                Lote.Codigo = item.Codigo;
                Lote.FechaExpiracion = item.FechaExpiracion;
                Lote.Estado = item.Estado;
            }
            return Lote;
        }
        public List<Lote> ConsultarLotePorCodigo(string Codigo)
        {
            List<Lote> ListaLote = new List<Lote>();
            foreach (var item in ConexionBD.sp_ConsultarLotePorCodigo(Codigo))
            {
                ListaLote.Add(new Lote()
                {
                    IdLote = Seguridad.Encriptar(item.IdLote.ToString()),
                    Codigo = item.Codigo,
                    FechaExpiracion = item.FechaExpiracion,
                    Capacidad = item.Capacidad,
                    Estado = item.Estado
                });
            }
            return ListaLote;
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
