using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBooksLibrary.PhoneBooks
{
    public class PhoneBook : IEquatable<PhoneBook>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public PhoneNumberType Type { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals(PhoneBook other)
        {
            return Id == other.Id;
        }
    }
}
