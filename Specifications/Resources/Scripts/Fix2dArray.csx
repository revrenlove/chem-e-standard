﻿// Ignore the red squiggle under `Env` - it's a scriptcs thing...
var filename = Env.ScriptArgs[0];

if (!System.IO.File.Exists(filename))
{
    throw new System.Exception($"{filename} does not exist.");
}

var fileContents = System.IO.File.ReadAllText(filename);

var rgx = new System.Text.RegularExpressions.Regex("(?<=typeof\\()(\\w+)(?=\\).+\n.+\\[\\]\\[\\])");

fileContents = rgx.Replace(fileContents, "$1[]");

System.IO.File.Delete(filename);
System.IO.File.WriteAllText(filename, fileContents);

System.Console.Write($"{filename} successfully updated.");
