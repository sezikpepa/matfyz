def sowing(number):
    numbers = [1, 1]
    if number == 0:
        return 1
    elif number == 1:
        return 1
    else:
        for i in range(number - 1):
            numbers.append(numbers[-1] + numbers[-2])

    return numbers[-1] + numbers[-2]

vstup = int(input())
print(sowing(vstup))

