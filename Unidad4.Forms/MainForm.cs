using TreeNode = NoLineales.Core.TreeNode;
using NoLineales.Core;
using System.Drawing.Drawing2D;
namespace NoLineales.Forms;
public partial class MainForm : Form
{
    private BinarySearchTree tree = Examples.Tree();
    private WeightedGraph graph = Examples.Graph();
    private ShortestPaths? result;
    private int[] route = [];
    public MainForm()
    {
        InitializeComponent();
        RefreshTree("Ejemplo inicial");SetGraphExample();
    }
    private void Safely(Action action)
    {
        try { action(); }
        catch (Exception ex) when(ex is ArgumentException or InvalidOperationException or IOException or UnauthorizedAccessException)
        { MessageBox.Show(this,ex.Message,"Revisa la entrada",MessageBoxButtons.OK,MessageBoxIcon.Information); }
    }
    private int Key()
    {
        if(!int.TryParse(keyInput.Text,out int key))throw new ArgumentException("Escribe un entero válido de 32 bits.");
        return key;
    }
    private void RefreshTree(string message)
    {
        treeOutput.AppendText(message+Environment.NewLine+Examples.TreeText(tree)+Environment.NewLine);
        treeCanvas.AutoScrollMinSize=new Size(Math.Max(850,tree.Count*62),Math.Max(300,(tree.Height()+1)*90+50));
        treeCanvas.Invalidate();
    }
    private void Insert_Click(object? s,EventArgs e)=>Safely(()=>
    {
        int key=Key();if(tree.Count>=80&&!tree.Contains(key))throw new ArgumentException("Límite visual: 80 nodos. El motor no tiene este límite.");
        RefreshTree(tree.Insert(key)?$"Insertado: {key}":$"Duplicado rechazado: {key}");
    });
    private void Search_Click(object? s,EventArgs e)=>Safely(()=>{int k=Key();RefreshTree($"Buscar {k}: {tree.Contains(k)}");});
    private void Remove_Click(object? s,EventArgs e)=>Safely(()=>{int k=Key();RefreshTree($"Eliminar {k}: {tree.Remove(k)}");});
    private void ExampleTree_Click(object? s,EventArgs e){tree=Examples.Tree();treeOutput.Clear();RefreshTree("Ejemplo restaurado");}
    private void ClearTree_Click(object? s,EventArgs e){tree.Clear();treeOutput.Clear();RefreshTree("Árbol vacío");}
    private void SaveTree_Click(object? s,EventArgs e)=>Safely(()=>Save(treeOutput.Text,"prueba_abb.txt"));
    private void SaveGraph_Click(object? s,EventArgs e)=>Safely(()=>Save(graphOutput.Text,"resultado_dijkstra.txt"));
    private void Save(string text,string name)
    {
        using var dlg=new SaveFileDialog{Filter="Texto|*.txt",FileName=name};
        if(dlg.ShowDialog(this)==DialogResult.OK)File.WriteAllText(dlg.FileName,text);
    }
    private void Canvas_Resize(object? sender,EventArgs e){if(sender is Control c)c.Invalidate();}
    private void TreeCanvas_Paint(object? sender,PaintEventArgs e)
    {
        var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;
        g.TranslateTransform(treeCanvas.AutoScrollPosition.X,treeCanvas.AutoScrollPosition.Y);
        if(tree.Root==null){g.DrawString("Árbol vacío",Font,Brushes.DimGray,20,20);return;}
        var order=tree.InOrder();var ranks=order.Select((k,i)=>(k,i)).ToDictionary(x=>x.k,x=>x.i);
        var positions=new Dictionary<TreeNode,PointF>();
        var stack=new Stack<(TreeNode Node,int Depth)>();stack.Push((tree.Root,0));
        float width=Math.Max(treeCanvas.ClientSize.Width,treeCanvas.AutoScrollMinSize.Width);
        while(stack.Count>0)
        {
            var(n,d)=stack.Pop();positions[n]=new PointF((ranks[n.Key]+.5f)*width/tree.Count,45+d*90);
            if(n.Left!=null)stack.Push((n.Left,d+1));if(n.Right!=null)stack.Push((n.Right,d+1));
        }
        using var pen=new Pen(Color.SlateGray,2);
        foreach(var(n,p) in positions)
        { if(n.Left!=null)g.DrawLine(pen,p,positions[n.Left]);if(n.Right!=null)g.DrawLine(pen,p,positions[n.Right]); }
        using var fill=new SolidBrush(Color.FromArgb(20,78,103));
        using var tiny=new Font("Segoe UI",8);
        var sf=new StringFormat{Alignment=StringAlignment.Center,LineAlignment=StringAlignment.Center};
        foreach(var(n,p) in positions)
        {var rect=new RectangleF(p.X-30,p.Y-21,60,42);g.FillEllipse(fill,rect);g.DrawString(n.Key.ToString(),tiny,Brushes.White,rect,sf);}
    }
    private void SetGraphExample()
    {
        labelsInput.Text="A,B,C,D,E,F";edgesInput.Text=Examples.GraphEdges.Replace("\n",Environment.NewLine);
        LoadGraph();Solve();
    }
    private void ExampleGraph_Click(object? s,EventArgs e)=>Safely(SetGraphExample);
    private void LoadGraph_Click(object? s,EventArgs e)=>Safely(LoadGraph);
    private void Solve_Click(object? s,EventArgs e)=>Safely(Solve);
    private void LoadGraph()
    {
        var candidate=Examples.ParseGraph(labelsInput.Text,edgesInput.Text);
        if(candidate.VertexCount>12)throw new ArgumentException("La vista permite hasta 12 vértices.");
        graph=candidate;result=null;route=[];sourceBox.Items.Clear();targetBox.Items.Clear();
        for(int i=0;i<graph.VertexCount;i++){sourceBox.Items.Add(graph.Name(i));targetBox.Items.Add(graph.Name(i));}
        sourceBox.SelectedIndex=0;targetBox.SelectedIndex=graph.VertexCount-1;
        graphOutput.Text="Red cargada. Pulsa Calcular. Cambios en el editor se aplican con Cargar red.";
        graphCanvas.Invalidate();
    }
    private void Solve()
    {
        int source=sourceBox.SelectedIndex,target=targetBox.SelectedIndex;
        result=Dijkstra.Solve(graph,source);route=result.PathTo(target);
        graphOutput.Text=Examples.GraphText(graph,result,target);graphCanvas.Invalidate();
    }
    private void GraphCanvas_Paint(object? sender,PaintEventArgs e)
    {
        var g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;
        int n=graph.VertexCount;var pts=new PointF[n];
        float cx=graphCanvas.Width/2f,cy=graphCanvas.Height/2f;
        float radius=Math.Max(20,Math.Min(cx,cy)-42);
        for(int i=0;i<n;i++){double a=2*Math.PI*i/n-Math.PI/2;pts[i]=new PointF(cx+radius*(float)Math.Cos(a),cy+radius*(float)Math.Sin(a));}
        using var normal=new Pen(Color.LightSlateGray,2);using var highlight=new Pen(Color.DarkOrange,4);
        for(int u=0;u<n;u++)foreach(var edge in graph.Neighbors(u))
        {
            int v=edge.To;if(u>=v)continue;
            bool onRoute=false;for(int i=1;i<route.Length;i++)if((route[i-1]==u&&route[i]==v)||(route[i-1]==v&&route[i]==u))onRoute=true;
            g.DrawLine(onRoute?highlight:normal,pts[u],pts[v]);
            var m=new PointF((pts[u].X+pts[v].X)/2,(pts[u].Y+pts[v].Y)/2);
            string w=edge.Weight.ToString();var size=g.MeasureString(w,Font);
            g.FillRectangle(Brushes.White,m.X-size.Width/2,m.Y-size.Height/2,size.Width,size.Height);
            g.DrawString(w,Font,Brushes.Black,m.X-size.Width/2,m.Y-size.Height/2);
        }
        var sf=new StringFormat{Alignment=StringAlignment.Center,LineAlignment=StringAlignment.Center};
        using var fill=new SolidBrush(Color.FromArgb(20,78,103));using var small=new Font("Segoe UI",8);
        for(int i=0;i<n;i++){var r=new RectangleF(pts[i].X-28,pts[i].Y-20,56,40);g.FillEllipse(fill,r);g.DrawString(graph.Name(i),small,Brushes.White,r,sf);}
    }
}
