using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary
{

    public class Book
    {
        [JsonProperty("Name")]
        private protected string Name { get; set; }
        [JsonProperty("Author")]
        private protected string Author { get; set; }
        [JsonProperty("Category")]
        private protected string Category { get; set; }
        [JsonProperty("Language")]
        private protected string Language { get; set; }
        [JsonProperty("Publication Date")]
        private protected string PublicationDate { get; set; }//DateTime
        [JsonProperty("ISBN")]
        private protected string ISBN { get; set; }//Used as ID
        public Book(RentedBook rented)
        {
            if (rented != null)
            {
                Name = rented.Name;
                Author = rented.Author;
                Category = rented.Category;
                Language = rented.Language;
                PublicationDate = rented.PublicationDate;
                ISBN = rented.ISBN;
            }
        }
        public Book(string name, string author, string category, string language, string date, string isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            PublicationDate = date;
            ISBN = isbn;
        }
        public Book()
        {

        }

        public override string ToString()
        {
            return string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-20} {5,-10}",
            Name, Author, Category, Language, PublicationDate, ISBN);
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string value)
        {
            Name = value;
        }

        public string GetAuthor()
        {
            return Author;
        }

        public void SetAuthor(string value)
        {
            Author = value;
        }

        public string GetCategory()
        {
            return Category;
        }

        public void SetCategory(string value)
        {
            Category = value;
        }

        public string GetLanguage()
        {
            return Language;
        }

        public void SetLanguage(string value)
        {
            Language = value;
        }

        public string GetPublicationDate()
        {
            return PublicationDate;
        }

        public void SetPublicationDate(string value)
        {
            PublicationDate = value;
        }

        public string GetISBN()
        {
            return ISBN;
        }

        public void SetISBN(string value)
        {
            ISBN = value;
        }
    }
}
