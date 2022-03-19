def decomposition(number, to_print=[], lowest_possible=1):
    if number == 0:
        print(*to_print, sep="+", end="")
        print()
        return
    for i in range(lowest_possible, number + 1):
        to_print_next = to_print.copy()
        to_print_next.append(i)
        decomposition(number - i, to_print_next, i)

number = int(input())
decomposition(number)
