using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	class Program {
		static void Main(string[] args) {

			Tests.TestAll();

			var graph = new DirectedGraph<int, int>();
			Console.WriteLine("Initialized new graph with no vertices.");

			var options = new Dictionary<int, Action<System.Diagnostics.Stopwatch>>() {
				{ 1, (s) => {
					// read graph from file
					var path = ConsoleEx.Input("File path: ");
					s.Start();
					graph = GraphUtils.FromFile(path);
					s.Stop();
					Console.WriteLine("Successfully read from file.");
				} },
				{ 2, (s) => {
					// write graph to file
					var path = ConsoleEx.Input("File path: ");
					s.Start();
					GraphUtils.ToFile(path, graph);
					s.Stop();
					Console.WriteLine("Successfully written to file.");
				} },
				{ 3, (s) => {
					// generate random graph
					var config = ConsoleEx
						.Input("Number of vertices, and edges, separated by space: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					s.Start();
					graph = GraphUtils.NewRandom(config[0], config[1]);
					s.Stop();
					Console.WriteLine("Successfully generated random graph.");
				} },
				{ 4, (s) => {
					s.Start();
					// print graph
					foreach (var v1 in graph.Vertices) {
						Console.WriteLine("{0} -> {1}", v1, string.Join(", ", graph.OutboundVerticesOf(v1)));
					}
					Console.WriteLine("Graph has {0} vertices with {1} edges", graph.VertexCount, graph.EdgeCount);
					s.Stop();
				} },
				{ 5, (s) => {
					// check if edge exists
					var config = ConsoleEx
						.Input("Edge endpoints, separated by space: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					s.Start();
					Console.WriteLine("({0},{1}) is{2} an edge in the graph.",
						config[0],
						config[1],
						graph.IsEdge(config[0], config[1]) ? "" : " NOT");
					s.Stop();
				} },
				{ 6, (s) => {
					// get in/out degree and edges of a vertex
					var vertex = Convert.ToInt32(ConsoleEx.Input("Vertex: "));
					s.Start();
					Console.WriteLine("Vertex {0} has in-degree {1} and out-degree {2}.",
						vertex,
						graph.InDegree(vertex),
						graph.OutDegree(vertex));
					Console.WriteLine("{0} -> {1}",
						vertex,
						string.Join(", ", graph.OutboundVerticesOf(vertex)));
					Console.WriteLine("{0} <- {1}",
						vertex,
						string.Join(", ", graph.InboundVerticesOf(vertex)));
					s.Stop();
				} },
				{ 7, (s) => {
					// get/set cost for edge
					var config = ConsoleEx
						.Input("Edge endpoints, followed by new cost if setting, or nothing if getting: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					if (config.Length == 2) {
						// get cost
						s.Start();
						Console.WriteLine($"Cost for edge ({config[0]}, {config[1]}) is {graph.GetCostFor(config[0], config[1])}.");
						s.Stop();
					} else {
						// set cost
						s.Start();
						graph.SetCostFor(config[0], config[1], config[2]);
						s.Stop();
						Console.WriteLine($"Cost for edge ({config[0]}, {config[1]}) has been set to {graph.GetCostFor(config[0], config[1])}.");
					}
				} },
				{ 8, (s) => {
					// add vertex or edge
					var config = ConsoleEx
						.Input("New vertex name, or new edge endpoints with cost separated by spaces: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					if (config.Length == 1) {
						// add vertex
						s.Start();
						graph.AddVertex(config[0]);
						s.Stop();
						Console.WriteLine($"Added new vertex {config[0]}");
					} else {
						// add edge
						s.Start();
						graph.AddEdge(config[0], config[1], config[2]);
						s.Stop();
						Console.WriteLine($"Added new edge ({config[0]}, {config[1]}, {config[2]})");
					}
				} },
				{ 9, (s) => {
					// remove vertex or edge
					var config = ConsoleEx
						.Input("Vertex name, or edge endpoints separated by space: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					if (config.Length == 1) {
						// remove vertex
						s.Start();
						graph.RemoveVertex(config[0]);
						s.Stop();
						Console.WriteLine($"Removed vertex {config[0]}");
					} else {
						// remove edge
						s.Start();
						graph.RemoveEdge(config[0], config[1]);
						s.Stop();
						Console.WriteLine($"Removed edge ({config[0]}, {config[1]})");
					}
				} },
				{ 10, (s) => {
					// lowest length path
					var config = ConsoleEx
						.Input("Vertex pair, separated by space: ")
						.Split(' ')
						.Select(x => Convert.ToInt32(x))
						.ToArray();
					s.Start();
					var path = graph.LowestLengthPath(config[0], config[1]);
					s.Stop();
					Console.WriteLine("Lowest length path: {0}\nLength: {1}",
						string.Join(" ", path),
						path.Count - 1);
				} },
				{ 11, (s) => {
					// solve wgc
					Console.WriteLine("Initial configuration: WGCB|");
					Console.WriteLine("Target configuration: |WGCB");
					s.Start();
					var wgcgraph = new WGCGraph();
					var path = wgcgraph.FindSolution();
					s.Stop();
					Console.WriteLine("Generated graph:");
					foreach (var v1 in wgcgraph.Vertices) {
						Console.WriteLine("{0} -> {1}", v1, string.Join(", ", wgcgraph.OutboundVerticesOf(v1)));
					}
					Console.WriteLine("Graph has {0} vertices with {1} edges", wgcgraph.VertexCount, wgcgraph.EdgeCount);
					Console.WriteLine("Solution:");
					Console.WriteLine(string.Join(" -> ", path));
				} },
				{ 12, (s) => {
					// solve 15 puzzle
					var input = ConsoleEx.Input("Starting puzzle configuration, or a filename without spaces to take it from: ");
					var parts = input.Split(' ');
					if (parts.Length != 16) {
						parts = System.IO.File.ReadAllText(input).Split(' ');
						Console.WriteLine("Solving {0}", string.Join(", ", parts));
					}
					int[] matrix = parts.Select(x => int.Parse(x)).ToArray();
					var dest = new Puzzle15Graph.Node(matrix);
					s.Start();
					Console.WriteLine("Generating graph");
					var p15graph = new Puzzle15Graph(dest);
					Console.WriteLine("Finding shortest path");
					var transfs = p15graph.FindSolution();
					s.Stop();
					Console.WriteLine("Solution:");
					Console.WriteLine(string.Join(" then ", transfs));
				} }
			};

			var stopwatch = new System.Diagnostics.Stopwatch();

			while (true) {
				try {
					Console.WriteLine(
					"\n\n" +
					"What do you want to do?\n" +
					"1. Read graph from file\n" +
					"2. Write graph to file\n" +
					"3. Generate random graph\n" +
					"4. Print graph\n" +
					"5. Check if edge exists\n" +
					"6. Get in/out degree and edges of a vertex\n" +
					"7. Get/set cost for edge\n" +
					"8. Add vertex or edge\n" +
					"9. Remove vertex or edge\n" +
					"10. Lowest length path\n" +
					"11. [BONUS] Solve Wolf, Goat, Cabbage problem\n" +
					"12. [BONUS] Solve 15 Puzzle\n" +
					"x. Exit program");
					dynamic option = Console.ReadLine();
					if (option == "x")
						break;
					option = Convert.ToInt32(option);
					options[option].Invoke(stopwatch);
					if (stopwatch.IsRunning) {
						ConsoleEx.WriteColoredLine(ConsoleColor.Yellow, "Warning: Stopwatch not stopped by operation");
						stopwatch.Stop();
					}
					ConsoleEx.WriteColoredLine(ConsoleColor.Blue, "Operation took {0}ms.", stopwatch.ElapsedMilliseconds);
					stopwatch.Reset();
				}
				catch (GraphException gex) {
					ConsoleEx.WriteErrorLine($"Graph error: {gex.Message}");
				}
				catch (Exception ex) when (ex is FormatException || ex is KeyNotFoundException) {
					ConsoleEx.WriteErrorLine("Invalid option.");
				}
				catch (Exception ex) {
					ConsoleEx.WriteErrorLine($"Critical: {ex.Message}");
				}
			}

		}
	}

	public static class ConsoleEx {
		public static string Input(string prompt) {
			Console.Write(prompt);
			return Console.ReadLine();
		}

		public static void WriteErrorLine(string format, params object[] args) {
			WriteColoredLine(ConsoleColor.Red, format, args);
		}

		public static void WriteColoredLine(ConsoleColor color, string format, params object[] args) {
			var prev = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(format, args);
			Console.ForegroundColor = prev;
		}
	}
}
