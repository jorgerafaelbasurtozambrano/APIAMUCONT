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
        public bool ModificarCorreo(Correo Correo)
        {
            try
            {
                ConexionBD.sp_ModificarCorreo(int.Parse(Correo.IdCorreo), int.Parse(Correo.IdPersona), Correo.CorreoValor);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IngresoCorreo(Correo Correo)
        {
            try
            {
                ConexionBD.sp_CrearCorreo(int.Parse(Correo.IdPersona), Correo.CorreoValor);                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
