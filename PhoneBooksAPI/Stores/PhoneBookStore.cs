using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PhoneBooksLibrary.PhoneBooks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhoneBooksAPI.Stores
{
    public class PhoneBookStore : IPhoneBookStore
    {
        public string BucketPath;
        private ValidoPhoneBook valid;
        private static readonly object phoneBookLock = new object();

        public PhoneBookStore(IWebHostEnvironment env)
        {
            BucketPath = Path.Combine(env.ContentRootPath, "Bucket");
            valid = new ValidoPhoneBook(this);
        }

        public string PhoneBookPath(PhoneBook phoneBook)
        {
            return Path.Combine(BucketPath, phoneBook.Id.ToString());
        }
        public async Task<ResultPhoneBook> CreateAsync(PhoneBook phoneBook)
        {
            return await Task.Run(() =>
            {
                lock (phoneBookLock)
                {
                    var list = GetAllPhoneBooks();

                    var validoResult = valid.ValidCreate(phoneBook);
                    if (!validoResult.isSuccess)
                    {
                        return new ResultPhoneBook
                        {
                            isSuccess = false,
                            Message = validoResult.message,
                        };
                    }

                    var jsonString = JsonConvert.SerializeObject(phoneBook);
                    File.WriteAllText(PhoneBookPath(phoneBook), jsonString);
                }
                return new ResultPhoneBook { isSuccess = true, Message = "Created successfully" };
            });
        }

        public async Task<ResultPhoneBook> DeleteAsync(PhoneBook phoneBook)
        {
            return await Task.Run(() =>
            {
                lock (phoneBookLock)
                {
                    FileInfo fileInfo = new FileInfo(PhoneBookPath(phoneBook));
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                }
                return new ResultPhoneBook { isSuccess = true , Message= "Deleted successfully" };
            });
        }

        public async Task<IList<PhoneBook>> GetAllPhoneBooksAsync()
        {
            var list = new SortedList<string, PhoneBook>(new DuplicateKeyComparer<string>());

            foreach (string file in Directory.EnumerateFiles(BucketPath))
            {
                var jsonString = await File.ReadAllTextAsync(file);
                var phonebook = JsonConvert.DeserializeObject<PhoneBook>(jsonString);
                list.Add(phonebook.FirstName, phonebook);

            }

            return list.Values.ToList();
        }

        public IList<PhoneBook> GetAllPhoneBooks()
        {
            var list = new SortedList<string, PhoneBook>(new DuplicateKeyComparer<string>());

            foreach (string file in Directory.EnumerateFiles(BucketPath))
            {
                var jsonString = File.ReadAllText(file);
                var phonebook = JsonConvert.DeserializeObject<PhoneBook>(jsonString);
                list.Add(phonebook.FirstName, phonebook);

            }

            return list.Values.ToList();
        }
        public async Task<ResultPhoneBook> UpdateAsync(PhoneBook phoneBook)
        {          
            return await Task.Run(() =>
            {
                lock (phoneBookLock)
                {
                    var validoResult = valid.ValidUpdate(phoneBook);
                    if (!validoResult.isSuccess)
                    {
                        return new ResultPhoneBook
                        {
                            isSuccess = false,
                            Message = validoResult.message,
                        };
                    }

                    var jsonString = JsonConvert.SerializeObject(phoneBook);
                    File.WriteAllText(PhoneBookPath(phoneBook), jsonString);

                    return new ResultPhoneBook { isSuccess = true, Message = "Updated successfully" };
                }

            });




        }
    }
}
