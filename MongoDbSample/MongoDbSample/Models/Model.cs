using System.Collections.Generic;

using MongoDB.Driver;
using MongoDbSample.Entities;

namespace MongoDbSample.Models
{
	public class Model
	{
		MongoCollection _collection;
		
		public Model()
		{
			MongoClient _client = new MongoClient();

			//MongoServer _server = MongoServer.Create(); //Obsolete
			MongoDatabase _database = _client.GetServer().GetDatabase("Video");

			_collection = _database.GetCollection<Movie>("Movies");

			//Enable index by tags
			_collection.EnsureIndex(new string[] { "tags" });
			
		}

		public IEnumerable<Movie> GetAll()
		{
			var movies = _collection.FindAllAs<Movie>();

			return movies;
		}

		public void Save(Movie film)
		{
			_collection.Save<Movie>(film);
		}
	}
}
