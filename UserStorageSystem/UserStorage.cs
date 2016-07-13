using System;


namespace UserStorageSystem
{
    public class UserStorage
    {
        public IStorage Storage { get; } = new MemoryStorage(new CustomIterator());

        public UserStorage()
        {           
        }

        public UserStorage(IStorage storage)
        {
            Storage = storage;
        }
    
    }
}
