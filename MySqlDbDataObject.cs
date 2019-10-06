﻿using ag.DbData.Abstraction;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ag.DbData.MySql
{
    /// <summary>
    /// Represents MySqlDbDataObject object.
    /// </summary>
    public class MySqlDbDataObject : DbDataObject
    {
        #region ctor
        /// <summary>
        /// Creates new instance of <see cref="MySqlDbDataObject"/>.
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> object.</param>
        /// <param name="options"><see cref="DbDataSettings"/> options.</param>
        public MySqlDbDataObject(ILogger<IDbDataObject> logger, IOptions<DbDataSettings> options) : base(logger, options) { }
        #endregion

        #region Overrides
        /// <inheritdoc />
        public override DataSet FillDataSet(string query) => innerFillDataSet(query, null, -1, false);

        /// <inheritdoc />
        public override DataSet FillDataSet(string query, int timeout) => innerFillDataSet(query, null, timeout, false);

        /// <inheritdoc />
        public override DataSet FillDataSet(string query, IEnumerable<string> tables) => innerFillDataSet(query, tables, -1, false);

        /// <inheritdoc />
        public override DataSet FillDataSet(string query, IEnumerable<string> tables, int timeout) => innerFillDataSet(query, tables, timeout, false);

        /// <inheritdoc />
        public override DataSet FillDataSetInTransaction(string query) => innerFillDataSet(query, null, -1, true);

        /// <inheritdoc />
        public override DataSet FillDataSetInTransaction(string query, int timeout) => innerFillDataSet(query, null, timeout, true);

        /// <inheritdoc />
        public override DataSet FillDataSetInTransaction(string query, IEnumerable<string> tables) => innerFillDataSet(query, tables, -1, true);

        /// <inheritdoc />
        public override DataSet FillDataSetInTransaction(string query, IEnumerable<string> tables, int timeout) => innerFillDataSet(query, tables, timeout, true);

        /// <inheritdoc />
        public override DataTable FillDataTable(string query) => innerFillDataTable(query, -1, false);

        /// <inheritdoc />
        public override DataTable FillDataTable(string query, int timeout) => innerFillDataTable(query, timeout, false);

        /// <inheritdoc />
        public override DataTable FillDataTableInTransaction(string query) =>
            innerFillDataTable(query, -1, true);

        /// <inheritdoc />
        public override DataTable FillDataTableInTransaction(string query, int timeout) =>
            innerFillDataTable(query, timeout, true);

        /// <inheritdoc />
        public override int ExecuteCommand(DbCommand cmd) => innerExecuteCommand((MySqlCommand)cmd, -1, false);

        /// <inheritdoc />
        public override int ExecuteCommand(DbCommand cmd, int timeout) => innerExecuteCommand((MySqlCommand)cmd, timeout, false);

        /// <inheritdoc />
        public override int ExecuteCommandInTransaction(DbCommand cmd) =>
            innerExecuteCommand((MySqlCommand)cmd, -1, true);

        /// <inheritdoc />
        public override int ExecuteCommandInTransaction(DbCommand cmd, int timeout) =>
            innerExecuteCommand((MySqlCommand)cmd, timeout, true);

        /// <inheritdoc />
        public override bool BeginTransaction(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    return false;
                if (TransConnection == null)
                {
                    TransConnection = new MySqlConnection(connectionString);
                }

                if (TransConnection == null)
                {
                    return false;
                }
                if (TransConnection.State != ConnectionState.Open)
                    TransConnection.Open();
                Transaction = TransConnection.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error at BeginTransaction");
                throw new DbDataException(ex, "");
            }
        }
        #endregion

        #region private procedures
        private DataSet innerFillDataSet(string query, IEnumerable<string> tables, int timeout, bool inTransaction)
        {
            try
            {
                var dataSet = new DataSet();
                using (var cmd = new MySqlCommand(query, inTransaction
                    ? (MySqlConnection)TransConnection
                    : (MySqlConnection)Connection))
                {
                    if (inTransaction)
                        cmd.Transaction = (MySqlTransaction)Transaction;
                    if (timeout != -1)
                    {
                        if (timeout >= 0)
                            cmd.CommandTimeout = timeout;
                        else
                            throw new ArgumentException("Invalid CommandTimeout value", nameof(timeout));
                    }
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dataSet);
                        if (tables == null) return dataSet;
                        var tablesArray = tables.ToArray();
                        if (tablesArray.Length <= dataSet.Tables.Count)
                            for (var i = 0; i < tablesArray.Length; i++)
                                dataSet.Tables[i].TableName = tablesArray[i];
                        else
                            for (var i = 0; i < dataSet.Tables.Count; i++)
                                dataSet.Tables[i].TableName = tablesArray[i];
                    }
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error at FillDataSet");
                throw new DbDataException(ex, query);
            }
        }

        private DataTable innerFillDataTable(string query, int timeout, bool inTransaction)
        {
            try
            {
                var table = new DataTable();
                using (var cmd = new MySqlCommand(query, inTransaction
                    ? (MySqlConnection)TransConnection
                    : (MySqlConnection)Connection))
                {
                    if (inTransaction)
                        cmd.Transaction = (MySqlTransaction)Transaction;
                    if (timeout != -1)
                    {
                        if (timeout >= 0)
                            cmd.CommandTimeout = timeout;
                        else
                            throw new ArgumentException("Invalid CommandTimeout value", nameof(timeout));
                    }
                    using (var da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }
                }
                return table;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error at FillDataTable");
                throw new DbDataException(ex, query);
            }
        }

        private int innerExecuteCommand(MySqlCommand cmd, int timeout, bool inTransaction)
        {
            try
            {
                if (inTransaction)
                {
                    cmd.Connection = (MySqlConnection)TransConnection;
                    cmd.Transaction = (MySqlTransaction)Transaction;
                }
                else
                {
                    cmd.Connection = (MySqlConnection)Connection;
                    Connection.Open();
                }
                if (timeout != -1)
                {
                    if (timeout >= 0)
                        cmd.CommandTimeout = timeout;
                    else
                        throw new ArgumentException("Invalid CommandTimeout value", nameof(timeout));
                }
                var rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, "Error at ExecuteCommand");
                throw new DbDataException(ex, cmd.CommandText);
            }
            finally
            {
                if (inTransaction && Connection.State == ConnectionState.Open)
                    Connection.Close();
            }
        }
        #endregion
    }
}
