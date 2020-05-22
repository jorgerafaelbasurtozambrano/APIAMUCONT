using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoSembrio
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        static List<Sembrio> ListaSembrios;
        Negocio.Metodos.Seguridad Seguridad = new Negocio.Metodos.Seguridad();

        public bool IngresarSembrio(Sembrio Sembrio)
        {
            try
            {
                ConexionBD.sp_CrearSembrio(Sembrio.Descripcion.ToUpper(), int.Parse(Sembrio.IdComunidad));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarSembrio(int IdSembrio)
        {
            try
            {
                ConexionBD.sp_EliminarSembrio(IdSembrio);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarSembrio(Sembrio Sembrio)
        {
            try
            {
                ConexionBD.sp_ModificarSembrio(int.Parse(Sembrio.IdSembrio), Sembrio.Descripcion.ToUpper(), int.Parse(Sembrio.IdComunidad));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void CargarSembrios()
        {
            ListaSembrios = new List<Sembrio>();
            foreach (var item in ConexionBD.sp_ConsultarSembrios())
            {
                ListaSembrios.Add(new Sembrio()
                {
                      IdSembrio =Seguridad.Encriptar(item.IdSembrio.ToString()),
                      Descripcion = item.Descripcion,
                      IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                      FechaCreacion = item.FechaCreacion,
                      FechaActualizacion = item.FechaActualizacion,
                      Estado = item.Estado,
                      Comunidad = new Comunidad()
                      {
                          IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                          Descripcion = item.ComunidadDescripcion,
                          Estado = item.ComunidadEstado,
                      },
                });
            }
        }
        public List<Sembrio> ListarSembrios()
        {
            CargarSembrios();
            return ListaSembrios.Where(p=>p.Estado!=false && p.Comunidad.Estado != false).ToList();
        }
        public Sembrio ConsultarSembrioPorId(int? IdSembrio)
        {
            Sembrio _Sembrio = new Sembrio();
            foreach (var item in ConexionBD.sp_ConsultarSembrios().Where(p=>p.IdSembrio == IdSembrio).ToList())
            {
                _Sembrio.IdSembrio = Seguridad.Encriptar(item.IdSembrio.ToString());
                _Sembrio.Descripcion = item.Descripcion;
                _Sembrio.IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString());
                _Sembrio.FechaCreacion = item.FechaCreacion;
                _Sembrio.FechaActualizacion = item.FechaActualizacion;
                _Sembrio.Estado = item.Estado;
                _Sembrio.Comunidad = new Comunidad()
                {
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    Descripcion = item.ComunidadDescripcion,
                    Estado = item.ComunidadEstado,
                };
            }
            return _Sembrio;
        }
    }
}
