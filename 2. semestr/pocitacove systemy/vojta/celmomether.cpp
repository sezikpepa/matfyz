#include <iostream>

#include "temps.h" // this file defines temperatures[] and no_value constants

int find_min(const int array[], int arr_size) {
    int min = 0;
    for (int i = 0; i < arr_size; ++i) {
        if (array[i] != no_value and array[i] < min) {
            min = array[i];
        }
    }
    return min;
}

void char_print(int n, char ch) {
    for (int i = 0; i < n; ++i) {
        printf("%c", ch);
    }
}

void print_temps(const int array[], int array_size, int min) {
    bool no_negative_temps = true;
    int last_temp = 0;
    int num_of_spaces = 0;

    if (min < 0) {
        min = min * -1;
        no_negative_temps = false;
        num_of_spaces = min;
    }
    for (int i = 0; i < array_size; ++i) {
        if (i > 0) {
            printf("\n");
        }
        int temp = array[i];
        if (!no_negative_temps) {
            if (temp >= 0) {
                num_of_spaces = min;
            }
            else {
                if (temp < 0 and temp != no_value) {
                    num_of_spaces = min + temp;
                }
            }
            char_print(num_of_spaces, ' ');
        }
        if (temp == no_value) {
            temp = last_temp;
        }
        if (temp >= 0) {
            char_print(1, '|');
        }
        char_print(abs(temp), '*');
        if (temp < 0) {
            char_print(1, '|');
        }
        last_temp = temp;
    }
}

int main(int argc, char** argv)
{
    // Here goes your code, but do not forget to decompose it to functions...
    int size = sizeof(temperatures) / sizeof(temperatures[0]);
    int min_temp = find_min(temperatures, size);
    print_temps(temperatures, size, min_temp);

}
