def find_path(positions):
    global chess_board
    global found
    new_positions = []
    for position in positions:
        for x in range(1, 8):
            if position[0] + x >= 8:
                continue
            if chess_board[position[0] + x][position[1]] == "c":
                found = True
            elif chess_board[position[0] + x][position[1]] == "x":
                break
            elif chess_board[position[0] + x][position[1]] == ".":
                chess_board[position[0] + x][position[1]] = "x"
                new_positions.append([position[0] + x, position[1]])

        for x in range(-1, -8, -1):
            if position[0] + x < 0:
                continue
            if chess_board[position[0] + x][position[1]] == "c":
                found = True
            elif chess_board[position[0] + x][position[1]] == "x":
                break
            elif chess_board[position[0] + x][position[1]] == ".":
                chess_board[position[0] + x][position[1]] = "x"
                new_positions.append([position[0] + x, position[1]])

        for y in range(1, 8):
            if position[1] + y >= 8:
                continue            
            if chess_board[position[0]][position[1] + y] == "c":
                found = True
            elif chess_board[position[0]][position[1] + y] == "x":
                break
            elif chess_board[position[0]][position[1] + y] == ".":
                chess_board[position[0]][position[1] + y] = "x"
                new_positions.append([position[0], position[1] + y])

        for y in range(-1, -8, -1):
            if position[1] + y < 0:
                continue
            if chess_board[position[0]][position[1] + y] == "c":
                found = True
            elif chess_board[position[0]][position[1] + y] == "x":
                break
            elif chess_board[position[0]][position[1] + y] == ".":
                chess_board[position[0]][position[1] + y] = "x"
                new_positions.append([position[0], position[1] + y])

    return new_positions


chess_board = []

for i in range(8):
    vstup = input()
    line = []
    for j in range(8):
        if vstup[j] == "v":
            start_position = [i, j]
        line.append(vstup[j])

    chess_board.append(line)
"""
found = False
chess_board =   [["v", "x", "x", "c", "x", ".", ".", "."],
                ["x", "x", "x", ".", ".", ".", ".", "."],
                [".", "x", "x", "x", "x", ".", ".", "."],
                [".", "x", "x", ".", ".", ".", ".", "."],
                [".", "x", "x", ".", ".", ".", ".", "."],
                [".", "x", "x", ".", ".", ".", ".", "."],
                [".", ".", ".", ".", ".", ".", ".", "."],
                [".", ".", ".", ".", ".", ".", ".", "."]]
start_position = [0, 0]
"""
found = False
length = 1
new_positions = find_path([start_position])
while not found:
    length += 1
    new_positions = find_path(new_positions)
    if not new_positions:
        found = True
        length = -1

print(length)

