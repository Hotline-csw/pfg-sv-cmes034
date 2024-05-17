in die Folder you can create json files to configure your own snippets usable in the <script></script> section  from the web editor 
sample 
javascriptsnippet.json

sample content :
{
	"BoxTagFormatted": {
        "prefix": "btf",
        "body": [
            "this.boxtagformated = window.DateTime.fromFormat(this.boxtag, 'yyyyMMdd').toFormat('dd.MM.yyyy');"
        ],
        "description": "get the correct BoxTag format"
    }
}


