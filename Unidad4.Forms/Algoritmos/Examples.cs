using System.Text;
namespace NoLineales.Core;
public static class Examples
{
    public static readonly int[] TreeKeys = [50, 30, 70, 20, 40, 60, 80, 65];
    public const string GraphEdges = "A B 4\nA C 2\nB C 1\nB D 5\nC D 8\nC E 10\nD E 2\nD F 6\nE F 3";
    public static BinarySearchTree Tree()
    {
        var tree = new BinarySearchTree();
        foreach (int key in TreeKeys) tree.Insert(key);
        return tree;
    }
    public static WeightedGraph Graph() => ParseGraph("A,B,C,D,E,F", GraphEdges);
    /// <summary>Formato de una arista por línea: nombreOrigen nombreDestino peso.</summary>
    public static WeightedGraph ParseGraph(string labels, string lines)
    {
        var names = labels.Split(',', StringSplitOptions.TrimEntries);
        if (names.Any(x => x.Length > 12 || x.Any(char.IsWhiteSpace)))
            throw new ArgumentException("Nombres de 1 a 12 caracteres, sin espacios.");
        var graph = new WeightedGraph(names);
        var rows = lines.Split('\n');
        for (int i = 0; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue;
            var p = rows[i].Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (p.Length != 3 || !int.TryParse(p[2], out int w))
                throw new ArgumentException($"Línea {i+1}: usa origen destino peso entero.");
            int u = graph.IndexOf(p[0]), v = graph.IndexOf(p[1]);
            if (u < 0 || v < 0) throw new ArgumentException($"Línea {i+1}: vértice desconocido.");
            graph.AddEdge(u, v, w);
        }
        return graph;
    }
    public static string TreeText(BinarySearchTree t) =>
        $"Nodos={t.Count}; altura={t.Height()}; íntegro={t.IsValid()}\n" +
        $"Preorden:  {string.Join(", ", t.PreOrder())}\n" +
        $"Inorden:   {string.Join(", ", t.InOrder())}\n" +
        $"Postorden: {string.Join(", ", t.PostOrder())}\n";
    public static string GraphText(WeightedGraph g, ShortestPaths r, int target)
    {
        var b = new StringBuilder();
        b.AppendLine("RED DIDÁCTICA; pesos hipotéticos en km. No son distancias reales.");
        b.AppendLine($"V={g.VertexCount}; E={g.EdgeCount}; dirigido={g.Directed}");
        b.AppendLine("LISTA DE ADYACENCIA");
        for (int u=0;u<g.VertexCount;u++) b.AppendLine(g.Name(u)+": "+
            string.Join(", ",g.Neighbors(u).Select(e=>$"{g.Name(e.To)}({e.Weight})")));
        b.AppendLine("MATRIZ: -- significa ausencia; 0 diagonal es camino vacío.");
        var m=g.Matrix();
        b.Append("".PadLeft(13));
        for(int j=0;j<g.VertexCount;j++) b.Append(g.Name(j).PadLeft(13));
        b.AppendLine();
        for(int i=0;i<g.VertexCount;i++)
        {
            b.Append(g.Name(i).PadLeft(13));
            for(int j=0;j<g.VertexCount;j++) b.Append((m[i,j]?.ToString()??"--").PadLeft(13));
            b.AppendLine();
        }
        b.AppendLine($"TRAZA DESDE {g.Name(r.Source)} (después de relajar):");
        b.AppendLine("Orden de columnas: "+string.Join(", ",Enumerable.Range(0,g.VertexCount).Select(g.Name)));
        b.AppendLine("Inicio: "+string.Join(", ",Enumerable.Range(0,g.VertexCount).Select(v=>v==r.Source?"0":"INF")));
        foreach(var s in r.Trace)
            b.AppendLine($"Fijar {g.Name(s.Vertex)}: [{string.Join(", ",s.Distances.Select(Format))}] | "+
                (s.Relaxations.Length==0?"Sin mejoras":string.Join("; ",s.Relaxations)));
        b.AppendLine("RESULTADOS");
        for(int v=0;v<g.VertexCount;v++) b.AppendLine($"{g.Name(v)}: {Format(r.Distances[v])}; "+
            "camino="+string.Join(" -> ",r.PathTo(v).Select(g.Name)));
        b.AppendLine($"Destino {g.Name(target)}: "+(r.Distances[target]==ShortestPaths.Infinity?"INALCANZABLE":
            $"{string.Join(" -> ",r.PathTo(target).Select(g.Name))}; costo={r.Distances[target]} km"));
        return b.ToString();
    }
    public static string Format(long d) => d==ShortestPaths.Infinity?"INF":d.ToString();
}
