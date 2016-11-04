﻿using Photon.OpCode;
using System.Collections.Generic;
using System.Text;

namespace Photon.AST
{
    public class FuncDeclare : Stmt
    {
        public Ident Name;

        public List<Ident> Params;

        public BlockStmt Body;

        public Scope ScopeInfo;

        public FuncDeclare(Ident name, List<Ident> param, BlockStmt body, Scope s)
        {
            Name = name;
            Params = param;
            Body = body;
            ScopeInfo = s;

            BuildRelation();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int index = 0;
            foreach (var i in Params)
            {
                if (index > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(i.Name.ToString());


                index++;
            }

            return string.Format("FuncDeclare {0}: {1}", Name, sb.ToString());
        }

        public override IEnumerable<Node> Child()
        {
            return Body.Child();
        }


        public override void Compile(Executable exe, CommandSet cm, bool lhs)
        {
            var newset = new CommandSet(Name.Name );

            var funcIndex = exe.AddCmdSet(newset);

            var c = new FuncValue(funcIndex);
            var ci = exe.Constants.Add(c);

            cm.Add(new Command(Opcode.LoadC, ci)).Comment = c.GetDesc();

            cm.Add(new Command(Opcode.SetG, Name.ScopeInfo.RegIndex )).Comment = Name.Name;

            Body.Compile(exe, newset, false);

            newset.InputValueCount = Params.Count;

            newset.OutputValueCount = CalcReturnValueCount();
            
        }

        int CalcReturnValueCount( )
        {            
            foreach (var c in Body.Child())
            {
                var retStmt = c as ReturnStmt;
                if (retStmt == null)
                {
                    continue;
                }

                return retStmt.Results.Count;
            }

            return 0;
        }

    }
}