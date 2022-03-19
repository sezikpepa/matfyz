def fibonaci(number):
	if number == 0 or number == 1:
		return 1
	return fibonaci(number - 1) + fibonaci(number - 2)
	

print(fibonaci(40))