numbers = []
while True:
    try:
        number = str(int(input()))
        numbers.append(number)
    except ValueError:
        break
    except EOFError:
        break


for i in range(2):
    for number in numbers:
        print(number)



