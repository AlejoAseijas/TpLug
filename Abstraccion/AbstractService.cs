using Abstraccion;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract class AbstractService<T>
    {
        public IMappable<T> Mapper { get; set; }
        public DataManager persistibleService = DataManager.GetInstance();

        public virtual int Create(T entity) 
        {
            int id = persistibleService.save(Mapper.TABLE_NAME, GetData(entity), Mapper.ID_COLUMN);
            return id;
        }

        public virtual void DeleteById(int Id)
        {
            persistibleService.delete(Mapper.TABLE_NAME, Id, Mapper.ID_COLUMN);
        }

        public virtual DataTable GetAll()
        {
            return persistibleService.getTable(Mapper.TABLE_NAME);
        }

        public virtual void Update(int id, T newData)
        {
            persistibleService.update(Mapper.TABLE_NAME, Mapper.ID_COLUMN, id, GetData(newData));
        }

        private Hashtable GetData(T entity)
        {
            Hashtable data = new Hashtable();
            Type type = typeof(T);

            foreach (PropertyInfo property in type.GetProperties())
            {
                string propertyName = property.Name;

                if (propertyName != "Id")
                {
                    object propertyValue = property.GetValue(entity) ?? DBNull.Value;
                    data.Add(propertyName, propertyValue);
                }

            }

            return data;
        }
    }
}
