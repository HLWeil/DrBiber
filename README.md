# DrBiber
FSharp BibTex file parser.

| Version | Downloads |
| :--------|-----------:|
|<a href="https://www.nuget.org/packages/DrBiber/"><img alt="Nuget" src="https://img.shields.io/nuget/v/DrBiber?logo=nuget&color=%239f8170"></a>|<a href="https://www.nuget.org/packages/DrBiber/"><img alt="Nuget" src="https://img.shields.io/nuget/dt/DrBiber?color=%239f8170"></a>|

## Usage

```
#r "nuget: DrBiber"

open DrBiber
```

Given the following BibTex entry:
```fsharp
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
```


### Reading
You can parse using the DirtyParser™️:

```fsharp
let entries = DirtyParser.parseBibTex s

let myEntry = entries.Head
```
->
```
val entries: BibTexEntry list = [DrBiber.BibTexEntry]
val myEntry: BibTexEntry
```

### Access

```fsharp
myEntry.EntryType
myEntry.CiteKey
myEntry.TryGetVolume()
```
->
```
val it: string = "article"
val it: string = "qiao_legume_2024"
val it: obj option = Some "15"
```


### Formatting
Format the full entry using
```
DynamicObj.DynObj.format myEntry
```


->
```
val it: string =
  "EntryType: article
CiteKey: qiao_legume_2024
?title: Legume rhizodeposition promotes nitrogen fixation by soil microbiota under crop diversification
?volume: 15
?issn: 2041-1723
?url: https://www.nature.com/articles/s41467-024-47159-x
?doi: 10.1038/s41467-024-47159-x
?pages: 2924"
```
