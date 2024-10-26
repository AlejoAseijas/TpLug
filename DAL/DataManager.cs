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
        private enum ModeOfPersistible
        {
            XML, DISCONECTED
        }

        private DataSet DATA_SET = null;
        private static DataManager instance = null;

        private DataXml dataXml = new DataXml();
        
        private ModeOfPersistible modeToSave = ModeOfPersistible.XML;

        private DataManager() 
        {
            //Segun el modo cargo su dataSet.
            if (ModeOfPersistible.XML.Equals(modeToSave)) 
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
         
            return ModeOfPersistible.XML.Equals(modeToSave) ? dataXml.Write(tableName, data) : DataDisconnected.Write(tableName, data, IdColumn);

        }

        public bool delete(string tableName, int id, string idColumn)
        {
            return ModeOfPersistible.XML.Equals(modeToSave) ? dataXml.DeleteById(tableName, idColumn, id) : DataDisconnected.delete(tableName, id, idColumn); ;
        }

        public void update(string tableName, string idColumn, int id, Hashtable data)
        {
            switch (modeToSave)
            {
                case ModeOfPersistible.XML:

                    break;
                case ModeOfPersistible.DISCONECTED:
                    break;

            }
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
