def move_hanoi_towers(number, start=1, end=2, for_free=3):
    if number == 1:
        print(f"Kotouc {number} z {start} na {end}") #přesunutí hlavního disku
    else:
        move_hanoi_towers(number - 1, start, for_free, end)  #odsunutí věže nad hlavním diskem
        print(f"Kotouc {number} z {start} na {end}")   #posunutí hlavního disku
        move_hanoi_towers(number - 1, for_free, end, start)   #vrácení odsunuté věže na již posunutý disk

number_of_discs = int(input())

move_hanoi_towers(number_of_discs)