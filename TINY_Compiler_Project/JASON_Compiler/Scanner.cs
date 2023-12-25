using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    DataType, Read, Write, Repeat, Until, If , ElseIf, Else , Then, Return , Endl, End,

    String,

    PlusOp, MinusOp, MultiplyOp, DivideOp,

    LessThanOp, GreaterThanOp, NotEqualOp, IsEqualOp,

    AndOp, OrOp,

    LParanthesisOp, RParanthesisOp,  LBrackectOp, RBracketOp,

    AssignmentOp, SemicolonOp, CommaOp,

    Idenifier, Number,
    Main,


    NewLine, Space
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
            ReservedWords.Add("int", Token_Class.DataType);
            ReservedWords.Add("float", Token_Class.DataType);
            ReservedWords.Add("string", Token_Class.DataType);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("end", Token_Class.End);

            ReservedWords.Add("main", Token_Class.Main);


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
            Operators.Add("\n", Token_Class.NewLine);  // done
            Operators.Add("\r", Token_Class.NewLine);  // done
            Operators.Add(" ", Token_Class.Space);  // done
        }
        
        public void StartScanning(string SourceCode)
        {
            for(int i=0; i<SourceCode.Length;i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                    continue;


                //number
                else if(CurrentChar >= '0' && CurrentChar <= '9') // if you read number
                {
                    for(j = j + 1; j < SourceCode.Length; j++)
                    {
                        CurrentChar = SourceCode[j];
                         if(Operators.ContainsKey(CurrentChar.ToString()))
                        {
                            
                            break;
                        }
                        else 
                        {
                            CurrentLexeme += CurrentChar.ToString();

                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //string
                else if (CurrentChar == '"')
                {
                    for (j = j + 1; j < SourceCode.Length; j++)
                    {
                        CurrentChar = SourceCode[j];
                        if(CurrentChar == '"' || CurrentChar == '\n')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            j++;
                            break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }


                //comment
                else if (CurrentChar == '/')
                {

                    if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '*')
                    {
                        CurrentLexeme += SourceCode[i + 1].ToString();
                        for (j = j + 2; j < SourceCode.Length; j++)
                        {
                            CurrentChar = SourceCode[j];
                            if (CurrentChar == '*')
                            {
                                if (j + 1 < SourceCode.Length && SourceCode[j + 1] == '/')
                                {
                                    CurrentLexeme += CurrentChar.ToString();
                                    CurrentChar = '/';
                                    CurrentLexeme += CurrentChar.ToString();
                                    j++;
                                    break;
                                }
                            }
                            CurrentLexeme += CurrentChar.ToString();

                        }
                    }
                    i = j;
                    //FindTokenClass(CurrentLexeme);
                }

            



                // if you read operator
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
                    {
                        CurrentChar = SourceCode[j + 1];
                        CurrentLexeme += CurrentChar.ToString();
                        i = j+1;
                    }
                    else
                    {
                        i = j;
                    }

                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '|')
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '|')
                    {
                        CurrentChar = SourceCode[j + 1];
                        CurrentLexeme += CurrentChar.ToString();
                        i = j + 1;
                    }
                    else
                    {
                        i = j;
                    }

                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == ':')
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '=')
                    {
                        CurrentChar = SourceCode[j + 1];
                        CurrentLexeme += CurrentChar.ToString();
                        i = j + 1;
                    }
                    else
                    {
                        i = j;
                    }

                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '<') 
                {
                    if (j != SourceCode.Length - 1 && SourceCode[j + 1] == '>')
                    {
                        CurrentChar = SourceCode[j + 1];
                        CurrentLexeme += CurrentChar.ToString();
                        i = j + 1;
                    }
                    else
                    {
                        i = j;
                    }
              
                    FindTokenClass(CurrentLexeme);
                }



                //identifiers
                else if ((CurrentChar >= 'a' && CurrentChar <= 'z') || (CurrentChar >= 'A' && CurrentChar <= 'Z'))
                {

                    for (j = j + 1; j < SourceCode.Length; j++)
                    {
                        CurrentChar = SourceCode[j];
                        if (!((CurrentChar >= 'a' && CurrentChar <= 'z') || (CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= '0' && CurrentChar <= '9')))
                        {
                            break;
                        }

                        CurrentLexeme += CurrentChar.ToString();
                    } 

                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }


                //None
                else
                {
                    FindTokenClass(CurrentChar.ToString());
                }
            }

            
            TINY_Compiler.TokenStream = Tokens;
        }


        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;

            //Is it a string
            if (isString(Lex))
            {
                Tok.lex = Lex.Substring(1, Lex.Length - 2);
                
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
             

            //Is it a reserved word?
            else if (isReservedkeyword(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok); 
            }


            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }

            //Is it a comment?
            /*else if (isCommentStatment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }*/

            //Is it a number?
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }

            //Is it an operator?
            else if (isOperator(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }

            //Is it an undefined?
            //Error list
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }


        bool isString(string Lex)
        {
            Regex re = new Regex(@"^\x22[^(\x22|\n)]*\x22$", RegexOptions.Compiled);
            if (re.IsMatch(Lex) == true)
            {
                return true;
            }
            return false;
        }

        bool isIdentifier(string lex)
        {

            Regex re = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
            if (re.IsMatch(lex) == true)
            {
                return true;
            }
            return false;
        }

        bool isNumber(string lex)
        {
            Regex re = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            if (re.IsMatch(lex) == true)
            {
                return true;
            }

            return false;
        }
        bool isReservedkeyword(string lex)
        {
            Regex re = new Regex(@"^(main|int|float|string|read|write|repeat|until|if|elseif|else|then|return|endl|end)$", RegexOptions.Compiled);
            if (re.IsMatch(lex) == true)
            {
                return true;
            }

            return false;
        }
        //bool isCommentStatment(string lex)
        //{
        //    Regex re = new Regex(@"^/\*(.|\n)*\*/$", RegexOptions.Compiled);
        //   if (re.IsMatch(lex) == true)
        //   {
        //     return true;
        //   }

        //    return false;
        //}


        bool isOperator(string lex)
        {
            Regex re = new Regex(@"^(\+|\-|\*|\/|,|;|<|>|=|&&|\|\||<>|:=|\(|\)|\{|\})$" , RegexOptions.Compiled);
            if (re.IsMatch(lex) == true)
            {
                return true;
            }

            return false;
            
        }
    }
}
