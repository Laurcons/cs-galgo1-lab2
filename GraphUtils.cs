using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public static class GraphUtils {
		public static void ToFile(string filename, DirectedGraph<int, int> graph) {
			var fs = File.OpenWrite(filename);
			var header = "";
			header += $"{graph.VertexCount} {graph.EdgeCount}\n";
			var headerBytes = Encoding.ASCII.GetBytes(header);
			fs.Write(headerBytes, 0, headerBytes.Length);
			foreach (var v1 in graph.Vertices) {
				foreach (var v2 in graph.OutboundVerticesOf(v1)) {
					string line = $"{v1} {v2} {graph.GetCostFor(v1, v2)}\n";
					var lineBytes = Encoding.ASCII.GetBytes(line);
					fs.Write(lineBytes, 0, lineBytes.Length);
				}
			}
			fs.Close();
		}

		public static DirectedGraph<int, int> FromFile(string filename) {
			var graph = new DirectedGraph<int, int>();
			var fs = File.OpenText(filename);
			var firstLine = fs.ReadLine();
			var headerParts = firstLine.Split(' ');
			var vertexCount = Convert.ToInt32(headerParts[0]);
			var edgeCount = Convert.ToInt32(headerParts[1]);
			for (int i = 0; i < vertexCount; i++)
				graph.AddVertex(i);
			for (int i = 0; i < edgeCount; i++) {
				var line = fs.ReadLine();
				var parts = line.Split(' ');
				var v1 = Convert.ToInt32(parts[0]);
				var v2 = Convert.ToInt32(parts[1]);
				var c = Convert.ToInt32(parts[2]);
				graph.AddEdge(v1, v2, c);
			}
			return graph;
		}

		public static DirectedGraph<int, int> NewRandom(int vertices, int edges) {
			var graph = new DirectedGraph<int, int>();
			if (vertices * vertices < edges)
				throw new GraphException($"Invalid configuration for directed graph: a {vertices}-vertex graph cannot have more than {vertices * vertices} edges (requested {edges} edges).");
			for (int i = 0; i < vertices; i++)
				graph.AddVertex(i);
			int remaining = edges;
			var rand = new Random();
			while (remaining > 0) {
				var v1 = rand.Next(vertices);
				var v2 = rand.Next(vertices);
				var c = rand.Next(1000);
				if (!graph.IsEdge(v1, v2)) {
					remaining--;
					graph.AddEdge(v1, v2, c);
				}
			}
			return graph;
		}
	}
}
