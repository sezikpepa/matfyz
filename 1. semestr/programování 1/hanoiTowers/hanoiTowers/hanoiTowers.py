def hanoi_towers(number_of_circle):
    if number_of_circle == 1:
        pass
    elif number_of_circle % 2 == 1:
        return hanoi_towers(number_of_circle - 1)
    else:
        return hanoi_towers(number_of_circle - 1)


number_of_circle = int(input())
