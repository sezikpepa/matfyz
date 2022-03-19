def generate_permutation(start, shift):
    pass


def factorial_base(number):
    if number == 0:
        raise ValueError("ERROR: must be higher than 0")
    factorial = 1
    another_element = 2
    while True:
        if factorial * another_element <= number:
            factorial *= another_element
        else:
            return factorial

        another_element += 1



print(factorial_base(6))
