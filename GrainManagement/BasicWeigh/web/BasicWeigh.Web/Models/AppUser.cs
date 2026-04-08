using System.ComponentModel.DataAnnotations;

namespace BasicWeigh.Web.Models;

public class AppUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Display(Name = "Username")]
    public string Username { get; set; } = "";

    [Required]
    [StringLength(100)]
    [Display(Name = "Display Name")]
    public string DisplayName { get; set; } = "";

    /// <summary>
    /// BCrypt hashed password
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = "";

    /// <summary>
    /// Role: User, Manager, Admin
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Role { get; set; } = "User";

    /// <summary>
    /// When true, user must change password on next login
    /// </summary>
    public bool MustChangePassword { get; set; }

    public bool Active { get; set; } = true;
}
