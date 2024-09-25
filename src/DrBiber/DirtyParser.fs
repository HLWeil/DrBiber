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
    let nameBuilder = new StringBuilder()
    let valueBuilder = new StringBuilder()
    let returnName() = Some (nameBuilder.ToString().Trim(), valueBuilder.ToString().Trim()), i
    let rec loop() = 
        let current = bibtex.[i]
        if current = ',' && insideBrace.IsNone then
            returnName()
        elif current = '}' && insideBrace = Some '{' then
            returnName()
        elif current = '}' && insideBrace = None && afterEquals then
            returnName()
        elif current = '{' && insideBrace = None then
            insideBrace <- Some '{'
            i <- i + 1
            loop()
        elif current = '\"' && insideBrace = Some  '\"' then
            returnName()
        elif current = '\"' && insideBrace = None then
            insideBrace <- Some '\"'
            i <- i + 1
            loop()
        elif current = '=' && insideBrace.IsNone then
            afterEquals <- true
            i <- i + 1
            loop()
        elif current = '}' && insideBrace = None then
            None, i
        else
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
            entry
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