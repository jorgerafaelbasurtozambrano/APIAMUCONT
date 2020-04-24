using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
using Negocio.Logica.TalentHumano;
namespace Negocio.Logica.TalentHumano
{
    public class CatalogoAsigancionPersonaComunidad
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        public bool IngresoAsignacionPersonaComunidad(Usuario Usuario)
        {
            try
            {
                ConexionBD.sp_CrearAsigancionPP(Usuario.Persona.IdPersona, int.Parse(Usuario.Persona.AsigancionPersonaComunidad.Comunidad.IdComunidad));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EliminarAsignacionPersonaComunidad(int IdAsignacionPersonaComunidad)
        {
            try
            {
                ConexionBD.sp_EliminarAsignacionPC(IdAsignacionPersonaComunidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModificarAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarAsignacionPC(int.Parse(AsignacionPersonaParroquiaEntidad.IdAsignacionPC), int.Parse(AsignacionPersonaParroquiaEntidad.IdPersona), int.Parse(AsignacionPersonaParroquiaEntidad.IdParroquia));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IngresoAsignacionPersonaComunidad(AsignacionPersonaParroquiaEntidad AsignacionPersonaParroquiaEntidad)
        {
            try
            {
                ConexionBD.sp_CrearAsigancionPP(int.Parse(AsignacionPersonaParroquiaEntidad.IdPersona), int.Parse(AsignacionPersonaParroquiaEntidad.IdParroquia));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
