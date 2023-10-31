using Microsoft.AspNetCore.Identity;

namespace To_Do_List.Domain.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }

    public string Created { get; set; } = DateTime.Now.ToLongTimeString();
    public string Updated { get; set; } = DateTime.Now.ToLongTimeString();
}