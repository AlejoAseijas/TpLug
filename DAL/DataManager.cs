using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataManager
    {
        private DataSet DATA_SET = null;
        private static DataManager instance = null;

        private DataManager() {  }
        public static DataManager GetInstance()
        {
            return instance = instance == null ? new DataManager() : instance;
        }

        public DataSet GetDataSet(bool force) 
        {

            if(DATA_SET == null || force) 
            {
                DATA_SET = DataDisconnected.ReadAll();
            }

            return DATA_SET;
        }

        public DataSet GetDataSet()
        {
            return GetDataSet(false);
        }


        public DataTable GetTableByName(string tableName) 
        {
            DATA_SET = DATA_SET == null ? GetDataSet(true) : DATA_SET;
            return DATA_SET.Tables[tableName];
        }

        public void UpdateDataSet(DataSet data) 
        {
            if (data != null) 
            {
                DATA_SET = data;
            }
        }
    }
}
