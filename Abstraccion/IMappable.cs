using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraccion
{
    public interface IMappable<T>
    {
        string TABLE_NAME { get; }
        string ID_COLUMN { get; }
        T GetById(string Id);
        DataTable GetAll();
        int Create(T entity);
        void DeleteById(int Id);
        void Update(T oldData, T newData);
        T ToMap(DataRow row);

    }
}
