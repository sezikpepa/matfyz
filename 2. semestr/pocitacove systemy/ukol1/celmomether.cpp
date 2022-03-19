#include <stdio.h>

#include "temps.h"

void segment_print(int number, char znak) {
	for (int i = 0; i < number; ++i) {
		printf("%c", znak);
	}
}

void alignment_print(int lowest, int value) {
	if (lowest != no_value) {
		if (lowest < 0) {
			if (value < 0)
				segment_print(-lowest + value, ' ');
			else
				segment_print(-lowest, ' ');		
		}
	}
}

int lowest_number(const int numbers[], int numbers_length) {
	int lowest = numbers[0];
	for (int i = 1; i < numbers_length; ++i) {
		if ((numbers[i] < lowest) || (lowest == no_value)) {
			if (numbers[i] != no_value)
				lowest = numbers[i];
		}
	}
	return lowest;
}

void print_temperature(int lowest, int next_number) {
	alignment_print(lowest, next_number);
	if (next_number >= 0) {
		printf("|");
		segment_print(next_number, '*');
	}
	else {
		segment_print(-next_number, '*');
		printf("|");
	}
	printf("\n");
}

void print_temperatures(const int numbers[], int numbers_length) {
	int lowest = lowest_number(numbers, numbers_length);
	int last_valid_number = 0;
	for (int i = 0; i < numbers_length; ++i) {
		int next_number = numbers[i];
		if (next_number == no_value)
			next_number = last_valid_number;
		
		print_temperature(lowest, next_number);
		last_valid_number = next_number;
	}
}


int main(int argc, char **argv)
{
	int length = sizeof(temperatures) / sizeof(temperatures[0]);
	print_temperatures(temperatures, length);
	
	return 0;
}

