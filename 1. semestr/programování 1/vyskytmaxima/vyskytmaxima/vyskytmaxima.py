import sys

def parse_numbers_from_console_line(number_of_numbers):
	space_count = 0
	buffer = 0
	numbers = []
	while space_count < number_of_numbers:
		char = sys.stdin.read(1)
		if char == " ":
			numbers.append(buffer)
			buffer = 0
			space_count += 1
			
		elif char in "0123456789":
			buffer *= 10
			buffer += (ord(char) - 48)
		else:
			numbers.append(buffer)
			space_count += 1

	return numbers

number_of_numbers = int(input())
numbers = parse_numbers_from_console_line(number_of_numbers)


maximum = numbers[0]
positions_of_maximum = [0]
for i in range(1, len(numbers)):
	if numbers[i] > maximum:
		maximum = numbers[i]
		positions_of_maximum = [i]
	elif numbers[i] == maximum:
		positions_of_maximum.append(i)

print(maximum)
for element in positions_of_maximum:
	print(element + 1, end=" ")










