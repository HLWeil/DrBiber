module Writer.Tests

open System
open Fable.Pyxpecto
open DrBiber
open Fable.Core

let tests_Entry = testList "bibTeXEntryToString" [
    testCase "BracketsOnly" <| fun _ ->
        let p = __SOURCE_DIRECTORY__ + "/TestFiles/SingleEntry_BracketsOnly.bib"
        let s = System.IO.File.ReadAllText p
        let i = s.IndexOf('@')
        let entry,i = DirtyParser.bibTeXEntryFromString (i + 1) s
        
        let actual = (DirtyParser.bibTeXEntryToString entry).ReplaceLineEndings()
        let expected = s.ReplaceLineEndings()
        Expect.equal actual expected "Should have correct output"
] 

//let tests_File = testList "TryParseBibTexFile" [
//    testCase "SingleEntry" <| fun _ ->
//]

let main = testList "Writer" [
    tests_Entry
    //tests_File
]