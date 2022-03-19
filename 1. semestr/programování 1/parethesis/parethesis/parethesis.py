def parenthesis(count, left, right, to_print=""):
    if right > left:
        return
    if count == 0:
        if right == left:
            print(to_print)
    else:
        to_print_1 = to_print + "("
        parenthesis(count - 1, left + 1, right, to_print_1)

        to_print_2 = to_print + ")"
        parenthesis(count - 1, left, right + 1, to_print_2)



vstup = int(input())
parenthesis(vstup * 2, 0, 0)
