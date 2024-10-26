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
        //Modo desconectado aclarar en DataManeger si es xml o mem
        public ModesOfPersistible mode = ModesOfPersistible.DISCONECTED;

        public IMappable<T> Mapper { get; set; }
        public DataManager persistibleService = DataManager.GetInstance();

        public virtual int Create(T entity) 
        {
            return ModesOfPersistible.DB.Equals(mode) ? Mapper.Create(entity) : persistibleService.save(Mapper.TABLE_NAME, GetData(entity), Mapper.ID_COLUMN);
        }

        public virtual void DeleteById(int Id)
        {
            if (ModesOfPersistible.DB.Equals(mode)) 
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
            return ModesOfPersistible.DB.Equals(mode) ? Mapper.GetAll() : persistibleService.GetAll(Mapper.TABLE_NAME);
        }

        public virtual void Update(T oldData, T newData)
        {
            if (ModesOfPersistible.DB.Equals(mode)) 
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
