using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CulinaryRecipes.DataAccess
{
	public class ApplicationSettings
	{
		public Uri Neo4jConnection { get; set; }

		public string Neo4jUser { get; set; }

		public string Neo4jPassword { get; set; }

		public string Neo4jDatabase { get; set; }
	}
}
