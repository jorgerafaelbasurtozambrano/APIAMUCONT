 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoMedida
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Medida> ListaMedida;
        public Medida InsertarMedida(Medida Medida)
        {
            foreach (var item in ConexionBD.sp_CrearMedida(Medida.Descripcion.ToUpper()))
            {
                Medida.IdMedida = Seguridad.Encriptar(item.IdMedida.ToString());
                Medida.Descripcion = item.Descripcion;
                Medida.FechaActualizacion = item.FechaActualizacion;
                Medida.FechaCreacion = item.FechaCreacion;
                Medida.Estado = item.Estado;
                Medida.MedidaUtilizado = "0";
            }
            return Medida;
        }
        public bool EliminarMedida(int IdMedida)
        {
            try
            {
                ConexionBD.sp_EliminarMedida(IdMedida);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Medida ModificarMedida(Medida Medida)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarMedida(int.Parse(Medida.IdMedida), Medida.Descripcion.ToUpper()))
                {
                    Medida.IdMedida = Seguridad.Encriptar(item.IdMedida.ToString());
                    Medida.Descripcion = item.Descripcion;
                    Medida.FechaActualizacion = item.FechaActualizacion;
                    Medida.FechaCreacion = item.FechaCreacion;
                    Medida.Estado = item.Estado;
                    Medida.MedidaUtilizado = item.MedidaUtilizado;
                }
                return Medida;
            }
            catch (Exception)
            {
                Medida.IdMedida = null;
                return Medida;
            }
        }
        public void CargarMedidas()
        {
            ListaMedida = new List<Medida>();
            foreach (var item in ConexionBD.sp_ConsultarMedida())
            {
                ListaMedida.Add(new Medida()
                {
                    IdMedida = Seguridad.Encriptar(item.IdMedida.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    MedidaUtilizado = item.MedidaUtilizado,
                });
            }
        }
        public List<Medida> ListarMedidas()
        {
            CargarMedidas();
            return ListaMedida.GroupBy(a => a.IdMedida).Select(grp => grp.First()).Where(p => p.Estado != false).ToList();
        }
        public List<Medida> ConsultarMedidaPorDescripcion(string Descripcion)
        {
            ListaMedida = new List<Medida>();
            foreach (var item in ConexionBD.sp_ConsultarSiExisteMedida(Descripcion))
            {
                ListaMedida.Add(new Medida()
                {
                    IdMedida = Seguridad.Encriptar(item.IdMedida.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                });
            }
            return ListaMedida;
        }
        public List<Medida> ConsultarMedidaPorId(int IdMedida)
        {
            ListaMedida = new List<Medida>();
            foreach (var item in ConexionBD.sp_ConsultarMedidaPorId(IdMedida))
            {
                ListaMedida.Add(new Medida()
                {
                    IdMedida = Seguridad.Encriptar(item.IdMedida.ToString()),
                    Descripcion = item.Descripcion,
                    FechaActualizacion = item.FechaActualizacion,
                    FechaCreacion = item.FechaCreacion,
                    Estado = item.Estado,
                    MedidaUtilizado = item.MedidaUtilizado
                });
            }
            return ListaMedida;
        }
    }
}
