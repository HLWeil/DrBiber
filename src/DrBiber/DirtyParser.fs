module DrBiber.DirtyParser

open System
open FSharpAux
open System.Text

let tryParseBibtexField (i : int) (bibtex:string) =
    let mutable i = i
    let mutable afterEquals = false
    let mutable insideBrace : char option = None
    let mutable braceCount = 0
    let nameBuilder = new StringBuilder()
    let valueBuilder = new StringBuilder()
    let returnName() = Some (nameBuilder.ToString().Trim().ToLower(), valueBuilder.ToString().Trim()), i
    let rec loop() = 
        let current = bibtex.[i]
        match current with
        | '=' when insideBrace.IsNone -> afterEquals <- true; i <- i + 1; loop()

        | ',' when insideBrace.IsNone -> returnName()

        | '}' when insideBrace = Some '{' && braceCount < 2 -> returnName()
        | '}' when insideBrace = Some '{' -> 
            valueBuilder.Append(current) |> ignore
            braceCount <- braceCount - 1
            i <- i + 1; loop()
        | '}' when insideBrace = None && afterEquals -> 
            i <- i - 1;
            returnName()
        | '}' when insideBrace = None -> None, i

        | '{' when insideBrace.IsSome -> 
            valueBuilder.Append(current) |> ignore
            braceCount <- braceCount + 1
            i <- i + 1; loop()
        | '{' when insideBrace = None ->             
            insideBrace <- Some '{'
            braceCount <- braceCount + 1
            i <- i + 1; loop()

        | '\"' when insideBrace = Some  '\"' -> returnName()
        | '\"' when insideBrace = None -> insideBrace <- Some '\"'; i <- i + 1; loop()

        | _ ->
            if afterEquals then
                valueBuilder.Append(current) |> ignore
            else
                nameBuilder.Append(current) |> ignore
            i <- i + 1
            loop()
    loop()

let parseCiteKey (i : int) (bibtex:string) =
    let mutable i = i
    let nameBuilder = new StringBuilder()
    let rec loop() = 
        let current = bibtex.[i]
        if current = ',' then
            nameBuilder.ToString().Trim(), i
        else
            nameBuilder.Append(current) |> ignore
            i <- i + 1
            loop()           
    loop()

let parseType (i : int) (bibtex:string) =
    let mutable i = i
    let nameBuilder = new StringBuilder()
    let rec loop() = 
        let current = bibtex.[i]
        if current = '{' then
            nameBuilder.ToString().Trim(), i
        else
            nameBuilder.Append(current) |> ignore
            i <- i + 1
            loop()           
    loop()

let parseBibTexEntry (i : int) (bibtex:string) =
    let entryType, i = parseType i bibtex
    let citeKey, i = parseCiteKey (i+1) bibtex
    let entry = BibTeXEntry(entryType, citeKey)
    let mutable i = i + 1
    let rec loop() = 
        let current = bibtex.[i]
        if current = '}' then
            entry, i
        elif current = ',' then
            let kv,j = tryParseBibtexField (i+1) bibtex
            match kv with
            | Some (name,value) -> 
                entry.SetProperty(name,value)
                i <- j + 1
            | None ->
                i <- j
            loop()
        else
            let kv,j = tryParseBibtexField (i) bibtex
            match kv with
            | Some (name,value) -> 
                entry.SetProperty(name,value)          
                i <- j + 1
            | None ->
                i <- j
            loop()
    loop()


let bibTeXEntryFromString (bibtex:string) =
    let rec loop (i : int) (entries : BibTeXEntry list) =
        if i = bibtex.Length then 
            entries |> List.rev
        elif bibtex.[i] = '@' then
            let entry, i = parseBibTexEntry (i + 1) bibtex
            loop (i + 1) (entry::entries)
        else
            loop (i + 1) entries
    loop 0 []

[<Obsolete>]
let parseBibTex = bibTeXEntryFromString

let bibTeXEntriesFromFile (path : string) =
    let s = System.IO.File.ReadAllText path
    bibTeXEntryFromString s

let parseBibTexFile = bibTeXEntriesFromFile

let bibTeXEntryToString (entry : BibTeXEntry) =
    let sb = new StringBuilder()
    sb.Append("@") |> ignore
    sb.Append(entry.EntryType) |> ignore
    sb.Append("{") |> ignore
    sb.Append(entry.CiteKey) |> ignore
    sb.Append(",\n") |> ignore
 
    entry.Properties
    |> Seq.iteri (fun i kv ->
        sb.Append("\t" + kv.Key)   |> ignore
        sb.Append(" = {")   |> ignore
        sb.Append(kv.Value) |> ignore
        let endString = if i + 1 = entry.Properties.Count then "}\n" else "},\n"
        sb.Append(endString) |> ignore
    )
    sb.Append("}") |> ignore
    sb.ToString()

let bibTeXEntriesToFile (path : string) (entries : BibTeXEntry list) =
    let sb = new StringBuilder()
    for entry in entries do
        sb.Append(bibTeXEntryToString entry) |> ignore
        sb.Append("\n\n") |> ignore
    System.IO.File.WriteAllText(path, sb.ToString())