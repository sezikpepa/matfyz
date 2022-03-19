def money_disassemble(value, coins, to_print=[]):
    if value == 0:
        print(to_print)
    else:
        for i in range(len(coins)):
            if value - coins[i] >= 0:
                money_disassemble(value - coins[i], coins[i:], to_print + [coins[i]])


money_disassemble(9, [5, 2, 1])