using DTO.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq.Expressions;

namespace User.BusinessLayer
{
    public class UserLogic : IUserLogic
    {

        private readonly IMapper mapper;
        private readonly UserContext context;

        public UserLogic(IMapper mapper, UserContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public Task<List<GetUserResponse>> GetUsers(UserDto request)
        {
            var firstName = request.FirstName;
            var lastName = request.LastName;
            var account = request.Account;
            var email = request.Email;

            return context
                .User
                .AsNoTracking()
                .ConditionalWhere(!string.IsNullOrEmpty(firstName), x => x.FirstName == firstName)
                .ConditionalWhere(!string.IsNullOrEmpty(lastName), x => x.LastName == lastName)
                .ConditionalWhere(!string.IsNullOrEmpty(account), x => x.Account == account)
                .ConditionalWhere(!string.IsNullOrEmpty(email), x => x.Email == email)
                .ProjectTo<GetUserResponse>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public Task<T> GetUser<T>(Guid id) where T : class
        {
            return context
                .User
                .ConditionalWhere(id != Guid.Empty, x => x.Id == id)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        }

        public async Task<Guid> SaveUser(CreateUserCommand command)
        {
            var user = mapper.Map<Entities.User>(command);
            user.Id = Guid.NewGuid();
            var salt = GetSalt();

            user.Salt = salt;
            user.Password = GeneratePassword(command.Password, salt);

            await context.User.AddAsync(user);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return Guid.Empty;

            return user.Id;
        }

        public async Task<bool> UpdateUser(Guid userId, JsonPatchDocument jsonPatchDocument)
        {
            var user = context
                .User
                .FirstOrDefault(x => x.Id == userId);

            jsonPatchDocument.ApplyTo(user);

            context.User.Update(user);
            var rowChanged = await context.SaveChangesAsync();
            if (rowChanged == 0)
                return false;

            return true;
        }

        public Task<T> GetUser<T>(Expression<Func<Entities.User, bool>> predicate) where T : class
        {
            return context
              .User
              .Where(predicate)
              .ProjectTo<T>(mapper.ConfigurationProvider)
              .FirstOrDefaultAsync();
        }


        public async Task<bool> VerifyUserLogin(string account, string password)
        {
            var user = await context
                .User
                .FirstOrDefaultAsync(x => x.Account == account);

            if(user == null && 
                user.Password != GeneratePassword(password, user.Salt))
            {
                return false;
            }            

            return true;            
        }


        #region Password Protection
        // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-3.1

        private byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private string GeneratePassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        #endregion
    }
}
