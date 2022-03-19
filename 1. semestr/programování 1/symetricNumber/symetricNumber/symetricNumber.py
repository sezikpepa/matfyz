def symetric_test(number: str) -> bool:
    if number[-1] == 0:
        return False
    if number != number[::-1]:
        return False
    return True

number = input()

while number != "0":
    if symetric_test(number):
        print(number, end=" ")
    number = input()