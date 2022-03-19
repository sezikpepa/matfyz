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

