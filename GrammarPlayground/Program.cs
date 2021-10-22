using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using GrammarPlayground;

namespace GrammarPlayground
{
    interface IAstNode { }

    sealed record RangeCondition(string Field, string Op, DateTime date) : IAstNode;

    record State(List<string> Tokens);
    
    class HelloVisitor : HelloBaseVisitor<State>
    {
        private State _state = new (new List<string>());
        // public override State Visit(IParseTree tree)
        // {
        //     _state.Tokens.Add(tree.ToStringTree());
        //     Console.WriteLine(tree.GetText());
        //     return _state;
        // }

        public override State VisitQuery([NotNull] HelloParser.QueryContext context)
        {
            return VisitChildren(context);
        }

        public override State VisitCondition(HelloParser.ConditionContext context)
        {
            _state.Tokens.Add($"c|{context.GetChild(0).GetText()}|{context.GetChild(1).GetText()}|{context.GetChild(2).GetText()}");
            return _state;
        }

        public override State VisitLike(HelloParser.LikeContext context)
        {
            _state.Tokens.Add($"l|{context.GetChild(1).GetText()}");
            return _state;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // var expression = "hello>='2021-11-01'";
            // var expression = "hello>='2021-11-01'&world=='2021-11-02'&here<'2021-11-03'&filenamelike'%2021-12-12%'";
            // var expression = "filenamelike'%2021-12-12%'";
            // var expression = "filenamelike'%2021-12-12%'";
            var expression = "hello>='2021-11-01'&world=='2021-11-02'&here<'2021-11-03'&filenamelike'%2021-12-12%'";

 
            var inputStream = new AntlrInputStream(expression);
            var lexer = new HelloLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new HelloParser(tokenStream);
 
            var visitor = new HelloVisitor();
            var query = parser.query();
            var result = visitor.Visit(query);
            Console.WriteLine(result);
            result.Tokens.ForEach(Console.WriteLine);
        }
    }
}