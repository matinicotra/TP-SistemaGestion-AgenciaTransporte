﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ClienteNegocio
    {
        private List<Cliente> listaClientes = new List<Cliente>();

        //----------------METODOS--------------------------
        public List<Cliente> ObtenerDatos(int idCliente = -1)
        {
            AccesoDatos datosCliente = new AccesoDatos();
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                if(idCliente == -1)
                {
                    datosCliente.SetearConsulta("SELECT IDCLIENTE, IDPERSONA, IDZONA, ESTADO FROM CLIENTE");
                }
                else
                {
                    datosCliente.SetearConsulta("SELECT IDCLIENTE, IDPERSONA, IDZONA, ESTADO FROM CLIENTE WHERE IDCLIENTE = @IDCLIENTE");
                    
                    datosCliente.SetearParametro("@IDCLIENTE", idCliente);
                }
                
                datosCliente.EjecutarConsulta();

                while (datosCliente.Lector.Read())
                {
                    Cliente clienteAux = new Cliente();
                    Persona personaAux = new Persona();
                    PersonaNegocio personaNegocioAux = new PersonaNegocio();

                    personaAux = personaNegocioAux.ObtenerPersona((int)datosCliente.Lector["IDPERSONA"]);

                    clienteAux.Nombres = personaAux.Nombres;
                    clienteAux.Apellidos = personaAux.Apellidos;
                    clienteAux.DNI = personaAux.DNI;
                    clienteAux.FechaNacimiento = personaAux.FechaNacimiento;
                    clienteAux.Direccion = personaAux.Direccion;
                    clienteAux.Nacionalidad = personaAux.Nacionalidad;
                    clienteAux.IDPersona = personaAux.IDPersona;
                    clienteAux.Email = personaAux.Email;
                    clienteAux.Telefono = personaAux.Telefono;

                    clienteAux.Estado = datosCliente.Lector["ESTADO"] is DBNull ? false : (bool)datosCliente.Lector["ESTADO"];
                    clienteAux.IDCliente = datosCliente.Lector["IDCLIENTE"] is DBNull? -1 : (int)datosCliente.Lector["IDCLIENTE"];
                    personaAux.IDPersona = datosCliente.Lector["IDPERSONA"] is DBNull? -1 : (int)datosCliente.Lector["IDPERSONA"];

                        //obtiene la zona
                    ChoferNegocio cnAux = new ChoferNegocio();

                    Zona zonaAux = ZonaNegocio.ObtenerZonas((int)datosCliente.Lector["IDZONA"])[0];
                    
                    clienteAux.zonaCliente = zonaAux;

                    listaClientes.Add(clienteAux);
                }

                return listaClientes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosCliente.CerrarConexion();
            }
        }
        public void AltaModificacionCliente(Cliente clienteAux, bool esAlta)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (!esAlta)
                {
                    datos.SetearConsulta("UPDATE DOMICILIO SET DIRECCION = @DIRECCION, LOCALIDAD = @LOCALIDAD, PROVINCIA = @PROVINCIA, DESCRIPCION = @DESCRIPCION WHERE IDDOMICILIO = @IDDOMICILIO");
                    
                    datos.SetearParametro("@DIRECCION", clienteAux.Direccion.Direccion);
                    datos.SetearParametro("@LOCALIDAD", clienteAux.Direccion.Localidad);
                    datos.SetearParametro("@PROVINCIA", clienteAux.Direccion.Provincia);
                    datos.SetearParametro("@DESCRIPCION", clienteAux.Direccion.Descripcion);
                    datos.SetearParametro("@IDDOMICILIO", clienteAux.Direccion.IDDomicilio);
                    
                    datos.EjecutarAccion();
                    datos.CerrarConexion();
                    
                    datos.SetearConsulta("UPDATE PERSONA SET NOMBRES = @NOMBRES, APELLIDOS = @APELLIDOS, DNI = @DNI, FECHANACIMIENTO = @FECHANACIMIENTO, DOMICILIO = @DOMICILIO, NACIONALIDAD = @NACIONALIDAD WHERE IDPERSONA = @IDPERSONA");
                    
                    datos.SetearParametro("@NOMBRES", clienteAux.Nombres);
                    datos.SetearParametro("@APELLIDOS", clienteAux.Apellidos);
                    datos.SetearParametro("@DNI", clienteAux.DNI);
                    datos.SetearParametro("@FECHANACIMIENTO", clienteAux.FechaNacimiento);
                    datos.SetearParametro("@DOMICILIO", clienteAux.Direccion.IDDomicilio);
                    datos.SetearParametro("@NACIONALIDAD", clienteAux.Nacionalidad);
                    datos.SetearParametro("@IDPERSONA", clienteAux.IDPersona);
                    
                    datos.EjecutarAccion();
                    datos.CerrarConexion();
                    
                    datos.SetearConsulta("UPDATE CLIENTE SET IDZONA = @IDZONA, IDPERSONA = @IDPERSONA1 WHERE IDCLIENTE = @IDCLIENTE");
                    
                    datos.SetearParametro("@IDZONA", clienteAux.zonaCliente.IDZona);
                    datos.SetearParametro("@IDPERSONA1", clienteAux.IDPersona);
                    datos.SetearParametro("@IDCLIENTE", clienteAux.IDCliente);

                }
                else
                {
                    datos.SetearConsulta("INSERT INTO DOMICILIO (DIRECCION, LOCALIDAD, PROVINCIA, DESCRIPCION) VALUES (@DIRECCION, @LOCALIDAD, @PROVINCIA, @DESCRIPCION)");
                    datos.SetearParametro("@DIRECCION", clienteAux.Direccion.Direccion);
                    datos.SetearParametro("@LOCALIDAD", clienteAux.Direccion.Localidad);
                    datos.SetearParametro("@PROVINCIA", clienteAux.Direccion.Provincia);
                    datos.SetearParametro("@DESCRIPCION", clienteAux.Direccion.Descripcion);
                    
                    datos.EjecutarAccion();
                    datos.CerrarConexion();

                    DomicilioNegocio domicilioNegocioAux = new DomicilioNegocio();
                    
                    long idDomicilio = domicilioNegocioAux.ultimoIdDomicilio();//obtiene el ultimo id de domicilio
                    
                    datos.SetearConsulta("INSERT INTO PERSONA (NOMBRES, APELLIDOS, DNI, FECHANACIMIENTO, DOMICILIO, NACIONALIDAD) VALUES (@NOMBRES, @APELLIDOS, @DNI, @FECHANACIMIENTO, @IDDOMICILIO, @NACIONALIDAD)");
                    
                    datos.SetearParametro("@NOMBRES", clienteAux.Nombres);
                    datos.SetearParametro("@APELLIDOS", clienteAux.Apellidos);
                    datos.SetearParametro("@DNI", clienteAux.DNI);
                    datos.SetearParametro("@FECHANACIMIENTO", clienteAux.FechaNacimiento);
                    datos.SetearParametro("@IDDOMICILIO", idDomicilio);//setea el idDomicilio recien insertado
                    datos.SetearParametro("@NACIONALIDAD", clienteAux.Nacionalidad);
                    
                    datos.EjecutarAccion();
                    datos.CerrarConexion();

                    PersonaNegocio personaNegocioAux = new PersonaNegocio();
                    
                    int idPersona = personaNegocioAux.ultimoIdPersona();//obtiene el ultimo id de persona
                    
                    datos.SetearConsulta("INSERT INTO CLIENTE (IDPERSONA, IDZONA) VALUES (@IDPERSONA, @IDZONA)");
                    
                    datos.SetearParametro("@IDPERSONA", idPersona);//setea el  idPersona recien insertado
                    datos.SetearParametro("@IDZONA", clienteAux.zonaCliente.IDZona);
                }

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void BajaLogicaCliente(int idCliente)
        {
            AccesoDatos Datos = new AccesoDatos();
            try
            {
                Datos.SetearConsulta("EXEC SP_BAJALOGICACLIENTE @IDCLIENTE");
                
                Datos.SetearParametro("@IDCLIENTE", idCliente);
                
                Datos.EjecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Datos.CerrarConexion();
            }
        }
        public int ultimoIdCliente()
        {
            int idCliente = -1;

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT TOP 1 * FROM CLIENTE ORDER BY IDCLIENTE DESC");

                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    idCliente = datos.Lector["IDCLIENTE"] is DBNull ? -1 : (int)datos.Lector["IDCLIENTE"];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }

            return idCliente;
        }
        public List<Cliente> Filtrar(string campo, string buscar)
        {
            List<Cliente> lista = new List<Cliente>();
            AccesoDatos datosCliente = new AccesoDatos();

            string consulta = "SELECT C.IDCLIENTE, C.IDPERSONA, C.IDZONA, C.ESTADO FROM CLIENTE AS C  ";

            try
            {
                if (campo == "NOMBRE")
                {
                    consulta += "INNER JOIN PERSONA AS P ON C.IDPERSONA = P.IDPERSONA ";
                    consulta += "WHERE UPPER(P.NOMBRES) LIKE '%" + buscar.ToUpper() + "%'";
                }
                else if (campo == "APELLIDO")
                {
                    consulta += "INNER JOIN PERSONA AS P ON C.IDPERSONA = P.IDPERSONA ";
                    consulta += "WHERE UPPER(P.APELLIDOS) LIKE '%" + buscar.ToUpper() + "%'";
                }
                else if (campo == "ZONA")
                {
                    consulta += "INNER JOIN ZONAS AS Z ON C.IDZONA = Z.IDZONA ";
                    consulta += "WHERE UPPER(Z.NOMBREZONA) LIKE '%" + buscar.ToUpper() + "%'";
                }

                datosCliente.SetearConsulta(consulta);
                datosCliente.EjecutarConsulta();

                while (datosCliente.Lector.Read())
                {
                    Cliente clienteAux = new Cliente();
                    Persona personaAux = new Persona();
                    PersonaNegocio personaNegocioAux = new PersonaNegocio();

                    personaAux = personaNegocioAux.ObtenerPersona((int)datosCliente.Lector["IDPERSONA"]);

                    clienteAux.Nombres = personaAux.Nombres;
                    clienteAux.Apellidos = personaAux.Apellidos;
                    clienteAux.DNI = personaAux.DNI;
                    clienteAux.FechaNacimiento = personaAux.FechaNacimiento;
                    clienteAux.Direccion = personaAux.Direccion;
                    clienteAux.Nacionalidad = personaAux.Nacionalidad;
                    clienteAux.IDPersona = personaAux.IDPersona;
                    clienteAux.Email = personaAux.Email;
                    clienteAux.Telefono = personaAux.Telefono;

                    clienteAux.Estado = datosCliente.Lector["ESTADO"] is DBNull ? false : (bool)datosCliente.Lector["ESTADO"];
                    clienteAux.IDCliente = datosCliente.Lector["IDCLIENTE"] is DBNull ? -1 : (int)datosCliente.Lector["IDCLIENTE"];
                    personaAux.IDPersona = datosCliente.Lector["IDCLIENTE"] is DBNull ? -1 : (int)datosCliente.Lector["IDPERSONA"];

                    //obtiene la zona
                    ChoferNegocio cnAux = new ChoferNegocio();

                    Zona zonaAux = ZonaNegocio.ObtenerZonas((int)datosCliente.Lector["IDZONA"])[0];

                    clienteAux.zonaCliente = zonaAux;

                    lista.Add(clienteAux);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosCliente.CerrarConexion();
            }

            return lista;
        }
    }
}
