using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CodeChallenge
{

    public class BooksService
    {

        private ObservableCollection<Book> inventory;
        private static BooksService instance = null;
        private DatabaseConnector db = null;

        //
        // Instance methods
        //

        private BooksService(DatabaseConnector connector)
        {
            
            if (connector != null)
            {
                this.db = connector;
                inventory = this.db.selectAllFromBooks();
            }
            else
            {
                inventory = new ObservableCollection<Book>();
            }
        }

        private bool localRemoveBook(Book toRemove)
        {
            if (Singleton.inventory.Remove(toRemove))
            {
                System.Console.WriteLine("Removed");
                return true;
            }
            else
            {
                System.Console.WriteLine("Not Found");
                return false;
            }
        }

        private bool localAddNewBook(string author, int id, int pageCount, string title)
        {
            Book newBook = new Book()
            {
                Author = author,
                Title = title,
                PageCount = pageCount
            };
            // Keep the old ID if provided (such as during an update), or 
            // assign the new book a new ID (may not be unique).
            newBook.Id = id == 0 ? Singleton.inventory.Count + 1 : id;
            Singleton.inventory.Add(newBook);
            return true;
        }

        private static BooksService Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new BooksService(null);
                }
                return instance;
            }
        }

        //
        // Static methods
        //

        private static bool validateInput(string author, int pageCount, string title)
        {
            return !String.IsNullOrWhiteSpace(title) && !String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(author) && !String.IsNullOrEmpty(author) && pageCount > 0;
        }

        /// <summary>
        /// This class provides the locally stored inventory of books.
        /// If there is no current instance of the singleton, one will
        /// be created and will query the data store for records.
        /// </summary>
        public static ObservableCollection<Book> getBookInventory()
        {
            return Singleton.inventory;
        }


        /// <summary>
        /// This class queries the data store for all records, and replaces
        /// the locally stored collection with the results.
        /// </summary>
        public static ObservableCollection<Book> refreshInventory()
        {
            if (Singleton.db != null)
            {
                Singleton.inventory = Singleton.db.selectAllFromBooks();
            }
            return getBookInventory();
        }


        /// <summary>
        /// This and the following methods provide create / update / delete
        /// functionality. If using a database, these changes will be performed
        /// on the database, otherwise they will be applied to the local data
        /// store.
        /// </summary>
        public static bool removeBook(Book toRemove)
        {
            if(Singleton.db != null)
            {
                if(Singleton.inventory.Contains(toRemove))
                {
                    return Singleton.db.deleteByBook(toRemove);
                }
                return false;
            }
            else
            {
                return Singleton.localRemoveBook(toRemove);
            }
        }

        /// <summary>
        /// removeAllBooks() will remove all local books. This method has no effect if
        /// connected to a database. Returns true if sucessful.
        /// </summary>
        public static bool removeAllBooks()
        {
            if(Singleton.db == null)
            {
                Singleton.inventory = new ObservableCollection<Book>();
                return true;
            }
            return false;
        }


        /// <summary>
        /// A code-behind should use this method of adding a new book, as
        /// this will call the neighboring overloaded method, passing an id of 0
        /// to indicate that a new ID should be assigned to this book (unlike when
        /// an update is called and an ID is provided).
        /// </summary>
        public static bool addNewBook(string author,  int pageCount, string title)
        {
            return addNewBook(author, 0, pageCount, title);
        }

        private static bool addNewBook(string author, int id, int pageCount, string title)
        {
            if (validateInput(author, pageCount, title))
            {
                if(Singleton.db != null)
                {
                    return Singleton.db.insertNewBook(author, pageCount, title);
                }
                else
                {
                    return Singleton.localAddNewBook(author, id, pageCount, title);
                }
            }
            return false;
        }


        /// <summary>
        /// When updating a book without a database, the book is first removed from the local
        /// data store and then re-added with the new information, but the same ID. This is done
        /// to emulate what would be a database for inventory.
        /// TODO: Add input checking similar to that of addNewBook()
        /// </summary>
        public static bool updateBook(Book toReplace, string author, int id, int pageCount, string title)
        {
            if(validateInput(author, pageCount, title))
            {
                if (Singleton.db != null)
                {
                    return Singleton.db.updateBookById(author, id, pageCount, title);
                }
                else if (removeBook(toReplace))
                {
                    return addNewBook(author, id, pageCount, title);
                }
            }
            return false;
        }


        /// <summary>
        /// Locally filter the inventory by a book's title using LINQ.
        /// </summary>
        public static ObservableCollection<Book> getBooksByTitle(string title)
        {
            if(!string.IsNullOrWhiteSpace(title))
            {
                ObservableCollection<Book> result = new ObservableCollection<Book>();
                title = title.ToLower();
                var booksQuery =
                    from book in Singleton.inventory.ToList<Book>()
                    where book.Title.ToLower().Contains(title)
                    || book.Title.ToLower().StartsWith(title)
                    || book.Title.ToLower().EndsWith(title)
                    select book;

                foreach (Book book in booksQuery)
                {
                    result.Add(book);
                }
                return result;
            }
            else
            {
                return getBookInventory();
            }
        }
    }
}
