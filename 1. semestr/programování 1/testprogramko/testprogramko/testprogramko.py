def lap_year(year):
    if year % 400 == 0:
        return True
    elif year % 100 == 0:
        return False
    elif year % 4 == 0:
        return True

    return False

def month_offset(month, year):
    offset = 0
    for i in range(1, month):
        if i == 2:
            if lap_year(year):
                offset += 29
            else:
                offset += 28
        else:
            offset += month_length.get(i)

    return offset

def year_offset(year):
    offset = 0
    for i in range(1, year):
        if lap_year(i):
            offset += 366
        else:
            offset += 365

    return offset


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

days_in_week = {1: "pondeli",
                2: "utery",
                3: "streda",
                4: "ctvrtek",
                5: "patek",
                6: "sobota",
                7: "nedele"}

day = int(input())
month = int(input())
year = int(input())


day_offset = day - 1

month_offset = month_offset(month, year)

year_offset = year_offset(year)

offset = day_offset + month_offset + year_offset

day_in_week_offset = offset % 7

print(days_in_week[day_in_week_offset + 1])