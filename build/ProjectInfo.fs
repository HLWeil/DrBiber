module ProjectInfo

open Fake.Core
open Helpers

let project = "DrBiber"

let summary = "F# library for parsing BibTex files."

let testProjects = 
    [
        "tests/DrBiber.Tests"
    ]

let solutionFile  = $"{project}.sln"

let configuration = "Release"

let gitOwner = "HLWeil"

let gitHome = $"https://github.com/{gitOwner}"

let projectRepo = $"https://github.com/{gitOwner}/{project}"

let netPkgDir = "./dist/net"
let npmPkgDir = "./dist/js"
let pyPkgDir = "./dist/py"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

let stableVersion = SemVer.parse release.NugetVersion

let stableVersionTag = (sprintf "%i.%i.%i" stableVersion.Major stableVersion.Minor stableVersion.Patch )

let assemblyVersion = $"{stableVersion.Major}.0.0"

let assemblyInformationalVersion = $"{stableVersion.Major}.{stableVersion.Minor}.{stableVersion.Patch}"

let mutable prereleaseSuffix = PreReleaseFlag.Alpha

let mutable prereleaseSuffixNumber = 0

let mutable isPrerelease = false