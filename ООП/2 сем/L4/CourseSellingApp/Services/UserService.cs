namespace CourseSellingApp.Services
{
    public enum UserRole
    {
        Client,
        Administrator
    }

    public static class UserService
    {
        private static UserRole _currentUserRole = UserRole.Client;

        public static UserRole CurrentUserRole
        {
            get => _currentUserRole;
            private set
            {
                if (_currentUserRole != value)
                {
                    _currentUserRole = value;
                    OnRoleChanged?.Invoke(null, _currentUserRole);
                }
            }
        }

        public static event System.EventHandler<UserRole>? OnRoleChanged;

        public static void LoginAs(UserRole role)
        {
            CurrentUserRole = role;
        }
    }
}
