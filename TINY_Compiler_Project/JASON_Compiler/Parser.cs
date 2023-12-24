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
