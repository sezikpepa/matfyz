def lower_factorial(number):
	if number <= 0:
		return False

	start = 1
	muliplier = 2
	while True:
		if start * muliplier <= number:
			start *= muliplier
		else:
			return muliplier - 1
		muliplier += 1

def next_permutation(permutation):
	numbers_for_change = [permutation[-1]]
	previous_number = permutation[-1]
	permutation.pop(-1)
	minimum_limit = None

	for i in range(1, len(permutation)):
		next_number = permutation.pop(-1)
		if next_number > previous_number:
			numbers_for_change.append(next_number)
		else:
			numbers_for_change.append(next_number)
			minimum_limit = next_number
			break
		previous_number = next_number

	if not minimum_limit:
		minimum_limit = next_number

	result = permutation
	number_for_next_permutation = my_minimum(numbers_for_change, minimum_limit)
	result.append(number_for_next_permutation)
	numbers_for_change.sort()
	for element in numbers_for_change:
		if element != number_for_next_permutation:
			result.append(element)

	return result

def my_minimum(numbers, minimum_limit):
	numbers.sort()
	for i in range(len(numbers)):
		if numbers[i] > minimum_limit:
			return numbers[i]
	return numbers[-1]
	

first_line = input()
second_line = input()

split = first_line.split(" ")
number_of_elements_in_permutation = int(split[0])
which_permutation = int(split[1])

split = second_line.split(" ")
permutation = []
for element in split:
	permutation.append(int(element))


lower_factorial = lower_factorial(which_permutation)
for _ in range(lower_factorial)
for _ in range(which_permutation):
	permutation = next_permutation(permutation)
print(*permutation)




