using System;
using System.Collections.Generic;

namespace Common.Entity
{
    public partial class UserClient
    {
        public Guid UserClientId { get; set; }
        public Guid UserId { get; set; }
        public int ClientId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpireDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Client Client { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
