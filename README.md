# Generic C# Code Sample

Project uses the following third party packages:
- Xceed WPF extensions
- MySqlConnector

This is a simple book store inventory that has a simple data store: a simple list, or can be connected to a MySQL database. The structure of the app is seperated as follows:
- A XAML front-end with a simple dependency property based code-behind.
- `BookService` is a class that behaves like a singleton, providing access to the data store. This is done to relieve the code-behind from needing to keep an instance of a book store, thus simplifying it. This class is not threadsafe however, as the singleton is not impelemented to be thread safe, but merely convinient.
- `MariaDB` is a concrete impelementation of a `DatabaseConnector`. It implements the methods for opening the database connection and what queries to execute.

To use a database instead of a mocked data store:
1. Open `BookService.cs` and replace the `new BookService(null)` (line 74) constructor call in the `Singleton` property to the database connector of your choice. For example, replace with `new BookService(new MariaDB(connectionString))`.
2. Replace the `connectionString` with the information needed to connect to your database. See `MariaDB.cs` for an example.

A remote database should be created with a schema like this:
```
MariaDB [inventory]> SHOW COLUMNS FROM books;
+------------+---------+------+-----+---------+----------------+
| Field      | Type    | Null | Key | Default | Extra          |
+------------+---------+------+-----+---------+----------------+
| book_id    | int(11) | NO   | PRI | NULL    | auto_increment |
| author     | text    | NO   |     | NULL    |                |
| title      | text    | NO   |     | NULL    |                |
| page_count | int(11) | NO   |     | 0       |                |
+------------+---------+------+-----+---------+----------------+
```
