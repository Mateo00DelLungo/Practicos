﻿using proyecto_Practica01_.Datos.ADO;
using proyecto_Practica01_.Datos.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyecto_Practica01_.Datos
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlTransaction _transaction;
        private SqlConnection _connection;
        private IFacturaRepository _repositorioFacturas;
        public IFacturaRepository RepositorioFacturas 
        { get 
            { if (_repositorioFacturas == null) 
                {
                    _repositorioFacturas= new FacturaRepo_ADO();
                }
                return _repositorioFacturas;
            } 
        }

        public UnitOfWork()
        {
            _connection = DataHelper.GetConnection();
            _transaction = _connection.BeginTransaction();
        }

        public void SaveChanges() 
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw new Exception("Error al guardar en la base de datos", ex);
            }
        }

        public void Dispose()
        {
            if (_transaction != null) 
            {
                _transaction.Dispose();
            }
            if(_connection != null) 
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

    }
}
