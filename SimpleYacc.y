%{
// Эти объявления добавляются в класс GPPGParser, представляющий собой парсер, генерируемый системой gppg
    public BlockNode root; // Корневой узел синтаксического дерева 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
			public double dVal; 
			public int iVal; 
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
			public WhileNode whVal;
			public RepeatNode rVal;
			public ForNode fVal;
			public WriteNode wrVal;
			public VarDefNode vrVal;
			public IfNode ifelVal;
			public ArrayNode arVal;
			public SliceNode slVal;
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END CYCLE ASSIGN SEMICOLON DO UNTIL TO LBRACKET RBRACKET
%token COMMA PLUS MINUS MULT DIVISION THEN MOD LSQBRACKET RSQBRACKET COLON STRINGAP
%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID
%token <whVal> WHILE
%token <rVal> REPEAT
%token <fVal> FOR
%token <wrVal> WRITE
%token <vrVal> VAR
%token <ifelVal> IF ELSE

%type <eVal> expr ident 
%type <stVal> assign statement cycle empty
%type <blVal> stlist block
%type <vrVal> var
%type <eVal> t f
%type <ifelVal> if
%type <arVal> array
%type <slVal> slice

%%

progr   : block { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1); 
			}
		| stlist SEMICOLON statement 
			{ 
				$1.Add($3); 
				$$ = $1; 
			}
		;

statement: assign { $$ = $1; }
		| block   { $$ = $1; }
		| cycle   { $$ = $1; }
		| WHILE expr DO statement
			{
				$$ = new WhileNode($2,$4);
			}
		| REPEAT stlist UNTIL expr
			{
				$$ = new RepeatNode($4,$2);
			}
		| FOR assign TO expr DO statement
			{
				$$ = new ForNode($2,$4,$6);
			}
		| WRITE LBRACKET expr RBRACKET
		{
			$$ = new WriteNode($3);
		}
		| if
		{
			$$ = $1;
		}		
		| VAR var
		{
			$$ = $2;
		}
		| empty   { $$ = $1; }
		;

empty	: { $$ = new EmptyNode(); }
		;
if: IF expr THEN statement ELSE statement 
	{
		$$ = new IfNode($2,$4,$6);
	}
	| IF expr THEN statement
	{
		$$ = new IfNode($2,$4,null);
	}
	;
var: 
		array {
			$$ = new VarDefNode($1);
		}
		| ID {
			$$ = new VarDefNode($1);
		}
		
		| array COMMA var 
		{
			$$.Add($1);
		}
		| ID COMMA var
		{
			$$.Add($1);
		} ;

ident 	: ID { $$ = new IdNode($1); }	
		;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1, $3); }
		| slice ASSIGN expr { $$ = new AssignNode($1, $3);}
		;

expr : t { $$ = $1; }
     | expr PLUS t { $$ = new BinaryNode($1,$3,'+'); }
     | expr MINUS t { $$ = new BinaryNode($1,$3,'-'); }
     ;

t    : f { $$ = $1; } 
     | t MULT f { $$ = new BinaryNode($1,$3,'*'); }
     | t DIVISION f { $$ = new BinaryNode($1,$3,'/'); }
     | t MOD f { $$ = new BinaryNode($1,$3,'%'); }
     ;

f    : ident { $$ = $1 as IdNode; }
	 | array { $$ = $1 as ArrayNode; }
	 | slice { $$ = $1 as SliceNode; }
     | INUM  { $$ = new IntNumNode($1); }
     | LBRACKET expr RBRACKET { $$ = $2; }
     ;

block	: BEGIN stlist END { $$ = $2; }
		;

cycle	: CYCLE expr statement { $$ = new CycleNode($2, $3); }
		;
array : ID LSQBRACKET INUM RSQBRACKET { $$ = new ArrayNode($1,$3); }
		;
slice:  ID LSQBRACKET INUM COLON RSQBRACKET { $$ = new SliceNode($1,$3,int.MaxValue); }
		| ID LSQBRACKET COLON INUM RSQBRACKET { $$ = new SliceNode($1,0,$4);}
		| ID LSQBRACKET INUM COLON INUM RSQBRACKET {$$ = new SliceNode($1,$3,$5);}
		| ID LSQBRACKET COLON RSQBRACKET {$$ = new SliceNode($1,0,int.MaxValue);}
		| ID LSQBRACKET INUM COLON COLON INUM RSQBRACKET {$$ = new SliceNode($1,$3,int.MaxValue,$6);}
		| ID LSQBRACKET COLON COLON INUM RSQBRACKET {$$ = new SliceNode($1,0,int.MaxValue,$5);}
		| ID LSQBRACKET COLON COLON RSQBRACKET {$$ = new SliceNode($1,0,int.MaxValue);}
		| ID LSQBRACKET INUM COLON COLON RSQBRACKET {$$ = new SliceNode($1,$3,int.MaxValue);}
		| ID LSQBRACKET INUM COLON INUM COLON INUM RSQBRACKET {$$ = new SliceNode($1,$3,$5,$7);}
		| ID LSQBRACKET INUM COLON INUM COLON RSQBRACKET {$$ = new SliceNode($1,$3,$5);}
		;
%%

