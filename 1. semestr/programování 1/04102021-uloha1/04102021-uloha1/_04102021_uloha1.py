list_of_coins = [1,1,1,1,2,1,1,1,1]

while True:
    coins_for_next_round = []
    buffer = []
    if len(list_of_coins) % 2:
        buffer = list_of_coins[-1]
        list_of_coins = list_of_coins[:-1]

    first_half = list_of_coins[:len(list_of_coins) // 2]
    second_half = list_of_coins[len(list_of_coins) // 2:]



    print(first_half)
    print(second_half)   
    print(buffer)
    break