using System;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{

	class ModelStore
	{
		private List<Book> books = new List<Book>();
		private List<Customer> customers = new List<Customer>();

		public IList<Book> GetBooks()
		{
			return books;
		}

		public Book GetBook(int id)
		{
			return books.Find(b => b.Id == id);
		}

		public Customer GetCustomer(int id)
		{
			return customers.Find(c => c.Id == id);
		}

		public static ModelStore LoadFrom()
		{

			var store = new ModelStore();

			try
			{
				if (Console.ReadLine() != "DATA-BEGIN")
				{
					return null;
				}
				while (true)
				{
					string line = Console.ReadLine();
					if (line == null)
					{
						return null;
					}
					else if (line == "DATA-END")
					{
						break;
					}

					string[] tokens = line.Split(';');
					switch (tokens[0])
					{
						case "BOOK":
							store.books.Add(new Book
							{
								Id = int.Parse(tokens[1]),
								Title = tokens[2],
								Author = tokens[3],
								Price = decimal.Parse(tokens[4])
							});
							break;
						case "CUSTOMER":
							store.customers.Add(new Customer
							{
								Id = int.Parse(tokens[1]),
								FirstName = tokens[2],
								LastName = tokens[3]
							});
							break;
						case "CART-ITEM":
							var customer = store.GetCustomer(int.Parse(tokens[1]));
							if (customer == null)
							{
								return null;
							}
							customer.ShoppingCart.Items.Add(new ShoppingCartItem
							{
								BookId = int.Parse(tokens[2]),
								Count = int.Parse(tokens[3])
							});
							break;
						default:
							return null;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is FormatException || ex is IndexOutOfRangeException)
				{
					//Console.WriteLine("yes error");
					return null;
				}
				throw;
			}

			return store;

		}
	}

	class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public decimal Price { get; set; }
	}

	class Customer
	{
		private ShoppingCart shoppingCart;

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public ShoppingCart ShoppingCart
		{
			get
			{
				if (shoppingCart == null)
				{
					shoppingCart = new ShoppingCart();
				}
				return shoppingCart;
			}
			set
			{
				shoppingCart = value;
			}
		}
	}

	class ShoppingCartItem
	{
		public int BookId { get; set; }
		public int Count { get; set; }
	}

	class ShoppingCart
	{
		public int CustomerId { get; set; }
		public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();

		public void AddOne(int bookId)
        {
			foreach(var element in this.Items)
            {
				if(element.BookId == bookId)
                {
					element.Count++;
					return;
                }
            }

			ShoppingCartItem item = new ShoppingCartItem();
			item.BookId = bookId;
			item.Count = 1;
			this.Items.Add(item);
        }

		public bool RemoveOne(int bookId)
        {
			foreach (var element in this.Items)
			{
				if (element.BookId == bookId)
				{
					element.Count--;
					if(element.Count == 0)
                    {
						this.Items.Remove(element);
                    }
					return true;
				}
			}
			return false;
		}
	}

	class BookHtmlGenerator
	{

		public void printSingleBook(Book book)
		{
			Console.WriteLine("\tBook details:");
			Console.WriteLine("\t<h2>" + book.Title + "</h2>");
			Console.WriteLine("\t<p style=\"margin-left: 20px\">");
			Console.WriteLine("\tAuthor: " + book.Author + "<br />");
			Console.WriteLine("\tPrice: " + book.Price + " EUR<br />");
			Console.WriteLine("\t</p>");
            Console.WriteLine("\t<h3>&lt;<a href=\"/ShoppingCart/Add/" + book.Id + "\">Buy this book</a>&gt;</h3>");
		}

		public void printBooks(IList<Book> books)
		{
            Console.WriteLine("\tOur books for you:");
			this.startTable();
			for (int i = 0; i < books.Count; i++)
			{
				if (i % 3 == 0)
				{
					Console.WriteLine("\t\t<tr>");
				}
				////////////////////////////////
				this.printBook(books[i]);

				if (i % 3 == 2)
				{
					Console.WriteLine("\t\t</tr>");
				}
			}
			if(books.Count > 0)
            {
				if (books.Count - 1 % 3 != 2)
				{
					Console.WriteLine("\t\t</tr>");
				}
			}
			

			this.endTable();
		}

		private void startTable()
		{
			Console.WriteLine("\t<table>");
		}

		private void endTable()
		{
			Console.WriteLine("\t</table>");
		}

		private void startRow()
		{
			Console.WriteLine("\t\t<tr>");
		}

		private void endRow()
		{
			Console.WriteLine("\t\t</tr>");
		}

		private void printBook(Book book)
		{
			Console.WriteLine("\t\t\t<td style=\"padding: 10px;\">");
			Console.WriteLine("\t\t\t\t<a href=\"/Books/Detail/"+ book.Id + "\">" + book.Title + "</a><br />");
			Console.WriteLine("\t\t\t\tAuthor: " + book.Author + "<br />");
			Console.WriteLine("\t\t\t\tPrice: " + book.Price + " EUR &lt;<a href=\"/ShoppingCart/Add/" + book.Id + "\">Buy</a>&gt;");
			Console.WriteLine("\t\t\t</td>");
		}

	}

	class ShoppingCartGenerator
    {
		public void printShoppingCart(Customer customer, IList<Book> books)
        {

			if(customer.ShoppingCart.Items.Count == 0)
            {
                Console.WriteLine("\tYour shopping cart is EMPTY.");
				return;
            }

			decimal finalSum = 0;

            Console.WriteLine("\tYour shopping cart:");
			this.startTable();
			this.generateCartHeading();

			foreach(var element in customer.ShoppingCart.Items)
            {
				foreach(var book in books)
                {
					if(book.Id == element.BookId)
                    {
						this.printItem(element, book);
						finalSum += book.Price * element.Count;
						break;
					}
                }

				
            }

			this.endTable();
			Console.WriteLine("\tTotal price of all items: " + finalSum + " EUR");


        }

		private void printItem(ShoppingCartItem item, Book book)
        {
			this.startRow();
            Console.WriteLine("\t\t\t<td><a href=\"/Books/Detail/" + item.BookId + "\">" + book.Title + "</a></td>");
			Console.WriteLine("\t\t\t<td>"+ item.Count + "</td>");
			if(item.Count > 1)
				Console.WriteLine("\t\t\t<td>" + item.Count + " * " + book.Price + " = " + (item.Count * book.Price) + " EUR" + "</td>");
			else
				Console.WriteLine("\t\t\t<td>" + book.Price + " EUR" + "</td>");

			Console.WriteLine("\t\t\t<td>&lt;<a href=\"/ShoppingCart/Remove/" + item.BookId + "\">Remove</a>&gt;</td>");
			this.endRow();
		}

		private void generateCartHeading()
        {
			this.startRow();
			Console.WriteLine("\t\t\t<th>Title</th>");
			Console.WriteLine("\t\t\t<th>Count</th>");
			Console.WriteLine("\t\t\t<th>Price</th>");
			Console.WriteLine("\t\t\t<th>Actions</th>");
			this.endRow();
		}

		private void startTable()
		{
			Console.WriteLine("\t<table>");
		}

		private void endTable()
		{
			Console.WriteLine("\t</table>");
		}

		private void startRow()
		{
			Console.WriteLine("\t\t<tr>");
		}

		private void endRow()
		{
			Console.WriteLine("\t\t</tr>");
		}
	}

	class HtmlGenerator
	{
		private BookHtmlGenerator bookGenerator;
		private ShoppingCartGenerator shoppingCartGenerator;
		public HtmlGenerator()
		{
			this.bookGenerator = new();
			this.shoppingCartGenerator = new();
		}

		public void printHtml(bool includeHeading)
		{
			this.printHtmlStart();

			this.printCssStyle();
			if (includeHeading)
			{
				//this.printHeading();
			}

			this.printHtmlEnd();

		}

		public void printWrongOrder()
        {
			this.printHtmlStart();
            Console.WriteLine("<p>Invalid request.</p>");
			this.printHtmlEnd();
        }

		public void printShoppingCart(Customer customer, IList<Book> books)
        {
			this.printHtmlStart();
			this.printCssStyle();
			this.printHeading(customer);
			this.shoppingCartGenerator.printShoppingCart(customer, books);
			this.printHtmlEnd();
		}

		public void printBooks(IList<Book> books, Customer customer)
		{
			this.printHtmlStart();
			this.printCssStyle();
			this.printHeading(customer);
			this.bookGenerator.printBooks(books);
			this.printHtmlEnd();
		}

		public void printSingleBook(Book book, Customer customer)
        {
			this.printHtmlStart();
			this.printCssStyle();
			this.printHeading(customer);
			this.bookGenerator.printSingleBook(book);
			this.printHtmlEnd();
        }

		public void printBook(Book book)
		{
			this.bookGenerator.printSingleBook(book);
		}

		public void printHtmlStart()
		{
			Console.WriteLine("<!DOCTYPE html>");
			Console.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
			Console.WriteLine("<head>");
			Console.WriteLine("\t<meta charset=\"utf-8\" />");
			Console.WriteLine("\t<title>Nezarka.net: Online Shopping for Books</title>");
			Console.WriteLine("</head>");
			Console.WriteLine("<body>");
		}

		public void printHtmlEnd()
		{
			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}

		public void printCssStyle()
		{
			Console.WriteLine("\t<style type=\"text/css\">");

			Console.WriteLine("\t\ttable, th, td {");
			Console.WriteLine("\t\t\tborder: 1px solid black;");
			Console.WriteLine("\t\t\tborder-collapse: collapse;");
			Console.WriteLine("\t\t}");

			Console.WriteLine("\t\ttable {");
			Console.WriteLine("\t\t\tmargin-bottom: 10px;");
			Console.WriteLine("\t\t}");

			Console.WriteLine("\t\tpre {");
			Console.WriteLine("\t\t\tline-height: 70%;");
			Console.WriteLine("\t\t}");

			Console.WriteLine("\t</style>");
		}

		public void printHeading(Customer customer)
		{
			ShoppingCart shoppingCart = customer.ShoppingCart;

            Console.WriteLine("\t<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
			Console.WriteLine("\t" + customer.FirstName + ", here is your menu:");
			Console.WriteLine("\t<table>");
			Console.WriteLine("\t\t<tr>");
			Console.WriteLine("\t\t\t<td><a href=\"/Books\">Books</a></td>");
			Console.WriteLine("\t\t\t<td><a href=\"/ShoppingCart\">Cart ("+ shoppingCart.Items.Count +")</a></td>");
			Console.WriteLine("\t\t</tr>");
			Console.WriteLine("\t</table>");
		}
	}

	class OrderFromCommandLineParser
	{
		public int userId;
		public string relevantOrder;
		public bool invalid = false;

		public OrderFromCommandLineParser(string line)
        {
			string[] parts = line.Split(' ');
			string rest;
            try
            {
				rest = parts[2];
			}
            catch
            {
				this.invalid = true;
				return;
            }
			
			rest = rest.Remove(0, 22);
			this.relevantOrder = rest;
			bool validity = this.checkValidityOrder(parts);
			if(validity == false)
            {
				this.invalid = true;
				return;
            }
		
			this.userId = Int32.Parse(parts[1]);

			
        }

		private bool checkValidityOrder(string[] parts)
        {
            try
            {
				if (parts.Length != 3)
					return false;

				if (parts[0] != "GET")
					return false;

				try
				{
					Int32.Parse(parts[1]);
				}
				catch
				{
					return false;
				}

				if (!parts[2].StartsWith("http://www.nezarka.net"))
					return false;

				parts[2] = parts[2].Remove(0, 22);
				string[] urlParts = parts[2].Split('/');

				if (urlParts.Length < 2)
				{
					return false;
				}

				if (urlParts[1] == "Books")
				{
					if (urlParts.Length == 2)
						return true;
					if (urlParts.Length == 3)
					{
						return false;
					}
					if (urlParts.Length == 4)
					{
						if (urlParts[2] == "Detail")
						{
							try
							{
								Int32.Parse(urlParts[3]);
							}
							catch
							{
								return false;
							}
							return true;
						}
					}
					return false;
				}

				else if (urlParts[1] == "ShoppingCart")
				{
					if (urlParts.Length == 2)
						return true;
					if (urlParts.Length == 3)
					{
						return false;
					}
					if (urlParts.Length == 4)
					{
						if (urlParts[2] == "Add" || urlParts[2] == "Remove")
						{
							try
							{
								Int32.Parse(urlParts[3]);
							}
							catch
							{
								return false;
							}
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				else
				{
					return false;
				}

				return true;
			}
            catch
            {
				return false;
            }
			
        }

		public int getBookId()
        {
			string[] parts = relevantOrder.Split('/');
            try
            {
				return Int32.Parse(parts[3]);
			}
            catch
            {
				return -1;
            }

		}

		public string getShoppingCartOrder()
        {
			string[] parts = relevantOrder.Split('/');
            try
            {
				return parts[2];
			}
            catch
            {
				return "";
            }
		}


	}

	class OrderHandler
    {
		public ModelStore store;
		public HtmlGenerator htmlGenerator;
		public bool stop = false;
		public OrderHandler()
        {
			this.store = ModelStore.LoadFrom();

			if (this.store == null)
			{
				Console.WriteLine("Data error.");
				this.stop = true;
				return;
			}

			this.htmlGenerator = new();
		}

		private void singleBookOrder()
        {

        }

		public void processOrders()
        {
			if (this.stop)
				return;
			while (true)
			{
				string line = Console.ReadLine();
				if (line == null)
				{
					break;
				}

				OrderFromCommandLineParser parser = new(line);
				if (parser == null)
				{
					htmlGenerator.printWrongOrder();
					Console.WriteLine("====");
					continue;
				}

				Customer customer = store.GetCustomer(parser.userId);

				if (customer == null)
				{
					htmlGenerator.printWrongOrder();
					Console.WriteLine("====");
					continue;
				}



				if (parser.relevantOrder.StartsWith("/Books/Detail"))
				{
					Book book = store.GetBook(parser.getBookId());
					if (book == null)
					{
						htmlGenerator.printWrongOrder();
						Console.WriteLine("====");
						continue;
					}

					htmlGenerator.printSingleBook(book, customer);
				}

				else if (parser.relevantOrder.StartsWith("/ShoppingCart"))
				{
					if (parser.getShoppingCartOrder() == "")
					{
						htmlGenerator.printShoppingCart(customer, store.GetBooks());
					}
					else if (parser.getShoppingCartOrder() == "Add")
					{


						int bookId = parser.getBookId();
						Book book = store.GetBook(bookId);
						if (book == null)
						{
							htmlGenerator.printWrongOrder();
							Console.WriteLine("====");
							continue;
						}
						customer.ShoppingCart.AddOne(bookId);

						htmlGenerator.printShoppingCart(customer, store.GetBooks());
					}
					else if (parser.getShoppingCartOrder() == "Remove")
					{
						Book book = store.GetBook(parser.getBookId());
						if (book == null)
						{
							htmlGenerator.printWrongOrder();
							Console.WriteLine("====");
							continue;
						}
						bool success = customer.ShoppingCart.RemoveOne(parser.getBookId());
						if (success == true)
							htmlGenerator.printShoppingCart(customer, store.GetBooks());
						else
						{
							htmlGenerator.printWrongOrder();
							Console.WriteLine("====");
							continue;
						}
					}
				}

				else if (parser.relevantOrder == "/Books")
				{
					htmlGenerator.printBooks(store.GetBooks(), customer);
				}
				Console.WriteLine("====");
			}
		}
    }



	internal class Program
    {
        static void Main(string[] args)
        {
			OrderHandler orderHandler = new();
			orderHandler.processOrders();
        }
    }
}