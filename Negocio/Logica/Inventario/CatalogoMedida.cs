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
        public string InsertarMedida(Medida Medida)
        {
            Medida Medida1 = ListarMedidas().Where(p => p.Descripcion == Medida.Descripcion.ToUpper()).FirstOrDefault();
            try
            {
                if (Medida1 == null)
                {
                    ConexionBD.sp_CrearMedida(Medida.Descripcion.ToUpper());
                    return "true";
                }
                else
                {
                    return "400";
                }
            }
            catch (Exception)
            {
                return "false";
            }
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
        public string ModificarMedida(Medida Medida)
        {
            Medida Medida1 = ListarMedidas().Where(p => p.Descripcion == Medida.Descripcion.ToUpper()).FirstOrDefault();
            try
            {
                if (Medida1 == null)
                {
                    ConexionBD.sp_ModificarMedida(int.Parse(Medida.IdMedida), Medida.Descripcion.ToUpper());
                    return "true";
                }
                else
                {
                    return "400";
                }
            }
            catch (Exception)
            {
                return "false";
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
    }
}
