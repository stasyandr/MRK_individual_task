using System.Collections.Generic;
using SimpleLang.Visitors;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };
    public abstract class Node // базовый класс для всех узлов    
    {
        public abstract void Visit(Visitor v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
        public override void Visit(Visitor v)
        {
            v.VisitIdNode(this);
        }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
        public override void Visit(Visitor v)
        {
            v.VisitIntNumNode(this);
        }
    }

    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {
        public ExprNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(ExprNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
        public override void Visit(Visitor v)
        {
            v.VisitAssignNode(this);
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitCycleNode(this);
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
        public override void Visit(Visitor v)
        {
            v.VisitBlockNode(this);
        }
    }
    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitWhileNode(this);
        }
    }
    public class RepeatNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public RepeatNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitRepeatNode(this);
        }
    }
    public class ForNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public StatementNode Assign { get; set; }
        public ForNode(StatementNode assign, ExprNode expr, StatementNode stat)
        {
            Assign = assign;
            Expr = expr;
            Stat = stat;
        }
        public override void Visit(Visitor v)
        {
            v.VisitForNode(this);
        }
    }
    public class WriteNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public WriteNode(ExprNode expr)
        {
            Expr = expr;
        }
        public override void Visit(Visitor v)
        {
            v.VisitWriteNode(this);
        }
    }
    public class IfNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Then { get; set; }
        public StatementNode Else { get; set; }
        public IfNode(ExprNode expr, StatementNode t, StatementNode e)
        {
            Expr = expr;
            Then = t;
            Else = e;
        }
        public override void Visit(Visitor v)
        {
            v.VisitIfNode(this);
        }
    }
    public class BinaryNode : ExprNode
    {
        public bool isarray;
        public ExprNode Left;
        public ExprNode Right;
        public char operation;
        public BinaryNode()
        {
            Left = null;
            Right = null;

        }
        public BinaryNode(ExprNode left, ExprNode right, char c)
        {
            Left = left;
            Right = right;
            operation = c;
        }
        public override void Visit(Visitor v)
        {
            v.VisitBinOpNode(this);
        }
    }

    public class VarDefNode : StatementNode
    {
        public List<ExprNode> Ids = new List<ExprNode>();
        public VarDefNode(ArrayNode n)
        {
            Ids.Add(n);
        }
        public VarDefNode(string id)
        {
            Ids.Add(new IdNode(id));
        }
        public void Add(ArrayNode n)
        {
            Ids.Add(n);
        }

        public void Add(string id)
        {
            Ids.Add(new IdNode(id));
        }
        public override void Visit(Visitor v)
        {
            v.VisitVarDefNode(this);
        }        
    }
    public class ArrayNode : ExprNode
    {
        public string Name;
        public int Length;
        public ArrayNode(string i, int leng)
        {
            Name = i;
            Length = leng;
        }
        public override void Visit(Visitor v)
        {
            v.VisitArrayNode(this);
        }
    }

    public class SliceNode : ExprNode
    {
        public string Name;
        public int Start;
        public int Stop;
        public int Step;
        public SliceNode(string id, int start, int stop, int step = 1)
        {
            Name = id;
            Start = start;
            Stop = stop;
            Step = step;
        }
        public override void Visit(Visitor v)
        {
            v.VisitSliceNode(this);
        }
    }
    public class EmptyNode : StatementNode
    {
        public override void Visit(Visitor v)
        {
            v.VisitEmptyNode(this);
        }
    }
}
