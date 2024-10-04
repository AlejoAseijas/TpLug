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
        T GetById(string Id);
        List<T> GetAll();
        int Create(T entity);
        void DeleteById(int Id);
        void Update(T docToUpdate, T newData);
        T ToMap(DataRow row);

    }
}
