import sys

words = []
word = ""
while True:
    char = sys.stdin.read(1)
    if word == "konec":
        break
    elif char in "qwertzuioplkjhgfdsayxcvbnmQWERTZUIOPLKJHGFDSAYXCVBNM":
        word += char
    elif char == " ":
        words.append(word)
        word = ""

current_line = ""
for word in words:
    if len(word) >= 30:
        print(current_line)
        print(word)
        current_line = ""
    else:
        if word != "":  
            if len(current_line) + len(word) <= 30:
                current_line += word + " "
            else:
                print(current_line)
                current_line = word + " "



print(current_line)