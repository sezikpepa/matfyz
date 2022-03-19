import sys

def parse_numbers_from_console_line():
	space_count = 0
	buffer = 0
	numbers = []
	space_found_test = False
	end_cycle = False
	negative = False

	while not end_cycle:
		char = sys.stdin.read(1)
		asci_char = ord(char)
		if char == " ":
			if space_found_test:
				pass
			else:
				if negative:
					buffer = -abs(buffer)
				negative = False

				numbers.append(buffer)
				buffer = 0
				space_count += 1
				space_found_test = True

		elif asci_char == 10:
			if buffer:
				if negative:
					buffer = -abs(buffer)
				negative = False
				numbers.append(buffer)

			end_cycle = True
	
		elif 48 <= asci_char <= 57:
			space_found_test = False
			buffer *= 10
			buffer += asci_char - 48

		elif asci_char == 45:
			negative = True

		#else:
			#raise ValueError("ERROR: you can only enter integers")
			
	return numbers

count = parse_numbers_from_console_line()
numbers_1 = parse_numbers_from_console_line()
count2 = parse_numbers_from_console_line()
numbers_2 = parse_numbers_from_console_line()


while numbers_1 and numbers_2:
	if numbers_1[0] > numbers_2[0]:
		print(numbers_2[0], end=" ")
		numbers_2 = numbers_2[1:]
	else:
		print(numbers_1[0], end=" ")
		numbers_1 = numbers_1[1:]


if numbers_1:
	for element in numbers_1:
		print(element, end=" ")
if numbers_2:
	for element in numbers_2:
		print(element, end=" ")

