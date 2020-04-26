using DTO.User;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace User.BusinessLayer
{
    public interface IUserLogic
    {
        Task<Guid> SaveUser(CreateUserCommand command);
        Task<ICollection<GetUserResponse>> GetUsers(UserDto request);
        Task<T> GetUser<T>(Expression<Func<Entities.User, bool>> predicate) where T : class;

        Task<bool> UpdateUser(Guid userId, JsonPatchDocument jsonPatchDocument);

        Task<bool> VerifyUserLogin(string account, string password);
    }
}
