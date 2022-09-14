﻿using GShopping.OrderAPI.Model;
using GShopping.OrderAPI.Models;

namespace GShopping.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder (OrderHeader header);
        Task UpdateOrderPaymentStatus (long orderHeaderId, bool paid);
        Task<IEnumerable<OrderViewModel>> FindAllOrders();
        Task<OrderViewModel> FindOrderById(long id);
        Task<OrderViewModel> UpdateOrder(OrderViewModel model);
        Task UpdateOrderStatus (long orderHeaderId, string status);
        Task<OrderHeader> SendEmail(OrderHeader model);
        Task<bool> DeleteOrderById(long id);
    }
}
