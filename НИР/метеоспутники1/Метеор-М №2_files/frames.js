function BuildFrames(PageURL)
{
var Lvl = CheckSubLvl(PageURL);
CheckFrames(PageURL, Lvl);
}

function CheckSubLvl(PageURL)
{
	//-------------------------CHECK DOCUMENT SUB-LEVEL-------------------//
	var subURL = PageURL;
	var length = PageURL.length;
	var slash = 0;
	var slashNum = 0;
	var subLvl = 0;
	for(;;)
	{
		slash = subURL.lastIndexOf("/");
		if(slash != -1)
		{
			subURL = subURL.substring(0, slash);
			slashNum++;
		}
		else
		{
			if(slashNum >= 3)
			{
				subLvl=slashNum-3;// Calculate current document sub-level (ignoring 3 slashes: "http://sitename/"...)
			}
			else
			{
				subLvl=1;
			}
			break;
		}
	}
return subLvl;
}

function CheckFrames(PageURL, subLvl)
{

	var plusSubLvl = "../";
	var leftMenu = "left_menu.html";
	var topMenu = "top_menu.html";
	
	for(;;)
	{
		//calculate a path to html pages displayed in other frames
		if(subLvl != 0)
		{
			leftMenu = plusSubLvl+leftMenu;
			topMenu = plusSubLvl+topMenu;
			subLvl--;
		}
		else
		{
			break;
		}
	}

  if (window.name != "body")
	{
    window.name="root";
    document.write("<HTML>");
    document.write("<HEAD>");
    document.write("<TITLE>НИЦ &quot;Планета&quot;</TITLE>");
    document.write("<META content='text/html; charset=windows-1251' http-equiv='Content-Type'>");
    document.write("</HEAD>");
    document.write("<FRAMESET cols='191,*' border='0' frameBorder='0' frameSpacing='0'>");
    document.write("<FRAME name='menu01' src='" + leftMenu + "' border='0' frameBorder='0' marginHeight='0' scrolling='no'>");
    document.write("<FRAMESET rows='36,*' border='0' frameBorder='0' frameSpacing='0'>");
    document.write("<FRAME name='menu02' src='" + topMenu + "' border='0' frameBorder='0' marginHeight='0' scrolling='no'>");
    document.write("<FRAME name='body' src='" + PageURL + "?embedded=yes' border='0' frameBorder='0' marginHeight='0' marginWidth='0' scrolling='YES'>");
    document.write("</FRAMESET>");
    document.write("</FRAMESET>");
    document.write("<noframes><body>");
    document.write("Error!<br>Unable to display the frameset.");
    document.write("</body></noframes>");
    document.write("</HTML>");
	//document.write("<frameset rows='40,*'>");
	//document.write("<frame name='menu' src='menu.htm'>");
	//document.write("<frame name='main' src='" + PageURL + "?embedded=yes'>");
	//document.write("</frameset>");
  }
}

