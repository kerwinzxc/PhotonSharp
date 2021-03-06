﻿using SharpLexer;
using System.Text;

namespace Photon
{
    internal class Ident : Expr
    {
        Token _token;

        internal Symbol Symbol; // 这个为空, 说明未找到定义

        internal Scope BaseScope;   // 所在的Scope

        internal bool UpValue; // 闭包中捕获的变量

        internal Command CmdGen;

        public Ident(Token t)
        {
            _token = t;
        }

        public TokenPos DefinePos
        {
            get { return _token.Pos; }
        }

        public string Name
        {
            get { return _token.Value; }
        }

        public override bool Equals(object obj)
        {
            var otherid = (Ident)obj;

            return Symbol == otherid.Symbol &&
                Name == otherid.Name;
        }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode() + Name.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Ident ");

            if (Symbol != null)
            {
                if (Symbol.RegIndex != -1 )
                {
                    sb.Append(Symbol);
                }
                else
                {
                    sb.Append(Name);
                }

                if ( UpValue )
                {
                    sb.Append(" (UpValue)");
                }

                return sb.ToString();
            }

            return Name;

        }

        static bool ResolveFuncEntry(Package pkg, string name, Command cmd)
        {
            var func = pkg.Exe.GetFuncByName(new ObjectName(pkg.Name, name));

            if ( func != null )
            {
                cmd.DataA = func.ID;
                return true;
            }

            return false;
        }

        internal override void Resolve(CompileParameter param)
        {
            // 故地重游, 再次拨叫
            if (Symbol == null && BaseScope != null )
            {
                Symbol = BaseScope.FindSymbolOutter(Name);
            }

            GenCode(param, 2);
        }

        internal override void Compile(CompileParameter param)
        {
            // 占位
            CmdGen = param.CS.Add(new Command(Opcode.NOP))
                .SetComment(string.Format("{0}.{1}", param.Pkg.Name, Name))
                .SetCodePos(DefinePos);

            GenCode(param, 1);

            if (CmdGen.Op == Opcode.NOP && !param.IsNodeInNextPass(this))
            {
                throw new CompileException("code not resolve", DefinePos);
            }
        }


        void GenCode(CompileParameter param, int pass)
        {            
            // 赋值
            if (param.LHS)
            {
                // TODO 左值在下方被定义
                if( Symbol == null )
                {
                    throw new CompileException(string.Format("undeclared symbol {0}", Name), DefinePos);
                }

                if (Symbol.IsGlobal)
                {
                    CmdGen.Op = Opcode.SETG;

                    CmdGen.DataA = param.Pkg.ID;
                    CmdGen.DataB = Symbol.RegIndex;
                }
                else if ( UpValue )
                {
                    CmdGen.Op = Opcode.SETU;
                    CmdGen.DataA = Symbol.RegIndex;
                }
                else
                {
                    CmdGen.Op = Opcode.SETR;

                    CmdGen.DataA = Symbol.RegIndex;
                }
 
            }
            else
            {
                // 取值
                if ( Symbol == null )
                {
                    if (pass == 1)
                    {
                        param.NextPassToResolve(this);
                    }
                    else
                    {
                        throw new CompileException(string.Format("undeclared symbol {0}", Name), DefinePos);
                    }
                }
                else
                {
                    switch (Symbol.Usage)    
                    {
                        case SymbolUsage.Delegate:
                        case SymbolUsage.Func:
                            {
                                CmdGen.Op = Opcode.LOADF;

                                CmdGen.EntryName = new ObjectName(param.Pkg.Name, Name);
 
                                // 函数入口, 留到VM链接时处理

                            }
                            break;
                        case SymbolUsage.Variable:
                        case SymbolUsage.Parameter:
                        case SymbolUsage.SelfParameter:
                            {
                                // 将自己视为变量

                                if (Symbol.IsGlobal)
                                {
                                    CmdGen.Op = Opcode.LOADG;
                                    CmdGen.DataA = param.Pkg.ID;
                                    CmdGen.DataB = Symbol.RegIndex;
                                }
                                else if (UpValue)
                                {
                                    CmdGen.Op = Opcode.LOADU;
                                    CmdGen.DataA = Symbol.RegIndex;
                                }
                                else
                                {
                                    CmdGen.Op = Opcode.LOADR;
                                    CmdGen.DataA = Symbol.RegIndex;
                                }

                                
                            }
                            break;
                        case SymbolUsage.Class:
                            {

                            }
                            break;
                        case SymbolUsage.Constant:
                            {
                                CmdGen.Op = Opcode.LOADK;
                                CmdGen.DataA = Symbol.RegIndex;
                            }
                            break;
                        default:
                            throw new CompileException("Unknown usage", DefinePos);
                    }
                }
      
                
            }
         
        }
    }
}
