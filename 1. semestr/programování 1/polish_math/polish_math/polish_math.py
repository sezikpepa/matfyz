def evaluate_form(to_evaluate):
    try:
        to_return = int(to_evaluate[0])
        return to_return
    except ValueError:
        if to_evaluate[0] == "+":
            try:
                return evaluate_form(to_evaluate[1:-1]) + int(to_evaluate[-1])
            except ValueError:
                return "CHYBA"
            except TypeError:
                return "CHYBA"
        if to_evaluate[0] == "-":
            try:
                return evaluate_form(to_evaluate[1:-1]) - int(to_evaluate[-1])
            except ValueError:
                return "CHYBA"
            except TypeError:
                return "CHYBA"
        if to_evaluate[0] == "*":
            try:
                return evaluate_form(to_evaluate[1:-1]) * int(to_evaluate[-1])
            except ValueError:
                return "CHYBA"
            except TypeError:
                return "CHYBA"
        if to_evaluate[0] == "/":
            try:
                return evaluate_form(to_evaluate[1:-1]) // int(to_evaluate[-1])
            except ZeroDivisionError:
                return "CHYBA"
            except TypeError:
                return "CHYBA"
            except ValueError:
                return "CHYBA"



to_evaluate = input()
to_evaluate = to_evaluate.split(" ")
to_evaluate_final = []
for elementa in to_evaluate:
    element = elementa.strip()

    to_evaluate_final.append(element)

print(evaluate_form(to_evaluate_final))

