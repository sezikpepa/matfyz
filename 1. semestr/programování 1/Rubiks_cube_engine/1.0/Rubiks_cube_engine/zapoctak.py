import pygame
import os
import math
from typing import List
from typing import Tuple
import copy
import time

from rubiks_cube_structures import Square_on_cube, Rubiks_cube_net, Cube, Rubiks_cube
from colors import white, red, yellow, orange, green, blue, grey, black, lime, bage, tyrkis, violet, pink, dark_green
from utilities import Timer, Button, Net_inserter, Algorithm_helper, Info_window
from keyboard_press_translator import keyboard_press_translator
from algs import swap_corners_alg, swap_edges_alg, rotate_edges_alg, rotate_corner_alg

from project_settings import speed, shift, net_scale, scale, net_x, net_y, fps, cube_position, window_width, window_height, window_caption

#SETTINGS
#os.environ["SDL_VIDEO_CENTERED"] = '1'

pygame.init()
pygame.display.set_caption(window_caption)
screen = pygame.display.set_mode((window_width, window_height))
clock = pygame.time.Clock()

#PICTURES
info_picture = pygame.image.load('images/moves_icon_1.png')

#REST
mode_selected = None

#MODE
user_mode = 0

if __name__ == "__main__":
    #RUBIKS CUBE
    rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y)

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

    insert_own_button = Button(1350, 920, 350, 40, blue, "INSERT OWN", 50)
    confirm_insert_button = Button(1350, 700, 350, 40, green, "CONFIRM", 50)


    #TIMER
    timer = Timer(1300, 150)

    #INSERTER
    net_inserter = Net_inserter(1385, 250, 40, 1400, 650, net_scale=30)

    keep_inserter_shown: bool = False

    algorithm_helper = Algorithm_helper(800, 840, 500, 100, white, [])
    info_window = Info_window(700, 100, 500, 50, white, "", False)

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
                    algorithm_helper.reset()
                    info_window.reset()

                if start_learning_button.clicked_check(mouse_position):
                    #TODO
                    pass
                if cross_practice_button.clicked_check(mouse_position):
                    rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y, mode=1)

                if first_layer_practice_button.clicked_check(mouse_position):
                    rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y, mode=2)

                if oll_practice_button.clicked_check(mouse_position):
                    rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y, mode=4)
                    algorithm_helper.reset()
                    info_window.reset()

                if pll_practice_button.clicked_check(mouse_position):
                    rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y, mode=5)
                    algorithm_helper.reset()
                    info_window.reset()



                
                algorithm_helper.clicked_check(mouse_position)
                info_window.clicked_check(mouse_position)

                if shuffle_button.clicked_check(mouse_position):
                    rubiks_cube_player.new_scramble()
                    rubiks_cube_player.scrambling = True

                if insert_own_button.clicked_check(mouse_position) and not insert_own_button.keep_pressed:
                    insert_own_button.keep_pressed = True
                    insert_own_button.text = "CLOSE"
                
                elif insert_own_button.clicked_check(mouse_position) and insert_own_button.keep_pressed:
                    insert_own_button.keep_pressed = False
                    insert_own_button.text = "INSERT OWN"
                
                if confirm_insert_button.clicked_check(mouse_position):
                    if net_inserter.check_validity():
                        rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y)
                        rubiks_cube_player.insert_own_net(net_inserter.net)
                        net_inserter.reset()
                        insert_own_button.keep_pressed = False
                        insert_own_button.text = "INSERT OWN"

                    else:
                        net_inserter.show_invalid_state()
                     

                net_inserter.color_input(mouse_position)
                net_inserter.box_clicked(mouse_position)


            elif event.type == pygame.KEYDOWN:
                if not rubiks_cube_player.user_moves_blocked:
                    try:
                        new_move = keyboard_press_translator[event.key]
                        if new_move[-1] == "2":
                            rubiks_cube_player.moves_buffer.append(new_move[0])
                            rubiks_cube_player.moves_buffer.append(new_move[0])
                        else:
                            rubiks_cube_player.moves_buffer.append(new_move)
                        if not algorithm_helper.locked:
                            algorithm_helper.add_move(new_move)
                    except KeyError:
                        pass
                    
        #CUBE THINGS
        if rubiks_cube_player.reset_waiting and not rubiks_cube_player.move_in_progress:
            rubiks_cube_player = Rubiks_cube(shift, net_scale, net_x, net_y)

        if not rubiks_cube_player.scrambling:
            rubiks_cube_player.do_next_move()
        else:
            rubiks_cube_player.scramble()

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
        if insert_own_button.keep_pressed:
            net_inserter.draw(screen)
            confirm_insert_button.draw(screen)

        #INFO WINDOW
        if rubiks_cube_player.mode == 5 and not rubiks_cube_player.scrambling:
            #CORNERS
            if not (algorithm_helper.moves or rubiks_cube_player.moves_buffer):
                help_for_pll = rubiks_cube_player.net.mode_five_hinter()
                if help_for_pll:
                    if help_for_pll[0] == 1:
                        if help_for_pll[1] != "back":
                            info_window.text = "ROTATE ADJACENT CORNERS TO BACK"
                            algorithm_helper.ingore_u = True
                        else:
                            algorithm_helper.ingore_u = False
                            info_window.text = "DO ALGORITHM TO SWAP CORNERS"
                            algorithm_helper.algorithm = swap_corners_alg


                    elif help_for_pll[0] == 0:
                        algorithm_helper.ingore_u = False
                        info_window.text = "DO ALGORITHM TO SWAP CORNERS"
                        algorithm_helper.algorithm = swap_corners_alg

                    else:
                        #EDGES
                        if len(help_for_pll[2]) == 1:
                            if help_for_pll[2][0] != "front":
                                info_window.text = "ROTATE STRIP TO FRONT"
                                algorithm_helper.ingore_u = True
                            else:
                                algorithm_helper.ingore_u = False
                                info_window.text = "DO EDGE SWITCH ALGORITHM"
                                algorithm_helper.algorithm = swap_edges_alg

                        elif len(help_for_pll[2]) == 4:
                            if rubiks_cube_player.net.solved_check():
                                info_window.text = "SOLVED. GONGRATULATION"
                                rubiks_cube_player.user_moves_blocked = True
                            else:
                                info_window.text = "ALIGN LAST LAYER"
                        else:
                            algorithm_helper.ingore_u = False
                            info_window.text = "DO EDGE SWITCH ALGORITHM"
                            algorithm_helper.algorithm = swap_edges_alg

        if rubiks_cube_player.mode == 4 and not rubiks_cube_player.scrambling:
            if not (algorithm_helper.moves or rubiks_cube_player.moves_buffer):
                help_for_oll = rubiks_cube_player.net.mode_four_hinter_edges()
                print(help_for_oll)
                if help_for_oll[0] == 4:
                    info_window.text = "DO EDGE ROTATE ALGORITHM"
                    algorithm_helper.algorithm = rotate_edges_alg

                elif help_for_oll[0] == 2:
                    if help_for_oll[2] == "line":
                        if help_for_oll[3] == "right":
                            algorithm_helper.ingore_u = False
                            info_window.text = "DO EDGE ROTATE ALGORITHM"
                            algorithm_helper.algorithm = rotate_edges_alg

                        elif help_for_oll[3] == "wrong":
                            info_window.text = "ROTATE LINE TO RIGHT POSITION"
                            algorithm_helper.ingore_u = True

                    elif help_for_oll[2] == "l-shape":
                        if "front" in help_for_oll[1] and "right" in help_for_oll[1]:
                            algorithm_helper.ingore_u = False
                            info_window.text = "DO EDGE ROTATE ALGORITHM"
                            algorithm_helper.algorithm = rotate_edges_alg

                        else:
                            info_window.text = "ROTATE L-SHAPE TO RIGHT POSITION"
                            algorithm_helper.ingore_u = True

                elif help_for_oll[0] == 0:
                    oll_corners_hint = rubiks_cube_player.net.mode_four_hinter_corners()
                    print(oll_corners_hint)
                    if oll_corners_hint[0] == 0:
                        info_window.text = "OLL SOLVED. CONGRATULATION"
                        rubiks_cube_player.user_moves_blocked = True

                    else:
                        if "4" in oll_corners_hint[1]:
                            info_window.text = "DO CORNER ROTATE ALG UNTIL IT IS SOLVED"
                            algorithm_helper.algorithm = copy.deepcopy(rotate_corner_alg)
                            algorithm_helper.ingore_u = False
                        else:
                            info_window.text = "ROTATE ANOTHER CORNER TO RIGHT SPOT"
                            algorithm_helper.ingore_u = True

        
        info_window.draw(screen)

        #ALGORITHM HELPER
        algorithm_helper.alg_done_check()

        algorithm_helper.draw(screen)


        
        pygame.display.update()

    pygame.quit()

