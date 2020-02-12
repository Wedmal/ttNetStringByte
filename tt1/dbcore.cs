using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;

namespace tt1
{

    public class dbcore
    {

        public Dictionary<int, string> countries = new Dictionary<int, string>(2)
    {
            { 1, "SqLite"},
            {2, "PgSql"},
    };

        private String dbFileName;
        private SQLiteConnection m_dbConn;
        private SQLiteCommand m_sqlCmd;

        /// <summary>
        /// Думаю, этот класс будет заниматься выполнением запросов к БД. К разным бд. 
        /// </summary>
        public dbcore()
        {
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();

            dbFileName = "sample.sqlite";

            if (!File.Exists(dbFileName))
                SQLiteConnection.CreateFile(dbFileName);

            try
            {
                m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
                m_dbConn.Open();
                m_sqlCmd.Connection = m_dbConn;

                m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";
                m_sqlCmd.ExecuteNonQuery();

                //lbStatusText.Text = "Connected";
            }
            catch (SQLiteException ex)
            {
                //lbStatusText.Text = "Disconnected";
                //MessageBox.Show("Error: " + ex.Message);
            }

            //read_all

        }
    }
}

