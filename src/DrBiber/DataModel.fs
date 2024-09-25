namespace DrBiber

open DynamicObj
open System.Collections.Generic

type BibTexEntry(entryType : string, ?citekey : string, ?attributes : Dictionary<string,string>) as this =
    
    inherit DynamicObj()

    do
        match attributes with
        | None -> ()
        | Some attributes ->
            for kv in attributes do
                this.Properties.Add(kv.Key, kv.Value)

    member this.EntryType = entryType

    member this.CiteKey = citekey
