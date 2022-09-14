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
            if (header == null) return false;
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

        public async Task<bool> DeleteOrderById(long id)
        {
            try
            {
                await using var _db = new MySQLContext(_context);

                OrderHeader header = await _db.Headers
                   .Where(h => h.Id == id)
                   .FirstOrDefaultAsync();

                if (header == null) return false;

                _db.Details
                    .RemoveRange(
                    _db.Details.Where(c => c.OrderHeaderId == header.Id));

                _db.Product
                    .RemoveRange(
                    _db.Product.Where(c => _db.Details
                    .Where(c => c.OrderHeaderId == header.Id)
                    .Select(x => x.OrderProductId)
                    .Contains(c.Key)
                  ));

                _db.Headers.Remove(header);

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
                        Status = (Model.Enum.StatusPedido)header.Status,
                        FullAddress = header.FullAddress,
                        UserId = header.UserId,
                        Observation = header.Observation
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
                    Status = (Model.Enum.StatusPedido)ordersHeader.Status,
                    FullAddress = ordersHeader.FullAddress,
                    UserId = ordersHeader.UserId,
                    Observation = ordersHeader.Observation
                },
                CartDetails = ordersDetails.Select(item =>
                {
                    return new CartDetailViewModel
                    {
                        Id = item.Id,
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

        public async Task<OrderViewModel> UpdateOrder(OrderViewModel model)
        {
            await using var _db = new MySQLContext(_context);

            OrderHeader ordersHeader = await _db.Headers
                .Where(x => x.Id == model.CartHeader.Id)
                .FirstOrDefaultAsync();

            if (ordersHeader == null)
            {
                throw new Exception();
            }

            ordersHeader.FirstName = model.CartHeader.FirstName;
            ordersHeader.LastName = model.CartHeader.LastName;
            ordersHeader.Email = model.CartHeader.Email;
            ordersHeader.Phone = model.CartHeader.Phone;
            ordersHeader.Status = model.CartHeader.Status;
            ordersHeader.FullAddress = model.CartHeader.FullAddress;
            ordersHeader.Observation = model.CartHeader.Observation;
            ordersHeader.PurchaseAmount = model.CartDetails.Select(x => (x.Product.Price * x.Count)).Sum();
            ordersHeader.CartTotalItems = model.CartDetails.Select(x => x.Count).Sum();

            List<OrderDetail> item = await _db.Details
                   .Where(x => x.OrderHeaderId == ordersHeader.Id)
                   .Include(c => c.OrderProduct)
                   .ToListAsync();

            item.ForEach(item =>
            {
                var modelItem = model.CartDetails.Where(x => x.Id == item.Id).FirstOrDefault();
                if (modelItem != null)
                {
                    item.Count = modelItem.Count;
                    if (item != null)
                    {
                        _db.Details.Update(item);
                    }
                }
            });


            await _db.SaveChangesAsync();

            return await FindOrderById(ordersHeader.Id);
        }

        public Task UpdateOrderStatus(long orderHeaderId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
