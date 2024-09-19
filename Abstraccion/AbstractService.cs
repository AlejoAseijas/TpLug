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
        public IGestor<T> Mapper { get; set; }

        public virtual int Create(T entity)
        {
            return Mapper.Create(entity);
        }

        public virtual void DeleteById(int Id)
        {
            Mapper.DeleteById(Id);
        }

        public virtual List<T> GetAll()
        {
            return Mapper.GetAll();
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
