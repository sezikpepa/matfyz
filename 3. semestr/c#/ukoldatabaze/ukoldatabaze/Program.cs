using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Nezarka
{
	//
	// Model
	//

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

		public static ModelStore LoadFrom(TextReader reader)
		{
			var store = new ModelStore();

			try
			{
				if (reader.ReadLine() != "DATA-BEGIN")
				{
					return null;
				}
				while (true)
				{
					string line = reader.ReadLine();
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
	}

	class Controller
	{
		public enum States
		{
			invalid,
			list_books,
			book_information,
			cart,
			end_requests
		}

		public static ModelStore store;

		private static bool AddItem(ShoppingCartItem item, int key)
		{
			if (item.BookId == key)
			{
				item.Count += 1;
				return true;
			}
			return false;
		}

		private static bool RemoveItem(ShoppingCartItem item, int key, Customer customer)
		{
			if (item.BookId == key)
			{
				item.Count -= 1;
				if (item.Count == 0)
					customer.ShoppingCart.Items.Remove(item);
				return true;
			}
			return false;
		}
		public static States LoadRequest(TextReader reader)
		{
			try
			{
				string record = reader.ReadLine();
				if (record == null)
				{
					return States.end_requests;
				}

				string[] tokens = record.Split(' ');
				if (tokens.Length > 3 || tokens[0] != "GET")
				{
					return States.invalid;
				}

				int customer_key = int.Parse(tokens[1]);
				Customer customer = store.GetCustomer(customer_key);

				if (customer == null)
				{
					return States.invalid;
				}
				if (tokens[2] == "http://www.nezarka.net/Books")
				{
					current_customer = customer;
					return States.list_books;

				}
				else
				{
					string[] parts = tokens[2].Split('/');
					int book_key = int.Parse(parts[5]);

					if (parts.Length > 6)
					{
						return States.invalid;
					}
					Book book = store.GetBook(book_key);

					string url_token = tokens[2];

					if (url_token.StartsWith("http://www.nezarka.net/Books/Detail/"))
					{

						if (book != null)
						{
							current_customer = customer;
							current_book = book;
							return States.book_information;
						}
						else
						{
							return States.invalid;
						}
					}
					else if (url_token == "http://www.nezarka.net/ShoppingCart")
					{
						current_customer = customer;
						return States.cart;
					}
					else if (url_token.StartsWith("http://www.nezarka.net/ShoppingCart/Add/"))
					{

						if (book == null)
						{
							return States.invalid;
						}
						else
						{
							bool added = false;
							foreach (ShoppingCartItem item in customer.ShoppingCart.Items)
							{
								added = AddItem(item, book_key);
								if (added) break;
							}
							if (!added) customer.ShoppingCart.Items.Add(new ShoppingCartItem { BookId = book_key, Count = 1 });
							current_customer = customer;
							return States.cart;
						}

					}
					else if (url_token.Contains("http://www.nezarka.net/ShoppingCart/Remove/"))
					{
						if (book == null)
						{
							return States.invalid;
						}
						else
						{
							bool removed = false;
							current_book = book;
							foreach (ShoppingCartItem item in customer.ShoppingCart.Items)
							{
								removed = RemoveItem(item, book_key, customer);
								if (removed) break;
							}
							if (!removed) return States.invalid;
						}
						current_customer = customer;
						return States.cart;
					}
					else
					{
						return States.invalid;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is FormatException || ex is IndexOutOfRangeException)
				{
					return States.invalid;
				}
				throw;
			}
		}
		public static Customer current_customer { get; set; }
		public static Book current_book { get; set; }
	}

	class View
	{
		public void ShoppingCart(Customer customer, ModelStore store)
		{
			Header(customer);
			if (customer.ShoppingCart.Items.Count == 0)
			{
				Program.writer.WriteLine("\tYour shopping cart is EMPTY.");
				Program.writer.WriteLine("</body>");
				Program.writer.WriteLine("</html>");
				return;
			}
			Program.writer.WriteLine("\tYour shopping cart:");
			Program.writer.WriteLine("\t<table>");
			Program.writer.WriteLine("\t\t<tr>");
			Program.writer.WriteLine("\t\t\t<th>Title</th>");
			Program.writer.WriteLine("\t\t\t<th>Count</th>");
			Program.writer.WriteLine("\t\t\t<th>Price</th>");
			Program.writer.WriteLine("\t\t\t<th>Actions</th>");
			Program.writer.WriteLine("\t\t</tr>");

			decimal full_price = 0;

			foreach (ShoppingCartItem item in customer.ShoppingCart.Items)
			{
				Book book = store.GetBook(item.BookId);
				full_price += item.Count * book.Price;
				Program.writer.WriteLine("\t\t<tr>");
				Program.writer.WriteLine("\t\t\t<td><a href=\"/Books/Detail/{0}\">{1}</a></td>", book.Id, book.Title);
				Program.writer.WriteLine("\t\t\t<td>{0}</td>", item.Count);

				if (item.Count > 1)
					Program.writer.WriteLine("\t\t\t<td>{0} * {1} = {2} EUR</td>", item.Count, book.Price, item.Count * book.Price);
				else
					Program.writer.WriteLine("\t\t\t<td>{0} EUR</td>", book.Price);

				Program.writer.WriteLine("\t\t\t<td>&lt;<a href=\"/ShoppingCart/Remove/{0}\">Remove</a>&gt;</td>", item.BookId);
				Program.writer.WriteLine("\t\t</tr>");
			}
			Program.writer.WriteLine("\t</table>");
			Program.writer.WriteLine("\tTotal price of all items: {0} EUR", full_price);
			Program.writer.WriteLine("</body>");
			Program.writer.WriteLine("</html>");
			return;
		}

		public void BookInfo(Book book, Customer customer)
		{
			Header(customer);
			Program.writer.WriteLine("\tBook details:");
			Program.writer.WriteLine("\t<h2>{0}</h2>", book.Title);
			Program.writer.WriteLine("\t<p style=\"margin-left: 20px\">");
			Program.writer.WriteLine("\tAuthor: {0}<br />", book.Author);
			Program.writer.WriteLine("\tPrice: {0} EUR<br />", book.Price);
			Program.writer.WriteLine("\t</p>");
			Program.writer.WriteLine("\t<h3>&lt;<a href=\"/ShoppingCart/Add/{0}\">Buy this book</a>&gt;</h3>", book.Id);
			Program.writer.WriteLine("</body>");
			Program.writer.WriteLine("</html>");
			return;
		}

		public void BookList(Customer customer, ModelStore store)
		{
			Header(customer);
			Program.writer.WriteLine("\tOur books for you:");
			Program.writer.WriteLine("\t<table>");

			int index = 0;
			IList<Book> books = store.GetBooks();
			foreach (Book book in books)
			{

				if (index == 0)
				{
					Program.writer.WriteLine("\t\t<tr>");
				}

				Program.writer.WriteLine("\t\t\t<td style=\"padding: 10px;\">");
				Program.writer.WriteLine("\t\t\t\t<a href=\"/Books/Detail/{0}\">{1}</a><br />", book.Id, book.Title);
				Program.writer.WriteLine("\t\t\t\tAuthor: {0}<br />", book.Author);
				Program.writer.WriteLine("\t\t\t\tPrice: {0} EUR &lt;<a href=\"/ShoppingCart/Add/{1}\">Buy</a>&gt;", book.Price, book.Id);
				Program.writer.WriteLine("\t\t\t</td>");

				if (index == 2)
				{
					Program.writer.WriteLine("\t\t</tr>");
				}

				index += 1;

				index = index % 3;
			}
			if ((index - 1) != 2 && books.Count != 0)
			{
				Program.writer.WriteLine("\t\t</tr>");
			}
			Program.writer.WriteLine("\t</table>");
			Program.writer.WriteLine("</body>");
			Program.writer.WriteLine("</html>");
			return;
		}
		public void Invalid()
		{

			Program.writer.WriteLine("<!DOCTYPE html>");
			Program.writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");

			Program.writer.WriteLine("<head>");
			Program.writer.WriteLine("\t<meta charset=\"utf-8\" />");
			Program.writer.WriteLine("\t<title>Nezarka.net: Online Shopping for Books</title>");
			Program.writer.WriteLine("</head>");

			Program.writer.WriteLine("<body>");
			Program.writer.WriteLine("<p>Invalid request.</p>");
			Program.writer.WriteLine("</body>");

			Program.writer.WriteLine("</html>");
			return;

		}
		public void Header(Customer customer)
		{
			int count = customer.ShoppingCart.Items.Count;

			Program.writer.WriteLine("<!DOCTYPE html>");
			Program.writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");

			Program.writer.WriteLine("<head>");
			Program.writer.WriteLine("\t<meta charset=\"utf-8\" />");
			Program.writer.WriteLine("\t<title>Nezarka.net: Online Shopping for Books</title>");
			Program.writer.WriteLine("</head>");

			Program.writer.WriteLine("<body>");
			Program.writer.WriteLine("\t<style type=\"text/css\">");
			Program.writer.WriteLine("\t\ttable, th, td {");
			Program.writer.WriteLine("\t\t\tborder: 1px solid black;");
			Program.writer.WriteLine("\t\t\tborder-collapse: collapse;");
			Program.writer.WriteLine("\t\t}");
			Program.writer.WriteLine("\t\ttable {");
			Program.writer.WriteLine("\t\t\tmargin-bottom: 10px;");
			Program.writer.WriteLine("\t\t}");
			Program.writer.WriteLine("\t\tpre {");
			Program.writer.WriteLine("\t\t\tline-height: 70%;");
			Program.writer.WriteLine("\t\t}");
			Program.writer.WriteLine("\t</style>");
			Program.writer.WriteLine("\t<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
			Program.writer.WriteLine("\t{0}, here is your menu:", customer.FirstName);
			Program.writer.WriteLine("\t<table>");
			Program.writer.WriteLine("\t\t<tr>");
			Program.writer.WriteLine("\t\t\t<td><a href=\"/Books\">Books</a></td>");
			Program.writer.WriteLine("\t\t\t<td><a href=\"/ShoppingCart\">Cart ({0})</a></td>", count);
			Program.writer.WriteLine("\t\t</tr>");
			Program.writer.WriteLine("\t</table>");
			return;
		}
	}
	class Program
	{

		static void ProcessRequests(ModelStore store, View view, TextReader reader)
		{
			bool valid_requests = true;
			Controller.States current_state;
			Controller.store = store;


			while (valid_requests)
			{
					/*
					current_state = Controller.LoadRequest(reader);
					switch (current_state)
					{
						case Controller.States.invalid:

							view.Invalid();
							break;

						case Controller.States.cart:
							view.ShoppingCart(Controller.current_customer, Controller.store);
							break;

						case Controller.States.list_books:						
							view.BookList(Controller.current_customer, Controller.store);
							break;

						case Controller.States.book_information:						
							view.BookInfo(Controller.current_book, Controller.current_customer);
							break;

						case Controller.States.end_requests:						
							valid_requests = false;
							break;						
					}
					*/
					
					current_state = Controller.LoadRequest(reader);

					if (current_state == Controller.States.invalid)
					{
						view.Invalid();
					}
					if (current_state == Controller.States.cart)
					{
						view.ShoppingCart(Controller.current_customer, Controller.store);
					}
					if (current_state == Controller.States.list_books)
					{
						view.BookList(Controller.current_customer, Controller.store);
					}
					if (current_state == Controller.States.book_information)
					{
						view.BookInfo(Controller.current_book, Controller.current_customer);
					}
					if (current_state == Controller.States.end_requests)
					{
						valid_requests = false;
						break;
					}
				

				Program.writer.WriteLine("====");
			}
			
		}
		public static System.IO.TextWriter writer;
		static void Main(string[] args)
		{
			View view = new View();

			System.IO.TextReader reader = System.Console.In;
			writer = System.Console.Out;

			ModelStore store = ModelStore.LoadFrom(reader);

			if (store != null)
			{
				ProcessRequests(store, view, reader);
			}
			else
			{
				Program.writer.WriteLine("Data error.");
			}
			return;
		}
	}
}