using System.Collections.Generic;

using MongoDbSample.Models;
using MongoDbSample.Entities;

namespace MongoDbSample.Controllers
{
	public class Controller
	{
		Model _model;

		public Controller(Model model)
		{
			_model = model;
		}

		public IEnumerable<Movie> ListMovies()
		{
			return _model.GetAll();
		}

		public void Save(Movie film)
		{
			_model.Save(film);
		}

		internal Movie Search(string idMovie)
		{
			throw new System.NotImplementedException();
		}
	}
}
