Carballo Chess Engine in C#
===========================

This project is a conversion of the Carballo Chess Engine code from Java to C# using Sharpen. The original project is at:

http://github.com/albertoruibal/carballo

It implements the UCI interface to be used in chess GUIs like Arena.

The performance of the converted code is about a 30% slower than the original Java code.

Code conversion
===============

The conversion was done with this version of Sharpen:

https://github.com/slluis/sharpen

The Sharpen jar and the conversion script (convert.sh) are included in the root of the project.

Classes not converted
=====================

These classes are excluded from the automatic conversion:

* The custom opening book in book/ResourceBook.cs
* The class uci/Uci.cs implmenting the UCI interface (and making use of ResourceBook.cs)

