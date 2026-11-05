using HelpDeskSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HelpDeskSystem.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    { 
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[]
        {
            "Admin",
            "ITStaff",
            "Employee",
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        // Initial admin account
        const string adminEmail = "admin@helpdesk.com";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true
            };

            var result = await userManager.CreateAsync(
                adminUser,
                "Admin@HelpDesk2027!"
            );

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        Console.WriteLine($"Admin Email: {adminUser.Email}");
        Console.WriteLine($"Is Active: {adminUser.IsActive}");
        Console.WriteLine($"Is Locked Out: {await userManager.IsLockedOutAsync(adminUser)}");
        Console.WriteLine($"Failed Attempts: {await userManager.GetAccessFailedCountAsync(adminUser)}");
        Console.WriteLine($"Password Valid: {await userManager.CheckPasswordAsync(adminUser, "Admin@HelpDesk2027!")}");

        // Assign Admin Role
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var result = await userManager.AddToRoleAsync(adminUser, "Admin");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to assign Admin role: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                );
            }
        }

        // TEMPORARY: Reset Admin Password
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);

        var resetResult = await userManager.ResetPasswordAsync(
            adminUser,
            resetToken,
            "Admin@HelpDesk2027!"
        );

        if (!resetResult.Succeeded)
        {
            throw new InvalidOperationException(
                $"Failed to reset admin password: {string.Join(", ", resetResult.Errors.Select(e => e.Description))}"
            );
        }

        // Reset Lockout
        await userManager.SetLockoutEndDateAsync(adminUser, null);
        await userManager.ResetAccessFailedCountAsync(adminUser);
    }

}
