import sys

def parse_numbers_from_console_line():
	space_count = 0
	buffer = 0
	numbers = []
	space_found_test = False
	end_cycle = False

	while not end_cycle:
		char = sys.stdin.read(1)
		asci_char = ord(char)
		if char == " ":
			if space_found_test:
				pass
			else:
				numbers.append(buffer)
				buffer = 0
				space_count += 1
				space_found_test = True

		elif asci_char == 10:
			numbers.append(buffer)
			end_cycle = True
	
		elif 48 <= asci_char <= 57:
			space_found_test = False
			buffer *= 10
			buffer += asci_char - 48

		else:
			raise ValueError("ERROR: můžete zadávat pouze celá čísla")
			
	return numbers

def my_minimum(numbers, minimum):
	result = None
	srest = []
	for element in numbers:
		if result:
			if element < result and element > minimum:
				srest.append(result)
				result = element
			else:
				srest.append(element)
		else:
			if element > minimum:
				result = element
			else:
				srest.append(element)

	return [result, srest]

count_of_numbers = parse_numbers_from_console_line()
numbers = parse_numbers_from_console_line()

numbers_for_change = []

for i in range(1, len(numbers)): #nezahrnuje první
	if numbers[-i - 1] < numbers[-i]:
		splitter = -i - 1
		break
	else:
		pass

try:
	solid_numbers = numbers[:splitter]
except:
	print("NEEXISTUJE")
	exit(0)

volatile_numbers = numbers[splitter:]


middle_section = my_minimum(volatile_numbers[1:], volatile_numbers[0])
middle_section_one = middle_section[0]
middle_section_two = middle_section[1]
middle_section_two.append(volatile_numbers[0])
middle_section_two.sort()


for element in solid_numbers:
	print(element, end=" ")

print(middle_section_one, end=" ")

for element in middle_section_two:
	print(element, end=" ")


