using System;
using System.Collections;
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

        private DataXml dataXml = new DataXml();
        
        private bool modeXML = false;

        private DataManager() 
        {
            //Segun el modo cargo su dataSet.
            if (modeXML) 
            {
                DATA_SET = dataXml.ReadAll();
            }
            else 
            {
                DATA_SET = DataDisconnected.ReadAll();
            }   
        }

        public static DataManager GetInstance()
        {
            return instance = instance == null ? new DataManager() : instance;
        }

        //CRUD 
        public DataTable GetAll(string tableName) 
        {
            return DATA_SET.Tables[tableName];
        }

        public int save(string tableName, Hashtable data, string IdColumn)
        {
         
            return modeXML ? dataXml.Write(tableName, data) : DataDisconnected.Write(tableName, data, IdColumn);

        }

        public bool delete(string tableName, int id, string idColumn)
        {
            return modeXML ? dataXml.DeleteById(tableName, idColumn, id) : DataDisconnected.delete(tableName, id, idColumn); ;
        }

        public void update(string tableName, string idColumn, int id, Hashtable data)
        {

        }

        public void SyncDataDB() 
        {
            //Evito error de mutacion 
            for (int i = 0; i < DATA_SET.Tables.Count; i++)
            {
                DataDisconnected.UpdateDB(DATA_SET.Tables[i]);
            }
        }

    }
}
