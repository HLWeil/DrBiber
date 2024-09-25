module Main.Tests

open Fable.Pyxpecto

let all = testSequenced <| testList "BibTex" [
    Reader.Tests.main
]

[<EntryPoint>]
let main argv = Pyxpecto.runTests [||] all