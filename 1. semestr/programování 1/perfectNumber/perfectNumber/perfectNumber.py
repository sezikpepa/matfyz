number = int(input())
result = ""

total_of_divisors = 0

for i in range(1, number):
    if number % i == 0:
        total_of_divisors += i

if number == total_of_divisors:
    result += "P"

#--------------------------------------
test_number = 1
power = 0
while power < number:
    power = test_number ** 2
    if power == number:
        result += "C"
    test_number += 1

test_number = 1
power = 0
while power < number:
    power = test_number ** 3
    if power == number:
        result += "K"
    test_number += 1

print(result)


