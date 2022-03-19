from pathlib import Path
import copy

longest = 10000000000
def king_path(board, start, length) -> int:
    global longest
    new_boards = []
    for x in (-1, 0, 1):
        for y in (-1, 0, 1):
            if board[start[0] + x][start[1] + y] == "C":
                if length < longest:
                    longest = length + 1
            elif board[start[0] + x][start[1] + y] == ".":
                new_board = copy.deepcopy(board)
                new_board[start[0]][start[1]] = "X"
                new_board[start[0] + x][start[1] + y] = "S"
                new_boards.append([new_board, [start[0] + x, start[1] + y]])
                
    for board in new_boards:
        king_path(board[0], board[1], length + 1)


board_file = Path("sachovnice.txt")
board = []
start_position = None
end_position = None

with board_file.open("r", newline="", encoding="utf-8") as reader:
    max_distance = 0
    x = 0
    y = 0
    counter = 0
    for line in reader: 
        if counter > 2:
            row = ["X"]
            for i in range(len(line)):
                if line[i] == "S":
                    start_position = [x + 1, y + 1]
                if line[i] == "C":
                    end_position = [x + 1, y + 1]
                if line[i] in ".XSC":
                    row.append(line[i])
                    max_distance += 1

                y += 1

            row.append("X")
            board.append(row)
            x += 1
            y = 0
        else:
            counter += 1

if not start_position or not end_position:
    print(-1)
    exit(0)

first_line = []
for i in range(len(board[0])):
    first_line.append("X")

board.append(first_line)
board.insert(0, copy.deepcopy(first_line))

king_path(board, start_position, 0)

if longest == 10000000000:
    print(-1)
else:
    print(longest)



    
