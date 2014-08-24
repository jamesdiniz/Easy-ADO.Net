using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace EasyAdo.Extensions
{
    /// <summary>
    /// Extensão do objeto DataReader com métodos auxiliares
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Método genérico para testar se valor é nulo e retornar valor correspondente
        /// </summary>
        /// <typeparam name="T">Tipo de dado a ser retornado</typeparam>
        /// <param name="dr">Instância do tipo IDataReader a ser usado</param>
        /// <param name="campo">Nome do campo a ser utilizado</param>
        /// <returns>Retorna o valor de acordo com o tipo</returns>
        public static T GetValueOrDefault<T>(this IDataReader dr, string campo) where T : IConvertible
        {
            object valueField = null;
            var indexField = dr.GetOrdinal(campo);
            var isFieldNull = dr.IsDBNull(indexField);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Int32:
                    valueField = isFieldNull ? 0 : dr.GetInt32(indexField);
                    break;
                case TypeCode.String:
                    valueField = isFieldNull ? string.Empty : dr.GetString(indexField);
                    break;
                case TypeCode.Decimal:
                    valueField = isFieldNull ? 0 : dr.GetDecimal(indexField);
                    break;
                case TypeCode.Double:
                    valueField = isFieldNull ? 0 : dr.GetDouble(indexField);
                    break;
                case TypeCode.Boolean:
                    valueField = !isFieldNull && dr.GetBoolean(indexField);
                    break;
                case TypeCode.DateTime:
                    valueField = isFieldNull ? DateTime.MinValue : dr.GetDateTime(indexField);
                    break;
            }

            return ConvertValueType<T>(valueField);
        }

        /// <summary>
        /// Converte o valor do objeto para um objeto do tipo T.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto.</typeparam>
        /// <param name="value">Valor do objeto.</param>
        /// <returns>Novo valor com o tipo especificado.</returns>
        public static T ConvertValueType<T>(object value)
        {
            return (T)(Convert.ChangeType(value, typeof(T)));
        }

        /// <summary>
        /// Converte um objeto IDataReader para uma lista de objeto do tipo T.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto.</typeparam>
        /// <param name="reader">Objeto IDataReader.</param>
        /// <returns>Lista com objetos do tipo T.</returns>
        public static IList<T> ConvertToList<T>(this IDataReader reader) where T : class
        {
            var result = new List<T>();

            while (reader.Read())
            {
                var item = ConvertToObject<T>(reader);
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Cria modelo utilizando campos do objeto IDataReader.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto.</typeparam>
        /// <param name="reader">Objeto IDataReader.</param>
        /// <returns>Objeto do tipo T.</returns>
        public static T ConvertToObject<T>(this IDataReader reader) where T : class
        {
            var instance = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (reader[propertyInfo.Name] != null)
                {
                    int ixField = reader.GetOrdinal(propertyInfo.Name);
                    if (!reader.IsDBNull(ixField))
                        propertyInfo.SetValue(instance, reader.GetValue(ixField));
                }
            }
            return instance;
        }
    }
}