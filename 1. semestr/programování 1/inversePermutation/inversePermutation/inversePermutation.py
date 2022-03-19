import sys

def parse_numbers_from_console_line(number_of_numbers):
	space_count = 0
	buffer = 0
	while True:
		char = sys.stdin.read(1)
				
		if char in "0123456789":
			buffer *= 10
			buffer += (ord(char) - 48)

		else:
			return number

	return numbers


print(parse_numbers_from_console_line(parse_numbers_from_console_line(number_of_numbers)))
