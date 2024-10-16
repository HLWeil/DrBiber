module Writer.Tests

open System
open Fable.Pyxpecto
open DrBiber
open Fable.Core

let tests_Entry = testList "TryParseBibTexEntry" [
    testCase "Mixed" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/SingleEntry_BracketsOnly.bib"
        let s = System.IO.File.ReadAllText p
        let i = s.IndexOf('@')
        let entry,i = DirtyParser.parseBibTexEntry (i + 1) s
        
        let outputString = DirtyParser.bibTeXEntryToString entry
        Expect.equal outputString s "Should have correct output"
] 

//let tests_File = testList "TryParseBibTexFile" [
//    testCase "SingleEntry" <| fun _ ->
//]

let main = testList "Reader" [
    tests_Entry
    //tests_File
]