using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Newtonsoft.Json;

namespace BookLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            string operation;
            operation = ReadDefault();
            while (true)
            {
                switch (operation)
                {
                    case "Add":
                        Console.WriteLine("Enter book name.");
                        var name = Console.ReadLine();
                        Console.WriteLine("Enter book author.");
                        var author = Console.ReadLine();
                        Console.WriteLine("Enter book category.");
                        var category = Console.ReadLine();
                        Console.WriteLine("Enter book language.");
                        var language = Console.ReadLine();
                        Console.WriteLine("Enter book publication date(yyyy-MM-dd).");
                        var date = Console.ReadLine();
                        Console.WriteLine("Enter book ISBN.");
                        var isbn = Console.ReadLine();
                        library.AddBook(name, author, category, language, date, isbn);
                        operation = ReadDefault();
                        break;
                    case "Delete":
                        Console.WriteLine("Write book id(ISBN) that you wish to delete.");
                        var id = Console.ReadLine();
                        library.DeleteBook(id);
                        operation = ReadDefault();
                        break;
                    case "Take":
                        Console.WriteLine("Write book id(ISBN) that you wish to take.");
                        var takeId = Console.ReadLine();
                        Console.WriteLine("Write your name.");
                        var readerName = Console.ReadLine();
                        Console.WriteLine("Write for how many days(1-60) you are taking the book.");
                        var period = Console.ReadLine();
                        library.TakeBook(takeId, readerName, period);
                        operation = ReadDefault();
                        break;
                    case "Return":
                        Console.WriteLine("Write book id(ISBN) that you wish to return.");
                        var returnId = Console.ReadLine();
                        Console.WriteLine("Write your name.");
                        var readerName2 = Console.ReadLine();
                        library.ReturnBook(returnId, readerName2);
                        Console.WriteLine("Write operation(Add, Delete, Take, Return).");
                        operation = Console.ReadLine();
                        break;
                    case "ListAll":
                        Console.WriteLine("Write filter to sort list(Name, Author, Category, Language, Date, ISBN, Taken).");
                        var filter = Console.ReadLine();
                        library.ListAllBooks(filter);
                        operation = ReadDefault();
                        break;
                    default:
                        operation = ReadDefault();
                        break;
                }
            }
        }

        static string ReadDefault()
        {
            Console.WriteLine("Write operation(Add, Delete, Take, Return, ListAll).");
            string operation = Console.ReadLine();
            return operation;
        }
    }
}
