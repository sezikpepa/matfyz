number_of_vertecies = int(input())
number_of_connections = int(input())


components = [[]]

for i in range(number_of_connections):
    numbers = input()
    numbers = numbers.split(" ")
    numbers = [int(number) for number in numbers]
    
    place_found = False
    for i in range(len(components)):
        if numbers[0] in components[i]:
            components[i].append(numbers[1])
            place_found = True
            break
        elif numbers[1] in components[i]:
            components[i].append(numbers[0])
            place_found = True
            break

    if not place_found:
        components.append([numbers[0], numbers[1]])
      

numbers = []
for component in components[1:]:
    for number in component:
        numbers.append(number)
    print(*component)

for i in range(1, number_of_vertecies + 1):
    if i not in numbers:
        print(i)