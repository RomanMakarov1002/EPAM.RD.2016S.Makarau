using System;

namespace UserStorageSystem.Entities
{
    [Serializable]
    public struct Visa
    {
        public string Country;
        public DateTime Start;
        public DateTime End;

        public Visa(string country, DateTime start, DateTime end)
        {
            Country = country;
            Start = start;
            End = end;
        }
    }
}
