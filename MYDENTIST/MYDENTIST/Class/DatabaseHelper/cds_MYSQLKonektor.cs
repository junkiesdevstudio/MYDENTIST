using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace MYDENTIST.Class.DatabaseHelper
{
    public class cds_MYSQLKonektor : cds_BaseKonektor, IDisposable
    {
        private IsolationLevel isolationLevel = IsolationLevel.Unspecified;
        private MySqlConnection mysqlConnection = null;
        private MySqlCommand mysqlCommand = null;
        private MySqlTransaction mysqlTransaction = null;
        private bool isTransaction = true;
        private bool disposed = false;

        public cds_MYSQLKonektor(cds_KoneksiString options, bool isTransaction = true, IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {


                this.isolationLevel = isolationLevel;
                this.isTransaction = isTransaction;
                base.SetKoneksiString(options);
                mysqlConnection = new MySqlConnection(base.koneksiString);
                if (!base.OpenKoneksi(mysqlConnection, 10)) throw new Exception("Unable to connect");
                if (isTransaction) mysqlTransaction = mysqlConnection.BeginTransaction(this.isolationLevel);
                mysqlCommand = mysqlConnection.CreateCommand();
                mysqlCommand.Connection = mysqlConnection;


        }

        public cds_MYSQLKonektor(string connectionString, bool isTransaction = true, IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            try
            {

                

                this.isolationLevel = isolationLevel;
                this.isTransaction = isTransaction;
                base.SetKoneksiString(connectionString);
                mysqlConnection = new MySqlConnection(base.koneksiString);


                if (!base.OpenKoneksi(mysqlConnection, 10)) throw new Exception("Unable to connect");
                if (isTransaction) mysqlTransaction = mysqlConnection.BeginTransaction(this.isolationLevel);
                mysqlCommand = mysqlConnection.CreateCommand();
                mysqlCommand.Connection = mysqlConnection;

            }
            catch (MySqlException ex)
            {
                MessageBox.Show("A");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    try
                    {
                        if (mysqlTransaction != null) mysqlTransaction.Dispose();
                        if (mysqlCommand != null) mysqlCommand.Dispose();
                        if (mysqlConnection != null)
                            mysqlConnection.Dispose();
                    }
                    catch (Exception ex)
                    {
                        base.DebugOutput("Dispose", ex.ToString());
                    }

                this.disposed = true;
            }
        }

        ~cds_MYSQLKonektor()
        {
            Dispose(false);
        }

        public void Commit(bool respring = false)
        {
            mysqlTransaction.Commit();

            if (respring)
                ReinitializeConnection();
        }

        public void Rollback(bool respring = false)
        {
            mysqlTransaction.Rollback();

            if (respring)
                ReinitializeConnection();
        }

        private void ReinitializeConnection()
        {
            Dispose(true);

            mysqlConnection = new MySqlConnection(base.koneksiString);
            if (!base.OpenKoneksi(mysqlConnection, 10)) throw new Exception("Unable to connect");
            mysqlTransaction = mysqlConnection.BeginTransaction(this.isolationLevel);
            mysqlCommand = mysqlConnection.CreateCommand();
            mysqlCommand.Connection = mysqlConnection;
        }

        public override long InsertRow(string database, string table, bool onDuplicateUpdate, params ParameterData[] parameterData)
        {
            return base.InsertRow(this.mysqlCommand, database, table, onDuplicateUpdate, parameterData);
        }

        public override long InsertRowGeneric<T>(string database, string table, bool onDuplicateUpdate, T data)
        {
            return base.InsertRowGeneric<T>(this.mysqlCommand, database, table, onDuplicateUpdate, data);
        }

        public override void GetRowGeneric<T>(string query, T t, params ParameterData[] parameterData)
        {
            base.GetRowGeneric<T>(this.mysqlCommand, query, t, false, parameterData);
        }

        public override void GetRowGenericParse<T>(string query, T t, params ParameterData[] parameterData)
        {
            base.GetRowGeneric<T>(this.mysqlCommand, query, t, true, parameterData);
        }

        public override long UpdateRow(string database, string table, string where, int limit, params ParameterData[] parameterData)
        {
            return base.UpdateRow(this.mysqlCommand, database, table, where, limit, parameterData);
        }

        public override int SendQuery(string query, params ParameterData[] parameterData)
        {
            return base.SendQuery(this.mysqlCommand, query, parameterData);
        }

        public override object GetObject(string query, params ParameterData[] parameterData)
        {
            return base.GetObject(this.mysqlCommand, query, parameterData);
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

        public override DataTable GetDataTable(string query, params ParameterData[] parameterData)
        {
            return base.GetDataTable(mysqlCommand, query, parameterData);
        }

        public object[,] GetDataTableAsObjectArray2d(string query, params ParameterData[] parameterData)
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

        public override IEnumerable<T> GetIEnumerable<T>(string query, params ParameterData[] parameterData)
        {
            return base.GetIEnumerable<T>(mysqlCommand, query, false, parameterData);
        }

        public override IEnumerable<T> GetIEnumerableParse<T>(string query, params ParameterData[] parameterData)
        {
            return base.GetIEnumerable<T>(mysqlCommand, query, true, parameterData);
        }

        public override IDictionary<Y, T> GetIDictionary<Y, T>(string keyColumn, string query, bool parseKey, params ParameterData[] parameterData)
        {
            return base.GetIDictionary<Y, T>(mysqlCommand, keyColumn, query, parseKey, parameterData);
        }

        public override IEnumerable<T> GetColumn<T>(string query, int column, bool parse, params ParameterData[] parameterData)
        {
            return base.GetColumn<T>(mysqlCommand, query, column, parse, parameterData);
        }

        public override IEnumerable<T> GetColumn<T>(string query, string column, bool parse, params ParameterData[] parameterData)
        {
            return base.GetColumn<T>(mysqlCommand, query, column, parse, parameterData);
        }

        public override long BulkSend(string database, string table, string column, IEnumerable<object> listData, bool onDuplicateUpdate, int updateBatchSize = 1000, bool continueUpdateOnError = false)
        {
            return base.BulkSend(this.mysqlCommand, database, table, column, listData, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);
        }

        public override long BulkSend(string database, string table, DataTable dataTable, bool onDuplicateUpdate, int updateBatchSize = 100, bool continueUpdateOnError = false)
        {
            return base.BulkSend(this.mysqlCommand, database, table, dataTable, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);
        }

        public override long BulkSendGeneric<T>(string database, string table, IEnumerable<T> listData, bool onDuplicateUpdate, int updateBatchSize = 1000, bool continueUpdateOnError = false)
        {
            return base.BulkSendGeneric(this.mysqlCommand, database, table, listData, onDuplicateUpdate, updateBatchSize, continueUpdateOnError);
        }
    }
}
