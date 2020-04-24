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
    public class CatalogoTelefono
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        public bool IngresarTelefono(TelefonoEntidad TelefonoEntidad)
        {
            try
            {
                ConexionBD.sp_CrearTelefono(int.Parse(TelefonoEntidad.IdPersona), TelefonoEntidad.Numero, int.Parse(TelefonoEntidad.IdTipoTelefono));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ModificarTelefono(TelefonoEntidad TelefonoEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarTelefono(int.Parse(TelefonoEntidad.IdTelefono), int.Parse(TelefonoEntidad.IdPersona), TelefonoEntidad.Numero, int.Parse(TelefonoEntidad.IdTipoTelefono));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool EliminarTelefono(int IdTelefono)
        {
            try
            {
                ConexionBD.sp_EliminarTelefono(IdTelefono);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool IngresoTelefono(Telefono Telefono)
        {
            try
            {
                ConexionBD.sp_CrearTelefono(int.Parse(Telefono.IdPersona), Telefono.Numero, int.Parse(Telefono.TipoTelefono.IdTipoTelefono));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
