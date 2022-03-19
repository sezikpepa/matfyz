count1 = int(input())
sequence1 = []

sequence1_non_parse = input()
sequence1_non_parse = sequence1_non_parse.split(" ")
for element in sequence1_non_parse:
	try:
		element = int(element)
		sequence1.append(element)
	except:
		pass

count2 = int(input())
sequence2 = []

sequence2_non_parse = input()
sequence2_non_parse = sequence2_non_parse.split(" ")
for element in sequence2_non_parse:
	try:
		element = int(element)
		sequence2.append(element)
	except:
		pass



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
