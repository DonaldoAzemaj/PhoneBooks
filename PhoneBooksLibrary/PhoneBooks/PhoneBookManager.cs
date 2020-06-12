using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBooksLibrary.PhoneBooks
{
    public class PhoneBookManager
    {
        

        protected IPhoneBookStore Store { get; }


        public PhoneBookManager(IPhoneBookStore store)
        {
            Store = store;           
        }

        public async Task<IList<PhoneBook>> GetAllPhoneBooks()
        {
            return await Store.GetAllPhoneBooksAsync();
        }


        public async Task<ResultPhoneBook> PhoneBookCreate(PhoneBook phoneBook)
        {          
            var result = await Store.CreateAsync(phoneBook);
            return result;
        }

        public async Task<ResultPhoneBook> PhoneBookDelete(PhoneBook phoneBook)
        {

            var result = await Store.DeleteAsync(phoneBook);
            return result;
        }

        public async Task<ResultPhoneBook> PhoneBookUpdate(PhoneBook phoneBook)
        {          
            var result = await Store.UpdateAsync(phoneBook);
            return result;
        }
    }
}
