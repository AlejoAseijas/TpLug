using BE.models;
using DAL;
using MPP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AuthService : AbstractService<User>
    {
        private UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        private SHA1CryptoServiceProvider algorit = new SHA1CryptoServiceProvider();
        private UserMapper mapper = new UserMapper();

        public AuthService() 
        {
            base.Mapper = mapper;
        }

        public DataSet GetAll()
        {
            List<User> users = new List<User>();
            DataSet dataSet = DataDisconnected.Read(base.Mapper.TABLE_NAME, false);
            
            return dataSet;
        }

        public bool SaveData(DataSet dataSet)
        {
            try 
            {
                DataDisconnected.Update(dataSet, base.Mapper.TABLE_NAME);
                DataDisconnected.Read(base.Mapper.TABLE_NAME, true);
                return true;
            }
            catch(Exception ex) 
            { 
                return false; 
            }
         
        }

        public bool LogIn(AuthDTO authDto)
        {
            bool valid = false;

            User user = mapper.GetById(authDto.DNI);

            if(user != null)
            {
                valid = authDto.password.Equals(user.Password);
            }

            return valid;
        }

        public string GenerateHash(string plainText)
        {
            string hash = string.Empty;

            try
            {
                byte[] ByteSourceText = unicodeEncoding.GetBytes(plainText);
                byte[] ByteHash = algorit.ComputeHash(ByteSourceText);
                return Convert.ToBase64String(ByteHash);

            }
            catch (CryptographicException ex)
            {

            }
            catch (Exception ex)
            {
            }

            return hash;
        }

    }
}
