using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace UserStorageSystem
{
    [Serializable]
    public class Message : ISerializable
    {
        public string MethodInfo { get; }
        public int Id { get; }
        public User User { get; }
        public Dictionary<int, User> UsersContainer { get; }

        public Message() { }

        public Message(string methodInfo, int id, User user, Dictionary<int, User> usersContainer)
        {
            MethodInfo = methodInfo;
            Id = id;
            User = user;
            UsersContainer = usersContainer;
        }

        public Message(SerializationInfo info, StreamingContext context)
        {
            MethodInfo = info.GetString("MethodInfo");
            Id = info.GetInt32("Id");
            User = (User)info.GetValue("User", typeof(User));
            UsersContainer = (Dictionary<int, User>) info.GetValue("UsersContainer", typeof (Dictionary<int, User>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MethodInfo", MethodInfo);
            info.AddValue("Id", Id);
            info.AddValue("User", User);
            info.AddValue("UsersContainer", UsersContainer);
        }
    }
}
