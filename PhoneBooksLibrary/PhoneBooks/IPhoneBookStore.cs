using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBooksLibrary.PhoneBooks
{
    public interface IPhoneBookStore
    {
        Task<ResultPhoneBook> CreateAsync(PhoneBook phoneBook);
        Task<ResultPhoneBook> DeleteAsync(PhoneBook phoneBook);
        Task<ResultPhoneBook> UpdateAsync(PhoneBook phoneBook);
        Task<IList<PhoneBook>> GetAllPhoneBooksAsync();
        IList<PhoneBook> GetAllPhoneBooks();
    }
}
