using System;


namespace UserStorageSystem
{
    public enum Gender
    {
        Male = 0,
        Female 
    }

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

    public class User : IValidation<User>
    {
        public string FirstName { get;}
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public int PersonalId { get; }
        public Gender Gender { get; }
        public Visa[] VisaRecords { get; }

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
            return this.PersonalId == user.PersonalId;
        }

        public override int GetHashCode()
        {
            return this.PersonalId;
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
