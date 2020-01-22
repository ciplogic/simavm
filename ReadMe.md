This is an experimental micro-JVM
===

What means Micro JVM?

It means that it should be a JVM that has some features off:

* no reflection
* no exceptons


Why?
====

Because it would offer a transpiler and optimizer that could make it run on low end machines (by generating C++ but without exceptions enabled)


As it is experimental it means obviously that it would be broken if not used properly.

How to read from files (or other APIs that do throw exceptions)?
===

There would be some Java API offered that would offer exception free code that would also be provided by SimaJVM
as fixed implementation, so no exception would be necessary.

