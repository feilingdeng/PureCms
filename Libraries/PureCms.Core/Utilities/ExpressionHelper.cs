using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Utilities
{
    public class ExpressionHelper
    {
        //获取类成员名称
        public static string GetPropertyName<T>(Expression<Func<T, object>> e)
        {
            //if (e.Body is MemberExpression)
            //{
            //    var member = (MemberExpression)e.Body;
            //    return member.Member.Name;
            //}
            //else if (e.Body is ConstantExpression)
            //{
            //    var ce = (ConstantExpression)e.Body;
            //    return ce.Value.ToString();
            //}
            //else if (e.Body is UnaryExpression)
            //{
            //    UnaryExpression ue = ((UnaryExpression)e.Body);
            //    return GetPropertyName<T>(ue.Type);
            //}
            //else
            //    throw new Exception(e + " is not a MemberExpression");
            return ExpressionRouter(e);
        }

        private static string ExpressionRouter(Expression exp)
        {
            string sb = string.Empty;
            if (exp is LambdaExpression)
            {
                LambdaExpression le = ((LambdaExpression)exp);
                return LambdaExpressionProvider(le);
            }
            else if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinaryExpressionProvider(be);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression me = ((MemberExpression)exp);
                return MemberExpressionProvider(me);
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return UnaryExpressionProvider(ue);
            }
            return null;
        }
        private static string LambdaExpressionProvider(LambdaExpression le)
        {
            return ExpressionRouter(le.Body);
        }
        private static string BinaryExpressionProvider(BinaryExpression be)
        {
            return ExpressionRouter(be.Left);
        }
        private static string MemberExpressionProvider(MemberExpression me)
        {
            return me.Member.Name;
        }
        private static string UnaryExpressionProvider(UnaryExpression ue)
        {
            return ExpressionRouter(ue.Operand);
        }
    }
}
