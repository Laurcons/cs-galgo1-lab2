using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lab2 {
	public static class Tests {

		private static void AssertThrows(Action action) {
			try {
				action.Invoke();
				Debug.Assert(false);
			} catch (Exception) {
				Debug.Assert(true);
			}
		}

		public static void TestAll() {
			TestWGCGraph();
			TestPuzzle15();

			Console.WriteLine("All tests pass.");
		}

		private static void TestWGCGraph() {

			Console.WriteLine("Testing WGCGraph");
			_ = new WGCGraph.Node("WGCB|");
			_ = new WGCGraph.Node("WC|BG");
			_ = new WGCGraph.Node("G|BWC");
			AssertThrows(() => _ = new WGCGraph.Node("asdf"));
			AssertThrows(() => _ = new WGCGraph.Node("asdf|"));
			AssertThrows(() => _ = new WGCGraph.Node("W|"));
			AssertThrows(() => _ = new WGCGraph.Node("WWCC|"));
			Debug.Assert(!new WGCGraph.Node("WG|BC").IsValid);
			Debug.Assert(!new WGCGraph.Node("GC|BW").IsValid);

			var n1 = new WGCGraph.Node("WCBG|");
			var n2 = new WGCGraph.Node("WGCB|");
			var n3 = new WGCGraph.Node("WC|BG");
			var n4 = new WGCGraph.Node("WCB|G");
			Debug.Assert(n1.Equals(n2));
			Debug.Assert(EqualityComparer<WGCGraph.Node>.Default.Equals(n1, n2));
			Debug.Assert(EqualityComparer<WGCGraph.Node>.Default.Equals(n2, n1));
			Debug.Assert(!EqualityComparer<WGCGraph.Node>.Default.Equals(n3, n2));
			Debug.Assert(!EqualityComparer<WGCGraph.Node>.Default.Equals(n2, n3));

			var graph = new WGCGraph();
			Debug.Assert(graph.IsVertex(new WGCGraph.Node("WC|BG")));
			Debug.Assert(graph.IsVertex(new WGCGraph.Node("CW|GB")));
			//Debug.Assert(graph.Graph.IsVertex(new WGCGraph.Node("|WGCB")));
			Debug.Assert(
				graph.IsEdge(n2, n3)
			);
			Debug.Assert(
				graph.IsEdge(n3, n4)
			);

		}

		public static void TestPuzzle15() {
			Console.WriteLine("Testing Puzzle15");

			_ = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 0, 15 });
			AssertThrows(() => _ = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 16, 14, 0, 15 }));
			AssertThrows(() => _ = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
			AssertThrows(() => _ = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }));
			AssertThrows(() => _ = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 }));

			var n1 = new Puzzle15Graph.Node();
			var n2 = new Puzzle15Graph.Node();
			var n3 = new Puzzle15Graph.Node(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 0, 15 });
			Debug.Assert(n1.ZeroPos == n2.ZeroPos);
			Debug.Assert(!n2.Equals(n3));
			Debug.Assert(n1.Equals(n2));
		}
	}
}
