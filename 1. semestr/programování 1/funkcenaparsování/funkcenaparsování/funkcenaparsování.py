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

		elif asci_char == 10:  #detekce mezery
			if buffer:
				if negative:
					buffer = -abs(buffer)
				negative = False
				numbers.append(buffer)

			end_cycle = True
	
		elif 48 <= asci_char <= 57:  #testování zda jde o číslo
			space_found_test = False
			buffer *= 10
			buffer += asci_char - 48

		elif asci_char == 45:  #zjišťování zda je číslo negativní
			negative = True

		else:
			raise ValueError("ERROR: you can only enter integers")
			
	return numbers

def parse_number_from_console_line():
	
	negative = False
	number = None

	while True:
		char = sys.stdin.read(1)
		if char == "-":
			negative = True

		elif char in "0123456789":
			if not number:
				number = 0
			number *= 10
			number += int(char)

		else:
			if number is not None:
				if negative:
					return -number
				return number

count1 = parse_number_from_console_line()
sequence1 = []
if count1 != 0:	
	for i in range(count1):
		sequence1.append(parse_number_from_console_line())
else:
	x = input()

count2 = parse_number_from_console_line()
sequence2 = []
if count2 != 0:
	for i in range(count2):
		sequence2.append(parse_number_from_console_line())
else:
	x = input()


while sequence1 and sequence2:
	if sequence1[0] > sequence2[0]:
		print(sequence2[0], end=" ")
		sequence2 = sequence2[1:]
	else:
		print(sequence1[0], end=" ")
		sequence1 = sequence1[1:]

if sequence1:
	print(*sequence1)
elif sequence2:
	print(*sequence2)
else:
	pass

	




