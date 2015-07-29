using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYDENTIST.Class.DatabaseHelper
{
    public class cds_MYSQLMultiKonektor : cds_BaseKonektor
    {
        public cds_MYSQLMultiKonektor(cds_KoneksiString options)
        {
            base.SetKoneksiString(options);
        }

        public cds_MYSQLMultiKonektor(string connectionString)
        {
            base.SetKoneksiString(connectionString);
        }

        public override long InsertRow(string database, string table, bool onDuplicateUpdate, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.InsertRow(mysqlCommand, database, table, onDuplicateUpdate, parameterData);
        }

        public override long InsertRowGeneric<T>(string database, string table, bool onDuplicateUpdate, T data)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.InsertRowGeneric<T>(mysqlCommand, database, table, onDuplicateUpdate, data);

        }

        public override void GetRowGeneric<T>(string query, T t, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                base.GetRowGeneric<T>(mysqlCommand, query, t, false, parameterData);
        }

        public override void GetRowGenericParse<T>(string query, T t, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                base.GetRowGeneric<T>(mysqlCommand, query, t, true, parameterData);
        }

        public override long UpdateRow(string database, string table, string where, int limit, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.UpdateRow(mysqlCommand, database, table, where, limit, parameterData);
        }

        public override long BulkSend(string database, string table, string column, IEnumerable<object> listData, bool onDuplicateUpdate, int updateBatchSize = 1000, bool continueUpdateOnError = false)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.BulkSend(mysqlCommand, database, table, column, listData, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);
        }

        public override long BulkSend(string database, string table, DataTable dataTable, bool onDuplicateUpdate, int updateBatchSize = 1000, bool continueUpdateOnError = false)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.BulkSend(mysqlCommand, database, table, dataTable, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);

        }

        public override long BulkSendGeneric<T>(string database, string table, IEnumerable<T> listData, bool onDuplicateUpdate, int updateBatchSize = 1000, bool continueUpdateOnError = false)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.BulkSendGeneric(mysqlCommand, database, table, listData, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);
        }

        public override object GetObject(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetObject(mysqlCommand, query, parameterData);
        }

        public T GetObject<T>(string query, bool parse = false, params ParameterData[] parameterData)
        {
            if (parse)
                return Misc.Parsing.ParseObject<T>(GetObject(query, parameterData));
            else
                return (T)GetObject(query, parameterData);
        }

        public T GetObjectParse<T>(string query, params ParameterData[] parameterData)
        {
            return GetObject<T>(query, true, parameterData);
        }
        public override int SendQuery(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.SendQuery(mysqlCommand, query, parameterData);
        }

        public override DataTable GetDataTable(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetDataTable(mysqlCommand, query, parameterData);
        }

        public object[,] GetDataTableAsObjectArray2d(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
            {
                using (DataTable dt = base.GetDataTable(mysqlCommand, query, parameterData))
                {
                    object[,] returnData = new object[dt.Rows.Count, dt.Columns.Count];

                    for (int row = 0; row < dt.Rows.Count; row++)
                        for (int col = 0; col < dt.Columns.Count; col++)
                            returnData[row, col] = dt.Rows[row][col];

                    return returnData;
                }
            }
        }

        public override IEnumerable<T> GetIEnumerable<T>(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetIEnumerable<T>(mysqlCommand, query, false, parameterData);
        }

        public override IEnumerable<T> GetIEnumerableParse<T>(string query, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetIEnumerable<T>(mysqlCommand, query, true, parameterData);
        }

        public override IDictionary<Y, T> GetIDictionary<Y, T>(string keyColumn, string query, bool parseKey, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetIDictionary<Y, T>(mysqlCommand, keyColumn, query, parseKey, parameterData);
        }

        public override IEnumerable<T> GetColumn<T>(string query, int column, bool parse, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetColumn<T>(mysqlCommand, query, column, parse, parameterData);
        }

        public override IEnumerable<T> GetColumn<T>(string query, string column, bool parse, params ParameterData[] parameterData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetColumn<T>(mysqlCommand, query, column, parse, parameterData);
        }

        private MySqlConnection GetMysqlConnection()
        {
            MySqlConnection mysqlConnection = new MySqlConnection(base.koneksiString);
            if (!base.OpenKoneksi(mysqlConnection, 10)) throw new Exception("Unable to connect");
            return mysqlConnection;
        }

    }
}
