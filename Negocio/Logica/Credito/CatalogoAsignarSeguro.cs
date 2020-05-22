using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
using Negocio.Entidades.DatoUsuarios;
namespace Negocio.Logica.Credito
{
    public class CatalogoAsignarSeguro
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public AsignarSeguro IngresoAsignarSeguro(AsignarSeguro _AsignarSeguro)
        {
            foreach (var item in ConexionBD.sp_CrearAsignarSeguro(int.Parse(_AsignarSeguro.IdConfigurarVenta),int.Parse(_AsignarSeguro.IdAsignarTUResp), int.Parse(_AsignarSeguro.IdAsignarTUTecn)))
            {
                _AsignarSeguro.IdAsignarSeguro = Seguridad.Encriptar(item.IdAsignarSeguro.ToString());
                _AsignarSeguro.IdAsignarTUResp = Seguridad.Encriptar(item.IdAsignacionTUResp.ToString());
                _AsignarSeguro.IdAsignarTUTecn = Seguridad.Encriptar(item.IdAsignacionTUTecn.ToString());
                _AsignarSeguro.FechaAsignacion = item.FechaAsignacion;
            }
            return _AsignarSeguro;
        }
        public List<AsignarSeguro> ConsultarAsignarSeguroPorConfigurarVenta(int _idConfigurarVenta)
        {
            List<AsignarSeguro> _AsignarSeguro = new List<AsignarSeguro>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarSeguroPorConfigurarVenta(_idConfigurarVenta))
            {
                PersonaEntidad _Responsable = new PersonaEntidad();
                PersonaEntidad _Tecnico = new PersonaEntidad();
                foreach (var item1 in ConexionBD.sp_ConsultarAsignarTipoUsuarioPersona(item.AsignarSeguroIdAsignacionTUResp))
                {
                    _Responsable.NumeroDocumento = item1.PersonaNumeroDocumento;
                    _Responsable.PrimerNombre = item1.PersonaPrimerNombre;
                    _Responsable.SegundoNombre = item1.PersonaSegundoNombre;
                    _Responsable.ApellidoMaterno = item1.PersonaApellidoMaterno;
                    _Responsable.ApellidoPaterno = item1.PersonaApellidoPaterno;
                    _Responsable.IdPersona = Seguridad.Encriptar(item1.PersonaIdPersona.ToString());
                    _Responsable.IdTipoDocumento = Seguridad.Encriptar(item1.PersonaIdTipoDocumento.ToString());
                    _Responsable.AsignacionTipoUsuario = new AsignacionTipoUsuario()
                    {
                        IdAsignacionTUEncriptada = Seguridad.Encriptar(item1.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                        TipoUsuario = new TipoUsuario() { 
                            IdTipoUsuario = Seguridad.Encriptar(item1.TipoUsuarioIdTipoUsuario.ToString()),
                            Identificacion = item1.TipoUsuarioIdentificacion,
                            Descripcion = item1.TipoUsuarioDescripcion,
                        }
                    };
                }
                foreach (var item1 in ConexionBD.sp_ConsultarAsignarTipoUsuarioPersona(item.AsignarSeguroIdAsignacionTUTecn))
                {
                    _Tecnico.NumeroDocumento = item1.PersonaNumeroDocumento;
                    _Tecnico.PrimerNombre = item1.PersonaPrimerNombre;
                    _Tecnico.SegundoNombre = item1.PersonaSegundoNombre;
                    _Tecnico.ApellidoMaterno = item1.PersonaApellidoMaterno;
                    _Tecnico.ApellidoPaterno = item1.PersonaApellidoPaterno;
                    _Tecnico.IdPersona = Seguridad.Encriptar(item1.PersonaIdPersona.ToString());
                    _Tecnico.IdTipoDocumento = Seguridad.Encriptar(item1.PersonaIdTipoDocumento.ToString());
                    _Tecnico.AsignacionTipoUsuario = new AsignacionTipoUsuario()
                    {
                        IdAsignacionTUEncriptada = Seguridad.Encriptar(item1.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                        TipoUsuario = new TipoUsuario()
                        {
                            IdTipoUsuario = Seguridad.Encriptar(item1.TipoUsuarioIdTipoUsuario.ToString()),
                            Identificacion = item1.TipoUsuarioIdentificacion,
                            Descripcion = item1.TipoUsuarioDescripcion,
                        }
                    };
                }
                _AsignarSeguro.Add(new AsignarSeguro()
                {
                    IdAsignarSeguro = Seguridad.Encriptar(item.AsignarSeguroIdAsignarSeguro.ToString()),
                    IdAsignarTUResp = Seguridad.Encriptar(item.AsignarSeguroIdAsignacionTUResp.ToString()),
                    IdAsignarTUTecn = Seguridad.Encriptar(item.AsignarSeguroIdAsignacionTUTecn.ToString()),
                    FechaAsignacion = item.AsignarSeguroFechaAsignacion,
                    _Responsable = _Responsable,
                    _Tecnico = _Tecnico
                });
            }
            return _AsignarSeguro;
        }
    }
}
