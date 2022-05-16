using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2 {
	public partial class DirectedGraph<TVertex, TCost> {

		public List<TVertex> LowestLengthPath(TVertex from, TVertex to) {

			var visited = new Dictionary<TVertex, bool>();
			var distance = new Dictionary<TVertex, int>();
			foreach (var v in Vertices) {
				visited[v] = false;
				distance[v] = 0;
			}
			var path = new Dictionary<TVertex, TVertex>();
			var queue = new Queue<TVertex>();

			visited[to] = true;
			queue.Enqueue(to);

			while (queue.Count != 0) {
				var v = queue.Dequeue();
				foreach (var vx in InboundVerticesOf(v)) {
					if (!visited[vx]) {
						visited[vx] = true;
						queue.Enqueue(vx);
						distance[vx] = distance[v] + 1;
						path[vx] = v;
					}
				}
			}

			if (!visited[from])
				throw new GraphException($"Couldn't find path from {from} to {to}");

			var shortest = new List<TVertex>();
			TVertex cp = from;
			shortest.Add(cp);
			while (path.ContainsKey(cp)) {
				shortest.Add(path[cp]);
				cp = path[cp];
			}
			//shortest.Reverse();

			return shortest;
		}

	}
}
