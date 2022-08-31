using GShopping.OrderAPI.Model;
using GShopping.OrderAPI.Model.Context;
using GShopping.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GShopping.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public OrderRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task<bool> AddOrder(OrderHeader header)
        {
            if(header == null) return false;
            await using var _db = new MySQLContext(_context);
            _db.Headers.Add(header);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            await using var _db = new MySQLContext(_context);
            var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
            if (header != null)
            {
                header.PaymentStatus = status;
                await _db.SaveChangesAsync();
            };
        }

        public Task<bool> DeleteOrderById(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderViewModel>> FindAllOrders()
        {
            await using var _db = new MySQLContext(_context);
            List<OrderHeader> ordersHeader = await _db.Headers.ToListAsync();
            List<OrderViewModel> ordersView = new List<OrderViewModel>();
            foreach (OrderHeader header in ordersHeader)
            {
                List<OrderDetail> ordersDetails = await _db.Details
                    .Where(x => x.OrderHeader.Id == header.Id)
                    .Include(c => c.OrderProduct)
                    .ToListAsync();

                ordersView.Add(new OrderViewModel
                {
                    CartHeader = new CartHeaderViewModel
                    {
                        Id = header.Id,
                        CardNumber = header.CardNumber,
                        Count = ordersDetails.Count,
                        CouponCode = header.CouponCode,
                        CVV = header.CVV,
                        DateTime = header.DateTime,
                        DiscountAmount = (decimal)header.DiscountAmount,
                        Email = header.Email,
                        ExpiryMothYear = header.ExpiryMonthYear,
                        FirstName = header.FirstName,
                        LastName = header.LastName,
                        Phone = header.Phone,
                        PurchaseAmount = header.PurchaseAmount,
                        Status = header.Status,
                        UserId = header.UserId
                    },
                    CartDetails = ordersDetails.Select(item =>
                    {
                        return new CartDetailViewModel
                        {
                            Id = item.Id,
                            Count = item.Count,
                            CartHeaderId = header.Id,
                            Product = new ProductViewModel
                            {
                                CategoryName = item.OrderProduct.CategoryName,
                                Count = item.Count,
                                Description = item.OrderProduct.Description,
                                Id = item.OrderProduct.Id,
                                ImageUrl = item.OrderProduct.ImageUrl,
                                Name = item.OrderProduct.Name,
                                Price = item.OrderProduct.Price
                            }
                        };
                    }).ToList()
                });
            }
            return ordersView;
        }

        public async Task<OrderViewModel> FindOrderById(long id)
        {
            await using var _db = new MySQLContext(_context);

            OrderHeader ordersHeader = await _db.Headers
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            List<OrderDetail> ordersDetails = await _db.Details
                    .Where(x => x.OrderHeader.Id == ordersHeader.Id)
                    .Include(c => c.OrderProduct)
                    .ToListAsync();

            return new OrderViewModel
            {
                CartHeader = new CartHeaderViewModel
                {
                    Id = ordersHeader.Id,
                    CardNumber = ordersHeader.CardNumber,
                    Count = ordersDetails.Count,
                    CouponCode = ordersHeader.CouponCode,
                    CVV = ordersHeader.CVV,
                    DateTime = ordersHeader.DateTime,
                    DiscountAmount = (decimal)ordersHeader.DiscountAmount,
                    Email = ordersHeader.Email,
                    ExpiryMothYear = ordersHeader.ExpiryMonthYear,
                    FirstName = ordersHeader.FirstName,
                    LastName = ordersHeader.LastName,
                    Phone = ordersHeader.Phone,
                    PurchaseAmount = ordersHeader.PurchaseAmount,
                    Status = ordersHeader.Status,
                    UserId = ordersHeader.UserId
                },
                CartDetails = ordersDetails.Select(item =>
                {
                    return new CartDetailViewModel
                    {
                        Count = item.Count,
                        CartHeaderId = ordersHeader.Id,
                        Product = new ProductViewModel
                        {
                            CategoryName = item.OrderProduct.CategoryName,
                            Count = item.Count,
                            Description = item.OrderProduct.Description,
                            Id = item.OrderProduct.Id,
                            ImageUrl = item.OrderProduct.ImageUrl,
                            Name = item.OrderProduct.Name,
                            Price = item.OrderProduct.Price
                        }
                    };
                }).ToList()
            };
        }

        public Task<OrderHeader> SendEmail(OrderHeader model)
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeader> UpdateOrder(OrderHeader model)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrderStatus(long orderHeaderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
