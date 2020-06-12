using System;
using System.Linq;


namespace PhoneBooksLibrary.PhoneBooks
{
    public class ValidoPhoneBook
    {

        protected IPhoneBookStore Store { get; }
        public ValidoPhoneBook(IPhoneBookStore Store)
        {
            this.Store = Store;
        }

        public ResultValid Valid(PhoneBook phoneBook) 
        {
            var firstNameResult = ValidFirstName(phoneBook.FirstName);
            if (firstNameResult.isSuccess == false) 
            {
                return firstNameResult;
            }

            var phoneNumberResult = ValidPhoneNumber(phoneBook.PhoneNumber);
            if(phoneNumberResult.isSuccess == false) 
            {
                return phoneNumberResult;
            }

            return new ResultValid { isSuccess = true };
        
        }

        private ResultValid ValidPhoneNumber(string phoneNumber)
        {
            if (String.IsNullOrEmpty(phoneNumber))
            {
                return new ResultValid { isSuccess = false, message = "Phone number cannot be empty" };
            }

            return new ResultValid { isSuccess = true };
        }

        private ResultValid ValidFirstName(string firstName)
        {
            if (String.IsNullOrEmpty(firstName)) 
            {
                return new ResultValid { isSuccess = false, message = "first Name cannot be empty" };
            }

            return new ResultValid { isSuccess = true };
        }

        public ResultValid ValidCreate(PhoneBook phoneBook) 
        {

            var validResult = Valid(phoneBook);
            if (!validResult.isSuccess) 
            {
                 return validResult;
            }
            

            var list =  Store.GetAllPhoneBooks();
            if (list.Cast<PhoneBook>().ToList().Find(x => x.Id == phoneBook.Id) != null)
            {
                return new ResultValid { isSuccess = false, message = "Phone book with this Id exist" };
            }

            if (list.Cast<PhoneBook>().ToList().Find(x => x.PhoneNumber == phoneBook.PhoneNumber) != null)
            {
                return new ResultValid{ isSuccess = false, message= "This phone number is registered" };
            }

            return new ResultValid { isSuccess = true}; ;

        }


        public ResultValid ValidUpdate(PhoneBook phoneBook)
        {
            var list = Store.GetAllPhoneBooks();
            var phone = list.Cast<PhoneBook>().ToList().Find(x => x.Id == phoneBook.Id);
            if (phoneBook == null)
            {
                return new ResultValid { isSuccess = false, message = "This record don't exist" };
            }

            var validResult = Valid(phoneBook);
            if (!validResult.isSuccess)
            {
                return validResult;
            }

            return new ResultValid { isSuccess = true }; ;

        }

    }
}
