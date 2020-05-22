using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Credito
{
    public class CaatalogoAbono
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        public Abono InsertarAbono(Abono _Abono)
        {
            foreach (var item in ConexionBD.sp_CrearAbono(int.Parse(_Abono.IdConfigurarVenta),int.Parse(_Abono.IdAsignarTU),_Abono.Monto))
            {
                _Abono.IdAbono = Seguridad.Encriptar(item.IdAbono.ToString());
                _Abono.IdAsignarTU = Seguridad.Encriptar(item.IdAsignacionTU.ToString());
                _Abono.IdConfigurarVenta = Seguridad.Encriptar(item.IdConfigurarVenta.ToString());
                _Abono.Monto = item.Monto;
                _Abono.FechaRegistro = item.FechaRegistro;
            }
            return _Abono;
        }
    }
}
