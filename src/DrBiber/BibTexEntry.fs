namespace DrBiber

open DynamicObj
open System.Collections.Generic

type BibTexEntry(entryType : string, citekey : string, ?attributes : Dictionary<string,string>) as this =
    
    inherit DynamicObj()

    do
        match attributes with
        | None -> ()
        | Some attributes ->
            for kv in attributes do
                this.Properties.Add(kv.Key, kv.Value)

    member this.EntryType = entryType

    member this.CiteKey = citekey

    member this.TryGetTitle() = this.TryGetPropertyValue("title")

    member this.TryGetAuthor() = this.TryGetPropertyValue("author")

    member this.TryGetYear() = this.TryGetPropertyValue("year")

    member this.TryGetJournal() = this.TryGetPropertyValue("journal")

    member this.TryGetVolume() = this.TryGetPropertyValue("volume")

    member this.TryGetNumber() = this.TryGetPropertyValue("number")

    member this.TryGetPages() = this.TryGetPropertyValue("pages")

    member this.TryGetMonth() = this.TryGetPropertyValue("month")

    member this.TryGetNote() = this.TryGetPropertyValue("note")

    member this.TryGetKey() = this.TryGetPropertyValue("key")

    member this.TryGetPublisher() = this.TryGetPropertyValue("publisher")

    member this.TryGetSeries() = this.TryGetPropertyValue("series")

    member this.TryGetAddress() = this.TryGetPropertyValue("address")

    member this.SetTitle(name, value) = this.SetProperty("title", value)

    member this.SetAuthor(name, value) = this.SetProperty("author", value)

    member this.SetYear(name, value) = this.SetProperty("year", value)

    member this.SetJournal(name, value) = this.SetProperty("journal", value)

    member this.SetVolume(name, value) = this.SetProperty("volume", value)

    member this.SetNumber(name, value) = this.SetProperty("number", value)

    member this.SetPages(name, value) = this.SetProperty("pages", value)

    member this.SetMonth(name, value) = this.SetProperty("month", value)

    member this.SetNote(name, value) = this.SetProperty("note", value)

    member this.SetKey(name, value) = this.SetProperty("key", value)

    member this.SetPublisher(name, value) = this.SetProperty("publisher", value)

    member this.SetSeries(name, value) = this.SetProperty("series", value)

    member this.SetAddress(name, value) = this.SetProperty("address", value)