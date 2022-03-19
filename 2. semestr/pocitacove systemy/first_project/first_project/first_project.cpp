#include <stdio.h>

void star_print(int number, char znak) {
    for (int i = 0; i < number; ++i) {
        printf("%c", znak);
       
    }
}

void triangle(int n, char znak) {
    for (int i = 0; i < n; ++i) {
        star_print(i + 1, znak);
        printf("\n");
    }
}

void symetric_triangle(int height, int skip) {
    for (int i = 0 + skip; i < height; ++i) {
        star_print(height - 1 - i, ' ');
        star_print(2 * i + 1, '*');
        printf("\n");
    }
}

void temperature_print(int temperatures[], int size_temperatures) {
    for (int i = 0; i < size_temperatures; ++i) {
        star_print(temperatures[i], '*');
        printf("\n");

    }
}

int main()
{
    int teploty[10] = { 2, 4, 9, 63, 7, 6, 7, 3, 8, 10 };
    temperature_print(teploty, 10);
}
