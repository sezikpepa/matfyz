#include "funshield.h"


constexpr byte EMPTY_GLYPH = 0b11111111;
constexpr byte D_LETTER = 0xa1;
constexpr int positionsCount = 4;
unsigned int scrollingInterval = 300;

unsigned long startTime = micros();
constexpr int typesOfDices[] = {4, 6, 8, 10, 12, 20, 100};
constexpr int animationPatterns[] =  {0xf7, 0xbf, 0xfe, 0xbf};

constexpr unsigned int numberGenerationAnimationSpeed = 160;

void setup() {
  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);

  randomSeed(analogRead(0));
}

int hashNumber(int number, int seed){
  long result = 0;
  int number_ = number;  
  randomSeed(seed);
  while(number_ > 0){
    long toHash = number_ % 10;
    number_ = number_ / 10;

    int base = random(1, 10);
    int exp = random(1, 5);
    int base2 = random(1, 10);
    int exp2 = random(1, 5);
    int base3 = random(1, 10);
    int exp3 = random(1, 5);
    toHash = toHash * pow(base, exp) + toHash * pow(base2, exp2) + toHash * pow(base3, exp3);
    result += toHash; 
    result = result % 1000;
  }
  return int(result) + 1;
}

class Button{
  public:
    Button(int pin){
      pin_ = pin;
    }

    void resetAfterButtonRelease(){
      state_ = false;
      actionAfterRelease();
    }

    virtual void actionAfterRelease(){
    }

    virtual void actionAfterClick(){
    }

    void loop() {
      auto currentState = digitalRead(pin_);
      if(currentState == ON)
      {     
        if (state_ == false){
          actionAfterClick();
        }
                 
        state_ = true; 
        return;
      }   
      resetAfterButtonRelease();     
          
    }

  protected:
    int pin_;
    int state_;
};

class HoldingButton{
  public:
    HoldingButton(int pin){
      pin_ = pin;
      state_ = OFF;
    }

    virtual void actionDuringPress(){
    }

    virtual void actionWithoutPress(){
    }

    void loop(){
      auto state = digitalRead(pin_);
      if(state == ON){
        actionDuringPress();
        state_ = ON;
        return;
      }
      if(state_ == ON){
        actionWithoutPress();
        state_ = OFF;      
      }
    }

  protected:
    int pin_;
    int state_;
};


class Display{
  public:
    int displayPosition;
    int forceSpace;
    int toDisplay[4];
    
    Display(){
      displayPosition = 0;
      for(int i = 0; i < 4; ++i){
        toDisplay[i] = 0xff;
      }
    }
   
    void display(){        
      displayPosition = (displayPosition + 1) % 4;

      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, toDisplay[displayPosition]);
      shiftOut(data_pin, clock_pin, MSBFIRST, 1 << displayPosition);
      digitalWrite(latch_pin, HIGH);  
    }   

    void input(int position, int number){
      toDisplay[position] = digits[number];
    }

    void resetToDisplay(){
      for(int i = 0; i < 4; ++i){
        toDisplay[i] = EMPTY_GLYPH;
      }
    }

    void input(int position, char letter){
      toDisplay[position] = D_LETTER;    
    }

    void inputBytes(int position, byte pattern){
      toDisplay[position] = pattern;
    }

    void clearPosition(int position){
      toDisplay[position] = EMPTY_GLYPH;
    }

    void wholeNumberInput(int number){
      for(int i = 3; i >= 0; --i){
        toDisplay[i] = digits[number % 10];
        number = number / 10;
        if(number == 0){
          break;
        }              
      }  
    }
};


Display display;

enum class ThrowsGeneratorModes {NORMAL, CONFIGURATION};

class ThrowsGenerator{
  private:
   int generatedNumbers[10];
   int generatedNumberPos;
   int currentNumberOfThrows;
   int diceType;
   int diceTypes[7];
   int patterns[4];
   int patternNumber;
   int lastPatternChange;
   int animationSpeed;
   int generatingStartTime;
   
public:
   ThrowsGeneratorModes mode;
   int lastGeneratedNumber;
   bool changedToNormalMode;

  public: 
    ThrowsGenerator(){
      for(int i = 0; i < 10; ++i){
        generatedNumbers[i] = 0;
        generatedNumberPos = 0;
        currentNumberOfThrows = 1;
        mode = ThrowsGeneratorModes::NORMAL;
        diceType = 0;
        for(int i = 0; i < 7; ++i)
          diceTypes[i] = typesOfDices[i];
        patternNumber = 0;
        lastGeneratedNumber = 0;

        for(int i = 0; i < 4; ++i)
          patterns[i] = animationPatterns[i];

        lastPatternChange = 0;

        bool changedToNormalMode = true;
        animationSpeed = numberGenerationAnimationSpeed;
        generatingStartTime = micros();
    }
  }
    void newThrows(){
      generatingStartTime = micros();
    }

    void getResult(){
        int result = 0;
        int timeNow = micros() / 151;
        int seed = timeNow / 141;
        randomSeed(generatingStartTime - timeNow);        
        for(int i = 0; i < currentNumberOfThrows; ++i){
          int part = generateThrow(diceTypes[diceType]); 
          part = hashNumber(part, seed);         
          result += part % diceTypes[diceType] + 1;
        }
        display.wholeNumberInput(result);    
        generatingStartTime = timeNow;      
        lastGeneratedNumber = result;
     
    }

    int generateThrow(int maxNumber){
      int result = random(1, maxNumber + 1);
      return result;
    }

    void changeDisplayValuesConfigMode(){
      display.input(0, currentNumberOfThrows);
      display.input(1, 'd');
      int diceValue = diceTypes[diceType];


      if(diceValue == 100){ //do not work with with if-statement diceValue >= wonder why
        display.input(3, 0);
        display.input(2, 0);
        return;
      }

      if(diceValue >= 10){      
        display.input(3, diceValue % 10);
        display.input(2, diceValue / 10);
        return;
      }
      display.input(2, diceValue);
      display.clearPosition(3);  
    }

    void changeDiceCountInput(){
      changedToNormalMode = false;
      if(mode == ThrowsGeneratorModes::NORMAL)
      {
        mode = ThrowsGeneratorModes::CONFIGURATION;
        changeDisplayValuesConfigMode();
        return;
      }
      currentNumberOfThrows = currentNumberOfThrows % 9 + 1;

      changeDisplayValuesConfigMode();
    }

    void changeDiceValueInput(){
      changedToNormalMode = false;
      if(mode == ThrowsGeneratorModes::NORMAL)
      {
        mode = ThrowsGeneratorModes::CONFIGURATION;
        changeDisplayValuesConfigMode();
        return;
      }
      diceType = (diceType + 1) % 7;

      changeDisplayValuesConfigMode();
    }

    void generateThrowsInput(){
      if(mode == ThrowsGeneratorModes::CONFIGURATION){
        mode = ThrowsGeneratorModes::NORMAL;
        return;
      }
      if(changedToNormalMode == true){ // there was a change to Normal mode, with a realease of a button
        displayDuringGeneratingSet();
        newThrows();
      }
    }

    void displayDuringGeneratingSet(){
      int time = millis();
      if(time - lastPatternChange >= animationSpeed)
      {
        patternNumber = (patternNumber + 1) % 4; 
        display.resetToDisplay();
        for(int i = 0; i < 4; ++i)
          display.inputBytes(i, patterns[(i + patternNumber) % 4]);
        lastPatternChange = time;    
      }     
    }

    void showLastResult(){
      display.resetToDisplay();
      display.wholeNumberInput(lastGeneratedNumber);
    }
};

ThrowsGenerator throwsGenerator;

class ChangeDiceCountButton: public Button{
  public:
    ChangeDiceCountButton(int pin):Button(pin){
    }

    void actionAfterClick(){     
      throwsGenerator.changeDiceCountInput();
    }
};

class ChangeDiceValueButton: public Button{
  public:
    ChangeDiceValueButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      throwsGenerator.changeDiceValueInput();
    }
};

class GenerateThrowsButton: public HoldingButton{
  public:
    GenerateThrowsButton(int pin):HoldingButton(pin){
    }

    void actionDuringPress(){
      throwsGenerator.generateThrowsInput();
    }

    void actionWithoutPress(){
      throwsGenerator.getResult();
      throwsGenerator.showLastResult();

      throwsGenerator.changedToNormalMode = true;
    }
};

ChangeDiceCountButton changeDiceCountButton(button2_pin);
ChangeDiceValueButton changeDiceValueButton(button3_pin);
GenerateThrowsButton generateThrowsButton(button1_pin);

void loop() {
  display.display(); 
  changeDiceCountButton.loop();
  changeDiceValueButton.loop();

  generateThrowsButton.loop();
}