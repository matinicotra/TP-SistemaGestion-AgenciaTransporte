﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class DomicilioNegocio
    {
        //----------------METODOS--------------------------
        public Domicilio ObtenerDomicilio(long IdDomicilio)
        {
            Domicilio domicilioAux = new Domicilio();
            AccesoDatos datosDomicilio = new AccesoDatos();
            try
            {
                datosDomicilio.SetearConsulta("SELECT IDDOMICILIO, DIRECCION, LOCALIDAD, PROVINCIA, DESCRIPCION FROM DOMICILIO WHERE IDDOMICILIO = @IDDOMICILIO");
                datosDomicilio.SetearParametro("@IDDOMICILIO", IdDomicilio);
                datosDomicilio.EjecutarConsulta();

                if (datosDomicilio.Lector.Read()) //si hay registro lo lee y setea
                {
                    domicilioAux.IDDomicilio = IdDomicilio;
                    domicilioAux.Direccion = datosDomicilio.Lector["DIRECCION"] is DBNull ? "S/D" : (string)datosDomicilio.Lector["DIRECCION"];
                    domicilioAux.Localidad = datosDomicilio.Lector["LOCALIDAD"] is DBNull ? "S/L" : (string)datosDomicilio.Lector["LOCALIDAD"];
                    domicilioAux.Provincia = datosDomicilio.Lector["PROVINCIA"] is DBNull ? "S/P" : (string)datosDomicilio.Lector["PROVINCIA"];
                    domicilioAux.Descripcion = datosDomicilio.Lector["DESCRIPCION"] is DBNull ? "S/D" : (string)datosDomicilio.Lector["DESCRIPCION"];

                    return domicilioAux;
                }
                else //si no hay registros que leer setea -1 al IdPersona
                {
                    domicilioAux.IDDomicilio = -1;

                    return domicilioAux;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosDomicilio.CerrarConexion();
            }
        }

        public void BajaDomicilio(long IdDomicilio)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("DELETE FROM DOMICILIO WHERE IDDOMICILIO = @IDDOMICILIO");

                datos.SetearParametro("@IDDOMICILIO", IdDomicilio);

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

        public void AltaModificacionDomicilio(Domicilio domiAux, bool esAlta)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (!esAlta)
                {
                    datos.SetearConsulta("UPDATE DOMICILIO SET DIRECCION = @DIRECCION, LOCALIDAD = @LOCALIDAD, PROVINCIA = @PROVINCIA, DESCRIPCION = @DESCRIPCION WHERE IDDOMICILIO = @IDDOMICILIO");

                    datos.SetearParametro("@DIRECCION", domiAux.Direccion);
                    datos.SetearParametro("@LOCALIDAD", domiAux.Localidad);
                    datos.SetearParametro("@PROVINCIA", domiAux.Provincia);
                    datos.SetearParametro("@DESCRIPCION", domiAux.Descripcion);
                    datos.SetearParametro("@IDDOMICILIO", domiAux.IDDomicilio);
                }
                else
                {
                    datos.SetearConsulta("INSERT INTO DOMICILIO (DIRECCION, LOCALIDAD, PROVINCIA, DESCRIPCION) VALUES (@DIRECCION, @LOCALIDAD, @PROVINCIA, @DESCRIPCION)");

                    datos.SetearParametro("@DIRECCION", domiAux.Direccion);
                    datos.SetearParametro("@LOCALIDAD", domiAux.Localidad);
                    datos.SetearParametro("@PROVINCIA", domiAux.Provincia);
                    datos.SetearParametro("@DESCRIPCION", domiAux.Descripcion);
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

        public long ultimoIdDomicilio()
        {
            long idDomicilio = 0;

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT TOP 1 * FROM DOMICILIO ORDER BY IDDOMICILIO DESC");

                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    idDomicilio = datos.Lector["IDDOMICILIO"] is DBNull ? -1 : (long)datos.Lector["IDDOMICILIO"];
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.CerrarConexion();
            }

            return idDomicilio;
        }

        public long existeDomicilio(Domicilio domicilio)
        {
            long domicilioNumero = -1;

            AccesoDatos datos = new AccesoDatos();

            try
            {
                //COMPARA LA DIRECCION Y LOCALIDAD, SI COINCIDEN DEVUELVE EL ID DEL DOMICILIO
                //SI NO ENCUENTRA NADA DEVUELVE -1
                datos.SetearConsulta("SELECT * FROM DOMICILIO WHERE DIRECCION LIKE @DIRECCION AND LOCALIDAD LIKE @LOCALIDAD AND PROVINCIA LIKE @PROVINCIA AND DESCRIPCION LIKE @DESCRIPCION");
                datos.SetearParametro("@DIRECCION", domicilio.Direccion);
                datos.SetearParametro("@LOCALIDAD", domicilio.Localidad);
                datos.SetearParametro("@PROVINCIA", domicilio.Provincia);
                datos.SetearParametro("@DESCRIPCION", domicilio.Descripcion);
                datos.EjecutarConsulta();

                if (datos.Lector.Read())
                {
                    domicilioNumero = datos.Lector["IDDOMICILIO"] is DBNull ? -1 : (long)datos.Lector["IDDOMICILIO"];
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

            return domicilioNumero;
        }
    }
}


