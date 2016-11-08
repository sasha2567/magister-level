// check browser version
NS4 = (document.layers) ? 1 : 0;

var topID  = -1;

// constructor of menu elements
function menuConstructor (id, content)
{
	this.ID            = id;
	this.parentID      = content [0]*1;
	this.parentItemID  = content [1]*1;
	this.width         = content [2]*1;
	this.timerID       = -1;
	this.isOn          = false;
	this.item          = new Array ();
	this.currItemID    = -1;

	if (this.parentID == -1)
	{
		this.x = initX + usedWidth;
		usedWidth = usedWidth + this.width;
		this.y = initY;
	}
	else
	{
		this.y = content [3]*1;
		if (this.y < 0)
			this.y =  menuElement [this.parentID].y
		 		      + itemHeight*this.parentItemID
				      + yOverlap;
		this.x = content [4]*1;
		if (this.x < 0)
			this.x =  menuElement [this.parentID].x
				      + menuElement [this.parentID].width
				      - xOverlap;
	}
	items = content [5];

	layerBody = '<table width=' + this.width + ' cellpadding=3 cellspacing=' + borderSize + ' border=0>';
	
	for (j = 0; j <= items.length - 2; j = j + 2)
	{
		controlBlock = ' onMouseOver = "enterItem (' + this.ID + ', ' + ((j + 2)/2 - 1) + ');" onMouseOut = "exitItem (' + this.ID + ', ' + ((j + 2)/2 - 1) + ');" ';
		layerBody += '<td height=' + itemHeight + ' width=' + this.width + ' bgcolor=' + backColor + '><a  class=black href='+ items [j + 1] +' ' + controlBlock + '>' + items [j] + '</a></td>';
		if (j < items.length - 2)
			layerBody = layerBody +  '<tr>\n';
		else
			layerBody = layerBody + '\n';
	}

	if (!NS4)
		layerHeader = '<div id=Menu' + this.ID +
				   	   ' onMouseOver="enterMenu (' + this.ID + ');" onMouseOut = "exitMenu (' + this.ID + ');"' +
		    	       ' style="background:#003366 ; width: ' + this.width + '; visibility: hidden; position: absolute; left: ' + this.x +
		        	   '; top: ' + this.y + ';">';
	else
		layerHeader = '<layer id=Menu' + this.ID +
					   ' onMouseOver="enterMenu (' + this.ID + ');" onMouseOut = "exitMenu (' + this.ID + ');"' +
					   ' visibility=hide left=' + this.x +
					   ' top =' + this.y + '>';

	layerHeader += '<table width=' + this.width + ' cellpadding=0 cellspacing=0 border=0>' +
				    '<td bgcolor=' + borderColor + '>';

	layerFooter = '</table></td></table>';

	if (!NS4)
		layerFooter = layerFooter + '</div>';
	else
		layerFooter = layerFooter + '</layer>';

	document.writeln (layerHeader + layerBody + layerFooter);

	return this;
}
function enterTopItem (ID)
{
	if (topID != ID && topID != -1)
		hideTree (topID);
	releaseTree (ID);
	topID = ID;
	show (ID);
}
function exitTopItem (ID)
{
	menuElement [ID].timerID = setTimeout ('hide (' + ID + ')', delay);
}
function enterItem (menuID, itemID)
{
	var currItemID = menuElement [menuID].currItemID;

	if (currItemID != i & currItemID > -1)
		hide (currItemID);

	for (var i = 0; i < numOfMenus; i++)
	{
		if (menuElement [i].parentID == menuID &&
		    menuElement [i].parentItemID == itemID)
		{
			clearTimeout (menuElement [i].timerID);
			menuElement [i].timerID = -1;
			show (i);
			return 0;
		}
	}

	return -1;
}
function exitItem (menuID, itemID)
{
	for (var i = 0; i < numOfMenus; i++)
	{
		if (menuElement [i].parentID == menuID &&
		    menuElement [i].parentItemID == itemID)
		{
			menuElement [i].timerID = setTimeout ('hide (' + i + ')', delay);
			return 0;
		}
	}
}
function enterMenu (ID)
{
	var parentID = menuElement [ID].parentID;
	if (parentID == -1)
	{
		clearTimeout (menuElement [ID].timerID);
		menuElement [ID].timerID = -1;
	}
	else
		releaseTree (ID);
}
function exitMenu (ID)
{
	timeoutTree (ID);
}
function hideTree (ID)
{
	hide (ID);
	for (var j = 0; j < numOfMenus; j++)
	{
		if (menuElement [j].parentID == ID &&
			menuElement [j].isOn)
		{
			hideTree (j);
			return 0;
		}
	}
}
function releaseTree (ID)
{
	clearTimeout (menuElement [ID].timerID);
	menuElement [ID].timerID = -1;

	var parentID = menuElement [ID].parentID;
	if (parentID > -1)
		releaseTree (parentID);
}
function timeoutTree (ID)
{
	menuElement [ID].timerID = setTimeout ('hide (' + ID + ')', delay);
	var parentID = menuElement [ID].parentID;
	if (parentID > -1)
		timeoutTree (parentID);
}

function show (ID)
{
	if (!NS4)
		document.all['Menu' + ID].style.visibility = "visible";
	else
		document.layers[ID].visibility = "visible";

	menuElement [ID].isOn = true;

	if (menuElement [ID].parentID > -1)
		menuElement [menuElement [ID].parentID].currItemID = ID;
}

function hide (ID)
{
	if (!NS4)
		document.all['Menu' + ID].style.visibility = "hidden";
	else
		document.layers['Menu' + ID].visibility = "hide";

	menuElement [ID].isOn = false;

	if (menuElement [ID].parentID > -1)
		menuElement [menuElement [ID].parentID].currItemID = -1;
}

function createMenuTree ()
{
	for (var i = 0; i < menuContent.length; i++)
	{
		menuElement [i] = new menuConstructor (i, menuContent [i]);
		numOfMenus++;
	}
}

createMenuTree ();