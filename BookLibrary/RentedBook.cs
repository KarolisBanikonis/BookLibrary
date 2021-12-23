using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{
    public class RentedBook : Book
    {
        [JsonProperty("Return Time")]
        private string ReturnTime { get; set; }
        public RentedBook(Book book, string returnTime)
        {
            if(book != null)
            {
                Name = book.GetName();
                Author = book.GetAuthor();
                Category = book.GetCategory();
                Language = book.GetLanguage();
                PublicationDate = book.GetPublicationDate();
                ISBN = book.GetISBN();
                ReturnTime = returnTime;
            }
            else
            {
                ReturnTime = returnTime;
            }
        }

        public override string ToString()
        {
            return string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-20} {5,-10} {6, -10}",
            Name, Author, Category, Language, PublicationDate, ISBN, ReturnTime);
        }

        public string GetReturnTime()
        {
            return ReturnTime;
        }

        public void SetReturnTime(string value)
        {
            ReturnTime = value;
        }
    }
}
