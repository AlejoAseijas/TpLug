using Abstraccion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class AbstractService<T> 
    {
        public IMappable<T> Mapper { get; set; }
        private PersistibleService persistibleService = new PersistibleService();

        public virtual int Create(T entity)
        {
            return Mapper.Create(entity);
        }

        public virtual void DeleteById(int Id)
        {
            Mapper.DeleteById(Id);
        }

        public virtual DataTable GetAll()
        {
            return persistibleService.getTable(Mapper.TABLE_NAME);
        }

        public virtual T GetById(string Id)
        {
            return Mapper.GetById(Id);
        }

        public virtual void Update(T docToUpdate, T newData)
        {
            Mapper.Update(docToUpdate, newData);
        }
    }
}
