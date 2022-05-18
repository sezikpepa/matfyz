#include "funshield.h"
#include "input.h"

// map of letter glyphs
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

constexpr int positionsCount = 4;
constexpr unsigned int scrollingInterval = 300;


/** 
 * Show chararcter on given position. If character is not letter, empty glyph is displayed instead.
 * @param ch character to be displayed
 * @param pos position (0 = leftmost)
 */
void displayChar(char ch, byte pos)
{
  byte glyph = EMPTY_GLYPH;
  if (isAlpha(ch)) {
    if(ch - (isUpperCase(ch) ? 'A' : 'a') >= 0 )
      glyph = LETTER_GLYPH[ ch - (isUpperCase(ch) ? 'A' : 'a') ];
  }
  
  digitalWrite(latch_pin, LOW);
  shiftOut(data_pin, clock_pin, MSBFIRST, glyph);
  shiftOut(data_pin, clock_pin, MSBFIRST, 1 << pos);
  digitalWrite(latch_pin, HIGH);
}

SerialInputHandler input;

size_t stringSize(const char *word){
  size_t size = 0;
  while(word[size]){
    size++;
  }
  return size;
}

const char *messagePointer = "";
size_t currentMessageLength;
int charCounter;


void setup() {
  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);

  input.initialize();

  messagePointer = input.getMessage();
  currentMessageLength = stringSize(messagePointer);
  charCounter = 0;
}


class Display{
  public:
    int lastChangeTime;
    int displayPosition;
    int forceSpace;
    
    Display(){
      lastChangeTime = millis();
      displayPosition = 0;
      forceSpace = 4;
    }

    void getNewMessage(){
        const char *newMessage = input.getMessage();
        messagePointer = newMessage;
        currentMessageLength = stringSize(messagePointer);
        charCounter = 0;
        forceSpace = 4;
      }
    
    void actualization(int time){
      if(time - lastChangeTime >= int(scrollingInterval)){
        charCounter++;
        moveText();
        lastChangeTime = time;   
        forceSpace--;
      }
      
      if(charCounter >= int(currentMessageLength + 4)){
        getNewMessage();
      }
        
    } 
    void moveText(){
      if(forceSpace <= 0)
        messagePointer += 1;
    }
    
    void display(){
      displayPosition++;
      displayPosition = displayPosition % 4;

      if(charCounter + displayPosition >= int(currentMessageLength) + 4) //bytes outside of the message
        return;
        
      displayChar(*(messagePointer + displayPosition), displayPosition + (forceSpace > 0 ? forceSpace : 0));          
    }   
};


Display display;


void loop() {
  input.updateInLoop();
  auto time = millis();
  display.actualization(time);
  display.display();
  
}
