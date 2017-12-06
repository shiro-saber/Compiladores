mcs -out:a.exe Driver.cs Scanner.cs Token.cs TokenCategory.cs Parser.cs SyntaxError.cs
shopt -s nullglob
for i in *.int64; do
    echo -e "\033[1;32m corriendo para"
    echo $i
    echo -e "\033[93m"
    mono a.exe $i
    echo -e "\033[0m ----------------------------------------------"
done
echo -e "\033[0m termine"
