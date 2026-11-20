namespace NoLineales.Core;
/// <summary>Nodo dinámico del ABB. Solo el ensamblado puede modificar enlaces.</summary>
public sealed class TreeNode
{
    public int Key { get; internal set; }
    public TreeNode? Left { get; internal set; }
    public TreeNode? Right { get; internal set; }
    internal TreeNode(int key) => Key = key;
}
