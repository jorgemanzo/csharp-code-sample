﻿using System;
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
        public void addNewBookChangesReturnsSuccessful(string author, int pageCount, string title, bool expectInserted)
        {
            bool inserted = BooksService.addNewBook(author, pageCount, title);

            Assert.AreEqual(expectInserted, inserted);
        }

        [DataTestMethod]
        [DataRow("Brian Kernighan", 272, "The C Programming Language", "Programming", 1)]
        [DataRow("Gamma, Helm, Johnson, Vlissides", 395, "Design Patterns", "des", 1)]
        [DataRow("Steve Klabnik, Carol Nichols", 526, "The Rust Programming Language", "Lang", 2)]
        public void getBooksByTitleReturnsFilteredBooks(string author, int pageCount, string title, string searchKeyword, int expectedResultCount)
        {
            BooksService.addNewBook(author, pageCount, title);

            ObservableCollection<Book> results = BooksService.getBooksByTitle(searchKeyword);

            Assert.AreEqual(expectedResultCount, results.Count);
        }
    }
}
