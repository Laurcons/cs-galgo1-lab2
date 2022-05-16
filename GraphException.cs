using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public class GraphException: Exception {
		public GraphException(string message) : base(message) { }
		public GraphException(string message, Exception inner) : base(message, inner) { }
	}
}
