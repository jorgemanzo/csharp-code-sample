using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using CodeChallenge;


namespace CodeChallengeTests
{
    [TestClass]
    public class BookServiceTest
    {
        [DataTestMethod]
        [DataRow("", 0, "", false)]
        [DataRow(null, -1, null, false)]
        [DataRow("Brian Kernighan", 272, "The C Programming Language", true)]
        [DataRow("Gamma, Helm, Johnson, Vlissides", -1, "Design Patterns", false)]
        public void addNewBookChangesCount(string author, int pageCount, string title, bool expectInserted)
        {
            bool inserted = BooksService.addNewBook(author, pageCount, title);

            Assert.AreEqual(expectInserted, inserted);
        }

    }
}
