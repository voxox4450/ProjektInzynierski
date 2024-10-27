﻿namespace Harmonogram.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
