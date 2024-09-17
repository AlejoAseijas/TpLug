using BE.models;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AuthService
    {
        private UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        private SHA1CryptoServiceProvider algorit = new SHA1CryptoServiceProvider();
        private UserMapper mapper = new UserMapper();

        public string GenerateHash(string plainText)
        {
            string hash = string.Empty;

            try
            {
                byte[] ByteSourceText = unicodeEncoding.GetBytes(plainText);
                byte[] ByteHash = algorit.ComputeHash(ByteSourceText);
                return Convert.ToBase64String(ByteHash);

            }
            catch(CryptographicException ex)
            {

            }
            catch (Exception ex)
            {
            }

            return hash;
        }

        public bool LogIn(AuthDTO authDto)
        {
            bool valid = false;

            User user = mapper.GetById(authDto.DNI);

            if(user != null)
            {

                string regenrateHash = GenerateHash(authDto.password);

                if (regenrateHash.Equals(user.Password))
                {
                    valid = true;
                }
            }

            return valid;
        }

        public void SaveUser(User user)
        {
            if (user != null)
            {
                user.Password = GenerateHash(user.Password);
                mapper.Create(user);
            }
        }

        public void DeleteUser(User user) 
        {
            if (user != null)
            {
                mapper.DeleteById(user.Id);
            }
        }

        public List<User> GetAll() 
        {
            return mapper.GetAll();
        }
     }
    }
