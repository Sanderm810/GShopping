using GShopping.EmailAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GShopping.EmailAPI.Model
{
    [Table("email_log")]
    public class EmailLog : BaseEntity
    {

        [Column("email")]
        public string Email { get; set; }

        [Column("log")]
        public string Log { get; set; }

        [Column("send_date")]
        public DateTime SendDate { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }
    }
}
