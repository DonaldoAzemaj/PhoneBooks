using Moq;
using NUnit.Framework;
using PhoneBooksLibrary.PhoneBooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneBookLibrary.Test
{
    [TestFixture]
    public class PhoneBookConcurrency
    {
        Mock<IPhoneBookStore> StoreMock = new Mock<IPhoneBookStore>();
        public Object lockObj = new Object();

        [Test]
        public async Task PhoneBook_ConcurrentModification()
        {

            PhoneBook phoneBooks = new PhoneBook();

            StoreMock.Setup(s => s.CreateAsync(It.IsAny<PhoneBook>()))
                .Returns<PhoneBook>(p =>
                {
                    phoneBooks = p;
                    return Task.FromResult(new ResultPhoneBook { isSuccess = true});
                });

            StoreMock.Setup(s => s.UpdateAsync(It.IsAny<PhoneBook>()))
                .Returns<PhoneBook>( async p =>
                {
                    return await Task.Run(async () =>
                    {
                        
                        lock (lockObj) 
                        {
                            phoneBooks.FirstName = p.FirstName;
                            Thread.Sleep(1);
                            phoneBooks.LastName = p.LastName;

                        }


                        return new ResultPhoneBook { isSuccess = true };
                    });
                });

            var phonebookManager = new PhoneBookManager(StoreMock.Object);
            var phonebookManager2 = new PhoneBookManager(StoreMock.Object);

            var phoneBook = new PhoneBook();

            await phonebookManager.PhoneBookCreate(phoneBook);

            for (int i = 0; i < 2000; i++)
            {
                Task.Run(() => phonebookManager2.PhoneBookUpdate(new PhoneBook { FirstName = "1", LastName = "1" }));

                Task.Run(() => phonebookManager.PhoneBookUpdate(new PhoneBook { FirstName = "3", LastName = "3" }));

                Task.Run(() => phonebookManager.PhoneBookUpdate(new PhoneBook { FirstName = "2", LastName = "2" }));
            }

            lock (lockObj)
            {
                Assert.IsTrue(phoneBooks.FirstName == phoneBooks.LastName);

            }


        }


    }
}
