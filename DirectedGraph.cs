using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab2 {
	/// <summary>
	/// Directed graph class that supports modify operations. Represented on adjacency maps.
	/// </summary>
	/// <typeparam name="TVertex">The type of the vertices.</typeparam>
	/// <typeparam name="TCost">The type of the costs of vertices. They're not handled specially, so anything goes.</typeparam>
	public partial class DirectedGraph<TVertex, TCost> : IDirectedGraph<TVertex, TCost> {
		protected Dictionary<TVertex, List<TVertex>> _in;
		protected Dictionary<TVertex, List<TVertex>> _out;
		protected Dictionary<(TVertex, TVertex), TCost> _costs;

		public DirectedGraph() {
			_in = new Dictionary<TVertex, List<TVertex>>(EqualityComparer<TVertex>.Default);
			_out = new Dictionary<TVertex, List<TVertex>>(EqualityComparer<TVertex>.Default);
			_costs = new Dictionary<(TVertex, TVertex), TCost>();
		}

		public DirectedGraph(List<TVertex> vertices, List<(TVertex, TVertex, TCost)> edges) : this() {
			foreach (var v in vertices) {
				_in[v] = new List<TVertex>();
				_out[v] = new List<TVertex>();
			}
			foreach (var e in edges) {
				_out[e.Item1].Add(e.Item2);
				_in[e.Item2].Add(e.Item1);
				_costs[(e.Item1, e.Item2)] = e.Item3;
			}
		}

		public int VertexCount => _in.Count;

		public int EdgeCount => Vertices.Sum(v => OutboundVerticesOf(v).Count());

		public IEnumerable<TVertex> Vertices => _in.Keys;

		public bool IsVertex(TVertex name) {
			return _out.ContainsKey(name);
		}

		public void AddVertex(TVertex name) {
			if (IsVertex(name))
				throw new GraphException("Vertex already exists.");
			_in.Add(name, new List<TVertex>());
			_out.Add(name, new List<TVertex>());
		}

		public void AddEdge(TVertex start, TVertex end, TCost cost) {
			if (IsEdge(start, end))
				throw new GraphException("Edge already exists.");
			if (!IsVertex(start))
				AddVertex(start);
			if (!IsVertex(end))
				AddVertex(end);
			_out[start].Add(end);
			_in[end].Add(start);
			_costs.Add((start, end), cost);
		}

		public bool IsEdge(TVertex start, TVertex end) {
			if (!IsVertex(start))
				return false;
			return _out[start].Contains(end, EqualityComparer<TVertex>.Default);
		}

		public int InDegree(TVertex v) {
			try {
				return _in[v].Count;
			} 
			catch (KeyNotFoundException) {
				throw new GraphException($"Invalid vertex {v}.");
			}
		}

		public int OutDegree(TVertex v) {
			try {
				return _out[v].Count;
			}
			catch (KeyNotFoundException) {
				throw new GraphException($"Invalid vertex {v}.");
			}
		}

		public IEnumerable<TVertex> InboundVerticesOf(TVertex v) {
			try {
				return _in[v].ToList();
			}
			catch (KeyNotFoundException) {
				throw new GraphException($"Invalid vertex {v}.");
			}
		}

		public IEnumerable<TVertex> OutboundVerticesOf(TVertex v) {
			try {
				return _out[v].ToList();
			}
			catch (KeyNotFoundException) {
				throw new GraphException($"Invalid vertex {v}.");
			}
		}

		public TCost GetCostFor(TVertex s, TVertex e) {
			try {
				return _costs[(s, e)];
			}
			catch (KeyNotFoundException) {
				throw new GraphException($"Invalid edge ({s}, {e}).");
			}
		}

		public void SetCostFor(TVertex s, TVertex e, TCost c) {
			if (!IsEdge(s, e))
				throw new GraphException($"Invalid edge ({s}, {e}).");
			_costs[(s, e)] = c;
		}

		public void RemoveVertex(TVertex v) {
			if (!IsVertex(v))
				throw new GraphException($"Invalid vertex {v}.");
			foreach (var x in OutboundVerticesOf(v)) {
				RemoveEdge(v, x);
			}
			foreach (var x in InboundVerticesOf(v)) {
				RemoveEdge(x, v);
			}
			_in.Remove(v);
			_out.Remove(v);
		}

		public void RemoveEdge(TVertex v1, TVertex v2) {
			if (!IsEdge(v1, v2))
				throw new GraphException($"Invalid edge ({v1}, {v2}).");
			_in[v2].Remove(v1);
			_out[v1].Remove(v2);
			_costs.Remove((v1, v2));
		}

		public IDirectedGraph<TVertex, TCost> Copy() {
			var graph = new DirectedGraph<TVertex, TCost>();
			foreach (var v in Vertices)
				graph.AddVertex(v);
			foreach (var v1 in Vertices) {
				foreach (var v2 in OutboundVerticesOf(v1)) {
					graph.AddEdge(v1, v2, GetCostFor(v1, v2));
				}
			}
			return graph;
		}
	}
}
