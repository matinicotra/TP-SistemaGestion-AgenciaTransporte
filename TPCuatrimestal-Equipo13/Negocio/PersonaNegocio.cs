﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class PersonaNegocio
    {
        public Persona ObtenerPersona(int IdPersona)
        {
            AccesoDatos datosPersona = new AccesoDatos();
            Persona personaAux = new Persona();

            try
            {
                datosPersona.SetearConsulta("SELECT IDPERSONA, NOMBRES, APELLIDOS, DNI, FECHANACIMIENTO, DOMICILIO, NACIONALIDAD FROM PERSONA WHERE IDPERSONA = @IDPERSONA");
                datosPersona.SetearParametro("@IDPERSONA", IdPersona);
                datosPersona.EjecutarConsulta();

                if (datosPersona.Lector.Read()) //si hay registro lo lee y setea
                {
                    personaAux.IDPersona = IdPersona;
                    personaAux.Nombres = (string)datosPersona.Lector["NOMBRES"];
                    personaAux.Apellidos = (string)datosPersona.Lector["APELLIDOS"];
                    personaAux.DNI = datosPersona.Lector["DNI"] is DBNull ? "S/D" : (string)datosPersona.Lector["DNI"];
                    personaAux.Nacionalidad = datosPersona.Lector["NACIONALIDAD"] is DBNull ? "S/N" : (string)datosPersona.Lector["NACIONALIDAD"];
                    personaAux.FechaNacimiento = (DateTime)datosPersona.Lector["FECHANACIMIENTO"];


                    //lectura domicilio
                    Domicilio domicilioAux = new Domicilio();
                    DomicilioNegocio dnAux = new DomicilioNegocio();

                    long idDomicilio = (long)datosPersona.Lector["DOMICILIO"];
                    domicilioAux = dnAux.ObtenerDomicilio(idDomicilio);

                    if (domicilioAux.IDDomicilio != -1) //si no devuelve -1 tiene domicilio
                    {
                        personaAux.Direccion = domicilioAux;
                    }
                    else //si devuelve -1 NO tiene domicilio
                    {
                        personaAux.Direccion = null; //ver como manejar el null
                    }

                    return personaAux;
                }
                else //si no hay registros que leer setea -1 al IdPersona
                {
                    personaAux.IDPersona = -1;

                    return personaAux;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datosPersona.CerrarConexion();
            }
        }

        public void BajaPersona(int IdPersona)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("DELETE FROM PERSONA WHERE IDPERSONA = @IDPERSONA");
                datos.SetearParametro("@IDPERSONA", IdPersona);
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

        public void AltaModificacionPersona(Persona perAux, bool esAlta)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                if (!esAlta)
                {
                    datos.SetearConsulta("UPDATE PERSONA SET NOMBRES = @NOMBRES, APELLIDOS = @APELLIDOS, DNI = @DNI, FECHANACIMIENTO = @FECHANACIMIENTO, DOMICILIO = @DOMICILIO, NACIONALIDAD = @NACIONALIDAD WHERE IDPERSONA = @IDPERSONA");
                    datos.SetearParametro("@NOMBRES", perAux.Nombres);
                    datos.SetearParametro("@APELLIDOS", perAux.Apellidos);
                    datos.SetearParametro("@DNI", perAux.DNI);
                    datos.SetearParametro("@FECHANACIMIENTO", perAux.FechaNacimiento);
                    datos.SetearParametro("@DOMICILIO", perAux.Direccion.IDDomicilio);
                    datos.SetearParametro("@NACIONALIDAD", perAux.Nacionalidad);
                    datos.SetearParametro("@IDPERSONA", perAux.IDPersona);

                }
                else
                {
                    datos.SetearConsulta("INSERT INTO PERSONA (NOMBRES, APELLIDOS, DNI, FECHANACIMIENTO, DOMICILIO, NACIONALIDAD) VALUES (@NOMBRES, @APELLIDOS, @DNI, @FECHANACIMIENTO, @IDDOMICILIO, @NACIONALIDAD)");
                    datos.SetearParametro("@NOMBRES", perAux.Nombres);
                    datos.SetearParametro("@APELLIDOS", perAux.Apellidos);
                    datos.SetearParametro("@DNI", perAux.DNI);
                    datos.SetearParametro("@FECHANACIMIENTO", perAux.FechaNacimiento);
                    datos.SetearParametro("@IDDOMICILIO", perAux.Direccion.IDDomicilio);//setea el idDomicilio recien insertado
                    datos.SetearParametro("@NACIONALIDAD", perAux.Nacionalidad);
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

        public int ultimoIdPersona()
        {
            int idPersona = 0;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT TOP 1 IDPERSONA FROM PERSONA ORDER BY IDPERSONA DESC");
                datos.EjecutarConsulta();
                if (datos.Lector.Read())
                {
                    idPersona = (int)datos.Lector["IDPERSONA"];
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
            return idPersona;
        }
    }
}