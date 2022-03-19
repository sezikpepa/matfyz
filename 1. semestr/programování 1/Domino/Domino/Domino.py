import sys
import copy

longest = 0
stop = False
def longest_sequence(pieces, length, fit_number):
    global longest
    global max_possible
    global stop
    if not stop:
        if length == max_possible:
            stop = True
        if length > longest:
            longest = length

        for i in range(len(pieces)):
            if fit_number:
                new_pieces = copy.copy(pieces)
                new_piece = new_pieces.pop(i)

                if new_piece[1] == fit_number:
                    count = final_numbers.count(new_piece) - 1
                    longest_sequence(new_pieces, length + 1 + count, new_piece[0])

                if new_piece[0] == fit_number and new_piece[0] != new_piece[1]:
                    longest_sequence(new_pieces, length + 1, new_piece[1])
            else:
                new_pieces = copy.copy(pieces)
                new_piece = new_pieces.pop(i)

                count = final_numbers.count(new_piece) - 1
                longest_sequence(new_pieces, length + 1 + count, new_piece[1])

char = sys.stdin.read(2)

number_of_pieces = int(char)
counter = 0

numbers = []
buffer = ""
while True:
    char = sys.stdin.read(1)
    if char in "01234568789":
        buffer += char
    else:
        if buffer:
            counter += 1
            numbers.append(int(buffer))
            buffer = ""
    if counter == 2 * number_of_pieces:
        break


final_numbers = []
for i in range(0, len(numbers), 2):
    final_numbers.append([numbers[i], numbers[i + 1]])

max_possible = len(final_numbers)
without_duplicities = []

for element in final_numbers:
    if element[0] == element[1] and element not in without_duplicities:
        without_duplicities.append(element)
    elif element[0] != element[1]:
        without_duplicities.append(element)

longest_sequence(without_duplicities, 0, None)
print(longest)