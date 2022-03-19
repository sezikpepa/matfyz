from __future__ import annotations
import pygame
from typing import Tuple, List
import copy
from colors import white, red, yellow, orange, green, blue, grey, black
from random import randint, choice
from matrix import matrix_multiplication, rotation_x, rotation_z, rotation_y, rotation_x_reverse, rotation_y_reverse, rotation_z_reverse
from matrix import draw_matrix, rotate_for_draw
from matrix import rotation_x_speed, rotation_z_speed, rotation_y_speed, rotation_x_reverse_speed, rotation_y_reverse_speed, rotation_z_reverse_speed
from rubiks_cube_nets import solved_cube_net, cross_on_yellow_net, cross_on_white_net, corners_solved_net, first_two_layers_solved_net, oll_dot_net


class Square_on_cube:
    def __init__(self, p1: Tuple[float, float, float], p2: Tuple[float, float, float], p3: Tuple[float, float, float], p4: Tuple[float, float, float], color: pygame.Color) -> None:
        self.p1: Tuple[float, float, float] = p1
        self.p2: Tuple[float, float, float] = p2
        self.p3: Tuple[float, float, float] = p3
        self.p4: Tuple[float, float, float] = p4
   
        self.color: pygame.Color = color
        self.score: float = sum(p1[0] + p2[0] + p3[0] + p4[0]) + (sum(p1[1] + p2[1] + p3[1] + p4[1]) * 2) + (sum(p1[2] + p2[2] + p3[2] + p4[2]) * 10)

    def draw(self, screen: pygame.Surface, scale: float, cube_position: List[int]) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        if not isinstance(scale, (int, float)):
            raise TypeError(f"ERROR: variable scale has to be int or float -> {scale}")
        if not isinstance(cube_position, (list)):
            raise TypeError(f"ERROR: variable cubeposition has to be list -> {cube_position}")
        if len(cube_position) != 2:
            raise ValueError(f"ERROR: variable cubeposition can include only 2 elements -> {cube_position}")
        if not isinstance(cube_position[0], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[0]}")
        if not isinstance(cube_position[1], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[1]}")
        if cube_position[0] < 0:
            raise ValueError(f"ERROR: variable cube_position can include only positive numbers -> {cube_position[0]}")
        if cube_position[1] < 0:
            raise ValueError(f"ERROR: variable cube_position can include only positive numbers -> {cube_position[1]}")
        if scale <= 0:
            raise ValueError(f"ERROR: variable scale can include only positive numbers -> {scale}")

        point1 = [self.p1[0][0] * scale + cube_position[0], self.p1[1][0] * scale + cube_position[1]]
        point2 = [self.p2[0][0] * scale + cube_position[0], self.p2[1][0] * scale + cube_position[1]]
        point3 = [self.p3[0][0] * scale + cube_position[0], self.p3[1][0] * scale + cube_position[1]]
        point4 = [self.p4[0][0] * scale + cube_position[0], self.p4[1][0] * scale + cube_position[1]]
        pygame.draw.polygon(screen, self.color, (point1, point2, point3, point4))

    def __lt__(self, other) -> bool:
        if not isinstance(other, Square_on_cube):
            raise TypeError("ERROR: variable other for comparing can only be type Square_on_cube")

        return self.score < other.score

    def __str__(self) -> str:
        #print basic coordinates of points p1, p2, p3, p4 and color
        return f"{self.p1}:{self.p2}:{self.p3}:{self.p4}:{self.color}"

class Rubiks_cube_net:
    def __init__(self, position_x, position_y, net):
        if not isinstance(net, list):
            raise TypeError(f"ERROR: variable net has to be list -> {net}")
        self.faces: List[List[str]] = net
        self.x: int = position_x
        self.y: int = position_y

    def y_axis_turn(self, x_start: int, x_end: int, direction: int) -> None:
        if not isinstance(x_start, (int)):
            raise TypeError(f"ERROR: variable x_start has to be int -> {x_start}")
        if not isinstance(x_end, (int)):
            raise TypeError(f"ERROR: variable x_end has to be int -> {x_end}")
        if not isinstance(direction, (int)):
            raise TypeError(f"ERROR: variable direction has to be int -> {direction}")

        if direction == 1:
            x = 1
        elif direction == -1:
            x = 3
        else:
            raise ValueError("Invalid value for direction - possible values are [1, -1]")

        if x_start not in (0, 1, 2):
            raise ValueError("Invalid value for x_start, possible values are [0, 1, 2]")
        if x_end not in (0, 1, 2):
            raise ValueError("Invalid value for x_end, possible values are [0, 1, 2]")
        if x_start > x_end:
            raise ValueError(f"Value of x_start have to be lower than x_end or same -> {x_start} > {x_end}")

        x_end += 1

        for _ in range(x):
            face1 = []
            for y in range(3, 6):
                row = []
                for x in range(3):
                    row.append(self.faces[y][x])
                face1.append(row)

            face2 = []
            for y in range(3):
                row = []
                for x in range(3, 6):
                    row.append(self.faces[y][x])
                face2.append(row)

            face3 = []
            for y in range(3, 6):
                row = []
                for x in range(6, 9):
                    row.append(self.faces[y][x])
                face3.append(row)

            face4 = []
            for y in range(6, 9):
                row = []
                for x in range(3, 6):
                    row.append(self.faces[y][x])
                face4.append(row)

            #face 1
            for y in range(3, 6):
                for x in range(x_start, x_end):
                    self.faces[y][x] = face2[x][-(y - 3 + 1)]
      
            #face2
            for y in range(x_start, x_end):
                for x in range(3, 6):
                    self.faces[y][x] = face3[x - 3][-(y + 1)]
           
            #face3
            for y in range(3, 6):
                for x in range(x_start + 1, x_end + 1):
                    self.faces[y][-x] = face4[-(x)][-(y - 3 + 1)]
            
            #face4
            for y in range(x_start + 6, x_end + 6):
                for x in range(3, 6):
                    self.faces[14 - y][x] = face1[x - 3][y - 6]



    def face_rotation(self, x_start: int, y_start: int, direction: int) -> None:
        if not isinstance(x_start, int):
            raise TypeError(f"ERROR: variable x_start has to be int -> {x_start}")
        if not isinstance(y_start, int):
            raise TypeError(f"ERROR: variable y_start has to be int -> {y_start}")
        if not isinstance(y_start, int):
            raise TypeError(f"ERROR: variable y_start has to be int -> {y_start}")

        if not (0 <= x_start <= 6):
            raise ValueError(f"ERROR: variable x_start has to be between 0 and 6 -> {x_start}")
        if not (0 <= y_start <= 9):
            raise ValueError(f"ERROR: variable x_start has to be between 0 and 9 -> {y_start}")

        if direction == -1:
            x = 3
        elif direction == 1:
            x = 1
        else:
            raise ValueError("Invalid value for direction - possible values are [1, -1]")

        for _ in range(x):
            backup = copy.deepcopy(self.faces)
            self.faces[y_start][x_start] = backup[y_start + 2][x_start]
            self.faces[y_start + 2][x_start] = backup[y_start + 2][x_start + 2]
            self.faces[y_start + 2][x_start + 2] = backup[y_start + 0][x_start + 2]
            self.faces[y_start + 0][x_start + 2] = backup[y_start + 0][x_start + 0]

            self.faces[y_start + 1][x_start + 0] = backup[y_start + 2][x_start + 1]
            self.faces[y_start + 2][x_start + 1] = backup[y_start + 1][x_start + 2]
            self.faces[y_start + 1][x_start + 2] = backup[y_start + 0][x_start + 1]
            self.faces[y_start + 0][x_start + 1] = backup[y_start + 1][x_start + 0]

    def draw(self, screen: pygame.Surface, net_scale: int) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        if not isinstance(net_scale, int):
            raise TypeError(f"ERROR: variable scale has to be int -> {net_scale}")

        for i in range(9):
            for j in range(12):
                coordinate_x = i * net_scale + self.x
                coordinate_y = j * net_scale + self.y
                if self.faces[j][i]:
                    pygame.draw.rect(screen, "black", (coordinate_x, coordinate_y, net_scale, net_scale))
                    pygame.draw.rect(screen, self.faces[j][i], (coordinate_x + 1, coordinate_y + 1, net_scale - 2, net_scale - 2))


    def x_axis_turn(self, x_start: int, x_end: int, direction: int) -> None: 
        if direction not in (1, -1):
            raise ValueError("Invalid value for direction, possible values are [1, -1]")
        if x_start not in (0, 1, 2):
            raise ValueError("Invalid value for x_start, possible values are [0, 1, 2]")
        if x_end not in (0, 1, 2):
            raise ValueError("Invalid value for x_end, possible values are [0, 1, 2]")
        if x_start > x_end:
            raise ValueError(f"Value of x_start have to be lower than x_end or same")

        backup = copy.deepcopy(self.faces)
        for x in range(x_start + 3, x_end + 4):
            for y in range(0, 12):
                self.faces[y][x] = backup[(y - (3 * direction) % 12)][x]

    def z_axis_turn(self, x_start: int, x_end: int, direction: int) -> None:
        if x_start not in (0, 1, 2):
            raise ValueError(f"ERROR: Value of x_start have to be from (0, 1, 2) - x_start was {x_start}")
        if x_end not in (0, 1, 2):
            raise ValueError(f"ERROR: Value of x_end have to be from (1, 2, 3) - x_end was {x_end}")
        if x_start > x_end:
            raise ValueError(f"Value of x_start have to be lower than x_end or same")
        if direction not in (1, -1):
            raise ValueError(f"Invalid value for direction, possible values are [1, -1] -> {direction}")
        
        x_end += 1
        

        if direction == 1:
            x = 1
        elif direction == -1: 
            x = 3
        else:
            raise ValueError("Invalid value for direction, possible values are [1, -1]")

        for _ in range(x):
            backup = copy.deepcopy(self.faces)
            bottom_face = []
            for x in range(3, 6):
                line = []
                for y in range(9, 12):
                    line.append(backup[y][x])
                bottom_face.append(line)

            right_face = []
            for x in range(6, 9):
                line = []
                for y in range(3, 6):
                    line.append(backup[y][x])
                right_face.append(line)

            for y in range(x_start, x_end):
                for x in range(3, 9):
                    self.faces[y + 3][x] = backup[y + 3][x - 3]

                for x in range(0, 3):
                    self.faces[y + 3][x] = bottom_face[-(x + 1)][-(y + 1)]

                for x in range(3, 6):
                    self.faces[11 - y][x] = right_face[-(x - 3 + 1)][y]


    def change(self, move: str) -> None:
        if move == "x":
            self.x_axis_turn(0, 2, -1)
            self.face_rotation(6, 3, 1)
            self.face_rotation(0, 3, -1)

        elif move == "x’":
            self.x_axis_turn(0, 2, 1)
            self.face_rotation(6, 3, -1)
            self.face_rotation(0, 3, 1)

        elif move == "z":
            self.z_axis_turn(0, 2, 1)
            self.face_rotation(3, 6, 1)
            self.face_rotation(3, 0, -1)

        elif move == "z’":
            self.z_axis_turn(0, 2, -1)
            self.face_rotation(3, 6, -1)
            self.face_rotation(3, 0, 1)

        elif move == "y’":
            self.y_axis_turn(0, 2, 1) 
            self.face_rotation(3, 3, -1)
            self.face_rotation(3, 9, 1)

        elif move == "y":
            self.y_axis_turn(0, 2, -1) 
            self.face_rotation(3, 3, 1)
            self.face_rotation(3, 9, -1)

        elif move == "r":
            self.x_axis_turn(2, 2, -1)
            self.face_rotation(6, 3, 1)
            
        elif move == "r’":
            self.x_axis_turn(2, 2, 1)
            self.face_rotation(6, 3, -1)

        elif move == "f":
            self.z_axis_turn(2, 2, 1)
            self.face_rotation(3, 6, 1)

        elif move == "f’":
            self.z_axis_turn(2, 2, -1)
            self.face_rotation(3, 6, -1)

        elif move == "l":
            self.x_axis_turn(0, 0, 1)
            self.face_rotation(0, 3, 1)
            
        elif move == "l’":
            self.x_axis_turn(0, 0, -1)
            self.face_rotation(0, 3, -1)

        elif move == "b’":
            self.face_rotation(3, 0, -1)
            self.z_axis_turn(0, 0, 1)

        elif move == "b":
            self.face_rotation(3, 0, 1)
            self.z_axis_turn(0, 0, -1)

        elif move == "d":
            self.y_axis_turn(0, 0, 1) 
            self.face_rotation(3, 9, 1)

        elif move == "d’":
            self.y_axis_turn(0, 0, -1) 
            self.face_rotation(3, 9, -1)

        elif move == "u":
            self.y_axis_turn(2, 2, -1) 
            self.face_rotation(3, 3, 1)

        elif move == "u’":
            self.y_axis_turn(2, 2, 1) 
            self.face_rotation(3, 3, -1)

        else:
            raise ValueError(f"ERROR: Invalid value for move ’{move}’")


    def solve_check(self) -> bool:
        for y in range(4):
            for x in range(3):
                color_checker = self.faces[y * 3][x * 3]
                for i in range(0, 3):                   
                    for j in range(0, 3):
                        if self.faces[y * 3 + i][x * 3 + j] != color_checker:                       
                            return False
        return True

    def white_cross_check(self) -> bool:
        for i in range(0, 9, 3):
            for j in range(0, 12, 3):
                x = i + 1
                y = j + 1
                if self.faces[y][x] == "white":
                    if self.faces[y - 1][x] == "white" and self.faces[y + 1][x] == "white" and self.faces[y][x - 1] == "white" and self.faces[y][x + 1] == "white":
                        return True
        return False

    def yellow_cross_check(self) -> bool:
        for i in range(0, 9, 3):
            for j in range(0, 12, 3):
                x = i + 1
                y = j + 1
                if self.faces[y][x] == "yellow":
                    if self.faces[y - 1][x] == "white" and self.faces[y + 1][x] == "white" and self.faces[y][x - 1] == "white" and self.faces[y][x + 1] == "white":
                        return True
        return False

class Cube:
    def __init__(self, coeff_a: int, coeff_b: int, coeff_c: int, shift: float):
        if not isinstance(coeff_a, int):
            raise TypeError(f"ERROR: variable coeff_a has to be int -> {coeff_a}")
        if not isinstance(coeff_b, int):
            raise TypeError(f"ERROR: variable coeff_b has to be int -> {coeff_b}")
        if not isinstance(coeff_c, int):
            raise TypeError(f"ERROR: variable coeff_c has to be int -> {coeff_c}")
        if not isinstance(shift, (int, float)):
            raise TypeError(f"ERROR: variable shift can only be int or float -> {shift}")

        if coeff_a not in (-3, 0, 3):
            raise ValueError(f"ERROR: variable coeff_a can be -3, 0, 3 -> {coeff_a}")
        if coeff_b not in (-3, 0, 3):
            raise ValueError(f"ERROR: variable coeff_b can be -3, 0, 3 -> {coeff_b}")
        if coeff_c not in (-3, 0, 3):
            raise ValueError(f"ERROR: variable coeff_c can be -3, 0, 3 -> {coeff_c}")


        self.points: List[float] = [None, None, None, None, None, None, None, None]

        self.coeff_a: int = coeff_a
        self.coeff_b: int = coeff_b
        self.coeff_c: int = coeff_c

        self.render_decider: List[float] = []
        self.squares: List[Square_on_cube] = []

        self.score: float = None
        self.shift: float = shift
        
        self.points[0] = [[-shift + coeff_a], [-shift + coeff_b], [-shift + coeff_c]]
        self.points[1] = [[shift + coeff_a], [-shift + coeff_b], [-shift + coeff_c]]
        self.points[2] = [[shift + coeff_a], [-shift + coeff_b], [shift + coeff_c]]
        self.points[3] = [[-shift + coeff_a], [-shift + coeff_b], [shift + coeff_c]]
        self.points[4] = [[-shift + coeff_a], [shift + coeff_b], [-shift + coeff_c]]
        self.points[5] = [[shift + coeff_a], [shift + coeff_b], [-shift + coeff_c]]
        self.points[6] = [[shift + coeff_a], [shift + coeff_b], [shift + coeff_c]]
        self.points[7] = [[-shift + coeff_a], [shift + coeff_b], [shift + coeff_c]]

    def evaluate_score(self) -> None:
        self.score = self.squares[0].score

    def draw(self, screen: pygame.Surface, scale: float, cube_position: List[int]) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        if not isinstance(scale, (int, float)):
            raise TypeError(f"ERROR: variable scale has to be int or float -> {scale}")
        if not isinstance(cube_position, (list)):
            raise TypeError(f"ERROR: variable cubeposition has to be list -> {cube_position}")
        if len(cube_position) != 2:
            raise ValueError(f"ERROR: variable cubeposition can include only 2 elements -> {cube_position}")
        if not isinstance(cube_position[0], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[0]}")
        if not isinstance(cube_position[1], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[1]}")

        for square in self.squares:
            square.draw(screen, scale, cube_position)

    def create_copy(self) -> Cube:
        to_return = Cube(self.coeff_a, self.coeff_b, self.coeff_c, self.shift)

        for i in range(8):
            to_return.points[i] = self.points[i]

        return to_return

    def make_faces(self, net: Rubiks_cube_net) -> None:
        if not isinstance(net, Rubiks_cube_net):
            raise TypeError("ERROR: you have to enter only Rubiks_cube_net class as argument")
        else:
            #top
            if self.coeff_b == -3:
                color = net.faces[3 - (self.coeff_c // 3) + 1][(self.coeff_a // 3) + 1 + 3]
            else:
                color = black
            square = Square_on_cube(self.points[0], self.points[1], self.points[2], self.points[3], color)
            self.squares.append(square)

            #bottom
            if self.coeff_b == 3:
                color = net.faces[(self.coeff_c // 3) + 1 + 9][(self.coeff_a // 3) + 1 + 3]
            else:
                color = black
            square = Square_on_cube(self.points[4], self.points[5], self.points[6], self.points[7], color)
            self.squares.append(square)

            #front
            if self.coeff_c == -3:
                color = net.faces[self.coeff_b // 3 + 1 + 6][self.coeff_a // 3 + 1 + 3]
            else:
                color = black
            square = Square_on_cube(self.points[0], self.points[1], self.points[5], self.points[4], color)
            self.squares.append(square)

            #back 
            if self.coeff_c == 3:
                color = net.faces[0 - self.coeff_b // 3 + 1][self.coeff_a // 3 + 1 + 3]
            else:
                color = black
            square = Square_on_cube(self.points[2], self.points[3], self.points[7], self.points[6], color)
            self.squares.append(square)

            #left
            if self.coeff_a == -3:
                color = net.faces[3 - self.coeff_c // 3 + 1][0 - self.coeff_b // 3 + 1]
            else:
                color = black
            square = Square_on_cube(self.points[3], self.points[0], self.points[4], self.points[7], color)
            self.squares.append(square)
            
            #right
            if self.coeff_a == 3:
                color = net.faces[3 - self.coeff_c // 3 + 1][self.coeff_b // 3 + 1 + 6]
            else:
                color = black
            square = Square_on_cube(self.points[1], self.points[2], self.points[6], self.points[5], color)       
            self.squares.append(square)
          

        self.squares.sort(reverse=True)
        self.evaluate_score()

        
    def __lt__(self, other) -> bool:
        return self.score < other.score

    def __str__(self) -> str:
        #coeff_a, coeff_b, coeff_c, score       
        return f"{self.coeff_a}:{self.coeff_b}:{self.coeff_c}:{self.score}"

class Rubiks_cube: 

    def __init__(self, shift: float, net_scale: int, net_x, net_y, mode:int = None) -> None:
        if not isinstance(shift, (int, float)):
            raise TypeError(f"ERROR: variable shift can be int or float -> {shift}")
        if not isinstance(net_scale, int):
            raise TypeError(f"ERROR: variable net_scale has to be int -> {net_scale}")
        if shift < 0:
            raise ValueError(f"ERROR: variable shift has to be a positive number -> {shift}")
        if net_scale < 0:
            raise ValueError(f"ERROR: variable net_scale has to be a positive number -> {net_scale}")

        self.net_x: int = net_x
        self.net_y: int = net_y

        self.solved: bool = False
        self.counter: int = 0
        self.moves_buffer: List[str] = []
        self.counter_max: int = 157
        self.net_scale: int = net_scale
        self.move_in_progress: bool = False
        self.reset_waiting: bool = False

        self.cubes: List[Cube] = []
        self.user_moves_blocked: bool = False

        if not mode:
            net = solved_cube_net
        elif mode == 1:
            net = cross_on_white_net

        self.net: Rubiks_cube_net = Rubiks_cube_net(self.net_x, self.net_y, copy.deepcopy(net))  #WARNING: it does not work without copy
        self.net_setup: Rubiks_cube_net = copy.deepcopy(self.net)

        for i in range(-3, 6, 3):
            for j in range(-3, 6, 3):
                for k in range(-3, 6, 3):
                    self.cubes.append(Cube(i, j, k, shift))

    def solve_check(self) -> None:
        self.solved = self.net.solve_check()
 
    def do_next_move(self) -> None:
        if self.moves_buffer:
            if self.counter == self.counter_max:
                self.move_in_progress = False
                self.counter = 0
                self.net.change(self.moves_buffer[0])
                self.moves_buffer = self.moves_buffer[1:]
                self.solve_check()

            else:
                self.move_in_progress = True
                #WHOLE CUBE--------------------------------------------------------------
                if self.moves_buffer[0] == "x":
                    self.whole_cube_rotation(rotation_x_reverse)
                elif self.moves_buffer[0] == "x’":
                    self.whole_cube_rotation(rotation_x)

                elif self.moves_buffer[0] == "y":
                    self.whole_cube_rotation(rotation_y_reverse)
                elif self.moves_buffer[0] == "y’":
                    self.whole_cube_rotation(rotation_y)

                elif self.moves_buffer[0] == "z":
                    self.whole_cube_rotation(rotation_z)
                elif self.moves_buffer[0] == "z’":
                    self.whole_cube_rotation(rotation_z_reverse)
                #-----------------------------------------------------------------------------
                #SINGLE MOVES-----------------------------------------------------------------
                elif self.moves_buffer[0] == "f":
                    self.f_turn(rotation_z, [-2, -4])
                elif self.moves_buffer[0] == "f’":
                    self.f_turn(rotation_z_reverse, [-2, -4])

                elif self.moves_buffer[0] == "d":
                    self.d_turn(rotation_y, [2, 4])
                elif self.moves_buffer[0] == "d’":
                    self.d_turn(rotation_y_reverse, [2, 4])

                elif self.moves_buffer[0] == "r’":
                    self.r_turn(rotation_x, [2, 4])
                elif self.moves_buffer[0] == "r":
                    self.r_turn(rotation_x_reverse, [2, 4])

                elif self.moves_buffer[0] == "l":
                    self.r_turn(rotation_x, [-2, -4])
                elif self.moves_buffer[0] == "l’":
                    self.r_turn(rotation_x_reverse, [-2, -4])

                elif self.moves_buffer[0] == "u":
                    self.d_turn(rotation_y_reverse, [-2, -4])
                elif self.moves_buffer[0] == "u’":
                    self.d_turn(rotation_y, [-2, -4])

                elif self.moves_buffer[0] == "b":
                    self.f_turn(rotation_z_reverse, [2, 4])
                elif self.moves_buffer[0] == "b’":
                    self.f_turn(rotation_z, [2, 4])

                else:
                    raise ValueError(f"ERROR: invalid value for move - value was {self.moves_buffer[0]}")

                #-------------------------------------------------------------------------------
                self.counter += 1

        else:
            self.user_moves_blocked = False
            

    def whole_cube_rotation(self, matrix: List[List[float]]) -> None:
        if not isinstance(matrix, list):
            raise TypeError(f"ERROR: variable matrix has to be a two_dimensional array -> {matrix}")
        for i in range(len(self.cubes)):
            for j in range(8):
                self.cubes[i].points[j] = matrix_multiplication(matrix, self.cubes[i].points[j])
           
    def draw(self, screen: pygame.Surface, scale: int, cube_position: List[int]) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        if not isinstance(scale, int):
            raise TypeError(f"ERROR: variable scale has to be int or float -> {scale}")
        if not isinstance(cube_position, (list)):
            raise TypeError(f"ERROR: variable cubeposition has to be list -> {cube_position}")
        if len(cube_position) != 2:
            raise ValueError(f"ERROR: variable cubeposition can include only 2 elements -> {cube_position}")
        if not isinstance(cube_position[0], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[0]}")
        if not isinstance(cube_position[1], int):
            raise TypeError(f"ERROR: there can only be ints in variable cube_position -> {cube_position[1]}")


        rubiks_cube_for_draw: list = []
        for element in self.cubes:
            rubiks_cube_for_draw.append(element.create_copy())
        for i in range(len(rubiks_cube_for_draw)):
            for j in range(8):
                rubiks_cube_for_draw[i].points[j] = matrix_multiplication(rotate_for_draw, rubiks_cube_for_draw[i].points[j])

        for cube in rubiks_cube_for_draw:
            cube.make_faces(self.net_setup)

        rubiks_cube_for_draw.sort(reverse=True)

        for cube in rubiks_cube_for_draw:
            cube.draw(screen, scale, cube_position)

        self.net.draw(screen, self.net_scale)

    def f_turn(self, matrix: List[list], checker: List[int]) -> None:
        if not isinstance(matrix, list):
            raise TypeError(f"ERROR: variable matrix has to be a two-dimensional array -> {matrix}")
        if not isinstance(checker, list):
            raise TypeError(f"ERROR: variable checker has to be a list -> {list}")

        for i in range(len(self.cubes)):
            for j in range(8):
                self.cubes[i].points[j][2][0] = round(self.cubes[i].points[j][2][0])
                if self.cubes[i].points[j][2][0] in checker:
                    self.cubes[i].points[j] = matrix_multiplication(matrix, self.cubes[i].points[j])

    def d_turn(self, matrix: List[list], checker: List[int]) -> None:
        if not isinstance(matrix, list):
            raise TypeError(f"ERROR: variable matrix has to be a two-dimensional array -> {matrix}")
        if not isinstance(checker, list):
            raise TypeError(f"ERROR: variable checker has to be a list -> {list}")

        for i in range(len(self.cubes)):
            for j in range(8):
                self.cubes[i].points[j][1][0] = round(self.cubes[i].points[j][1][0])
                if self.cubes[i].points[j][1][0] in checker:
                    self.cubes[i].points[j] = matrix_multiplication(matrix, self.cubes[i].points[j])

    def r_turn(self, matrix: List[list], checker: List[int]) -> None:
        if not isinstance(matrix, list):
            raise TypeError(f"ERROR: variable matrix has to be a two-dimensional array -> {matrix}")
        if not isinstance(checker, list):
            raise TypeError(f"ERROR: variable checker has to be a list -> {list}")

        for i in range(len(self.cubes)):
            for j in range(8):
                self.cubes[i].points[j][0][0] = round(self.cubes[i].points[j][0][0])
                if self.cubes[i].points[j][0][0] in checker:
                    self.cubes[i].points[j] = matrix_multiplication(matrix, self.cubes[i].points[j])

    def generate_random_move(self) -> None:
        if not self.moves_buffer:
            random_number = randint(0, 15)
            moves = ["x", "x’", "y", "y’", "z", "z’", "u", "u’", "d", "d’", "l", "l’", "r", "r’", "f", "f’", "b", "b’"]
            self.moves_buffer.append(moves[random_number])

    def new_scramble(self) -> None:
        self.reset_waiting = False
        self.user_moves_blocked = True
        moves = ["u", "u’", "d", "d’", "l", "l’", "r", "r’", "f", "f’", "b", "b"]
        opposite_moves = [["u", "u’"], ["d", "d’"], ["l", "l’"], ["r", "r’"], ["f", "f’"], ["b", "b’"]]

        previous_move = None
        while len(self.moves_buffer) < 20:
            generated_element = choice(moves)
            if [previous_move, generated_element] not in opposite_moves and [generated_element, previous_move] not in opposite_moves:
                self.moves_buffer.append(generated_element)
                previous_move = generated_element

    def white_cross_check(self) -> bool:
        return self.net.white_cross_check()

    def yellow_cross_check(self) -> bool:
        return self.net.yellow_cross_check()



#-----------------------------------------------------------------------------------------------------------------

import unittest

class TestStringMethods(unittest.TestCase):

    def test_rubiks_cube_net_solve_check(self):
        net1 = Rubiks_cube_net(solved_cube_net)
        net1.faces = [[None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None]]

        net2 = Rubiks_cube_net(solved_cube_net)
        net2.faces = [[None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    [None, None, None, "green", "green", "white", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None]]

        net3 = Rubiks_cube_net(solved_cube_net)
        net3.faces = [[None, None, None, "orange", "orange", "orange", None, None, None],
                    [None, None, None, "orange", "orange", "orange", None, None, None],
                    [None, None, None, "orange", "orange", "orange", None, None, None],
                    ["green", "green", "green", "white", "white", "white", "blue", "blue", "blue"],
                    ["green", "green", "green", "white", "white", "white", "blue", "blue", "blue"],
                    ["green", "green", "green", "white", "white", "white", "blue", "blue", "blue"],
                    [None, None, None, "red", "red", "red", None, None, None],
                    [None, None, None, "red", "red", "red", None, None, None],
                    [None, None, None, "red", "red", "red", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None]]
        
        self.assertTrue(net1.solve_check())
        self.assertFalse(net2.solve_check())
        self.assertTrue(net3.solve_check())
    
    def test_rubiks_cube_net_moves_sequence_correct(self):
        net1 = Rubiks_cube_net(solved_cube_net)
        moves = ["r", "u", "l", "x", "b", "d", "f", "z", "u", "f’", "r’", "u", "y", "l’", "f", "f", "z", "l’", "b’", "f", "u"]

        for move in moves:
            net1.change(move)

        check_net = [[None, None, None, "white", "green", "white", None, None, None],
                    [None, None, None, "white", "blue", "white", None, None, None],
                    [None, None, None, "white", "blue", "orange", None, None, None],
                    ["red", "red", "red", "blue", "orange", "blue", "yellow", "blue", "blue"],
                    ["yellow", "red", "white", "orange", "yellow", "green", "white", "orange", "yellow"],
                    ["orange", "blue", "yellow", "green", "red", "yellow", "orange", "red", "red"],
                    [None, None, None, "red", "green", "green", None, None, None],
                    [None, None, None, "red", "green", "yellow", None, None, None],
                    [None, None, None, "white", "green", "yellow", None, None, None],
                    [None, None, None, "green", "yellow", "blue", None, None, None],
                    [None, None, None, "blue", "white", "orange", None, None, None],
                    [None, None, None, "green", "orange", "orange", None, None, None]]

        self.assertListEqual(net1.faces, check_net)

    
    def test_white_cross_check(self):
        net = Rubiks_cube_net(solved_cube_net)
        net.faces = [[None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None]]

        net2 = Rubiks_cube_net(solved_cube_net)
        net2.faces = [[None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    [None, None, None, "blue", "blue", "blue", None, None, None],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "white", "white", "red", "red", "red"],
                    ["orange", "orange", "orange", "white", "yellow", "white", "red", "red", "red"],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "green", "green", "green", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None],
                    [None, None, None, "yellow", "yellow", "yellow", None, None, None]]

        self.assertTrue(net.white_cross_check())
        self.assertFalse(net2.white_cross_check())

    def test_squares_str(self):
        square = Square_on_cube([[1], [-2], [2]], [[0], [-1], [1]], [[1], [-1], [2]], [[1], [-2], [2]], "white")
        self.assertEqual(str(square), "[[1], [-2], [2]]:[[0], [-1], [1]]:[[1], [-1], [2]]:[[1], [-2], [2]]:white")

    def test_cube_str(self):
        cube = Cube(-3, 0, 3, 1)
        self.assertEqual(str(cube), "-3:0:3:None")
        
    def test_squares_ordering(self):
        #TODO
        pass
        #squares = [Square_on_cube()]
       

if __name__ == '__main__':
    unittest.main()