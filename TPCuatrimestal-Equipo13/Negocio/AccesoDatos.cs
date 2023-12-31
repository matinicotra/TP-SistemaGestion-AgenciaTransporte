﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    internal class AccesoDatos
    {   
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
       
        public SqlDataReader Lector { get { return lector; } }

        //----------------CONSTRUCTOR--------------------------
        public AccesoDatos()
        {
            conexion = new SqlConnection();
            comando = new SqlCommand();
            comando.CommandType = System.Data.CommandType.Text;
            conexion.ConnectionString = "server= .\\SQLEXPRESS; database=BBDD_Equipo13; integrated security=true";
        }

        //----------------METODOS--------------------------

        public void SetearConsulta(string consulta)
        {
            comando.CommandText = consulta;
        }

        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void EjecutarConsulta()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EjecutarAccion()
        {
            try
            {
                comando.Connection = conexion;

                conexion.Open();

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CerrarConexion()
        {
            if (!(lector == null)) lector.Close();

            conexion.Close();
        }
    }
}
