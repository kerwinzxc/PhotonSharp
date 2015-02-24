﻿using Photon.AST;
using Photon.Scanner;
using System.Collections.Generic;

namespace Photon.Parser
{
    public partial class Parser
    {
        FuncDeclare ParseFuncDecl( )
        {
            Expect(TokenType.Func);

            var scope = new Scope(_topScope, ScopeType.Function);
            _scopeSet.Add(scope);

            var ident = ParseIdent();

            var paramlist = ParseParameters(scope);

            var body = ParseBody(scope);

            var decl = new FuncDeclare(ident, paramlist, body, scope);

            ident.ScopeInfo = Declare(decl, _global, ident.Name );

            return decl;
        }

        List<Ident> ParseParameters( Scope s )
        {
            Expect(TokenType.LBracket);

            List<Ident> p = new List<Ident>();


            if (_token.Type != TokenType.RBracket )
            {
                while (true)
                {
                    var param = ParseIdent();
                    p.Add(param);

                    Declare(param, s, param.Name);

                    if (_token.Type != TokenType.Comma)
                    {
                        break;
                    }

                    Next();
                }
            }
            
            Expect(TokenType.RBracket);

            return p;
        }


        BlockStmt ParseBody(Scope s) 
        {
            Expect(TokenType.LBrace);

            _topScope = s;

            var list = ParseStatmentList();

            CloseScope();

            Expect(TokenType.RBrace);

            return new BlockStmt(list);
        }

        Stmt ParseVarDecl()
        {
            Expect(TokenType.Var);

            var idents = ParseIdentList();

            foreach( var i in idents )
            {
                Declare(i, _topScope, i.Name);
            }

            List<Expr> values = new List<Expr>();

            if (_token.Type == TokenType.Assign)
            {
                Next();

                values = ParseRHSList();
            }

            return new VarDeclare(idents, values);
        }


    }
}
