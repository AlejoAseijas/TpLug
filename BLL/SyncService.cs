using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SyncService
    {
        private DataManager dataManager = DataManager.GetInstance();

        public void sync() 
        {
            dataManager.SyncDataDB();
        }
    }
}
