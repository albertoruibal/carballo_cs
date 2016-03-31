#!/bin/bash
#
# Copy Java files to be converted to a tmp directory
# It assumes that the carballo source code is at ../carballo
#
mkdir tmp
cp -a ../carballo/core/src/main/java/com tmp/
cp -a ../carballo/jse/src/main/java/com tmp/
#cp -a ../carballo/jse/src/test/java/com tmp/

#
# Delete unwanted files in the conversion
#
# Avoid more than one main() in the code
rm ./tmp/com/alonsoruibal/chess/evaluation/KPKBitbaseGenerator.java
rm ./tmp/com/alonsoruibal/chess/evaluation/PcsqGenerator.java
rm ./tmp/com/alonsoruibal/chess/bitboard/MagicNumbersGenerator.java
rm ./tmp/com/alonsoruibal/chess/Pgn.java
rm ./tmp/com/alonsoruibal/chess/PgnFile.java
# These classes are converted manually
rm ./tmp/com/alonsoruibal/chess/book/FileBook.java
rm ./tmp/com/alonsoruibal/chess/uci/Uci.java

#
# Call Sharpen, be careful, it must be run with Java 7
#
$JAVA_HOME/bin/java -jar sharpencore-0.0.1-SNAPSHOT-jar-with-dependencies.jar ./tmp/ @sharpen-all-options

#
# Move generated code to the root directory and clear tmp
#
cp -a tmp/tmp.net/com .

rm -rf tmp
