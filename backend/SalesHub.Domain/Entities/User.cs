namespace SalesHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SalesHub.Domain.Common;
using SalesHub.Domain.Entities;

public class User : IEntityBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
