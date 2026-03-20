using System;

namespace CourseSellingApp.Services
{
    public enum UserRole
    {
        Client,
        Administrator
    }

    public class UserService : IUserService
    {
        private UserRole _currentUserRole = UserRole.Client;

        public UserRole CurrentUserRole
        {
            get => _currentUserRole;
            private set
            {
                if (_currentUserRole != value)
                {
                    _currentUserRole = value;
                    OnRoleChanged?.Invoke(this, _currentUserRole);
                }
            }
        }

        public event EventHandler<UserRole>? OnRoleChanged;

        public void LoginAs(UserRole role)
        {
            CurrentUserRole = role;
        }
    }
}
