using Abstraccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.models;
using System.Data;
using System.Data.SqlClient;
using DAL;
using System.Collections;

namespace MPP
{
    public class UserMapper : IMappable<User>
    {
        public int Create(User entity)
        {
            int id = -1;
            SqlCommand sqlCommand = new SqlCommand("CreateUser");

            Hashtable queryParams = new Hashtable();

            queryParams.Add("@DNI", entity.DNI);
            queryParams.Add("@Password", entity.Password);

            try
            {
                id = DatabaseSql.WriteAndReturnId(sqlCommand, queryParams);
            }
            catch (SqlException ex) 
            { }
            catch (Exception ex) { }

            return id;
        }

        public void DeleteById(int Id)
        {
            SqlCommand sqlCommand = new SqlCommand("DeleteUserById");
            Hashtable queryParams = new Hashtable { { "@IdUser", Id } };

            try 
            {
                DatabaseSql.Write(sqlCommand, queryParams);
            }
            catch (SqlException ex) 
            { }
            catch(Exception ex) 
            { }

        }

        public List<User> GetAll()
        {
            List<User> list = new List<User>();
            SqlCommand sqlCommand = new SqlCommand("GetAllUsers");

            try 
            {
                DataTable Tabla = DatabaseSql.Read(sqlCommand, null);

                if(Tabla != null && Tabla.Rows.Count > 0)
                {
                    foreach (DataRow row in Tabla.Rows)
                    {
                        list.Add(ToMap(row));
                    }
                } 

            }
            catch (SqlException ex) 
            { }
            catch(Exception ex) 
            { }

            return list;
        }

        public User GetById(string Id)
        {
            User user = null;

            if (Id != null)
            {
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("GetUserByDNI");
                    Hashtable queryParams = new Hashtable { { "@DNI", int.Parse(Id) } };

                    DataTable Tabla = DatabaseSql.Read(sqlCommand, queryParams);

                    if (Tabla != null && Tabla.Rows.Count > 0)
                    {
                        foreach (DataRow row in Tabla.Rows)
                        {
                            user = ToMap(row);
                        }
                    }

                }
                catch(SqlException ex)
                {

                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                throw new Exception("El id del user es null");
            }

            return user;
        }

        public User ToMap(DataRow row)
        {
            User user = new User();

            user.Id = Convert.ToInt32(row["IdUser"]);
            user.DNI = row["DNI"].ToString();
            user.Password = row["Password"].ToString();

            return user;
        }

        public void Update(User docToUpdate, User newData)
        {
            throw new NotImplementedException();
        }

    }
}
