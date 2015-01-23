using System;
using System.Collections.Generic;

using MongoDbSample.Controllers;
using MongoDbSample.Entities;

namespace MongoDbSample.Views
{
	class ViewConsole
	{
		private Controller _controller;

		public ViewConsole(Controller controller)
		{
			_controller = controller;
		}

		public void Show()
		{
			string option = string.Empty;

			do
			{
				option = ShowMenu();

				RunOption(option);

				if (option != "0") { ShowFooter(); }

			} while (option != "0");
		}

		private string ShowMenu()
		{
			string retVal = string.Empty;

			Console.WriteLine("1. List all movies");
			Console.WriteLine("2. Add new movie");
			Console.WriteLine("3. Update movie");
			Console.WriteLine("4. Delete movie");
			Console.WriteLine("5. Find movie");
			Console.WriteLine("0. Exit");

			Console.Write("\n# Select option--> ");
			retVal = Console.ReadLine();


			return retVal;

		}

		private void ShowFooter()
		{
			Console.WriteLine("\nPress any key to return menu...");
			Console.ReadLine();
			Console.Clear();
		}

		private void RunOption(string option)
		{
			switch (option)
			{
				case "1":
					ListMovies();
					break;
				case "2":
					AddMovie();
					break;
				case "3":
					UpdateMovie();
					break;
				case "4":
					DeleteMovie();
					break;
				case "5":
					SearchMovie();
					break;
			}
		}

		private void ListMovies()
		{
			var movies = _controller.ListMovies();

			foreach (Movie film in movies)
			{
				ShowMovie(film);
			}
		}

		private void ShowMovie(Movie film)
		{
			string movie = String.Format("\nId: {0}\nTitle: {1}\nYear: {2}\nTags: {3}",
										film.Id,
										film.Title,
										film.Year,
										string.Join<string>(",", film.Tags));
			Console.WriteLine(movie);
		}

		private void AddMovie()
		{
			Movie movie = new Movie();
			string tag = string.Empty;

			Console.Write("Title: ");
			movie.Title = Console.ReadLine();

			Console.Write("Year: ");
			movie.Year = Int32.Parse(Console.ReadLine());

			Console.WriteLine("\nWrite tag name and press enter for add. Write . (dot) and press enter forn end.");
			do
			{
				Console.Write("Tag-> ");
				tag = Console.ReadLine();

				if (tag != ".")
					movie.Tags.Add(tag);

			} while (tag != ".");

			movie.LastUpdate = DateTime.UtcNow;

			_controller.Save(movie);
		}

		private void UpdateMovie()
		{
			string idMovie = string.Empty;
			Movie movieUpdate;

			Console.Write("Id: ");
			movieUpdate = _controller.Search(idMovie);

			if (movieUpdate == null)
				return;

			Console.Write(String.Format("Title [{0}]: ", movieUpdate.Title));
			movieUpdate.Title = UpdateValue<string>(movieUpdate.Title, Console.ReadLine());

			Console.WriteLine(String.Format("Year [{0}]: ", movieUpdate.Year));
			movieUpdate.Year = UpdateValue<int>(movieUpdate.Year, Console.ReadLine());


		}

		private TOut UpdateValue<TOut>(TOut originalValue, string newValue)
		{
			if (String.IsNullOrEmpty(newValue))
				return originalValue;

			return (TOut) Convert.ChangeType(newValue, typeof (TOut));
		}

		private void DeleteMovie()
		{
			//TODO
		}

		private void SearchMovie()
		{
			//TODO
		}
	}
}
