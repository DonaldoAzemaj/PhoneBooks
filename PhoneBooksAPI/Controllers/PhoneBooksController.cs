using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBooksLibrary.PhoneBooks;
using Swashbuckle.AspNetCore.Annotations;

namespace PhoneBooksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneBooksController : ControllerBase
    {
        PhoneBookManager phoneBookManager;
        public PhoneBooksController(PhoneBookManager phoneBookManager) 
        {
            this.phoneBookManager = phoneBookManager;
        }


        [HttpPost]
        [SwaggerOperation(
          Summary = "Create new phone book.",
          Description = "First name and phone number should not be empty to created successfully."
          )]
        public async Task<ResultPhoneBook> CreatePhoneBook(PhoneBook phoneBook )
        {
            var result =  await phoneBookManager.PhoneBookCreate(phoneBook);

            return result;
        }

        [HttpGet]
        [SwaggerOperation(
          Summary = "Get all phone books",
          Description = "Get all phone book in alphabetical order."
          )]
        public async Task<IList<PhoneBook>> GetAllPhoneBook()
        {
            var result = await phoneBookManager.GetAllPhoneBooks();

            return result;
        }

        [HttpPut]
        [SwaggerOperation(
          Summary = "Update phone book.",
          Description = "Update phone book by id."
          )]
        public async Task<ResultPhoneBook> UpdatePhoneBook(PhoneBook phoneBook)
        {
            var result = await phoneBookManager.PhoneBookUpdate(phoneBook);

            return result;
        }

        [HttpDelete]
        [SwaggerOperation(
          Summary = "Delete phone book.",
          Description = "Delete phone book by id."
          )]
        public async Task<ResultPhoneBook> DeletePhoneBook(PhoneBook phoneBook)
        {
            var result = await phoneBookManager.PhoneBookDelete(phoneBook);

            return result;
        }


    }
}