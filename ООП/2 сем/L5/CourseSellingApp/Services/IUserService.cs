using System;

namespace CourseSellingApp.Services
{
    public interface IUserService
    {
        event EventHandler<UserRole>? OnRoleChanged;
        UserRole CurrentUserRole { get; }
        void LoginAs(UserRole role);
    }
}
