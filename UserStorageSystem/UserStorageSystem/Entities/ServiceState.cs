using System.Collections.Generic;

namespace UserStorageSystem.Entities
{
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int GeneratedId { get; set; }
    }
}
