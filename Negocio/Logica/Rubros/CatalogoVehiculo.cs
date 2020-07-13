using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Rubros
{
    public class CatalogoVehiculo
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        List<Vehiculo> _ListaVehiculos = new List<Vehiculo>();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public Vehiculo IngresarVehiculo(Vehiculo _Vehiculo)
        {
            foreach (var item in ConexionBD.sp_CrearVehiculo(_Vehiculo.Placa, int.Parse(_Vehiculo.IdAsignarTU)))
            {
                _Vehiculo.IdVehiculo = Seguridad.Encriptar(item.IdVehiculo.ToString());
                _Vehiculo.Placa = item.Placa;
                _Vehiculo.Estado = item.Estado;
                _Vehiculo.IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString());
                _Vehiculo.FechaCreacion = item.FechaCreacion;
            }
            return _Vehiculo;
        }
        public List<Vehiculo> ConsultarVehiculoPorPlaca(string Placa)
        {
            _ListaVehiculos = new List<Vehiculo>();
            foreach (var item in ConexionBD.sp_ConsultarVehiculoPorPlaca(Placa))
            {
                _ListaVehiculos.Add(new Vehiculo()
                {
                    IdVehiculo = Seguridad.Encriptar(item.IdVehiculo.ToString()),
                    Placa = item.Placa,
                    Estado = item.Estado,
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString()),
                    FechaCreacion = item.FechaCreacion
                });
            }
            return _ListaVehiculos;
        }
        public List<Vehiculo> ConsultarVehiculos()
        {
            _ListaVehiculos = new List<Vehiculo>();
            foreach (var item in ConexionBD.sp_ConsultarVehiculos())
            {
                _ListaVehiculos.Add(new Vehiculo()
                {
                    IdVehiculo = Seguridad.Encriptar(item.IdVehiculo.ToString()),
                    Placa = item.Placa,
                    Estado = item.Estado,
                    IdAsignarTU = Seguridad.Encriptar(item.IdAsignarTU.ToString()),
                    FechaCreacion = item.FechaCreacion
                });
            }
            return _ListaVehiculos;
        }
    }
}
