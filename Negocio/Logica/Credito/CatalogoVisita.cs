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
    public class CatalogoVisita
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public Visita InsertarVisita(Visita _Visita)
        {
            foreach (var item in ConexionBD.sp_CrearVisita(int.Parse(_Visita.IdAsignarTecnicoPersonaComunidad),int.Parse(_Visita.IdAsignarTU), _Visita.Observacion))
            {
                _Visita.IdVisita = Seguridad.Encriptar(item.IdVisita.ToString());
                _Visita.IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString());
                _Visita.IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString());
                _Visita.FechaRegistro = item.FechaRegistro;
                _Visita.Observacion = item.Observacion;
            }
            return _Visita;
        }
        public List<Visita> ConsultarHistorialDeVisita(int _IdAsignarTecnicoPersonaComunidad)
        {
            List<Visita> HistorialVisita = new List<Visita>();
            PersonaEntidad Persona = new PersonaEntidad();
            foreach (var item in ConexionBD.sp_ConsultarVisita(_IdAsignarTecnicoPersonaComunidad))
            {
                foreach (var item1 in ConexionBD.sp_ConsultarAsignarTipoUsuarioPersona(item.IdAsignarTU))
                {
                    Persona.IdPersona = Seguridad.Encriptar(item1.PersonaIdPersona.ToString());
                    Persona.IdTipoDocumento = Seguridad.Encriptar(item1.PersonaIdTipoDocumento.ToString());
                    Persona.NumeroDocumento = item1.PersonaNumeroDocumento;
                    Persona.ApellidoPaterno = item1.PersonaApellidoPaterno;
                    Persona.ApellidoMaterno = item1.PersonaApellidoMaterno;
                    Persona.PrimerNombre = item1.PersonaPrimerNombre;
                    Persona.SegundoNombre = item1.PersonaSegundoNombre;
                    Persona.AsignacionTipoUsuario = new AsignacionTipoUsuario()
                    {
                        IdAsignacionTUEncriptada = Seguridad.Encriptar(item1.AsignacionTipoUsuarioIdAsignacionTU.ToString()),
                        FechaCreacion = item1.AsignacionTipoUsuarioFechaCreacion,
                        Estado = item1.AsignacionTipoUsuarioEstado,
                        TipoUsuario = new TipoUsuario()
                        {
                            IdTipoUsuario = Seguridad.Encriptar(item1.TipoUsuarioIdTipoUsuario.ToString()),
                            Descripcion = item1.TipoUsuarioDescripcion,
                            Identificacion = item1.TipoUsuarioIdentificacion,
                        }
                    };
                }
                HistorialVisita.Add(new Visita()
                {
                    IdVisita = Seguridad.Encriptar(item.IdVisita.ToString()),
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString()),
                    FechaRegistro = item.FechaRegistro,
                    Observacion = item.Observacion,
                });
            }
            return HistorialVisita;
        }
        public bool EliminarVisita(int _idVisita)
        {
            try
            {
                ConexionBD.sp_EliminarVisita(_idVisita);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Visita ModificarVisita(Visita _Visita)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarVisita(int.Parse(_Visita.IdVisita),int.Parse(_Visita.IdAsignarTU), _Visita.Observacion))
                {
                    _Visita.IdVisita = Seguridad.Encriptar(item.IdVisita.ToString());
                    _Visita.IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString());
                    _Visita.IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString());
                    _Visita.FechaRegistro = item.FechaRegistro;
                    _Visita.Observacion = item.Observacion;
                }
                return _Visita;
            }
            catch (Exception)
            {
                _Visita.IdVisita = null;
                return _Visita;
            }
        }
        public List<Visita> ConsultarVisitaPorId(int _idVisita)
        {
            List<Visita> _Visitas = new List<Visita>();
            foreach (var item in ConexionBD.sp_ConsultarVisitaPorIdVisita(_idVisita))
            {
                _Visitas.Add(new Visita()
                {
                    IdVisita = Seguridad.Encriptar(item.IdVisita.ToString()),
                    IdAsignarTecnicoPersonaComunidad = Seguridad.Encriptar(item.IdAsignarTecnicoPersonaComunidad.ToString()),
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString()),
                    FechaRegistro = item.FechaRegistro,
                    Observacion = item.Observacion,
                });
            }
            return _Visitas;
        }
    }
}
