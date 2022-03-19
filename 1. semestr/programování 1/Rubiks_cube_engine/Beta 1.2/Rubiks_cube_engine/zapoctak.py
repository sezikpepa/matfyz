import pygame
import os
import math
from typing import List
from typing import Tuple
import copy
import time

from rubiks_cube_structures import Square_on_cube, Rubiks_cube_net, Cube, Rubiks_cube
from colors import white, red, yellow, orange, green, blue, grey, black, lime, bage, tyrkis, violet, pink, dark_green
from utilities import Timer, Button, Net_inserter
from keyboard_press_translator import keyboard_press_translator

#SETTINGS
os.environ["SDL_VIDEO_CENTERED"] = '1'
width, height = 1920, 1080

pygame.init()
pygame.display.set_caption("Rubik's cube")
screen = pygame.display.set_mode((width, height))
clock = pygame.time.Clock()
fps = 100

cube_position = [width // 2, height // 2]
scale = 50
net_scale = int(0.6 * scale)
speed = 100
shift = 1

#PICTURES
info_picture = pygame.image.load('images/moves_icon_1.png')

#REST
mode_selected = None

if __name__ == "__main__":
    #RUBIKS CUBE
    rubiks_cube_player = Rubiks_cube(shift, net_scale, 100, 100)

    #BUTTONS
    start_learning_button = Button(250, 520, 350, 40, lime, "START LEARNING", 50)
    start_solving_with_help_button = Button(250, 570, 350, 40, orange, "START WITH HELP", 50)   
    cross_practice_button = Button(250, 620, 350, 40, blue, "CROSS PRACTICE", 50)
    first_layer_practice_button = Button(250, 670, 350, 40, tyrkis, "FIRST LAYER PRACTICE", 50)
    second_layer_practice_button = Button(250, 720, 350, 40, violet, "SECOND LAYER PRACTICE", 50)
    oll_practice_button = Button(250, 770, 350, 40, pink, "OLL PRACTICE", 50)
    pll_practice_button = Button(250, 820, 350, 40, dark_green, "PLL PRACTICE", 50)
    shuffle_button = Button(250, 870, 350, 40, bage, "SHUFFLE", 50)
    reset_button = Button(250, 920, 350, 40, red, "RESET", 50)

    insert_own_button = Button(1300, 800, 350, 40, blue, "INSERT OWN", 50)

    #TIMER
    timer = Timer(1300, 150)

    #INSERTER
    net_inserter = Net_inserter(700, 350, 40)


    keep_inserter_shown: bool = False

    run = True
    while run:
        clock.tick(fps)
        screen.fill(grey)

        #CONTROLS
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                run = False

            elif event.type == pygame.MOUSEBUTTONDOWN:
                mouse_position = pygame.mouse.get_pos()
                if reset_button.clicked_check(mouse_position):
                    rubiks_cube_player.reset_waiting = True

                if start_learning_button.clicked_check(mouse_position):
                    #TODO
                    pass
                if cross_practice_button.clicked_check(mouse_position):
                    rubiks_cube_player = Rubiks_cube(shift, net_scale, mode=1)

                if shuffle_button.clicked_check(mouse_position):
                    rubiks_cube_player.new_scramble()

                if insert_own_button.clicked_check(mouse_position):
                    insert_own_button.keep_pressed = True


            elif event.type == pygame.KEYDOWN:
                if not rubiks_cube_player.user_moves_blocked:
                    try:
                        rubiks_cube_player.moves_buffer.append(keyboard_press_translator[event.key])
                    except KeyError:
                        pass
                    
        #CUBE THINGS
        if rubiks_cube_player.reset_waiting and not rubiks_cube_player.move_in_progress:
            rubiks_cube_player = Rubiks_cube(shift, net_scale, 100, 100)

        rubiks_cube_player.do_next_move()
        rubiks_cube_player.draw(screen, scale, cube_position)

        #BUTTONS DRAW
        start_learning_button.draw(screen)
        reset_button.draw(screen)
        start_solving_with_help_button.draw(screen)
        cross_practice_button.draw(screen)
        shuffle_button.draw(screen)
        first_layer_practice_button.draw(screen)
        second_layer_practice_button.draw(screen)
        oll_practice_button.draw(screen)
        pll_practice_button.draw(screen)

        insert_own_button.draw(screen)

        #TIMER THINGS     
        timer.draw(screen)

        #INSERTER
        net_inserter.draw(screen, net_scale)

        
        pygame.display.update()

    pygame.quit()

