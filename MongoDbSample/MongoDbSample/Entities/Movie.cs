using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MongoDbSample.Entities
{
	public class Movie
	{
		[BsonId]
		public ObjectId Id { get; set; }
		public string Title { get; set; }
		public int Year { get; set; }
		public List<string> Tags { get; set; }
		public DateTime LastUpdate { get; set; }

		public Movie()
		{
			this.Tags = new List<string>();
		}
	}
}
