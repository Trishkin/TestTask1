using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<User> GetUser()
        {
            return _applicationDbContext.Orders.Where(x => x.CreatedAt.Year == 2003)
                .GroupBy(x => x.User).ToList()
                .OrderByDescending(x => x.Sum(s => s.Quantity * s.Price))
                .FirstOrDefault().Key;
        }

        public Task<List<User>> GetUsers()
        {
            return _applicationDbContext.Users.Where(x => x.Orders.Any(o => o.CreatedAt.Year == 2010 &&
            o.Status == Enums.OrderStatus.Paid)).ToListAsync();
        }
    }
    
}
