// write me if you have questions: rusanov@megashop.ru
// the best e-commerce programmer: rusanov@megashop.ru
// Internet-design, programming and more...

// constants
var initX = 200; // x-coordinate of top left corner of dropdown menu 
var initY       = 120; // y-coordinate of top left corner of dropdown menu 
var backColor   = '#ffffff'; // the background color of dropdown menu, set empty '' for transparent
var borderColor = '#333666'; // the color of dropdown menu border
var borderSize  = '1'; // the width of dropdown menu border
var itemHeight  = 20;
var xOverlap    = 5;
var yOverlap    = 10;
//

// Don't change these parameters
var delay        = 500; /////
var menuElement  = new Array ();
var usedWidth    = 0;
var numOfMenus   = 0;
/// ----------------------------

menuContent     = new Array ();

menuContent [0] = new Array ( 
-1, // the id of parent menu, -1 if this is a first level menu
-1, // the number of line in parent menu, -1 if this is a first level menu
120, // the width of current menu list 
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'������� ������', 'about.htm',
'������������', 'notfound.htm'
));

menuContent [1] = new Array ( 
-1, 
-1,
130,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'�������', 'http://www.ntsomz.ru/imgcatalog/query.asp'
));


menuContent [2] = new Array ( 
-1, 
-1,
130,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (

' ������', 'techreg.htm',
' �������������', 'techcat.htm',
'����������� ����������������', 'techzeml.htm',
' ����������� ������ ������������ ', 'techaqua.htm',
'����������� ������ � ��������� ���������� ', 'techriver.htm',
'�������-�������������� ����������� ', 'techeco.htm',
' ����������� ������ �������������� � �������', 'techfair.htm'
));

menuContent [3] = new Array ( 
-1, 
-1,
100,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'������-�1 �2', 'notfound.htm',
'�����-� �1', 'notfound.htm',
'������-�1 �4', 'notfound.htm',
'������-3� �1', 'notfound.htm',
'�����-�1', 'notfound.htm',
'������-�1 �3', 'notfound.htm'
));

menuContent [4] = new Array ( 
-1, 
-1,
100,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'�������  ', 'notfound.htm',
'�������  ', 'notfound.htm',
'�������  ', 'notfound.htm',
'�������  ','notfound.htm',
'�������  ','notfound.htm',
'�������  ','notfound.htm',
'���������...', 'notfound.htm'
));


menuContent [5] = new Array ( 
-1, 
-1,
90,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'������', 'notfound.htm',
'��������', 'notfound.htm',
'��������', 'notfound.htm',
'����� �������','notfound.htm',
'���������...', 'notfound.htm'
));
