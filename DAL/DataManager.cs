using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
            int id = GetLastId(tableName);
            data.Add(IdColumn, id);

            if (ModeOfPersistible.XML.Equals(modeToSave))
            {
                dataXml.Write(tableName, data);
                DATA_SET = dataXml.ReadAll();
            }
            else 
            { 
                DataDisconnected.Write(tableName, data, IdColumn); 
            }

            return id;
        }

        public bool delete(string tableName, int id, string idColumn)
        {
            bool status = false;

            if (ModeOfPersistible.XML.Equals(modeToSave))
            {
                dataXml.DeleteById(tableName, idColumn, id);
                DATA_SET = dataXml.ReadAll();
                status = true; 
            }
            else
            {
                DataDisconnected.delete(tableName, id, idColumn);
                status = true;
            }
            return  status;
        }

        public void update(string tableName, string idColumn, int id, Hashtable data)
        {
            switch (modeToSave)
            {
                case ModeOfPersistible.XML:
                    dataXml.Update(tableName, idColumn, id, data);
                    DATA_SET = dataXml.ReadAll();
                    break;
                case ModeOfPersistible.DISCONECTED:
                    DataDisconnected.update(tableName, id, idColumn, data);
                    break;

            }
        }

        private int GetLastId(string tableName)
        {
            int id = 1;
            DataTable table = DATA_SET.Tables[tableName];

            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    id = Convert.ToInt32(table.Rows[table.Rows.Count - 1][0]);
                    id++;
                }
            }


            return id;
        }

        public bool SyncDataDB() 
        {
            bool status = false;

                try
                {
                    for (int i = 0; i < DATA_SET.Tables.Count; i++)
                    {
                        DataDisconnected.UpdateDB(DATA_SET.Tables[i]);
                    }
                    status = true;
                }
                catch (Exception e)
                { }

            return status;
        }

    }
}
