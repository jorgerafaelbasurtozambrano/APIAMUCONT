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
    public class CatalogoAsignarTecnicoPersonaComunidad
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        CatalogoAsignarComunidadFactura _CatalogoAsignarComunidadFactura = new CatalogoAsignarComunidadFactura();
        public AsignarTecnicoPersonaComunidad InsertarAsignarTecnicoPersonaComunidad(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            foreach (var item in ConexionBD.sp_CrearAsignarTecnicoPersonaComunidad(int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico),int.Parse(_AsignarTecnicoPersonaComunidad.IdPersona),int.Parse(_AsignarTecnicoPersonaComunidad.IdComunidad)))
            {
                _AsignarTecnicoPersonaComunidad.IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString());
                _AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString());
                _AsignarTecnicoPersonaComunidad.IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString());
                _AsignarTecnicoPersonaComunidad.IdPersona = Seguridad.Encriptar(item.IdPersona.ToString());
                _AsignarTecnicoPersonaComunidad.Estado = item.Estado;
                _AsignarTecnicoPersonaComunidad.FechaAsignacion = item.FechaAsignacion;
            }
            return _AsignarTecnicoPersonaComunidad;
        }
        public bool ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura(int _idPersona, int _idTecnico)
        {
            try
            {
                AsignarTecnicoPersonaComunidad _Dato = new AsignarTecnicoPersonaComunidad();
                PersonaEntidad Persona = new PersonaEntidad();
                _Dato = ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona).FirstOrDefault();
                if (_Dato == null)
                {
                    // si en esta instancia es igual a null es porque la persona no se le a asignado ningun tecnico por lo que el administradord el sistema deberea hacerlo
                    if (_idTecnico != 0)
                    {
                        Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                        if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya Asignar el administrador
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = _idTecnico.ToString();
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                    }
                }
                else
                {
                    Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                    if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya tiene
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = Seguridad.DesEncriptar(_Dato.IdAsignarTUTecnico);
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ListarPersonasParaAsignarPersonasComunidadAlFinalizarUnaFactura1(AsignarTecnicoPersonaComunidad _AsignarTecnicoPersonaComunidad)
        {
            try
            {
                int _idPersona = int.Parse(_AsignarTecnicoPersonaComunidad.IdPersona);
                int _idTecnicos = int.Parse(_AsignarTecnicoPersonaComunidad.IdAsignarTUTecnico);
                AsignarTecnicoPersonaComunidad _Dato = new AsignarTecnicoPersonaComunidad();
                PersonaEntidad Persona = new PersonaEntidad();
                _Dato = ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona).FirstOrDefault();
                if (_Dato == null)
                {
                    // si en esta instancia es igual a null es porque la persona no se le a asignado ningun tecnico por lo que el administradord el sistema deberea hacerlo
                    if (_idTecnicos != 0)
                    {
                        Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                        if (Persona != null)
                        {
                            foreach (var item in Persona.ListaComunidad)
                            {
                                AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                                _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                                //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                                if (_DatoAsginarTecnicoPersona == null)
                                {
                                    //Asignarle A la persona Con el tecnico que ya Asignar el administrador
                                    AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                    _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                    _DatoAIngresar.IdAsignarTUTecnico = _idTecnicos.ToString();
                                    _DatoAIngresar.IdPersona = _idPersona.ToString();
                                    InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Persona = _CatalogoAsignarComunidadFactura.ConsultarPersonasEnFacturasParaSeguimientoPorPersona(_idPersona).FirstOrDefault();
                    if (Persona != null)
                    {
                        foreach (var item in Persona.ListaComunidad)
                        {
                            AsignarTecnicoPersonaComunidad _DatoAsginarTecnicoPersona = new AsignarTecnicoPersonaComunidad();
                            _DatoAsginarTecnicoPersona = ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, int.Parse(Seguridad.DesEncriptar(item.IdComunidad))).FirstOrDefault();
                            //si dato es igual a null significa que esta personas con esta comunidad no se le a asignado esta comunidad
                            if (_DatoAsginarTecnicoPersona == null)
                            {
                                //Asignarle A la persona Con el tecnico que ya tiene
                                AsignarTecnicoPersonaComunidad _DatoAIngresar = new AsignarTecnicoPersonaComunidad();
                                _DatoAIngresar.IdComunidad = Seguridad.DesEncriptar(item.IdComunidad);
                                _DatoAIngresar.IdAsignarTUTecnico = Seguridad.DesEncriptar(_Dato.IdAsignarTUTecnico);
                                _DatoAIngresar.IdPersona = Seguridad.DesEncriptar(_Dato.IdPersona);
                                InsertarAsignarTecnicoPersonaComunidad(_DatoAIngresar);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarAsignarTecnicoPersonaComunidadPorPersona(int _idPersona)
        {
            List<AsignarTecnicoPersonaComunidad> ListaAsignarTecnico = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorPersona(_idPersona))
            {
                ListaAsignarTecnico.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return ListaAsignarTecnico;
        }
        public List<AsignarTecnicoPersonaComunidad> ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(int _idPersona,int _IdComunidad)
        {
            List<AsignarTecnicoPersonaComunidad> ListaAsignarTecnico = new List<AsignarTecnicoPersonaComunidad>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarTecnicoPersonaComunidadPorPersonaYComunidad(_idPersona, _IdComunidad))
            {
                ListaAsignarTecnico.Add(new AsignarTecnicoPersonaComunidad()
                {
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTUTecnico = Seguridad.Encriptar(item.IdAsignarTUTecnico.ToString()),
                    IdComunidad = Seguridad.Encriptar(item.IdComunidad.ToString()),
                    IdPersona = Seguridad.Encriptar(item.IdPersona.ToString()),
                    Estado = item.Estado,
                    FechaAsignacion = item.FechaAsignacion,
                });
            }
            return ListaAsignarTecnico;
        }
        public bool EliminarPersonaDeUnTecnico(int _idPersona)
        {
            try
            {
                ConexionBD.sp_EliminarAsignacionPersonaDeUnTecnico(_idPersona);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<TipoUsuario> ConsultarTipoUsuarioTecncioDeUnaPersona(int IdAsignacionTU)
        {
            List<TipoUsuario> TipoUsuario = new List<TipoUsuario>();
            foreach (var item in ConexionBD.sp_ConsultarSiUsuarioEsTecnico(IdAsignacionTU))
            {
                TipoUsuario.Add(new Entidades.TipoUsuario()
                {
                    IdTipoUsuario = Seguridad.Encriptar(item.IdTipoUsuario.ToString()),
                    Identificacion = item.Identificacion,
                    Estado = item.Estado,
                });
            }
            return TipoUsuario;
        }

    }
}
