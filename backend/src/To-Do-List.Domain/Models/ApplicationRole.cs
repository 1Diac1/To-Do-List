using Microsoft.AspNetCore.Identity;

namespace To_Do_List.Domain.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public string NameRU { get; set; }
    public string Icon { get; set; }

    public IList<ApplicationUser> Users { get; set; }
}