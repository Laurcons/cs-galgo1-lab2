using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public interface IDirectedGraph<TVertex, TCost> {
		/// <summary>
		/// Returns the number of vertices in the graph.
		/// </summary>
		int VertexCount { get; }

		/// <summary>
		/// Returns a list of all the vertices.
		/// </summary>
		IEnumerable<TVertex> Vertices { get; }

		bool IsVertex(TVertex v);

		/// <summary>
		/// Returns whether there exists an edge between the two vertices.
		/// </summary>
		bool IsEdge(TVertex start, TVertex end);

		/// <summary>
		/// Returns the inbound degree of the vertex.
		/// </summary>
		int InDegree(TVertex v);

		/// <summary>
		/// Returns the outbound degree of the vertex.
		/// </summary>
		int OutDegree(TVertex v);

		/// <summary>
		/// Returns a collection of all the vertices that have an inbound edge to the specified vertex.
		/// </summary>
		IEnumerable<TVertex> InboundVerticesOf(TVertex v);

		/// <summary>
		/// Returns a collection of all the vertices that have an outbound edge to the specified vertex.
		/// </summary>
		IEnumerable<TVertex> OutboundVerticesOf(TVertex v);

		TCost GetCostFor(TVertex s, TVertex e);
		void SetCostFor(TVertex v, TVertex e, TCost c);

		void AddVertex(TVertex v);
		void RemoveVertex(TVertex v);
		void AddEdge(TVertex v1, TVertex v2, TCost cost);
		void RemoveEdge(TVertex v1, TVertex v2);

		/// <summary>
		/// Creates and returns a deep copy of the graph.
		/// </summary>
		IDirectedGraph<TVertex, TCost> Copy();
	}
}
