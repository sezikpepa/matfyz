def evaluate(list):
    start_list = list
    while len(start_list) > 1:
        new_list = []
        i = 0
        while i < len(start_list):
            try:
                number1 = int(start_list[i + 1])
                number2 = int(start_list[i + 2])

                if start_list[i] == "+":
                    new_list.append(number1 + number2)
                    i += 2
                elif start_list[i] == "-":
                    new_list.append(number1 - number2)
                    i += 2
                elif start_list[i] == "*":
                    new_list.append(number1 * number2)
                    i += 2
                elif start_list[i] == "/":
                    if number2 == 0:
                        return ["CHYBA"]
                    else:
                        new_list.append(number1 // number2)
                        i += 2
            except:
                new_list.append(start_list[i])

            i += 1

        start_list = new_list

    return start_list



form = input()
form = form.split(" ")

final_form = []
for element in form:
    element = element.strip()
    if element:
        final_form.append(element)


print(*evaluate(final_form))