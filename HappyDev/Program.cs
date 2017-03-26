using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HappyDev
{
    class Program
    {
        // Method Creation Using Expression Tree
        static Func<int, int> ETFact()
        {

            // Creating a parameter expression.
            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable.
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.
            BlockExpression block1 = Expression.Block(

            // Adding a local variable.
            new[] { result },

            // Assigning a constant to a local variable: result = 1
            Expression.Assign(result, Expression.Constant(1)),

            // Adding a loop.
            Expression.Loop(

            // Adding a conditional block into the loop.
            Expression.IfThenElse(

            // Condition: value > 1
            Expression.GreaterThan(value, Expression.Constant(1)),

            // If true: result *= value —
            Expression.MultiplyAssign(result,

            Expression.PostDecrementAssign(value)),

            // If false, exit from loop and go to a label.
            Expression.Break(label, result)

            ),

            // Label to jump to.
            label

            ));

            // Compile an expression tree and return a delegate.
            return Expression.Lambda<Func<int, int>>(block1, value).Compile();

        }

        static void Main(string[] args)
        {
            // Basic
            Expression<Func<int, int, int>> function = (a, b) => a + b;
            Console.WriteLine(function.Compile()(2, 5));

            // Method 1
            Expression<Action<int>> printExpr = (arg) => Console.WriteLine(arg);
            printExpr.Compile()(10);

            // Comparison
            ParameterExpression left = Expression.Parameter(typeof(int), "arg");
            ParameterExpression right = Expression.Parameter(typeof(int), "arg1");

            ParameterExpression[] parameters = new ParameterExpression[2] { left, right};

            Expression exp1 = BinaryExpression.MakeBinary(ExpressionType.Equal, left, right);
            var result = Expression.Lambda<Func<int,int, bool>>(exp1, parameters).Compile()(4,4);
            Console.WriteLine(result);

            // Airthmetic Operation iE. Addition
            ParameterExpression left1 = Expression.Parameter(typeof(int), "arg1");
            ParameterExpression right2 = Expression.Parameter(typeof(int), "arg11");

            ParameterExpression[] params1 = new ParameterExpression[2] { left1, right2 };

            Expression exp2 = BinaryExpression.MakeBinary(ExpressionType.Add, left1, right2);
            var res = Expression.Lambda<Func<int, int, int>>(exp2, params1).Compile()(4, 4);
            Console.WriteLine(res);

            // Dynamic Method Generation
            ParameterExpression param = Expression.Parameter(typeof(int), "arg");
            MethodCallExpression methodCall = Expression.Call(
                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }), param);
            Expression.Lambda<Action<int>>(methodCall, new ParameterExpression[] { param }).Compile()(10);

            //Dynamic Block Generation
            ParameterExpression param1 = Expression.Parameter(typeof(int), "arg");

            MethodCallExpression firstMethodCall = Expression.Call(
                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
                Expression.Constant("Print arg:"));

            MethodCallExpression secondMethodCall = Expression.Call(
                typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }),
                param);

            BlockExpression block = Expression.Block(firstMethodCall, secondMethodCall);

            Expression.Lambda<Action<int>>(block, new ParameterExpression[] { param }).Compile()(15);

            //Dynamic Factorial Method
            var eTFactorial = Program.ETFact()(4);
            Console.WriteLine(eTFactorial);

            Console.ReadKey();
        }
    }
}