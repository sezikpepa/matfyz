def lap_year(year):
    if year % 400 == 0:
        return True
    if year % 100 == 0:
        return False
    if year % 4 == 0:
        return True
    return False

def add_day(date_1, date_2):
    counter = 0
    if date_1 == date_2:
        return counter
    while True:
        if date_1[2] <= date_2[2]:
            if date_1[1] == date_2[1]:
                if date_1[0] < date_2[0] or True:
                    if date_1[1] != 2:
                        if date_1[0] < month_length[date_1[1]]:
                            date_1[0] += 1
                        else:
                            date_1[0] = 1
                            date_1[1] += 1
                    else:
                        if lap_year(date_1[2]):
                            if date_1[0] < 29:
                                date_1[0] += 1
                            else:
                                date_1[0] = 1
                                date_1[1] += 1
                        else:
                            if date_1[0] < 28:
                                date_1[0] += 1
                            else:
                                date_1[0] = 1
                                date_1[1] += 1
                else:
                    break

            elif date_1[1] < date_2[1]:
                if date_1[1] == 2 and date_1[0] == 28 and lap_year(date_1[2]):
                    date_1[0] = 29
                    date_1[1] = 2
                elif date_1[1] == 2 and date_1[0] == 29:
                    date_1[0] = 1
                    date_1[1] = 3
                elif date_1[0] < month_length[date_1[1]]:
                    date_1[0] += 1
                else:
                    date_1[0] = 1
                    date_1[1] += 1

            elif date_1[1] > date_2[1]:
                if date_1[1] == 2 and date_1[0] == 28 and lap_year(date_1[2]):
                    date_1[0] = 29
                    date_1[1] = 2
                elif date_1[1] == 2 and date_1[0] == 29:
                    date_1[0] = 1
                    date_1[1] = 3
                elif date_1[0] < month_length[date_1[1]]:
                    date_1[0] += 1
                else:
                    date_1[0] = 1
                    date_1[1] += 1
                    if date_1[1] == 13:
                        date_1[1] = 1
                        date_1[2] += 1
        else:
            break

        counter += 1
        if date_1[1] == 12 and date_1[0] == 31:
            date_1[0] = 1
            date_1[1] = 1
            date_1[2] += 1

        if date_1 == date_2:
            return counter

    return counter

def add_year(date_1, date_2):
    counter = 0
    while True:
        if date_1[2] < date_2[2]:
            if date_1[1] < date_2[1]:
                if date_1[1] == 2 and date_1[0] != 29:
                    if lap_year(date_1[2]):
                        counter += 366
                    else:
                        counter += 365

                else:
                    counter += 365

                date_1[2] += 1

            elif date_1[1] == date_2[1]:
                if date_1[0] <= date_2[0]:
                    if date_1[1] == 2 and date_1[0] != 29:
                        if lap_year(date_1[2]):
                            counter += 366
                        else:
                            counter += 365
                    else:
                        if lap_year(date_1[2]):
                            counter += 366
                        else:
                            counter += 365

                else:
                    return [counter, date_1, date_2]
            
                date_1[2] += 1

            else:
                return [counter, date_1, date_2]

        else:
            return [counter, date_1, date_2]



month_length = {1: 31,
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
                12: 31}


vstup = input()
rozdeleni = vstup.split(" ")
rozdeleni_final = []
for element in rozdeleni:
    rozdeleni_final.append(int(element))

date_1 = [rozdeleni_final[0], rozdeleni_final[1], rozdeleni_final[2]]
date_2 = [rozdeleni_final[3], rozdeleni_final[4], rozdeleni_final[5]]


swap = False
if date_1[2] > date_2[2]:
    date_1, date_2 = date_2, date_1
    swap = True

elif date_1[2] == date_2[2]:
    if date_1[1] > date_2[1]:
        date_1, date_2 = date_2, date_1
        swap = True

    elif date_1[1] == date_2[1]:
        if date_1[0] > date_2[0]:
            date_1, date_2 = date_2, date_1
            swap = True
        elif date_1[0] == date_2[0]:
            print(0)
            exit(0)

number_of_days = 0
roky = date_2[2] - date_1[2]

vystup = add_year(date_1, date_2)

number_of_days += vystup[0]

number_of_days += add_day(vystup[1], vystup[2])

number_of_days += int(roky / 100)
if not swap:
    print(number_of_days)
else:
    print(f"-{number_of_days}")