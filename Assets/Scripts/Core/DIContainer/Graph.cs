using System.Collections.Generic;
using System.Linq;

namespace DIContainerLib
{
    public interface IGraph<T>
    {
        IEnumerable<T> GetNeighbours(T s);
    }

    public static class GraphSearch
    {
        public static IEnumerable<T> DepthFirstTraversal<T>(this IGraph<T> graph, T start)
        {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            stack.Push(start);

            while(stack.Count != 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current))
                    continue;

                yield return current;

                var neighbours = graph.GetNeighbours(current).Where(n => !visited.Contains(n));

                foreach (var neightbour in neighbours.Reverse())
                    stack.Push(neightbour);
            }
        }
    }
}

