namespace NoLineales.Core;
/// <summary>ABB de enteros únicos, sin balanceo automático. Operaciones iterativas.</summary>
public sealed class BinarySearchTree
{
    public TreeNode? Root { get; private set; }
    public int Count { get; private set; }

    /// <returns>false si la clave ya existe; no modifica el árbol.</returns>
    public bool Insert(int key)
    {
        TreeNode? parent = null, node = Root;
        while (node != null)
        {
            parent = node;
            if (key == node.Key) return false;
            node = key < node.Key ? node.Left : node.Right;
        }
        var added = new TreeNode(key);
        if (parent == null) Root = added;
        else if (key < parent.Key) parent.Left = added;
        else parent.Right = added;
        Count++;
        return true;
    }

    public bool Contains(int key)
    {
        var node = Root;
        while (node != null)
        {
            if (key == node.Key) return true;
            node = key < node.Key ? node.Left : node.Right;
        }
        return false;
    }

    /// <summary>Hoja: desconecta. Un hijo: enlaza hijo. Dos: usa sucesor inorden.</summary>
    public bool Remove(int key)
    {
        TreeNode? parent = null, node = Root;
        while (node != null && node.Key != key)
        {
            parent = node;
            node = key < node.Key ? node.Left : node.Right;
        }
        if (node == null) return false;
        if (node.Left != null && node.Right != null)
        {
            // Menor del subárbol derecho; nunca tiene hijo izquierdo.
            var successorParent = node;
            var successor = node.Right;
            while (successor.Left != null)
            {
                successorParent = successor;
                successor = successor.Left;
            }
            node.Key = successor.Key;
            parent = successorParent;
            node = successor;
        }
        var child = node.Left ?? node.Right;
        if (parent == null) Root = child;
        else if (ReferenceEquals(parent.Left, node)) parent.Left = child;
        else parent.Right = child;
        Count--;
        return true;
    }

    /// <summary>Izquierda, raíz, derecha: claves en orden creciente.</summary>
    public int[] InOrder()
    {
        var result = new List<int>();
        var stack = new Stack<TreeNode>();
        var node = Root;
        while (node != null || stack.Count > 0)
        {
            while (node != null) { stack.Push(node); node = node.Left; }
            node = stack.Pop(); result.Add(node.Key); node = node.Right;
        }
        return result.ToArray();
    }

    /// <summary>Raíz, izquierda, derecha. LIFO: apilar primero derecha.</summary>
    public int[] PreOrder()
    {
        var result = new List<int>();
        if (Root == null) return result.ToArray();
        var stack = new Stack<TreeNode>(); stack.Push(Root);
        while (stack.Count > 0)
        {
            var node = stack.Pop(); result.Add(node.Key);
            if (node.Right != null) stack.Push(node.Right);
            if (node.Left != null) stack.Push(node.Left);
        }
        return result.ToArray();
    }

    /// <summary>Izquierda, derecha, raíz. Marca para emitir tras ambos hijos.</summary>
    public int[] PostOrder()
    {
        var result = new List<int>();
        if (Root == null) return result.ToArray();
        var stack = new Stack<(TreeNode Node, bool Emit)>();
        stack.Push((Root, false));
        while (stack.Count > 0)
        {
            var (node, emit) = stack.Pop();
            if (emit) { result.Add(node.Key); continue; }
            stack.Push((node, true));
            if (node.Right != null) stack.Push((node.Right, false));
            if (node.Left != null) stack.Push((node.Left, false));
        }
        return result.ToArray();
    }

    /// <summary>Altura en aristas: árbol vacío -1, hoja 0.</summary>
    public int Height()
    {
        if (Root == null) return -1;
        var stack = new Stack<(TreeNode Node, int Depth)>();
        stack.Push((Root, 0)); int max = 0;
        while (stack.Count > 0)
        {
            var (node, depth) = stack.Pop(); max = Math.Max(max, depth);
            if (node.Left != null) stack.Push((node.Left, depth + 1));
            if (node.Right != null) stack.Push((node.Right, depth + 1));
        }
        return max;
    }

    /// <summary>Verifica rangos globales, ausencia de ciclos y contador.</summary>
    public bool IsValid()
    {
        if (Root == null) return Count == 0;
        var seen = new HashSet<TreeNode>();
        var stack = new Stack<(TreeNode Node, long Min, long Max)>();
        stack.Push((Root, long.MinValue, long.MaxValue));
        while (stack.Count > 0)
        {
            var (node, min, max) = stack.Pop();
            if (!seen.Add(node) || node.Key <= min || node.Key >= max) return false;
            if (node.Left != null) stack.Push((node.Left, min, node.Key));
            if (node.Right != null) stack.Push((node.Right, node.Key, max));
        }
        return seen.Count == Count;
    }
    public void Clear() { Root = null; Count = 0; }
}
