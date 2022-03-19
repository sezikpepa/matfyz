first_number = int(input())
second_number = int(input())

if first_number > second_number:
    largest = first_number
    second_largest = second_number

else:
    largest = second_number
    second_largest = first_number

number = int(input())


while number != -1:
    if number >= largest:
        second_largest = largest
        largest = number

    elif number >= second_largest:
        second_largest = number

    number = int(input())

print(second_largest)
