using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace EasyAdo.Builder
{
    /// <summary>
    /// Classe auxiliar para criação de objetos DbParameter
    /// </summary>
    public class DbParameterBuilder
    {
        #region Internal members
        
        private readonly IDbCommand _command;
        private List<DbParameter> _parameters;

        #endregion

        #region Ctor

        /// <summary>
        /// Construtor padrão com instância do objeto IDbCommand
        /// </summary>
        /// <param name="command">Intância do objeto IDbCommand</param>
        public DbParameterBuilder(IDbCommand command)
        {
            _command = command;
            _parameters = new List<DbParameter>();
        }

        #endregion

        #region Fields

        /// <summary>
        /// Lista contendo objetos DbParameter
        /// </summary>
        public List<DbParameter> Parameters { get { return _parameters; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Método de inserção de novo parâmentro
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="value">Valor do parâmetro</param>
        /// <returns>Retorna o próprio objeto atualizado com os parâmetros</returns>
        public DbParameterBuilder Add(string name, object value)
        {
            _parameters.Add(CreateParameter(name, value));
            return this;
        }

        /// <summary>
        /// Método de inserção de novo parâmentro
        /// </summary>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="value">Valor do parâmetro</param>
        /// <param name="direction">Direção do campo em relação ao DataSet</param>
        /// <param name="dbType">Tipo do campo</param>
        /// <param name="size">Tamanho do campo</param>
        /// <returns>Retorna o próprio objeto atualizado com os parâmetros</returns>
        public DbParameterBuilder Add(string name, object value, ParameterDirection direction, DbType dbType, int size)
        {
            _parameters.Add(CreateParameter(name, value, direction, dbType, size));
            return this;
        }

        /// <summary>
        /// Método de inserção de novo parâmentro com base no objeto
        /// </summary>
        /// <param name="instance">Objeto a ser refletido</param>
        /// <param name="caracterParameter">Caracter de parâmetro</param>
        /// <returns>Retorna o próprio objeto atualizado com os parâmetros</returns>
        public DbParameterBuilder Add(object instance, char caracterParameter = '@')
        {
            _parameters = CreateParameter(instance, caracterParameter);
            return this;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Cria parâmetro.
        /// </summary>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <returns>Objeto com base nos dados informados</returns>
        private DbParameter CreateParameter(string name, object value)
        {
            var parameter = (DbParameter) _command.CreateParameter();
            if (parameter == null) throw new NullReferenceException("Falha ao criar objeto DbParameter.");

            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }

        /// <summary>
        /// Cria parâmetro.
        /// </summary>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="direction"></param>
        /// <param name="dbType">Tipo do dado.</param>
        /// <param name="size">Tamanho máximo do dados.</param>
        /// <returns>Objeto com base nos dados informados</returns>
        private DbParameter CreateParameter(string name, object value, ParameterDirection direction, DbType dbType, int size)
        {
            DbParameter parameter = CreateParameter(name, value);
            parameter.Direction = direction;
            parameter.DbType = dbType;
            parameter.Size = size;

            return parameter;
        }

        /// <summary>
        /// Cria parâmetros de acordo com as propriedades do objeto.
        /// </summary>
        /// <param name="instance">Instância do objeto.</param>
        /// <param name="caracterParameter">Caracter de parâmetro</param>
        /// <returns>Uma coleção de DbParameter.</returns>
        private List<DbParameter> CreateParameter(object instance, char caracterParameter)
        {
            PropertyInfo[] properties = instance.GetType().GetProperties();

            return properties.Select(propertyInfo => CreateParameter(string.Format("{0}{1}", caracterParameter, propertyInfo.Name), propertyInfo.GetValue(instance))).ToList();
        }

        #endregion
    }
}