def symetric_test(number):
	if number[-1] == "0":
		return False

	if number == number[::-1]:
		return True

number = input()	
while number != "0":
	if symetric_test():
		print(number, sep=" ", end="")
	number = input()
