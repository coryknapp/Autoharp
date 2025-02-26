using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoharp.Models
{
    public class Token
    {
        public SyntaxNode node;

        public Token(SyntaxNode node)
        {
            this.node = node;
        }
    }
}
