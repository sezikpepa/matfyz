from math import factorial

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

first_line = input()
second_line = input()

split = first_line.split(" ")
number_of_elements_in_permutation = int(split[0])
which_permutation = int(split[1])

split = second_line.split(" ")
permutation = []
for element in split:
	permutation.append(int(element))


max_factorial_base = lower_factorial(which_permutation)
coefficients_of_factorial = [0] * max_factorial_base
which_permutation_for_calculation = which_permutation

index = 0
divisor = factorial(max_factorial_base)

while which_permutation_for_calculation > 0:
	if which_permutation_for_calculation - divisor >= 0:
		which_permutation_for_calculation -= divisor
		coefficients_of_factorial[index] += 1
	else:
		divisor = factorial(lower_factorial(divisor) - 1)
		index += 1

while len(coefficients_of_factorial) != len(permutation) - 1:
	coefficients_of_factorial = [0] + coefficients_of_factorial

#----------------------------------------------------------------------------------
print(coefficients_of_factorial)
result = []

for element in coefficients_of_factorial:
	for_result = permutation.pop(element)
	result.append(for_result)

print(*result, end=" ")
print(*permutation)