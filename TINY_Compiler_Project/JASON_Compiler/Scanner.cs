using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Int, Float, String , Read, Write, Repeat, Until, If , ElseIf, Else , Then, Return , Endl,

    PlusOp, MinusOp, MultiplyOp, DivideOp,

    LessThanOp, GreaterThanOp, NotEqualOp, IsEqualOp,

    AndOp, OrOp,

    LParanthesisOp, RParanthesisOp,  LBrackectOp, RBracketOp,

    AssignmentOp, SemicolonOp, CommaOp,

    Idenifier, Number
}

namespace TINY_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("Repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);

            
            Operators.Add(";", Token_Class.SemicolonOp);  // done
            Operators.Add(",", Token_Class.CommaOp);        // done
            Operators.Add("(", Token_Class.LParanthesisOp); // done
            Operators.Add(")", Token_Class.RParanthesisOp);  // done
            Operators.Add("=", Token_Class.IsEqualOp);    // done
            Operators.Add("<", Token_Class.LessThanOp);   // done
            Operators.Add(">", Token_Class.GreaterThanOp); // done
            Operators.Add("<>", Token_Class.NotEqualOp); // done
            Operators.Add("+", Token_Class.PlusOp);  // done
            Operators.Add("-", Token_Class.MinusOp); // done
            Operators.Add("*", Token_Class.MultiplyOp); // done
            Operators.Add("/", Token_Class.DivideOp);  // done
            Operators.Add("&&", Token_Class.AndOp);    // done
            Operators.Add("||", Token_Class.OrOp);    // done
            Operators.Add(":=", Token_Class.AssignmentOp); //done
            Operators.Add("{", Token_Class.LBrackectOp); // done
            Operators.Add("}", Token_Class.RBracketOp);  // done
        }
        
        public void StartScanning(string SourceCode)
        {
            for(int i=0; i<SourceCode.Length;i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                   
                }

                else if(CurrentChar >= '0' && CurrentChar <= '9')
                {
                    
                }
                else if(CurrentChar == '{' || CurrentChar == '}' 
                     || CurrentChar == '(' || CurrentChar == ')'
                     || CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*' || CurrentChar == '/'
                     || CurrentChar == ',' || CurrentChar == ';' || CurrentChar == '=' || CurrentChar == '>')
                {
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else if(CurrentChar=='&') 
                {
                    if (j != SourceCode.Length-1 && SourceCode[j + 1] == '&')
                        CurrentLexeme +=CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '|')
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '|')
                        CurrentLexeme += CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == ':')
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '=')
                        CurrentLexeme += CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '<') 
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '>')
                        CurrentLexeme += CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else
                {
                   
                }
            }
            
            TINY_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            

            //Is it an identifier?
            

            //Is it a number?

            //Is it an operator?
              
            //Is it an undefined?
        }

    

        bool isIdentifier(string lex)
        {
            bool isValid=true;
            // Check if the lex is an identifier or not.
            
            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            // Check if the lex is a Number (constant) or not.

            return isValid;
        }
        bool isOperator(string lex)
        {
            if (lex == "{" || lex == "}"
               || lex == "(" || lex == ")"
               || lex == "+" || lex == "-" || lex == "*" || lex == "/"
               || lex == "," || lex == ";" || lex == "=" || lex == ">"
               || lex == "&&"|| lex == "||"|| lex == ":="|| lex == "<>")
                    return true;  // return true if condition satsifed
            return false;
            
        }
    }
}
