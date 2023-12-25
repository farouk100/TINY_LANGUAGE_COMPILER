using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Hosting;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace TINY_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program");
            program.Children.Add(Function_Statement());
            program.Children.Add(Main_Function());
            MessageBox.Show("Success");
            return program;
        }

        Node Function_Statement()
        {
            Node function_statement = new Node("Function_Statement");
            if (InputPointer < TokenStream.Count &&
                (TokenStream[InputPointer].token_type == Token_Class.DataType))
            {
                function_statement.Children.Add(Function_Declaration());
                function_statement.Children.Add(Function_Body());
                function_statement.Children.Add(Function_Statement());

                return function_statement;
            }
            else
            {
                return null;
            }
            
        }

        Node Function_Declaration()
        {
            Node function_declaration = new Node("Function_Declaration");
            function_declaration.Children.Add(match(Token_Class.DataType));
            function_declaration.Children.Add(match(Token_Class.Idenifier));
            function_declaration.Children.Add(match(Token_Class.LParanthesisOp));
            function_declaration.Children.Add(ParameterList());
            function_declaration.Children.Add(match(Token_Class.RParanthesisOp));
            function_declaration.Children.Add(match(Token_Class.SemicolonOp));
            return function_declaration;
        }

        Node ParameterList()
        {
            Node parameterList = new Node("ParameterList");

            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.DataType))
                && (InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer+1].token_type == Token_Class.Idenifier))
                && (InputPointer + 2 < TokenStream.Count && (TokenStream[InputPointer+2].token_type == Token_Class.CommaOp)))
            {
                parameterList.Children.Add(Parameter());
                parameterList.Children.Add(match(Token_Class.CommaOp));
                parameterList.Children.Add(ParameterList());
                return parameterList;
            }
            else if((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.DataType))
                    && (InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.Idenifier)))
            {
                parameterList.Children.Add(Parameter());
                return parameterList;
            }
            else
            {
                return null;
            }
        }

        Node Parameter()
        {
            Node parameter = new Node("Parameter ");
            parameter.Children.Add(match(Token_Class.DataType));
            parameter.Children.Add(match(Token_Class.Idenifier));
            
            return parameter;
        }

        Node Function_Body()
        {
            Node function_body = new Node("Function_Body ");
            function_body.Children.Add(match(Token_Class.LBrackectOp));
            function_body.Children.Add(Statements());
            function_body.Children.Add(Return_Statement());

            function_body.Children.Add(match(Token_Class.RBracketOp));
            return function_body;
        }

        Node Main_Function()
        {
            Node main_function = new Node("Main_Function ");
            // expected error
            main_function.Children.Add(match(Token_Class.DataType));

            main_function.Children.Add(match(Token_Class.Main));
            main_function.Children.Add(match(Token_Class.LParanthesisOp));
            main_function.Children.Add(match(Token_Class.RParanthesisOp));
            main_function.Children.Add(Function_Body());
            return main_function;
        }

        Node Statements()
        {
            Node statements = new Node("Statements ");
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.DataType))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Write))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Read))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Return))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.If))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Repeat)))
            {
                statements.Children.Add(Statement());
                statements.Children.Add(Statements());
                return statements;
            }
            else
            {
                return null;
            }
        }

        Node Statement()
        {
            Node statement = new Node("Statement ");
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.DataType)))
            {
                statement.Children.Add(Declaration_Statement());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Write)))
            {
                statement.Children.Add(Write_Statement());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Read)))
            {
                statement.Children.Add(Read_Statement());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Return)))
            {
                statement.Children.Add(Return_Statement());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Repeat)))
            {
                statement.Children.Add(Repeat_Statement());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.LParanthesisOp)))
            {
                statement.Children.Add(Function_Call());
                return statement;
            }
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
                || (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)))
            {
                statement.Children.Add(Assignment_Statement());
                return statement;
            }
            return null;
        }

        Node Declaration_Statement()
        {
            Node declaration_statement = new Node("Declaration_Statement ");
            declaration_statement.Children.Add(match(Token_Class.DataType));
            declaration_statement.Children.Add(IdentifierList());
            declaration_statement.Children.Add(match(Token_Class.SemicolonOp));
            return declaration_statement;
        }

        Node IdentifierList()
        {
            Node identifierList = new Node("IdentifierList ");

            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
            && (InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.CommaOp)))
            {
                identifierList.Children.Add(match(Token_Class.Idenifier));
                identifierList.Children.Add(match(Token_Class.CommaOp));
                identifierList.Children.Add(IdentifierList());
            }
            else
            {
                identifierList.Children.Add(match(Token_Class.Idenifier));
            }

            return identifierList;
        }

        Node Assignment_Statement()
        {
            Node assignment_statement = new Node("Assignment_Statement ");
            assignment_statement.Children.Add(match(Token_Class.Idenifier));
            assignment_statement.Children.Add(match(Token_Class.AssignmentOp));
            assignment_statement.Children.Add(Expression());
            assignment_statement.Children.Add(match(Token_Class.SemicolonOp));

            return assignment_statement;
        }

        Node Write_Statement()
        {
            Node write_statement = new Node("Write_Statement ");
            write_statement.Children.Add(match(Token_Class.Write));
            write_statement.Children.Add(WriteContent());
            write_statement.Children.Add(match(Token_Class.SemicolonOp));

            return write_statement;
        }

        Node WriteContent()
        {
            Node writeContent = new Node("WriteContent ");
            if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Endl)))
            {
                writeContent.Children.Add(match(Token_Class.Endl));
            }
            else
            {
                writeContent.Children.Add(Expression());
            }

            return writeContent;
        }

        Node Read_Statement()
        {
            Node read_statement = new Node("Read_Statement ");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(match(Token_Class.Idenifier));
            read_statement.Children.Add(match(Token_Class.SemicolonOp));

            return read_statement;
        }
        Node Return_Statement()
        {
            Node return_statement = new Node("Return_Statement");
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.SemicolonOp));

            return return_statement;
        }

        Node If_Statement()
        {
            Node if_statement = new Node("If_Statement");
            if_statement.Children.Add(match(Token_Class.If));
            if_statement.Children.Add(Condition_Statement());
            if_statement.Children.Add(match(Token_Class.Then));

            if_statement.Children.Add(Statements());
            if_statement.Children.Add(Else_If_Statement());
            if_statement.Children.Add(Else_Statement());
            if_statement.Children.Add(match(Token_Class.End));

            return if_statement;
        }

        Node Else_If_Statement()
        {
            if (InputPointer < TokenStream.Count &&
            (TokenStream[InputPointer].token_type == Token_Class.ElseIf))
            {

                Node else_if_statement = new Node("Else_If_Statement");
                else_if_statement.Children.Add(match(Token_Class.ElseIf));
                else_if_statement.Children.Add(Condition_Statement());
                else_if_statement.Children.Add(match(Token_Class.Then));
                else_if_statement.Children.Add(Statements());
                else_if_statement.Children.Add(Else_If_Statement());


                return else_if_statement;
            }
            else
            {
                return null;
            }
        }

        Node Else_Statement()
        {
            Node else_statement = new Node("Else_If_Statement");
            else_statement.Children.Add(match(Token_Class.Else));
            else_statement.Children.Add(Statements());
            else_statement.Children.Add(match(Token_Class.End));

            return else_statement;
        }

        Node Repeat_Statement()
        {
            Node repeat_statement = new Node("Repeat_Statement");
            repeat_statement.Children.Add(match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_Statement());

            return repeat_statement;
        }

        Node Function_Call()
        {
            Node function_statement = new Node("Function_Call");
            function_statement.Children.Add(match(Token_Class.Idenifier));
            function_statement.Children.Add(match(Token_Class.LParanthesisOp));
            function_statement.Children.Add(ArgumentList());
            function_statement.Children.Add(match(Token_Class.RParanthesisOp));

            return function_statement;
        }

        Node ArgumentList()
        {
            Node argumentList = new Node("ArgumentList");

                argumentList.Children.Add(Expression());
                argumentList.Children.Add(ARG());
                return argumentList;
        }
        Node ARG()
        {
            Node arg = new Node("ARG");
            if (InputPointer < TokenStream.Count &&
           (TokenStream[InputPointer].token_type == Token_Class.CommaOp))
            {
                arg.Children.Add(match(Token_Class.CommaOp));
                arg.Children.Add(Expression());
                arg.Children.Add(ARG());
                return arg;

            }
            
            return null;
        }

        Node Condition_Statement()
        {
            Node condition_statement = new Node("Condition_Statement");
            if ((InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.AndOp))
                || (InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.OrOp)))
            {
                condition_statement.Children.Add(Condition());
                condition_statement.Children.Add(Boolean_Operator());
                condition_statement.Children.Add(Condition());

                return condition_statement;
            }
            else
            {
                condition_statement.Children.Add(Condition());

                return condition_statement;
            }


        }

        bool isEquation(int pointer)
        {
            return ((pointer < TokenStream.Count && (TokenStream[pointer].token_type == Token_Class.LParanthesisOp))
            ||
            (pointer < TokenStream.Count && (TokenStream[pointer].token_type == Token_Class.Number))
            ||
            (pointer < TokenStream.Count && (TokenStream[pointer].token_type == Token_Class.Idenifier))
            );
        }


        Node Condition()
        {
            Node condition = new Node("Condition");
            condition.Children.Add(Term());
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());

            return condition;
        }
        // have to check equation
        Node Term()
        {
            Node term = new Node("Term");

            //number
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Number))
            {
                //number and after it is equation check then it's an equation
                if (isEquation(InputPointer + 1))
                {
                    term.Children.Add(Equation());
                    return term;
                }

                else
                {
                    term.Children.Add(match(Token_Class.Number));
                    return term;
                }
            }

            //function call
            else if ((InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
                    && (InputPointer + 1 < TokenStream.Count && (TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesisOp)))
            {
                //funcion call and after it is equation check then it's an equation
                if (isEquation(InputPointer + 1))
                {
                    term.Children.Add(Equation());
                    return term;
                }
                else
                {
                    term.Children.Add(Function_Call());
                    return term;
                }

            }
            //identifier
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.Idenifier))
            {
                //identifier and after it is equation check then it's an equation
                if (isEquation(InputPointer + 1))
                {
                    term.Children.Add(Equation());
                    return term;
                }
                else
                {
                    term.Children.Add(match(Token_Class.Idenifier));
                    return term;
                }
            }

            //just parenthesis then it's an equation
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.LParanthesisOp))
            {
                term.Children.Add(Equation());
                return term;
            }
            return null;
        }


        Node Equation()
        {
            Node equation = new Node("Equation");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.LParanthesisOp))
            {
                equation.Children.Add(match(Token_Class.LParanthesisOp));
                equation.Children.Add(Equation());
                equation.Children.Add(match(Token_Class.RParanthesisOp));
            }
            else
            {
                equation.Children.Add(Term());
                equation.Children.Add(Arithmetic_Operator());
                equation.Children.Add(Term());
            }

            return equation;
        }

        Node Expression()
        {
            Node expression = new Node("Expression");

            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.String))
            {
                expression.Children.Add(match(Token_Class.String));
            }
            else
            {
                expression.Children.Add(Term());
            }

            return expression;
        }

        Node Arithmetic_Operator()
        {
            Node arithmetic_operator = new Node("Arithmetic_Operator");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.PlusOp))
            {
                arithmetic_operator.Children.Add(match(Token_Class.PlusOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.MinusOp))
            {
                arithmetic_operator.Children.Add(match(Token_Class.MinusOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.MultiplyOp))
            {
                arithmetic_operator.Children.Add(match(Token_Class.MultiplyOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.DivideOp))
            {
                arithmetic_operator.Children.Add(match(Token_Class.DivideOp));
            }

            return arithmetic_operator;
        }

        Node Condition_Operator()
        {
            Node condition_operator = new Node("Condition_Operator");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.LessThanOp))
            {
                condition_operator.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp))
            {
                condition_operator.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.IsEqualOp))
            {
                condition_operator.Children.Add(match(Token_Class.IsEqualOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp))
            {
                condition_operator.Children.Add(match(Token_Class.NotEqualOp));
            }

            return condition_operator;
        }

        Node Boolean_Operator()
        {
            Node boolean_operator = new Node("Boolean_Operator");
            if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.AndOp))
            {
                boolean_operator.Children.Add(match(Token_Class.AndOp));
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.OrOp))
            {
                boolean_operator.Children.Add(match(Token_Class.OrOp));
            }

            return boolean_operator;
        }
        // Implement your logic here

        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
