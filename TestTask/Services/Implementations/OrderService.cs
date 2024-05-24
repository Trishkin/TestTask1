using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public OrderService(ApplicationDbContext applicationDbContext) 
        {
            _applicationDbContext = applicationDbContext;
        }
        public Task<Order> GetOrder()
        {
            return _applicationDbContext.Orders.Where(x => x.Quantity > 1)
                .OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();
        }

        public Task<List<Order>> GetOrders()
        {
            return _applicationDbContext.Orders.Where(x => x.User.Status == Enums.UserStatus.Active)
                .OrderByDescending(x => x.CreatedAt).ToListAsync();
        }
    }
}
