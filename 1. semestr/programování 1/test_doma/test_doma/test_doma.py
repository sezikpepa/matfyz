def lap_year(year):
	if year % 400 == 0:
		return True
	elif year % 100 == 0:
		return False
	elif year % 4 == 0:
		return True

	return False

def days_from_start(date):
	number_of_days = 0
	start_date = [1, 1, 1]
	#dny
	number_of_days += date[0] - start_date[0]

	#mesice
	for i in range(1, date[1]):
		if i == 2:
			if lap_year(date[2]):
				number_of_days += 29
			else:
				number_of_days += 28
		else:
			number_of_days += months_length[i]

	#roky
	for i in range(1, date[2]):
		if lap_year(i):
			number_of_days += 366
		else:
			number_of_days += 365

	return number_of_days



months_length = {1: 31,
				2: 28,
				3: 31,
				4: 30,
				5: 31,
				6: 30,
				7: 31,
				8: 31,
				9: 30,
				10: 31,
				11: 30,
				1: 31}




datum1 = [1, 1, 1900]
datum2 = [1, 1, 1904]


print(days_from_start(datum2) - days_from_start(datum1))
