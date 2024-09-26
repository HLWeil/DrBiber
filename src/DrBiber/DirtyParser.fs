module DrBiber.DirtyParser

open System
open FSharpAux
open System.Text
open System.Text.RegularExpressions

[<Literal>]
let BibitemSplitRegex = @".*@(?<type>[^{]+){(?<id>[^,]*),(?<body>.+)}"

// let BibitemBodyAttributesRegex = @"(?<attribute>[^{}]*)\s*=\s*\{(?<value>(?:[^{}]|(?<open>\{)|(?<-open>\}))*(?(open)(?!)))\}(,|$)"
[<Literal>]
let BibitemBodyAttributesRegex = @"(?<attribute>[^{}]*)\s*=\s*\{(?<value>[^{}]*)\}(,|$)"


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
        | '}' when insideBrace = None && afterEquals -> returnName()
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
    let entry = BibTexEntry(entryType, citeKey)
    let mutable i = i + 1
    let rec loop() = 
        let current = bibtex.[i]
        if current = '}' then
            entry, i
        elif current = ',' then
            let kv,j = tryParseBibtexField (i+1) bibtex
            match kv with
            | Some (name,value) -> 
                entry.SetValue(name,value)
                i <- j + 1
            | None ->
                i <- j
            loop()
        else
            let kv,j = tryParseBibtexField (i) bibtex
            match kv with
            | Some (name,value) -> 
                entry.SetValue(name,value)          
                i <- j + 1
            | None ->
                i <- j
            loop()
    loop()

let parseBibTex (bibtex:string) =
    let rec loop (i : int) (entries : BibTexEntry list) =
        if i = bibtex.Length then 
            entries |> List.rev
        elif bibtex.[i] = '@' then
            let entry, i = parseBibTexEntry (i + 1) bibtex
            loop (i + 1) (entry::entries)
        else
            loop (i + 1) entries
    loop 0 []

let parseBibTexFile (path : string) =
    let s = System.IO.File.ReadAllText path
    parseBibTex s