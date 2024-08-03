using System;
using System.Collections.Generic;

namespace Common.Entity
{
    public partial class Client
    {
        public Client()
        {
            UserClients = new HashSet<UserClient>();
        }

        public int ClientId { get; set; }
        public string HarewareInfo { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserClient> UserClients { get; set; }
    }
}
