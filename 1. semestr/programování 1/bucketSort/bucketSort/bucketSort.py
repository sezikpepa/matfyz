import sys

run = True

numbers_in_buckets = [[],[],[],[],[],[],[],[],[],[]]

numbers = []

for line in sys.stdin.readlines():
    numbers.append(int(line.strip()))


for i in range(7):
    divisor = 10 ** i
    for number in numbers:
        help_number = number // divisor
        help_number = help_number % 10

        numbers_in_buckets[help_number].append(number)

    numbers = []

    for element in numbers_in_buckets:
        for part in element:
            numbers.append(part)


            
    numbers_in_buckets = [[],[],[],[],[],[],[],[],[],[]]
    print(*numbers)





        

        
