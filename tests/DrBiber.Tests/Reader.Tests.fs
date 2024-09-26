module Reader.Tests

open System
open Fable.Pyxpecto
open DrBiber
open Fable.Core

let tests_Field = testList "TryParseBibtexField" [
    testCase "valueInQuotes" <| fun _ ->
        let testLine = "title = \"Title of the book\","
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "Title of the book" "Should have correct value"

    testCase "valueInBrackets" <| fun _ ->
        let testLine = "title = {Let's eat grandpa}"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "Let's eat grandpa" "Should have correct value"

    testCase "valueInBracketsWithComma" <| fun _ ->
        let testLine = "title = {Let's eat, grandpa}"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "Let's eat, grandpa" "Should have correct value"

    testCase "valueNoBracketsComma" <| fun _ ->
        let testLine = "title = 123123,"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "123123" "Should have correct value"

    testCase "valueNoBracketsEndBracket" <| fun _ ->
        let testLine = "title = 123123}"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "123123" "Should have correct value"

    testCase "valueInBracketsAdditionalBrackets" <| fun _ ->
        let testLine = "title = {Some words{in curly brackets}}"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "Some words{in curly brackets}" "Should have correct value"

    testCase "valueInQuotesAdditionalBrackets" <| fun _ ->
        let testLine = "title = \"Some words{in curly brackets}\""
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "title" "Should have correct name"
        Expect.equal value "Some words{in curly brackets}" "Should have correct value"

    testCase "valueInBracketsSpecielChar" <| fun _ ->
        let testLine = "author = {Uflewski, Michał}"
        let field,i = DirtyParser.tryParseBibtexField 0 testLine
        let name,value = Expect.wantSome field "Should have parsed field"
        Expect.equal name "author" "Should have correct name"
        Expect.equal value "Uflewski, Michał" "Should have correct value"
        
]


let tests_Entry = testList "TryParseBibTexEntry" [
    testCase "Mixed" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/SingleEntry_Mixed.bib"
        let s = System.IO.File.ReadAllText p
        let i = s.IndexOf('@')
        let entry,i = DirtyParser.parseBibTexEntry (i + 1) s
        
        Expect.equal entry.EntryType "article" "Should have correct entry type"
        Expect.equal entry.CiteKey "qiao_legume_2024" "Should have correct citekey"

        let volume = Expect.wantSome (entry.TryGetPropertyValue "volume") "Should have volume"
        Expect.equal volume "15" "Should have correct volume"

        let pages = Expect.wantSome (entry.TryGetPropertyValue "pages") "Should have pages"
        Expect.equal pages "2924" "Should have correct pages"
] 

let tests_File = testList "TryParseBibTexFile" [
    testCase "SingleEntry" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/SingleEntry_Mixed.bib"
        let s = System.IO.File.ReadAllText p
        let entries = DirtyParser.parseBibTex s
        Expect.hasLength entries 1 "Should have one entry"
        let entry = entries.[0]
        Expect.equal entry.EntryType "article" "Should have correct entry type"
        Expect.equal entry.CiteKey "qiao_legume_2024" "Should have correct citekey"

        let volume = Expect.wantSome (entry.TryGetPropertyValue "volume") "Should have volume"
        Expect.equal volume "15" "Should have correct volume"

        let pages = Expect.wantSome (entry.TryGetPropertyValue "pages") "Should have pages"
        Expect.equal pages "2924" "Should have correct pages"

    testCase "MultipleEntries" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/MultipleEntries.bib"
        let s = System.IO.File.ReadAllText p
        let entries = DirtyParser.parseBibTex s
        Expect.hasLength entries 3 "Should have one entry"

        Expect.isTrue (entries.Length > 1) "Should have more than one entry"
        
        Expect.isTrue (entries.Length >= 1) "Should have one or more entries"

        let entry1 = entries.[0]
        let entry2 = entries.[1]
                
        Expect.equal entry1.EntryType "article" "Should have correct entry type"        
        Expect.equal entry1.CiteKey "shen_arabidopsis_2024" "Should have correct citekey"
        Expect.equal entry2.EntryType "article" "Should have correct entry type"        
        Expect.equal entry2.CiteKey "uflewski_thylakoid_2024" "Should have correct citekey"

]

let main = testList "Reader" [
    tests_Field
    tests_Entry
    tests_File
]