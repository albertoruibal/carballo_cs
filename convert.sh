#!/bin/bash

#
# Copy Java files to be converted to a tmp directory
#
mkdir tmp
cp -a ../carballo/core/src/main/java/com tmp/
cp -a ../carballo/jse/src/main/java/com tmp/

#
# Delete unwanted files in the conversion
#
# Avoid more than one main() in the code
rm ./tmp/com/alonsoruibal/chess/evaluation/KPKBitbaseGenerator.java
rm ./tmp/com/alonsoruibal/chess/bitboard/MagicNumbersGenerator.java
# Do not use File Book
rm ./tmp/com/alonsoruibal/chess/book/FileBook.java
# This class is converted manually
rm ./tmp/com/alonsoruibal/chess/uci/Uci.java

#
# Call Sharpen
#
java -jar sharpencore-0.0.1-SNAPSHOT-jar-with-dependencies.jar ./tmp/ @sharpen-all-options

#
# Move generated code to the root directory and clear tmp
#
cp -a tmp/tmp.net/com .

rm -rf tmp
