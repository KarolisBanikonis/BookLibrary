using BookLibrary;
using NUnit.Framework;

namespace TestBookLibrary
{
    public class Tests
    {
        Library library;

        [SetUp]
        public void Setup()
        {
            library = new Library();
        }

        [Test]
        public void TestAddBook()
        {
            int beforeAddCount = library.BooksList.Count;
            library.AddBook("Harry Potter", "J.K.Rowling", "Fantasy", "English", "1997-06-26", "0-7475-3269-9");
            //Book is added to the list so the count should be bigger
            Assert.AreEqual(beforeAddCount + 1, library.BooksList.Count);
        }

        [Test]
        public void TestDeleteBook()
        {
            int beforeAddCount = library.BooksList.Count;
            library.DeleteBook("0-7475-3269-9");
            //Book is removed from the list so the count should be smaller
            Assert.AreEqual(beforeAddCount - 1, library.BooksList.Count);
        }

        [Test]
        public void TestDeleteNotExistingBook()
        {
            int beforeAddCount = library.BooksList.Count;
            library.DeleteBook("NotExisting");
            //Book is not existing so count should be the same
            Assert.AreEqual(beforeAddCount, library.BooksList.Count);
        }

        [Test]
        public void TestAddBookWithEmptyISBN()
        {
            int beforeAddCount = library.BooksList.Count;
            library.AddBook("TestName", "TestAuthor", "TestCategory", "TestLang", "1990-10-10", "");
            //Book is not added, so count should be the same
            Assert.AreEqual(beforeAddCount, library.BooksList.Count);
        }

        [Test]
        public void TestAddBookWithWrongDateFormat()
        {
            int beforeAddCount = library.BooksList.Count;
            library.AddBook("TestName", "TestAuthor", "TestCategory", "TestLang", "January 1th 2021", "TestISBN");
            //Book is not added, so count should be the same
            Assert.AreEqual(beforeAddCount, library.BooksList.Count);
        }

        [Test]
        public void TestAddingBookWithExistingISBN()
        {
            if (library.BooksList.Count > 0)
            {
                var beforeAddCount = library.BooksList.Count;
                var isbn = library.BooksList[0].GetISBN();
                library.AddBook("TestName", "TestAuthor", "TestCategory", "TestLang", "1990-10-10", isbn);
                Assert.AreEqual(beforeAddCount, library.BooksList.Count);
            }
            else
            {
                Assert.Pass("The book list is empty.");
            }
        }

        [Test]
        public void TestTakeBook()
        {
            if (library.BooksList.Count > 0)
            {
                if(library.ReadersList.Count > 0)
                {
                    var beforeBooksTakeCount = library.BooksList.Count;
                    var beforeReadersTakeCount = library.ReadersList[0].RentedBooksList.Count;
                    var isbn = library.BooksList[0].GetISBN();
                    var readerName = library.ReadersList[0].GetName();
                    library.TakeBook(isbn, readerName, "10");
                    Assert.Multiple(() =>
                    {
                        //Removed book from books list and added it to concrete reader's rented book list
                        Assert.AreEqual(beforeBooksTakeCount - 1, library.BooksList.Count);
                        Assert.AreEqual(beforeReadersTakeCount + 1, library.ReadersList[0].RentedBooksList.Count);
                    });
                }
                else
                {
                    Assert.Pass("The readers list is empty.");
                }
            }
            else
            {
                Assert.Pass("The book list is empty.");
            }
        }

        [Test]
        public void TestTakeBookWithWrongDateFormat()
        {
            if (library.BooksList.Count > 0)
            {
                var beforeBooksTakeCount = library.BooksList.Count;
                var isbn = library.BooksList[0].GetISBN();
                //Good date format is between 1-60
                library.TakeBook(isbn, "Reader", "100");
                //Book will not be taken so count is the same
                Assert.AreEqual(beforeBooksTakeCount, library.BooksList.Count);
            }
            else
            {
                Assert.Pass("The book list is empty.");
            }
        }

        [Test]
        public void TestReturnBook()
        {
            var beforeBooksTakeCount = library.BooksList.Count;
            var beforeReadersTakeCount = library.ReadersList[0].RentedBooksList.Count;
            library.ReturnBook(library.ReadersList[0].RentedBooksList[0].GetISBN(), library.ReadersList[0].GetName());
            Assert.Multiple(() =>
            {
                //Added book to books list and removed it from concrete reader's rented book list
                Assert.AreEqual(beforeBooksTakeCount + 1, library.BooksList.Count);
                Assert.AreEqual(beforeReadersTakeCount - 1, library.ReadersList[0].RentedBooksList.Count);
            });
        }

        [Test]
        public void TestTakeMoreThanThreeBooks()
        {
            if (library.BooksList.Count > 3)
            {
                if (library.ReadersList.Count > 0)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //Try to take 4 books
                        library.TakeBook(library.BooksList[0].GetISBN(), library.ReadersList[0].GetName(), "10");
                    }
                    //One reader can only have 3 books
                    Assert.AreEqual(library.ReadersList[0].RentedBooksList.Count, 3);
                }
                else
                {
                    Assert.Pass("The readers list is empty.");
                }
            }
            else
            {
                Assert.Pass("The book list does not have has enough books.");
            }
        }
    }
}