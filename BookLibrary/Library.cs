using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookLibrary
{
    public class Library
    {
        private static readonly string jsonFile = @"C:\Users\Karolis\Desktop\BookLibrary\BookLibrary\Json\book.json";
        private static readonly string jsonReadersFile = @"C:\Users\Karolis\Desktop\BookLibrary\BookLibrary\Json\reader.json";
        public List<Book> BooksList { get; private set; }
        public List<Reader> ReadersList { get; private set; }

        public Library()
        {
            BooksList = new List<Book>();
            ReadersList = new List<Reader>();
            BooksList = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(jsonFile));
            ReadersList = JsonConvert.DeserializeObject<List<Reader>>(File.ReadAllText(jsonReadersFile));
        }

        public void AddBook(string name, string author, string category, string language, string date, string isbn)
        {
            DateTime parsedDate;
            var hasParsed = DateTime.TryParseExact(date, "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out parsedDate);
            var newBook = new Book(name, author, category, language, parsedDate.ToString("yyyy-MM-dd"), isbn);
            if (!hasParsed)
            {
                Console.WriteLine("Publication date format is (yyyy-MM-dd).");
            }
            bool isValidId = true;
            //Check are there any matching id in BooksList
            for (int i = 0; i < BooksList.Count; i++)
            {
                if (BooksList[i].GetISBN() == isbn)
                {
                    isValidId = false;
                    break;
                }
            }
            //If there are not then check ReadersList
            if (isValidId)
            {
                for (int i = 0; i < ReadersList.Count; i++)
                {
                    for (int j = 0; j < ReadersList[i].RentedBooksList.Count; j++)
                    {
                        if(ReadersList[i].RentedBooksList[j].GetISBN() == isbn)
                        {
                            isValidId = false;
                            break;
                        }
                    }
                }
            }
            if (!isValidId)
            {
                Console.WriteLine($"Book with ISBN {isbn} already exists!");
            }
            if (string.IsNullOrEmpty(newBook.GetISBN()))
            {
                Console.WriteLine("Book's ISBN field can not be empty.");
            }
            else
            {
                if (hasParsed && isValidId)
                {
                    Console.WriteLine("Successfully added book to the library!");
                    BooksList.Add(newBook);
                    string json = JsonConvert.SerializeObject(BooksList, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                }
            }
        }

        public void DeleteBook(string id)
        {
            Book book = null;
            for (int i = 0; i < BooksList.Count; i++)
            {
                if(BooksList[i].GetISBN() == id)
                {
                    book = BooksList[i];
                    BooksList.RemoveAt(i);
                    Console.WriteLine("Successfully deleted book.");
                }
            }
            if (book == null)
            {
                Console.WriteLine($"Book with ISBN {id} does not exist.");
            }
            string json = JsonConvert.SerializeObject(BooksList, Formatting.Indented);
            File.WriteAllText(jsonFile, json);
        }

        public bool CreateUser(string readerName)
        {
            if (string.IsNullOrEmpty(readerName))
            {
                Console.WriteLine("Reader's name field can not be empty.");
                return false;
            }
            else
            {
                ReadersList.Add(new Reader(readerName));
                return true;
            }
        }

        public void TakeBook(string id, string readerName, string returnTime)
        {
            Book book = null;
            for (int i = 0; i < BooksList.Count; i++)
            {
                if (BooksList[i].GetISBN() == id)
                {
                    book = BooksList[i];
                    //BooksList.RemoveAt(i);
                }
            }
            if(book != null)
            {
                var hasParsed = Int32.TryParse(returnTime, out int parsedDays);
                bool isDateValid = true;
                if (hasParsed)
                {
                    if(parsedDays <= 0)
                    {
                        Console.WriteLine("You have to take book atleast for a day.");
                        isDateValid = false;
                    }
                    else if(parsedDays > 60)
                    {
                        Console.WriteLine("You can not take book for longer than two months.");
                        isDateValid = false;
                    }
                }
                else
                {
                    Console.WriteLine("Write number of days between 1 - 60.");
                }
                if (isDateValid)
                {
                    var date = DateTime.Now.AddDays(parsedDays).ToString("yyyy-MM-dd");
                    if(ReadersList.Count == 0)
                    {
                        var isCreated = CreateUser(readerName);
                        if (!isCreated)
                        {
                            return;
                        }
                    }
                    for (int i = 0; i < ReadersList.Count; i++)
                    {
                        if (ReadersList[i].GetName() == readerName)
                        {
                            if (ReadersList[i].RentedBooksList.Count >= 3)
                            {
                                Console.WriteLine("You can not borrow more books. Limit is 3.");
                                return;
                            }
                            else
                            {
                                BooksList.Remove(book);
                                RentedBook rented = new RentedBook(book, date);
                                ReadersList[i].RentedBooksList.Add(rented);
                                break;
                            }
                        }
                        else
                        {
                            //End of loop reader does not exist
                            if (i == ReadersList.Count - 1)
                            {
                                var isCreated = CreateUser(readerName);
                                if (!isCreated)
                                {
                                    return;
                                }
                                BooksList.Remove(book);
                                RentedBook rented = new RentedBook(book, date);
                                ReadersList[ReadersList.Count - 1].RentedBooksList.Add(rented);
                                break;
                            }
                        }
                    }
                    var a = ReadersList[0].RentedBooksList.Count;
                    string json = JsonConvert.SerializeObject(BooksList, Formatting.Indented);
                    File.WriteAllText(jsonFile, json);
                    json = JsonConvert.SerializeObject(ReadersList, Formatting.Indented);
                    File.WriteAllText(jsonReadersFile, json);
                    Console.WriteLine($"Successfully taken book with ISBN {id}.");
                }
            }
            else
            {
                Console.WriteLine($"Book with ISBN {id} does not exist.");
            }
        }

        public void ReturnBook(string id, string readerName)
        {
            Book book = null;
            for (int i = 0; i < ReadersList.Count; i++)
            {
                if (ReadersList[i].GetName() == readerName)
                {
                    for(int j = 0; j < ReadersList[i].RentedBooksList.Count; j++)
                    {
                        if(ReadersList[i].RentedBooksList[j].GetISBN() == id)
                        {
                            book = new Book(ReadersList[i].RentedBooksList[j]);
                            DateTime promisedReturnTime;
                            var hasParsed = DateTime.TryParse(ReadersList[i].RentedBooksList[j].GetReturnTime(), out promisedReturnTime);
                            if (!hasParsed)
                            {
                                Console.WriteLine("Wrong data format.");
                            }
                            //Message if late returning
                            if (DateTime.Now > promisedReturnTime)
                            {
                                Console.WriteLine("You have REALLY liked this one.");
                            }
                            ReadersList[i].RentedBooksList.RemoveAt(j);
                            BooksList.Add(book);
                            string json = JsonConvert.SerializeObject(ReadersList, Formatting.Indented);
                            File.WriteAllText(jsonReadersFile, json);
                            json = JsonConvert.SerializeObject(BooksList, Formatting.Indented);
                            File.WriteAllText(jsonFile, json);
                            Console.WriteLine($"Successfully returned book with ISBN {id}.");
                            break;
                        }
                    }
                }
            }
            if(book == null)
            {
                Console.WriteLine($"Book with ISBN {id} does not exist.");
            }
        }

        public void ListAllBooks(string filter)
        {
            if(filter == "Taken")
            {
                Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-20} {5,-10} {6,-10} \n",
                "Name", "Author", "Category", "Language", "Publication Date", "ISBN", "Return Time"));
                for (int i = 0; i < ReadersList.Count; i++)
                {
                    for (int j = 0; j < ReadersList[i].RentedBooksList.Count; j++)
                    {
                        Console.WriteLine(ReadersList[i].RentedBooksList[j].ToString());
                    }
                }
            }
            else
            {
                switch (filter)
                {
                    case "Name":
                        BooksList.Sort((x, y) => x.GetName().CompareTo(y.GetName()));
                        break;
                    case "Author":
                        BooksList.Sort((x, y) => x.GetAuthor().CompareTo(y.GetAuthor()));
                        break;
                    case "Category":
                        BooksList.Sort((x, y) => x.GetCategory().CompareTo(y.GetCategory()));
                        break;
                    case "Language":
                        BooksList.Sort((x, y) => x.GetLanguage().CompareTo(y.GetLanguage()));
                        break;
                    case "Date":
                        BooksList.Sort((x, y) => x.GetPublicationDate().CompareTo(y.GetPublicationDate()));
                        break;
                    case "ISBN":
                        BooksList.Sort((x, y) => x.GetISBN().CompareTo(y.GetISBN()));
                        break;
                    default:
                        break;
                }
                Console.WriteLine(string.Format("{0,-10} {1,-10} {2,-10} {3,-10} {4,-20} {5,-10} \n",
                "Name", "Author", "Category", "Language", "Publication Date", "ISBN"));
                for (int i = 0; i < BooksList.Count; i++)
                {
                    Console.WriteLine(BooksList[i].ToString());
                }
            }
        }
    }
}
