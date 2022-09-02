#include "funshield.h"


constexpr int buttons[] = { button1_pin, button2_pin, button3_pin };
constexpr int buttons_count = sizeof(buttons) / sizeof(buttons[0]);
constexpr int digit_pos = sizeof(digit_muxpos) / sizeof(digit_muxpos[0]);
constexpr byte LETTER_GLYPH[] {
  0b10001000,   // A
  0b10000011,   // b
  0b11000110,   // C
  0b10100001,   // d
  0b10000110,   // E
  0b10001110,   // F
  0b10000010,   // G
  0b10001001,   // H
  0b11111001,   // I
  0b11100001,   // J
  0b10000101,   // K
  0b11000111,   // L
  0b11001000,   // M
  0b10101011,   // n
  0b10100011,   // o
  0b10001100,   // P
  0b10011000,   // q
  0b10101111,   // r
  0b10010010,   // S
  0b10000111,   // t
  0b11000001,   // U
  0b11100011,   // v
  0b10000001,   // W
  0b10110110,   // ksi
  0b10010001,   // Y
  0b10100100,   // Z
};

constexpr byte EMPTY_GLYPH = 0b11111111;


class Button {
  public:
    Button(int pin) {
      pin_ = pin;
    }
    bool pressed(bool current_state)
    {
      if (!current_state) {
        previous_state_ = false;
        return false;
      }
      if (previous_state_ == true && current_state) return false;
      previous_state_ = true;
      return true;
    }
  private:
    bool previous_state_ = false;
    int pin_;
};

int decimal[] = {1, 10, 100, 1000};

class Segmented_display {

  public:
    void text_display(bool show_config, int thrown_num, char config[]) {
      for (int i = 0; i < digit_pos; i++) {
        if (show_config) display_char(config[i], i);
        else digit_write( (thrown_num / decimal[i]) % 10 , i);
      }
    }
    void display_char(char character, byte pos)
    {
      byte glyph = EMPTY_GLYPH;
      if (isAlpha(character)) {
        glyph = LETTER_GLYPH[ character - (isUpperCase(character) ? 'A' : 'a') ];
      }
      else {
        glyph = digits[ character - '0' ];
      }

      write_glyph_left(glyph, pos);
    }
    void write_glyph_bitm( byte glyph, byte bitm_pos) {
      digitalWrite( latch_pin, LOW);
      shiftOut( data_pin, clock_pin, MSBFIRST, glyph);
      shiftOut( data_pin, clock_pin, MSBFIRST, bitm_pos);
      digitalWrite( latch_pin, HIGH);
    }

    void write_glyph_right(byte glyph, int pos)
    {
      write_glyph_bitm(glyph, digit_muxpos[digit_pos - pos - 1]);
    }
    void write_glyph_left(byte glyph, int pos)
	{
	  write_glyph_bitm(glyph, digit_muxpos[pos]);
	}

    void digit_write(int n, int pos)
    {
      write_glyph_right(digits[n], pos);
    }
  private:
    int pos_ = 0;
};

const int loading_pattern[] = {254,254,254,254,223,239,247,247,247,247,251,253};

const int values_of_dice[] = {4, 6, 8, 10, 12, 20, 100};
const int dice_count = sizeof(values_of_dice) / sizeof(values_of_dice[0]);
const int repeat = 300;

class Dice {

  public:

    bool show_config;
    int thrown_num;
    char config[digit_pos];
	enum Mode {
      NORMAL,
      CONFIGURATION
    };

    Dice() {
      mode_ = CONFIGURATION;

      thrown_nums_count = 1;
      type_of_dice = 0;

      show_config = true;
      config_change();
    }

    void deal_with_press(int button) {
      if (mode_ == CONFIGURATION && button == 1) {
        mode_ = NORMAL;
        show_config = false;
      }
      else if (mode_ == NORMAL && (button == 2 || button == 3)) {
        mode_ = CONFIGURATION;
        show_config = true;
      }
      else if (mode_ == CONFIGURATION && button == 2) {
        thrown_nums_count++;
        if (thrown_nums_count > 9) {
          thrown_nums_count = 1;
        }
        config_change();
      }
      else if (mode_ == CONFIGURATION && button == 3) {
        type_of_dice++;
        type_of_dice %= dice_count;
        config_change();
      }
      else if (mode_ == NORMAL && button == 1) {
        bool pressed_down = true;
        long current_time = millis();
        long repeat_time = current_time + repeat;
        thrown_num = generate_random();

        while (pressed_down) {
          if (digitalRead(buttons[0])) {
            pressed_down = false;
          }
          current_time = millis();
          if (current_time >= repeat_time) {
            repeat_time += repeat;
            thrown_num = generate_random();

            loading_pattern_screen(index++);
            index %= 12;
          }
        }
      }
    }
  private:
    void loading_pattern_screen(int i){
      int number = loading_pattern[i];
      
      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, number);
      shiftOut(data_pin, clock_pin, MSBFIRST, 1 << loading_pattern_pos);
      digitalWrite(latch_pin, HIGH);

      if(number == 254 && loading_pattern_pos != 0){
        loading_pattern_pos--;
      }
      if(number == 247 && loading_pattern_pos != 3){
        loading_pattern_pos++;
      }
    }
    
    void config_change() {
      config[0] = thrown_nums_count + '0';
      config[1] = 'd';
      config[2] = ((values_of_dice[type_of_dice] % 100)/ 10) + '0';
      config[3] = (values_of_dice[type_of_dice] % 10) + '0';
    }

    int generate_random() {
      int value = 0;
      
      for (int i = 0; i < thrown_nums_count; i++) {
        int random_val = random(1, values_of_dice[type_of_dice] + 1);
        value += random_val;
      }
      return value;
    }
    int index = 0;
    int loading_pattern_pos = 0;
    Mode mode_;
    int thrown_nums_count;
    int type_of_dice;
};

void setup() {
  for (int i = 0; i < buttons_count; ++i) {
    pinMode(buttons[i], INPUT);
  }
  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);
}

Button button[] { Button(buttons[0]), Button(buttons[1]), Button(buttons[2]) };
Segmented_display d;
Dice dice = Dice();

void loop() {
  for (int i = 0; i < buttons_count; ++i) {
    if (button[i].pressed(!digitalRead(buttons[i]))) {
      dice.deal_with_press(i + 1);
    }
  }
  d.text_display(dice.show_config, dice.thrown_num, dice.config);

}
