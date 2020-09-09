using System;
using MySqlConnector;
using System.Collections.ObjectModel;


namespace CodeChallenge
{
    class MariaDB : DatabaseConnector
    {
        private static readonly string DEFAULT_CONNECTION_STRING = "server=0.0.0.0;user=USER;password=PASSWORD;database=DB";
        private static MySqlConnection mariaDB;

        public MariaDB(String connectionString)
        {
            if(String.IsNullOrWhiteSpace(connectionString))
            {
                mariaDB = new MySqlConnection(DEFAULT_CONNECTION_STRING);
            }
            else
            {
                mariaDB = new MySqlConnection(connectionString);
            }
        }

        public override ObservableCollection<Book> selectAllFromBooks()
        {

            ObservableCollection<Book> queryResult = new ObservableCollection<Book>();

            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM books;", mariaDB);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    queryResult.Add(new Book()
                    {
                        Id = (int)rdr[0],
                        Author = (string)rdr[1],
                        Title = (string)rdr[2],
                        PageCount = (int)rdr[3]
                    });
                }
                rdr.Close();
            }
            catch (MySqlException e)
            {

                Console.WriteLine(e.Message);
            }
            mariaDB.Close();

            return queryResult;
        }


        public override bool deleteByBook(Book toDelete)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("DELETE FROM books WHERE book_id = @book_id;", mariaDB);
                cmd.Parameters.AddWithValue("@book_id", toDelete.Id);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            mariaDB.Close();
            return true;
        }

        public override bool insertNewBook(string author, int pageCount, string title)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO books (author, title, page_count) VALUES (@author, @title, @page_count);", mariaDB);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@page_count", pageCount);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            mariaDB.Close();
            return true;
        }

        public override bool updateBookById(string author, int id, int pageCount, string title)
        {
            try
            {
                mariaDB.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE books SET author = @author, title = @title, page_count = @page_count WHERE book_id = @book_id;", mariaDB);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@book_id", id);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@page_count", pageCount);
                MySqlDataReader rdr = cmd.ExecuteReader();

                rdr.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            mariaDB.Close();
            return true;
        }

    }
}
