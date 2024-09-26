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

    member this.TryGetTitle() = this.TryGetValue("title")

    member this.TryGetAuthor() = this.TryGetValue("author")

    member this.TryGetYear() = this.TryGetValue("year")

    member this.TryGetJournal() = this.TryGetValue("journal")

    member this.TryGetVolume() = this.TryGetValue("volume")

    member this.TryGetNumber() = this.TryGetValue("number")

    member this.TryGetPages() = this.TryGetValue("pages")

    member this.TryGetMonth() = this.TryGetValue("month")

    member this.TryGetNote() = this.TryGetValue("note")

    member this.TryGetKey() = this.TryGetValue("key")

    member this.TryGetPublisher() = this.TryGetValue("publisher")

    member this.TryGetSeries() = this.TryGetValue("series")

    member this.TryGetAddress() = this.TryGetValue("address")

    member this.SetTitle(name, value) = this.SetValue("title", value)

    member this.SetAuthor(name, value) = this.SetValue("author", value)

    member this.SetYear(name, value) = this.SetValue("year", value)

    member this.SetJournal(name, value) = this.SetValue("journal", value)

    member this.SetVolume(name, value) = this.SetValue("volume", value)

    member this.SetNumber(name, value) = this.SetValue("number", value)

    member this.SetPages(name, value) = this.SetValue("pages", value)

    member this.SetMonth(name, value) = this.SetValue("month", value)

    member this.SetNote(name, value) = this.SetValue("note", value)

    member this.SetKey(name, value) = this.SetValue("key", value)

    member this.SetPublisher(name, value) = this.SetValue("publisher", value)

    member this.SetSeries(name, value) = this.SetValue("series", value)

    member this.SetAddress(name, value) = this.SetValue("address", value)