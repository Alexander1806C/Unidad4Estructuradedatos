namespace NoLineales.Core;
/// <summary>Una instantánea después de relajar vecinos del vértice fijado.</summary>
public sealed record TraceStep(int Vertex, long[] Distances, int[] Previous,
    string[] Relaxations);
public sealed class ShortestPaths
{
    public const long Infinity = long.MaxValue;
    public int Source { get; }
    public long[] Distances { get; }
    public int[] Previous { get; }
    public IReadOnlyList<TraceStep> Trace { get; }
    internal ShortestPaths(int source, long[] distances, int[] previous,
        List<TraceStep> trace)
    { Source = source; Distances = distances; Previous = previous; Trace = trace.AsReadOnly(); }
    /// <summary>Camino vacío si el destino es inalcanzable.</summary>
    public int[] PathTo(int target)
    {
        if (target < 0 || target >= Distances.Length)
            throw new ArgumentOutOfRangeException(nameof(target));
        if (Distances[target] == Infinity) return Array.Empty<int>();
        var path = new List<int>();
        for (int v = target; v != -1; v = Previous[v])
        {
            if (path.Count >= Distances.Length)
                throw new InvalidOperationException("Predecesores alterados: ciclo.");
            path.Add(v);
        }
        path.Reverse(); return path.ToArray();
    }
}
public static class Dijkstra
{
    /// <summary>Selección lineal: O(V²+E); sin traza, espacio auxiliar O(V).</summary>
    public static ShortestPaths Solve(WeightedGraph graph, int source, bool trace = true)
    {
        ArgumentNullException.ThrowIfNull(graph);
        if (source < 0 || source >= graph.VertexCount)
            throw new ArgumentOutOfRangeException(nameof(source));
        int n = graph.VertexCount;
        var distance = new long[n]; Array.Fill(distance, ShortestPaths.Infinity);
        var previous = new int[n]; Array.Fill(previous, -1);
        var settled = new bool[n]; var steps = new List<TraceStep>();
        distance[source] = 0;
        for (int iteration = 0; iteration < n; iteration++)
        {
            int u = -1;
            // El primer índice gana los empates; no se fija un inalcanzable.
            for (int v = 0; v < n; v++)
                if (!settled[v] && distance[v] != ShortestPaths.Infinity
                    && (u == -1 || distance[v] < distance[u])) u = v;
            if (u == -1) break;
            settled[u] = true;
            List<string>? changes = trace ? new List<string>() : null;
            foreach (var edge in graph.Neighbors(u))
            {
                if (settled[edge.To]) continue;
                // long evita desbordar int al sumar varias aristas.
                long candidate = checked(distance[u] + edge.Weight);
                if (candidate < distance[edge.To])
                {
                    distance[edge.To] = candidate; previous[edge.To] = u;
                    changes?.Add($"{graph.Name(edge.To)}={candidate} vía {graph.Name(u)}");
                }
            }
            if (trace) steps.Add(new TraceStep(u, (long[])distance.Clone(),
                (int[])previous.Clone(), changes!.ToArray()));
        }
        return new ShortestPaths(source, distance, previous, steps);
    }
}
