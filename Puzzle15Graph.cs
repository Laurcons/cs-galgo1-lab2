using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public class Puzzle15Graph : DirectedGraph<Puzzle15Graph.Node, Puzzle15Graph.Transformation> {
		private Node _destination;

		public class Transformation {
			public (int x, int y) From { get; set; }
			public (int x, int y) To { get; set; }

			public Transformation(int fx, int fy, int tx, int ty) {
				From = (fx, fy);
				To = (tx, ty);
			}
			public override string ToString() {
				return $"({From.x},{From.y})>({To.x},{To.y})";
			}
		}

		public class Node : IEquatable<Node> {
			private int[] _state;
			public int this[int row, int col] {
				get {
					return _state[4 * row + col];
				}
				set {
					_state[4 * row + col] = value;
				}
			}
			public (int x, int y) ZeroPos {
				get {
					for (int y = 0; y < 4; y++)
						for (int x = 0; x < 4; x++)
							if (this[x, y] == 0)
								return (x, y);
					throw new GraphException("Zero not found - invalid configuration for Node");
				}
			}
			public Node() {
				_state = new int[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0 };
			}
			public Node(Node other) {
				_state = new int[16];
				other._state.CopyTo(_state, 0);
			}
			public Node(int[] matrix) {
				if (!matrix.Distinct().All(Enumerable.Range(0, 16).Contains) || matrix.Length != 16)
					throw new GraphException("Invalid configuration for Node");
				_state = matrix;
			}
			public bool Equals(Node other) {
				return _state.SequenceEqual(other._state);
			}
			public override int GetHashCode() {
				int h = 0;
				foreach (var e in _state)
					h ^= e;
				return h;
			}
		}

		public Puzzle15Graph(Node destination) {
			_destination = destination;
			var initialState = new Node();
			var queue = new Queue<Node>();
			queue.Enqueue(initialState);
			while (queue.Count != 0) {
				var current = queue.Dequeue();
				// try permutations of the current state
				var zero = current.ZeroPos;
				for (int dy = -1; dy <= 1; dy++) {
					for (int dx = -1; dx <= 1; dx++) {
						if (Math.Abs(dx + dy) != 1)
							continue;
						int nx = zero.x + dx;
						int ny = zero.y + dy;
						if (!(nx >= 0 && nx < 4 && ny >= 0 && ny < 4))
							continue;
						Node modif = new Node(current);
						// do permutation
						int aux = modif[nx, ny];
						modif[nx, ny] = modif[zero.x, zero.y];
						modif[zero.x, zero.y] = aux;
						// add edge and maybe to the queue
						if (!IsEdge(current, modif)) {
							AddEdge(current, modif, new Transformation(zero.x, zero.y, nx, ny));
							if (modif.Equals(destination))
								return;
							else
								queue.Enqueue(modif);
						}
					}
				}
			}
		}

		public List<Transformation> FindSolution() {
			var nodes = LowestLengthPath(new Node(), _destination);
			var transfs = new List<Transformation>();
			for (int i = 0; i < nodes.Count - 1; i++) {
				transfs.Add(GetCostFor(nodes[i], nodes[i + 1]));
			}
			transfs.Reverse();
			return transfs;
		}

	}
}
