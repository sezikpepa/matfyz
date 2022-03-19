first_largest = None
second_largest = None

while True:
    number = int(input())
    if number == -1:
        print(second_largest)
        exit(0)

    if not second_largest:
        if first_largest:
            if number >= first_largest:
                second_largest = first_largest
                first_largest = number
            else:
                second_largest = number
        else:
            first_largest = number
    else:
        if number >= first_largest:
            second_largest = first_largest
            first_largest = number

        elif number >= second_largest:
            second_largest = number
         






