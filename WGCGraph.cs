using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public class WGCGraph : DirectedGraph<WGCGraph.Node, short> {

		public class Node : IEquatable<Node> {
			public string Config { get; private set; }
			public Node(string config) {
				if (!config.All("WGCB|".Contains) ||
					config.Length != 5 ||
					new string(config.Distinct().ToArray()) != config
					) {
					throw new Exception("Invalid WGC configuration");
				}
				var parts = config.Split('|');
				parts = parts.Select(p => new string(p.OrderBy(c => c).ToArray())).ToArray();
				Config = string.Join("|", parts);
			}

			public bool IsValid {
				get {
					var parts = Config.Split('|');
					foreach (var part in parts) {
						if (part.Contains('W') && part.Contains('G') && !part.Contains('B'))
							return false;
						if (part.Contains('G') && part.Contains('C') && !part.Contains('B'))
							return false;
					}
					return true;
				}
			}

			public bool Equals(Node other) {
				return
					Config.Split('|')[0].SequenceEqual(other.Config.Split('|')[0])
					&&
					Config.Split('|')[1].SequenceEqual(other.Config.Split('|')[1]);
			}

			public override int GetHashCode() {
				var parts = Config.Split('|');
				if (parts.Length == 1)
					parts = new string[] { parts[0], "" };
				int h1 = parts[0].GetHashCode();
				int h2 = parts[1].GetHashCode();
				return h1 & h2;
			}

			public override string ToString() => Config;
		}

		public WGCGraph() {
			// generate graph
			var initialState = new Node("WGCB|");
			var queue = new Queue<Node>();
			queue.Enqueue(initialState);
			while (queue.Count != 0) {
				var current = queue.Dequeue();
				var parts = current.Config.Split('|');
				var left = parts[0];
				var right = parts[1];
				// iterate through all the possible transformations and add edges to them from the current node
				foreach (var o in " WGC") {
					// space is a special object, it is treated as "boat moves empty"
					Node n;
					// if the boat is in the left, move it (along with o) to the right
					if (left.Contains('B') && (left.Contains(o) || o == ' '))
						// do the movement. it is string-based, so this is doing string trickery
						n = new Node(
							left.Replace(o.ToString(), "").Replace("B", "")
							+ "|" +
							right + "B" + o.ToString().Trim()
						);
					// if the boat is in the right, move it (along with o) to the left
					else if (right.Contains('B') && (right.Contains(o) || o == ' '))
						n = new Node(
							left + "B" + o.ToString().Trim()
							+ "|" +
							right.Replace(o.ToString(), "").Replace("B", "")
						);
					else
						continue;
					// if our movement is valid (no one eats no one), and doesn't exist already, add it to the graph
					if (n.IsValid && !IsEdge(current, n)) {
						AddEdge(current, n, 0);
						queue.Enqueue(n);
					}
				}
			}
		}

		public List<Node> FindSolution() {
			return LowestLengthPath(
				new Node("WGCB|"),
				new Node("|WGCB")
			);
		}
		
	}
}
