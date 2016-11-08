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
'история центра', 'about.htm',
'деятельность', 'notfound.htm'
));

menuContent [1] = new Array ( 
-1, 
-1,
130,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'каталог', 'http://www.ntsomz.ru/imgcatalog/query.asp'
));


menuContent [2] = new Array ( 
-1, 
-1,
130,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (

' приема', 'techreg.htm',
' каталогизации', 'techcat.htm',
'мониторинга землепользования', 'techzeml.htm',
' мониторинга водных поверхностей ', 'techaqua.htm',
'мониторинга речных и пойменных затоплений ', 'techriver.htm',
'эколого-геологического мониторинга ', 'techeco.htm',
' мониторинга лесной растительности и пожаров', 'techfair.htm'
));

menuContent [3] = new Array ( 
-1, 
-1,
100,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'Ресурс-О1 №2', 'notfound.htm',
'Океан-О №1', 'notfound.htm',
'Ресурс-О1 №4', 'notfound.htm',
'Метеор-3М №1', 'notfound.htm',
'Океан-О1', 'notfound.htm',
'Ресурс-О1 №3', 'notfound.htm'
));

menuContent [4] = new Array ( 
-1, 
-1,
100,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'спутник  ', 'notfound.htm',
'спутник  ', 'notfound.htm',
'спутник  ', 'notfound.htm',
'спутник  ','notfound.htm',
'спутник  ','notfound.htm',
'спутник  ','notfound.htm',
'подробнее...', 'notfound.htm'
));


menuContent [5] = new Array ( 
-1, 
-1,
90,
-1, // x coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent x-coordinate
-1, // y coordinate (absolute) of left corner of this menu list, -1 if the coordinate is defined from parent y-coordinate
new Array (
'адреса', 'notfound.htm',
'телефоны', 'notfound.htm',
'интернет', 'notfound.htm',
'схема проехда','notfound.htm',
'подробнее...', 'notfound.htm'
));
