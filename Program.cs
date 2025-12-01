// based on Clojure/Clojure.Main/Main.cs original copyright Rich Hickey (EPL 1.0)
using clojure.lang;
using System.Reflection;

// see config in csproj
string assemblyPath = Path.Combine(AppContext.BaseDirectory, "ClojureDepsBundle.dll");
Assembly.LoadFrom(assemblyPath);

// need to make dependencies available so that (ns foo (:import Bar.Baz)) works cleanly
Assembly.Load("Parquet");

#if DEBUG
Environment.SetEnvironmentVariable("clojure.server.prepl","{:port,5555,:accept,clojure.core.server/io-prepl,:server-daemon,false}");
#endif

RT.Init();

var CLOJURE_MAIN = Symbol.intern("clojure.main");
var REQUIRE = RT.var("clojure.core", "require");
var MAIN = RT.var("clojure.main", "main");

REQUIRE.invoke(CLOJURE_MAIN);
MAIN.applyTo(RT.seq(args));
