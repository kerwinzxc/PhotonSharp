﻿using System.Collections.Generic;
using SharpLexer;

namespace Photon
{

    internal class CallExpr : Expr
    {
        public Expr Func;
        public List<Expr> Args;
        public Scope S;

        public TokenPos LParen;
        public TokenPos RParen;

        public CallExpr(Expr f, List<Expr> args, Scope s, TokenPos lparen, TokenPos rparen)
        {
            Func = f;
            Args = args;
            S = s;
            LParen = lparen;
            RParen = rparen;

            BuildRelation();
        }

        public override IEnumerable<Node> Child()
        {
            yield return Func;

            foreach( var e in Args)
            {
                yield return e;
            }
        }

        public override string ToString()
        {
            return "CallExpr";
        }

        int GetReceiverCount( )
        {            
            // 单独的一句时, 需要平衡数据栈
            var assign = Parent as AssignStmt;
            if (assign != null )
            {
                return assign.LHS.Count;
            }

            // 没有任何接收者
            if ( Parent is ExprStmt )
            {
                return 0;
            }

            // 无需处理返回值
            if ( Parent is ReturnStmt )
            {
                return -1;
            }


            return 1;
        }

        internal override void Compile(CompileParameter param)
        {
            int SelfCount = 0;


            // 尝试放入self作为第一个参数
            var selector = Func as SelectorExpr;
            if (selector != null)
            {
                SelfCount += selector.CompileSelfParameter(param.SetLHS(false));
            }
            

            // 先放参数
            foreach (var arg in Args)
            {
                arg.Compile(param.SetLHS(false));                
            }

                // 本包及动态闭包调用
            Func.Compile(param.SetLHS(false));

            param.CS.Add(new Command(Opcode.CALL, Args.Count + SelfCount, GetReceiverCount())).SetCodePos(LParen);
        }
    }
}
