using GShopping.EmailAPI.Messages;
using GShopping.EmailAPI.Model;
using GShopping.EmailAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GShopping.EmailAPI.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<MySQLContext> _context;

        public EmailRepository(DbContextOptions<MySQLContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(CheckoutMessage message)
        {
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SendDate = DateTime.Now,
                Log = $"Pedido - {message.Id} foi criada com sucesso!",
                OrderId = message.Id
            };
            await using var _db = new MySQLContext(_context);
            _db.Emails.Add(email);
            await _db.SaveChangesAsync();
        }
    }
}
