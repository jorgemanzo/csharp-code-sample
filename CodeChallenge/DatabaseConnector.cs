
using System.Collections.ObjectModel;

namespace CodeChallenge
{
    public abstract class DatabaseConnector
    {
        public abstract ObservableCollection<Book> selectAllFromBooks();

        public abstract bool deleteByBook(Book toDelete);

        public abstract bool insertNewBook(string author, int pageCount, string title);

        public abstract bool updateBookById(string author, int id, int pageCount, string title);
    }


}
