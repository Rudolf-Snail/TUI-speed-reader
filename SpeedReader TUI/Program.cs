// See https://aka.ms/new-console-template for more information
using Speed_reader;

var text = "Hello, fellas. How we doing? We doin'? Doing well? I hope so. I haven't had the best of days myself, so I hope you're faring better.";
var speedReader = new SpeedReader(300, text);
speedReader.SpeedReadText();