#r "nuget: DynamicObj, 4.0.0"

#r "bin/Debug/netstandard2.0/DrBiber.dll"

let bibTexEntry = new DrBiber.BibTexEntry("article", "citekey")

bibTexEntry
|> DynamicObj.DynObj.format