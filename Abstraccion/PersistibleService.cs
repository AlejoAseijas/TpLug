using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{
    public class PersistibleService
    {
        private DataManager dataManager = DataManager.GetInstance();

        public DataTable getTable(string tableName)
        {
            return dataManager.GetTableByName(tableName);
        }
    }
}
