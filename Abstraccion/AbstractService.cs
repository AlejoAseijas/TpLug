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
    public abstract class AbstractService<T> where T : AbstracEntity
    {
        public bool modeConnected = false;
        public IMappable<T> Mapper { get; set; }
        public DataManager persistibleService = DataManager.GetInstance();

        public virtual int Create(T entity) 
        {
            return modeConnected ? Mapper.Create(entity) : persistibleService.save(Mapper.TABLE_NAME, GetData(entity), Mapper.ID_COLUMN);
        }

        public virtual void DeleteById(int Id)
        {
            if (modeConnected) 
            {
                Mapper.DeleteById(Id);
            }
            else 
            {
                persistibleService.delete(Mapper.TABLE_NAME, Id, Mapper.ID_COLUMN);
            }
        }

        public virtual DataTable GetAll()
        {
            return modeConnected ? Mapper.GetAll() : persistibleService.GetAll(Mapper.TABLE_NAME);
        }

        public virtual void Update(T oldData, T newData)
        {
            if (modeConnected) 
            {
                Mapper.Update(oldData, newData);
            }
            else 
            {
                persistibleService.update(Mapper.TABLE_NAME, Mapper.ID_COLUMN, oldData.Id, GetData(newData));
            }
        }

        public virtual Hashtable GetData(T entity)
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
