grammar Hello;
query   : condition
        | like 
        | (condition|like) AND query ;

condition: field op DATE ;
like: 'filenamelike\'' LIKE '\'' ;
op: ('>='|'<'|'==') ;
field: FIELD ;

fragment D     : [0-9] ;
DATE: '\'' D D D D '-' D D '-' D D '\'' ;
FIELD : [A-Za-z] [A-Za-z0-9_]* ;             
AND   : '&' ;
LIKE  : [A-Za-z0-9_ %()[\]\\-]+ ;
WS    : [ \t\r\n]+ -> skip ; // skip spaces, tabs, newlines
