using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Logica.TalentHumano;
using Negocio.Logica.Usuarios;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica
{
    public class CatalogoPersona
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoUsuario GestionUsuario = new CatalogoUsuario();
        ConsultarUsuariosYPersonas BuscaPersona = new ConsultarUsuariosYPersonas();

        public void EliminarPersona(int IdPersona)
        {
            /*PersonaEntidad persona = new PersonaEntidad();
            persona = BuscaPersona.FiltrarPersona(IdPersona);
            if (persona != null)
            {
                UsuarioEntidad Usuario = new UsuarioEntidad();
                if (persona.IdUsuario !="" )
                {
                    persona.IdUsuario = Seguridad.DesEncriptar(persona.IdUsuario);
                    if (GestionUsuario.ObtenerUsuario(int.Parse(persona.IdUsuario)) != null)
                    {
                        return false;
                    }
                    else
                    {
                        ConexionBD.sp_EliminarPersona(IdPersona);
                        return true;
                    }
                }
                else
                {*/
                    ConexionBD.sp_EliminarPersona(IdPersona); 
                    /*return true;
                }
            }
            else
            {
                return false;
            }*/
        }
        public string ModificarPersona(PersonaEntidad PersonaEntidad)
        {
            try
            {
                ConexionBD.sp_ModificarPersona(int.Parse(PersonaEntidad.IdPersona), PersonaEntidad.NumeroDocumento, PersonaEntidad.ApellidoPaterno.ToUpper(), PersonaEntidad.ApellidoMaterno.ToUpper(), PersonaEntidad.PrimerNombre.ToUpper(), PersonaEntidad.SegundoNombre.ToUpper(), int.Parse(PersonaEntidad.IdTipoDocumento));
                return "true";
            }
            catch (Exception)
            {
                return "false";
            }
            /*
            List<PersonaEntidad> persona = new List<PersonaEntidad>();
            persona = BuscaPersona.ObtenerUsuariosClientes();
            for (int i = 0; i < persona.Count; i++)
            {
                persona[i].IdPersona = Seguridad.DesEncriptar(persona[i].IdPersona);
            }

            PersonaEntidad persona1 = new PersonaEntidad();
            persona1 = persona.Where(p => p.IdPersona == PersonaEntidad.IdPersona).FirstOrDefault();
            if (persona1!=null)
            {
                if (persona1.NumeroDocumento == PersonaEntidad.NumeroDocumento)
                {
                    return "true";
                }
                if (persona.Where(p => p.NumeroDocumento == PersonaEntidad.NumeroDocumento.Trim()).FirstOrDefault() == null)
                {
                    ConexionBD.sp_ModificarPersona(int.Parse(PersonaEntidad.IdPersona), PersonaEntidad.NumeroDocumento, PersonaEntidad.ApellidoPaterno.ToUpper(), PersonaEntidad.ApellidoMaterno.ToUpper(), PersonaEntidad.PrimerNombre.ToUpper(), PersonaEntidad.SegundoNombre.ToUpper(), int.Parse(PersonaEntidad.IdTipoDocumento));
                    return "true";
                }else
                {
                    return "false";
                }
            }
            else
            {
                return "false";
            }*/
        }
        public string IngresarPersona(PersonaEntidad PersonaEntidad)
        {
            try
            {
                List<PersonaEntidad> persona = new List<PersonaEntidad>();
                persona = BuscaPersona.ObtenerUsuariosClientes();
                if (persona.Where(p => p.NumeroDocumento == PersonaEntidad.NumeroDocumento.Trim()).FirstOrDefault() == null)
                {
                    int idPersona = int.Parse(ConexionBD.sp_CrearPersona(PersonaEntidad.NumeroDocumento.Trim(), PersonaEntidad.ApellidoPaterno.ToUpper(), PersonaEntidad.ApellidoMaterno.ToUpper(), PersonaEntidad.PrimerNombre.ToUpper(), PersonaEntidad.SegundoNombre.ToUpper(), int.Parse(PersonaEntidad.IdTipoDocumento)).Select(e => e.Value.ToString()).First());
                    return Seguridad.Encriptar(idPersona.ToString());
                }
                else
                { 
                    return "false";
                }
            }
            catch (Exception)
            {
                return "400";
            }
            
        }
        
    }
}
