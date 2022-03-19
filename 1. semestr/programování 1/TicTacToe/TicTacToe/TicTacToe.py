import copy

def decide_looser(position):
    for x in range(3):
        if position[0][x] == position[1][x] == position[2][x]:
            if position[0][x] != ".":
                return position[0][x]

    for y in range(3):
        if position[y][0] == position[y][1] == position[y][2]:
            if position[y][0] != ".":
                return position[y][0]

    if position[0][0] == position[1][1] == position[2][2]:
        if position[0][0] != ".":
            return position[0][0]

    if position[0][2] == position[1][1] == position[2][0]:
        if position[1][1] != ".":
            return position[1][1]

    for x in range(3):
        for y in range(3):
            if position[y][x] == ".":
                return False

    return None

def engine(position, player_on_move):
    looser = decide_looser(position)
    if looser or looser is None:
        return looser

    scenarios = []
    second_player = "X" if player_on_move == "O" else "O"
    for x in range(3):
        for y in range(3):
            if position[y][x] == ".":
                new_position = copy.deepcopy(position)
                new_position[y][x] = player_on_move
                scenarios.append(engine(new_position, second_player))

    if scenarios:
        if second_player in scenarios:
            return second_player
        elif None in scenarios:
            return None
        else:
            return player_on_move

    else:
        return None


boards = []

number_of_boards = int(input())
for i in range(number_of_boards):
    x_count = 0
    o_count = 0
    board = []
    neco = input()
    for j in range(3):
        row = input()
        line = []
        for char in row:
            line.append(char)
            if char == "X":
                x_count += 1
            elif char == "O":
                o_count += 1

        board.append(line)

    boards.append([board, x_count, o_count])

results = []
for board in boards:
    if board[2] < board[1]:
        player_on_move = "O"
    else:
        player_on_move = "X"

    result = engine(board[0], player_on_move)
    if result == "O":
        results.append("X")
    elif result == "X":
        results.append("O")
    else:
        results.append("N")

print(*results, sep="")