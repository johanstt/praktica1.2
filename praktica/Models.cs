namespace praktica
{
    public enum UserRole
    {
        Guest,
        Operator,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}


