﻿using System.Collections.Generic;

namespace Photon.AST
{
    public class Chunk : Node
    {
        public BlockStmt Block;

        public Chunk(BlockStmt block)
        {
            Block = block;
        }

        public override IEnumerable<Node> Child()
        {
            return Block.Child();
        }

        public override string ToString()
        {
            return "Chunk";
        }
    }
}
