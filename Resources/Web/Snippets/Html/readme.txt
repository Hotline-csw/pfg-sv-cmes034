in die Folder you can create json files to configure your own snippets usable in the <html></html> section from the web editor 
sample 
htmlsnippet.json

sample content :
{
   "Class Dark for Div": {
    "prefix": "cd",
    "body": [
        "<div :class=\" this.colormode === 'light' ? 'first-itemData' : 'first-itemData_dark' \" class=\"${1:WorkPlace}\"><b>",
        " {{${2:workingplaceshow}}}</b></div>"
       
    ],
    "description": "Class Dark for Div"
	}
}


