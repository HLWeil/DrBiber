#r "nuget: DynamicObj, 4.0.3"

#r "bin/Release/netstandard2.0/DrBiber.dll"


#r "nuget: DrBiber"

open DrBiber

let bibTexEntry = new DrBiber.BibTeXEntry("article", "citekey")


let s = """
@article{qiao_legume_2024,
	title = {Legume rhizodeposition promotes nitrogen fixation by soil microbiota under crop diversification},
	volume = {15},
	issn = {2041-1723},
	url = {https://www.nature.com/articles/s41467-024-47159-x},
	doi = {10.1038/s41467-024-47159-x},
	pages = {2924}
}
"""

let entries = DirtyParser.bibTeXFromString s

let myEntry = entries.Head

myEntry.EntryType
myEntry.CiteKey
myEntry.TryGetVolume()



DirtyParser.bibTeXToString entries



DynamicObj.DynObj.format myEntry


bibTexEntry
|> DynamicObj.DynObj.format