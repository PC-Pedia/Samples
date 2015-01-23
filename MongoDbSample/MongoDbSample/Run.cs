/*
 * Run mongoDb service with existing path for database:
 *		mongod.exe --dbpath D:\Proyectos\Samples\MongoDbSample\MongoDbSample\data
 */

using MongoDbSample.Views;
using MongoDbSample.Controllers;
using MongoDbSample.Models;

namespace MongoDbSample
{
	class Run
	{
		public static void Main()
		{
			var model = new Model();
			var controller = new Controller(model);
			var view = new ViewConsole(controller);

			view.Show();
		}
	}
}
