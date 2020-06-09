using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Negocio.Entidades;
namespace Negocio.Logica.Inventario
{
    public class CatalogoKit
    {
        AMUCOMTEntities ConexionBD = new AMUCOMTEntities();
        Negocio.Metodos.Seguridad Seguridad = new Metodos.Seguridad();
        static List<Kit> ListaKit;
        CatalogoAsignarDescuentoKit GestionAsignarDescuentoKit = new CatalogoAsignarDescuentoKit();
        public Kit InsertarKit(Kit Kit)
        {
            Descuento DatoDescuento = new Descuento();
            DatoDescuento = ConsultarDescuento(Kit.AsignarDescuentoKit.Descuento.Porcentaje).FirstOrDefault();
            if (DatoDescuento == null)
            {
                DatoDescuento = InsertarDescuento(new Descuento() { Porcentaje = Kit.AsignarDescuentoKit.Descuento.Porcentaje });
            }
            if (DatoDescuento.IdDescuento == null)
            {
                Kit.IdKit = null;
                return Kit;
            }
            else
            {
                foreach (var item in ConexionBD.sp_CrearKit(Kit.Codigo, Kit.Descripcion.ToUpper()))
                {
                    Kit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                    Kit.Descripcion = item.Descripcion;
                    Kit.Codigo = item.Codigo;
                    Kit.Descripcion = item.Descripcion;
                    Kit.FechaActualizacion = item.FechaActualizacion;
                    Kit.FechaCreacion = item.FechaCreacion;
                    Kit.KitUtilizado = "1";
                }
                if (Kit.IdKit == null)
                {
                    Kit.IdKit = null;
                    return Kit;
                }
                else
                {
                    AsignarDescuentoKit DatoAsignarDescuentoKit = new AsignarDescuentoKit();
                    DatoAsignarDescuentoKit = InsertarAsignarDescuentoKit(new AsignarDescuentoKit() { IdDescuento = Seguridad.DesEncriptar(DatoDescuento.IdDescuento), IdKit = Seguridad.DesEncriptar(Kit.IdKit) });
                    if (DatoAsignarDescuentoKit.IdAsignarDescuentoKit == null)
                    {
                        EliminarKit(int.Parse(Seguridad.DesEncriptar(Kit.IdKit)));
                        Kit.IdKit = null;
                        return Kit;
                    }
                    else
                    {
                        return ConsultarKitPorId(int.Parse(Seguridad.DesEncriptar(Kit.IdKit))).FirstOrDefault();
                    }
                }
            }
        }
        public List<AsignarDescuentoKit> CargarAsignarDescuentoKitPorIdKit(int idKit)
        {
            List<AsignarDescuentoKit>  ListaAsignarDescuentoKit = new List<AsignarDescuentoKit>();
            foreach (var item in ConexionBD.sp_ConsultarAsignarDescuentoKitPorKit(idKit))
            {
                ListaAsignarDescuentoKit.Add(new AsignarDescuentoKit()
                {
                    IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString()),
                    IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString()),
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaModificacion,
                    Estado = item.Estado,
                });
            }
            return ListaAsignarDescuentoKit;
        }

        public AsignarDescuentoKit InsertarAsignarDescuentoKit(AsignarDescuentoKit AsignarDescuentoKit)
        {
            try
            {
                AsignarDescuentoKit DatoAsignarDescuentoKit = new AsignarDescuentoKit();
                DatoAsignarDescuentoKit = CargarAsignarDescuentoKitPorIdKit(int.Parse(AsignarDescuentoKit.IdKit)).FirstOrDefault();
                if (DatoAsignarDescuentoKit == null)
                {
                    foreach (var item in ConexionBD.sp_CrearAsignarDescuentoKit(int.Parse(AsignarDescuentoKit.IdDescuento), int.Parse(AsignarDescuentoKit.IdKit)))
                    {
                        AsignarDescuentoKit.IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString());
                        AsignarDescuentoKit.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                        AsignarDescuentoKit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                        AsignarDescuentoKit.FechaCreacion = item.FechaCreacion;
                        AsignarDescuentoKit.FechaActualizacion = item.FechaModificacion;
                        AsignarDescuentoKit.Estado = item.Estado;
                    }
                }
                else
                {
                    foreach (var item in ConexionBD.sp_ModificarAsignarDescuentoKit(int.Parse(Seguridad.DesEncriptar(DatoAsignarDescuentoKit.IdAsignarDescuentoKit)),int.Parse(AsignarDescuentoKit.IdDescuento), int.Parse(AsignarDescuentoKit.IdKit)))
                    {
                        AsignarDescuentoKit.IdAsignarDescuentoKit = Seguridad.Encriptar(item.IdAsignarDescuentoKit.ToString());
                        AsignarDescuentoKit.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                        AsignarDescuentoKit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                        AsignarDescuentoKit.FechaCreacion = item.FechaCreacion;
                        AsignarDescuentoKit.FechaActualizacion = item.FechaModificacion;
                        AsignarDescuentoKit.Estado = item.Estado;
                    }
                }
                return AsignarDescuentoKit;
            }
            catch (Exception)
            {
                AsignarDescuentoKit.IdAsignarDescuentoKit = null;
                return AsignarDescuentoKit;
            }
        }
        public bool EliminarKit(int IdKit)
        {
            try
            {
                ConexionBD.sp_EliminarKit(IdKit);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Kit ModificarKit(Kit Kit)
        {
            try
            {
                foreach (var item in ConexionBD.sp_ModificarKit(int.Parse(Kit.IdKit), Kit.Codigo, Kit.Descripcion.ToUpper()))
                {
                    Kit.IdKit = Seguridad.Encriptar(item.IdKit.ToString());
                    Kit.Descripcion = item.Descripcion;
                    Kit.Codigo = item.Codigo;
                    Kit.Descripcion = item.Descripcion;
                    Kit.FechaActualizacion = item.FechaActualizacion;
                    Kit.FechaCreacion = item.FechaCreacion;
                    Kit.KitUtilizado = item.KitUtilizado;
                }
                return Kit;
            }
            catch (Exception)
            {
                Kit.IdKit = null;
                return Kit;
            }
        }
        public void CargarKits()
        {
            ListaKit = new List<Kit>();
            foreach (var item in ConexionBD.sp_ConsultarKit())
            {
                ListaKit.Add(new Kit()
                {
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                    KitUtilizado = item.KitUtilizado,
                    AsignarDescuentoKit = new AsignarDescuentoKit()
                    {
                        IdAsignarDescuentoKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
                        IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                        IdKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdKit.ToString()),
                        FechaCreacion = item.AsignarDescuentoKitFechaCreacion,
                        FechaActualizacion = item.AsignarDescuentoKitFechaModificacion,
                        Estado = item.AsignarDescuentoKitEstado,
                        Descuento = new Descuento()
                        {
                            IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                            Porcentaje = item.DescuentoPorcentaje,
                            Estado = item.DescuentoEstado,
                        }
                    },
                });
            }
        }
        public List<Kit> ListarKit()
        {
            CargarKits();
            //return ListaKit.Where(p=>p.Estado !=false).GroupBy(a => a.IdKit).Select(grp => grp.First()).Where(p => p.Estado != false).ToList();
            return ListaKit.Where(p => p.Estado == true && p.AsignarDescuentoKit.Estado == true ).GroupBy(a => a.IdKit).Select(grp => grp.First()).ToList();
        }
        public List<Kit> ConsultarKitPorCodigo(string Codigo)
        {
            ListaKit = new List<Kit>();
            foreach (var item in ConexionBD.sp_ConsultarSiExisteKitPorCodigo(Codigo))
            {
                ListaKit.Add(new Kit()
                {
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                });
            }
            return ListaKit;
        }
        public List<Kit> ConsultarKitPorId(int idKit)
        {
            ListaKit = new List<Kit>();
            foreach (var item in ConexionBD.sp_ConsultarKitPorId(idKit))
            {
                ListaKit.Add(new Kit()
                {
                    IdKit = Seguridad.Encriptar(item.IdKit.ToString()),
                    Codigo = item.Codigo,
                    Descripcion = item.Descripcion,
                    FechaCreacion = item.FechaCreacion,
                    FechaActualizacion = item.FechaActualizacion,
                    Estado = item.Estado,
                    KitUtilizado = item.KitUtilizado,
                    AsignarDescuentoKit = new AsignarDescuentoKit()
                    {
                        IdAsignarDescuentoKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdAsignarDescuentoKit.ToString()),
                        IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                        IdKit = Seguridad.Encriptar(item.AsignarDescuentoKitIdKit.ToString()),
                        FechaCreacion = item.AsignarDescuentoKitFechaCreacion,
                        FechaActualizacion = item.AsignarDescuentoKitFechaModificacion,
                        Estado = item.AsignarDescuentoKitEstado,
                        Descuento = new Descuento()
                        {
                            IdDescuento = Seguridad.Encriptar(item.AsignarDescuentoKitIdDescuento.ToString()),
                            Porcentaje = item.DescuentoPorcentaje,
                            Estado = item.DescuentoEstado,
                        }
                    },
                });
            }
            return ListaKit;
        }

        public List<Descuento> ConsultarDescuento(int? Descuento)
        {
            List<Descuento> ListaDescuento = new List<Descuento>();
            foreach (var item in ConexionBD.sp_ConsultarDescuento().Where(p=>p.Porcentaje == Descuento).ToList())
            {
                ListaDescuento.Add(new Descuento()
                {
                    IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString()),
                    Porcentaje = item.Porcentaje,
                    Estado = item.Estado,
                    DescuentoUtilizado = item.DescuentoUtilizado
                });
            }
            return ListaDescuento;
        }
        public Descuento InsertarDescuento(Descuento Descuento)
        {
            foreach (var item in ConexionBD.sp_CrearDescuento(Descuento.Porcentaje))
            {
                Descuento.IdDescuento = Seguridad.Encriptar(item.IdDescuento.ToString());
                Descuento.Porcentaje = item.Porcentaje;
            }
            return Descuento;
        }
    }
}
