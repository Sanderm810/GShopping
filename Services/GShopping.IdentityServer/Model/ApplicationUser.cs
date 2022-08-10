using Microsoft.AspNetCore.Identity;

namespace GShopping.IdentityServer.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}
