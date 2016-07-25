using System;
using System.Collections.Generic;


namespace UserStorageSystem
{
    public class ServiceState
    {
        public List<User> Users { get; set; }
        public int GeneratedId { get; set; }
    }
    [Serializable]
    public enum Gender
    {
        Male = 0,
        Female 
    }
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

    [Serializable]
    public class User : IValidation<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int PersonalId { get; set; }
        public Gender Gender { get; set; }
        public Visa[] VisaRecords { get; set; }

        public User(string firstName, string lastName, DateTime dateOfBirth, int personalId, Gender gender,
            Visa[] visaRecords)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            PersonalId = personalId;
            Gender = gender;
            VisaRecords = visaRecords;
        }

        public User() { }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var user = obj as User;
            if (user == null)
                return false;
            return this.Equals(user);

        }

        public bool Equals(User user)
        {
            if (ReferenceEquals(null, user)) return false;
            if (ReferenceEquals(this, user)) return true;
            return this.PersonalId == user.PersonalId && this.FirstName == user.FirstName && this.LastName == user.LastName && this.DateOfBirth == user.DateOfBirth;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = this.PersonalId;
                hash = hash * 23 + this.FirstName.GetHashCode();
                hash = hash * 23 + this.LastName.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"Name: {this.FirstName}, LastName: {this.LastName}, DateOfBirth: {this.DateOfBirth}, PersonalId: {this.PersonalId}";
        }

        public bool IsValid()
        {
            return (!String.IsNullOrWhiteSpace(FirstName) && !String.IsNullOrWhiteSpace(LastName) &&  (DateOfBirth != default(DateTime)) &&  (PersonalId != default(int)));
        }
    }
}
