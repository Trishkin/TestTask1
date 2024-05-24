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
            return await _applicationDbContext.Users
                .Join(_applicationDbContext.Orders,
                user => user.Id,
                order => order.UserId,
                (user, order) => new { User = user, Order = order })
                .Where(joined => joined.Order.CreatedAt.Year == 2003)
                .GroupBy(joined => joined.User)
                .Select(grouped => new {
                    User = grouped.Key,
                    TotalAmount = grouped.Sum(joined => joined.Order.Price * joined.Order.Quantity)
                })
                .OrderByDescending(grouped => grouped.TotalAmount)
                .Select(grouped => grouped.User)
                .FirstOrDefaultAsync();
        }

        public Task<List<User>> GetUsers()
        {
            return _applicationDbContext.Users.Where(x => x.Orders.Any(x => x.CreatedAt.Year == 2010 &&
            x.Status == Enums.OrderStatus.Paid)).ToListAsync();
        }
    }
    
}
