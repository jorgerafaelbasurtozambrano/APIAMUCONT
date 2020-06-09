using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoCorreo
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public bool IngresarCorreo(Usuario Usuario)
        {
            try
            {
                ConexionBD.sp_CrearCorreo(Usuario.Persona.IdPersona, Usuario.Persona.Correo.CorreoValor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarCorreo(int IdCorreo)
        {
            try
            {
                ConexionBD.sp_EliminarCorreo(IdCorreo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Correo ModificarCorreo(Correo Correo)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarCorreo(int.Parse(Correo.IdCorreo), int.Parse(Correo.IdPersona), Correo.CorreoValor))
                {
                    Correo.IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString());
                    Correo.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                    Correo.CorreoValor = item.Correo;
                    Correo.Estado = item.Estado;
                    Correo.FechaCreacion = item.FechaCreacion;
                }
                return Correo;
            }
            catch (Exception)
            {
                Correo.IdCorreo = null;
                return Correo;
            }
        }
        public Correo IngresoCorreo(Correo Correo)
        {
            foreach (var item in ConexionBD.sp_CrearCorreo(int.Parse(Correo.IdPersona), Correo.CorreoValor))
            {
                Correo.IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString());
                Correo.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                Correo.CorreoValor = item.Correo;
                Correo.Estado = item.Estado;
                Correo.FechaCreacion = item.FechaCreacion;
            }
            return Correo;
        }
        public List<Correo> ConsultarCorreoPorPersona(Correo _Correo)
        {
            List<Correo> ListaCorreo = new List<Correo>();
            foreach (var item in ConexionBD.sp_ConsultarCorreoPorPersona(int.Parse(_Correo.IdPersona),_Correo.CorreoValor))
            {
                ListaCorreo.Add(new Correo()
                {
                    IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString()),
                    CorreoValor = item.Correo,
                    Estado = item.Estado,
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    FechaCreacion = item.FechaCreacion
                });
            }
            return ListaCorreo;
        }
        public List<Correo> ConsultarCorreoPorIdCorreo(int idCorreo)
        {
            List<Correo> ListaCorreo = new List<Correo>();
            foreach (var item in ConexionBD.sp_ConsultarCorreoPorId(idCorreo))
            {
                ListaCorreo.Add(new Correo()
                {
                    IdCorreo = Seguridad.Encriptar(item.IdCorreo.ToString()),
                    CorreoValor = item.Correo,
                    Estado = item.Estado,
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    FechaCreacion = item.FechaCreacion
                });
            }
            return ListaCorreo;
        }
        public bool HabilitarCorreo(Correo _Correo)
        {
            try
            {
                ConexionBD.sp_HabilitarCorreo(int.Parse(_Correo.IdCorreo), int.Parse(_Correo.IdPersona));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
