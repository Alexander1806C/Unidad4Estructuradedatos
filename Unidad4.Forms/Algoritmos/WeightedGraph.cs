namespace NoLineales.Core;
public readonly record struct Edge(int To, int Weight);
/// <summary>Grafo simple ponderado; lista de adyacencia. Admite peso cero.</summary>
public sealed class WeightedGraph
{
    private readonly string[] names;
    private readonly List<Edge>[] adjacency;
    public int VertexCount => names.Length;
    public int EdgeCount { get; private set; }
    public bool Directed { get; }
    public WeightedGraph(IEnumerable<string> labels, bool directed = false)
    {
        ArgumentNullException.ThrowIfNull(labels);
        names = labels.Select(x => x?.Trim() ?? "").ToArray();
        if (names.Length == 0 || names.Any(string.IsNullOrWhiteSpace)
            || names.Distinct(StringComparer.Ordinal).Count() != names.Length)
            throw new ArgumentException("Se requieren nombres únicos y no vacíos.");
        adjacency = new List<Edge>[names.Length];
        for (int i = 0; i < names.Length; i++) adjacency[i] = new List<Edge>();
        Directed = directed;
    }
    private void Check(int v)
    {
        if (v < 0 || v >= VertexCount) throw new ArgumentOutOfRangeException(nameof(v));
    }
    public string Name(int v) { Check(v); return names[v]; }
    public int IndexOf(string name) => Array.IndexOf(names, name);
    public IReadOnlyList<Edge> Neighbors(int v)
    {
        Check(v); return adjacency[v].AsReadOnly();
    }
    public void AddEdge(int from, int to, int weight)
    {
        Check(from); Check(to);
        if (weight < 0) throw new ArgumentOutOfRangeException(nameof(weight), "Peso negativo.");
        if (from == to) throw new ArgumentException("No se admiten lazos.");
        if (adjacency[from].Any(e => e.To == to))
            throw new ArgumentException("Arista repetida.");
        adjacency[from].Add(new Edge(to, weight));
        if (!Directed) adjacency[to].Add(new Edge(from, weight));
        EdgeCount++;
    }
    /// <summary>null significa ausencia; diagonal cero = camino vacío, no lazo.</summary>
    public int?[,] Matrix()
    {
        var matrix = new int?[VertexCount, VertexCount];
        for (int i = 0; i < VertexCount; i++)
        {
            matrix[i, i] = 0;
            foreach (var e in adjacency[i]) matrix[i, e.To] = e.Weight;
        }
        return matrix;
    }
}
