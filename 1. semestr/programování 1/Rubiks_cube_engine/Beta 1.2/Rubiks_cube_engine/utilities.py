import datetime
import pygame
from colors import white, red, yellow, orange, green, blue, grey, black, lime
from typing import Tuple

from rubiks_cube_structures import Rubiks_cube_net
from rubiks_cube_nets import empty_net

class Timer():
    def __init__(self, x: int, y: int):
        if not isinstance(x, int):
            raise TypeError(f"ERROR: variable x has to be int -> {x}")
        if not isinstance(y, int):
            raise TypeError(f"ERROR: variable y has to be int -> {y}")
        if x < 0:
            raise ValueError(f"ERROR: variable x has to be positive number -> {x}")
        if y < 0:
            raise ValueError(f"ERROR: variable y has to be positive number -> {y}")

        self.utc_start = datetime.datetime.utcnow()
        self.time_since = self.utc_start - self.utc_start
        self.x: int = x
        self.y: int = y

    def evaluate_time(self) -> None:
        now = datetime.datetime.utcnow()
        self.time_since = now - self.utc_start
        
    def draw(self, screen: pygame.Surface) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        time_on_timer = self.time_since.total_seconds()

        rest = time_on_timer % 3600
        hours = int(time_on_timer // 3600)
        rest = time_on_timer % 60
        minutes = int(time_on_timer // 60)
        
        rest = str(rest).split(".")
        seconds = rest[0]
        miliseconds = str(rest[1])[:2]

        time_on_timer = f"{hours:0>2}:{minutes:0>2}:{seconds:0>2}.{miliseconds:0>2}"
        font = pygame.font.SysFont(None, 100)
        img = font.render(time_on_timer, True, black)
        screen.blit(img, (self.x, self.y))

    def __str__(self) -> str:
        return f"{hours:0>2}:{minutes:0>2}:{seconds:0>2}.{miliseconds:0>2}"


class Button:
    def __init__(self, x: int, y: int, width: int, height: int, color, text: str, border_radius: int) -> None:
        if not isinstance(x, int):
            raise TypeError(f"ERROR: variable x has to be int -> {x}")
        if not isinstance(y, int):
            raise TypeError(f"ERROR: variable y has to be int -> {y}")
        if not isinstance(width, int):
            raise TypeError(f"ERROR: variable width has to be int -> {width}")
        if not isinstance(height, int):
            raise TypeError(f"ERROR: variable height has to be int -> {height}")
        if not isinstance(text, str):
            raise TypeError(f"ERROR: variable text has to be str -> {text}")

        if x < 0:
            raise ValueError(f"ERROR: variable x can not be negative -> {x}")
        if y < 0:
            raise ValueError(f"ERROR: variable y can not be negative -> {y}")
        if width < 0:
            raise ValueError(f"ERROR: variable width can not be negative -> {width}")
        if height < 0:
            raise ValueError(f"ERROR: variable height can not be negative -> {height}")

        self.x: int = x
        self.y: int = y
        self.width: int = width
        self.height: int = height
        self.color = color
        self.text: str = text
        self.keep_pressed: bool = False
        self.border_radius: int = border_radius

    def draw(self, screen: pygame.Surface) -> None:
        if not isinstance(screen, pygame.Surface):
            raise TypeError(f"ERROR: variable screen has to be type pygame.Surface")
        pygame.draw.rect(screen, black, (self.x, self.y, self.width, self.height), border_radius = self.border_radius)
        pygame.draw.rect(screen, self.color, (self.x + 2, self.y + 2, self.width - 4, self.height - 4), border_radius = self.border_radius)

        font = pygame.font.SysFont(None, 35)
        img = font.render(self.text, True, black)
        text_size = img.get_size()
        x_for_print = self.x + ((self.width - text_size[0]) // 2)
        y_for_print = self.y + ((self.height - text_size[1]) // 2)
        screen.blit(img, (x_for_print, y_for_print))

    def clicked_check(self, mouse_position: Tuple[int]) -> bool:
        if not isinstance(mouse_position, Tuple):
            raise TypeError(f"ERROR: variable mouse_position has to be tuple -> {mouse_position}")
        if len(mouse_position) != 2:
            raise TypeError(f"ERROR: variable mouse_position has to have two elements -> {mouse_position}")
        if not isinstance(mouse_position[0], int):
            raise TypeError(f"ERROR: variable mouse_position can include only ints -> {mouse_position[0]}")
        if not isinstance(mouse_position[1], int):
            raise TypeError(f"ERROR: variable mouse_position can include only ints -> {mouse_position[1]}")
        if mouse_position[0] < 0:
            raise ValueError(f"ERROR: variable mouse_position can include only positive numbers -> {mouse_position[0]}")
        if mouse_position[1] < 0:
            raise ValueError(f"ERROR: variable mouse_position can include only positive numbers -> {mouse_position[1]}")

        return self.x < mouse_position[0] < self.x + self.width and self.y < mouse_position[1] < self.y + self.height

class Parts_solved:
    def __init__(self):
        self.cross = False
        self.first_layer = False
        self.first_two_layers = False
        self.oll_cross = False
        self.oll = False
        self.pll_lights = False
        self.pll = False


class Net_inserter:
    def __init__(self, x: int, y: int, color_pickers_size: int):
        self.color_pickers_size = color_pickers_size
        self.valid: bool = False

        self.x = x
        self.y = y

        self.net = Rubiks_cube_net(self.x, self.y, empty_net)

        self.choose_white_button = Button(500 + self.color_pickers_size * 0, 550, self.color_pickers_size, self.color_pickers_size, white, "", 5)
        self.choose_yellow_button = Button(500 + self.color_pickers_size * 1, 550, self.color_pickers_size, self.color_pickers_size, yellow, "", 5)
        self.choose_orange_button = Button(500 + self.color_pickers_size * 2, 550, self.color_pickers_size, self.color_pickers_size, orange, "", 5)
        self.choose_red_button = Button(500 + self.color_pickers_size * 3, 550, self.color_pickers_size, self.color_pickers_size, red, "", 5)
        self.choose_green_button = Button(500 + self.color_pickers_size * 4, 550, self.color_pickers_size, self.color_pickers_size, green, "", 5)
        self.choose_blue_button = Button(500 + self.color_pickers_size * 5, 550, self.color_pickers_size, self.color_pickers_size, blue, "", 5)
        

    def draw(self, screen: pygame.Surface, scale: int):
        self.net.draw(screen, scale)

        self.choose_white_button.draw(screen)
        self.choose_yellow_button.draw(screen)
        self.choose_orange_button.draw(screen)
        self.choose_red_button.draw(screen)
        self.choose_blue_button.draw(screen)
        self.choose_green_button.draw(screen)







