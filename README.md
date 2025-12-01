# prebuild-example

I've been trying to find the best way to mix C# and Clojure code in the same
.NET project and made this example to share an experiment in bundling deps.

To see how it works, clone and the repo and open PrebuildExample.csproj in the
Rider editor from Jetbrains. There is an MSBuild pre build task which invokes
cljr to run prebuild/bundle.cljr which creates a DLL of Clojure source files.

This will fail if you don't already have cljr installed. Fix by running:
```
dotnet tool install -g Clojure.Main --version 1.12.3-alpha4
dotnet tool install -g Clojure.Cljr --version 0.1.0-alpha11
```

The main program is a Program.cs stub (not cljr!) which loads the bundle DLL,
loads assemblies (procured from NuGet presumably) and then drops you into a
REPL from which you can `(require 'clojure.data.csv)` just like normal or you
can (import ...) types from external libraries.

One advantage of using a stub program instead of cljr is that it feels more
like a normal C# project. Your colleagues can just clone the repo and click
run. The build process should "just work" because cljr fetches all the gitlibs
then the mainstream dotnet / msbuild toolchain fetches the NuGet packages.

The other advantage to using a stub program is that you can use C# `using ...`
or `Assembly.Load` to get everything sorted _before_ Clojure starts. This means
you avoid getting stuck in the world of (assembly-load ...) where you may or
encounter quirks of AssemblyLoadContext and friends.

One disadvantage of bundling source files in a DLL is that it will fail on any
name collisions across libraries. In practice this is rare because namespaces
typically include a unique prefix (reverse domain name or trademark name) and
the file-directory tree is structured accordingly.

I suppose the other disadvantage is that loading e.g. "clojure.data.csv.clj"
from a resource-only DLL is arguably an implementation detail that David Miller
may not wish to carry forward in his compiler rewrite. Not an official API.

## Questions

 - Does this "just work" in Visual Studio too? I have not yet tried.
 - How does this fit in with `<PublishSingleFile>` and/or AOT?
 - What if I have both a git lib and a NuGet for the same namespaces?
 - Could my git libs have their own dependencies from NuGet?
 - What about `dotnet run app.cs` and virtual csproj?
 - Is this only for .NET9 / .NET10 with PersistedAssemblyBuilder? I guess so.
 - To what extent does running with a debugger attached prevent compiling
   clojure.core.async from source? At least on macOS (arm64) it does.
