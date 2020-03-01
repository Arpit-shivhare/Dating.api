using Dating.api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating.api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _datacontext;

        public AuthRepository(DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        public async Task<bool> UserExists(string username)
        {
           if(await _datacontext.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }
            return false;
        }

        public async Task<User> Login(string username, string password)
        {
            var User = await _datacontext.Users.FirstOrDefaultAsync(x => x.Username == username);
            
            if(User == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, User.PasswordHash, User.PasswordSalt))
            {
                return null;
            }

            return User;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                
                for(int i=0; i<ComputedHash.Length; i++)
                {
                    if(ComputedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }




        //Create User with PaswordHash and PasswordSalt
        public async Task<User> Register(User user, string password)
        { 
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

            await _datacontext.Users.AddAsync(user);
            await _datacontext.SaveChangesAsync();

            return user;
           
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
    }
}
