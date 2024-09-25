module Reader.Tests

open System
open Fable.Pyxpecto
open DrBiber
open Fable.Core

let tests_Field = testList "TryParseBibtexField" [
    testCase "valueInParantheses" <| fun _ ->
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
]


let tests_Entry = testList "TryParseBibTexEntry" [
    testCase "Mixed" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/SingleEntry_Mixed.bib"
        let s = System.IO.File.ReadAllText p
        let i = s.IndexOf('@')
        let entry = DirtyParser.parseBibTexEntry (i + 1) s
        
        Expect.equal entry.EntryType "article" "Should have correct entry type"
        Expect.equal entry.CiteKey (Some "qiao_legume_2024") "Should have correct citekey"

        let volume = Expect.wantSome (entry.TryGetValue "volume") "Should have volume"
        Expect.equal volume "15" "Should have correct volume"

        let pages = Expect.wantSome (entry.TryGetValue "pages") "Should have pages"
        Expect.equal pages "2924" "Should have correct pages"
] 

let tests_File = testList "TryParseBibTexFile" [
    ptestCase "mutable" <| fun _ ->
        Expect.isFalse true "Not implemented"
]

let main = testList "Reader" [
    tests_Field
    tests_Entry
    tests_File
]