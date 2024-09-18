using RealEstate.Core.Interfaces.Utils;
using System.Linq.Expressions;

namespace RealEstate.Core.Services.Utils
{
    public class ReplaceVisitor : ExpressionVisitor, IReplaceVisitor
    {
        private Expression from, to;

        ///<inheritdoc/>
        public void SetData(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        ///<inheritdoc/>
        public override Expression Visit(Expression node) =>  node == from ? to : base.Visit(node);
    }
}
