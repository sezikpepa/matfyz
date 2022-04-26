#include "funshield.h"

class Button{
  public:
    Button(int pin){
      pin_ = pin;
    }

    void resetAfterButtonRelease(){
      state_ = false;
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

class Display{
  public:
    Display(int decimalPosition){
      for(int i = 0; i < 4; ++i){
        digitArray[i] = 0;
      }
      lastIndex = 0;
    }
    int digitArray[4]; 
    int lastIndex;
    int decimalPosition;
    int counter;

    void displayDigit(byte digit, byte position){ 
      digitalWrite(latch_pin, ON);      
      shiftOut(data_pin, clock_pin, MSBFIRST, digit);
      shiftOut(data_pin, clock_pin, MSBFIRST, position);     
      digitalWrite(latch_pin, OFF);
    }       

    void whichDigit(){
      if(counter < lastIndex){
        counter = lastIndex;
      }
      auto digit = digits[digitArray[counter]];      
      if(counter == 2){
          digit = digit & 0b01111111;          
        }    
      displayDigit(digit, digit_muxpos[counter]);
      
    }
    void digitValues(int number){
      int number_ = number;
      for(int i = 3; i >= 0; --i){
        digitArray[i] = number_ % 10;
        number_ = number_ / 10;
        if(number_ == 0){
          lastIndex = i;
          break;
        }              
      }   
      if(number < 10){
        lastIndex--;
      }    
    }    

    void reset(){
      for(int i = 0; i < 4; ++i){
        digitArray[i] = 0;
      }
    }

    void setForNextNumber(){
      counter++;
      counter = counter % 4;
    }
};

enum class StopwatchStates { STOPPED, RUNNING, LAPPED};
enum class UserInputs {START, RESET, STOP};

class Stopwatch{  
  public:
    Display display;
    
    unsigned long lastTime;  
    unsigned long alreadyMeasured; 
    unsigned long lappedValue; 
    StopwatchStates stopwatchStates;    

    Stopwatch(): display(3){
      stopwatchStates = StopwatchStates::STOPPED;
      alreadyMeasured = 0;
    }

    void displayTime(){
      if(stopwatchStates == StopwatchStates::LAPPED){
        display.digitValues((lappedValue) / 100);
        display.whichDigit();
        return;
      }
      display.digitValues((alreadyMeasured) / 100);
      display.whichDigit();
    }

    void evaluateTime(int time){
      switch(stopwatchStates){
        case StopwatchStates::RUNNING:
          alreadyMeasured += time - lastTime;
          lastTime = time;
        case StopwatchStates::STOPPED:
          lastTime = time;      
        case StopwatchStates::LAPPED:
          alreadyMeasured += time - lastTime;
          lastTime = time;
      }
    }

    void startStopInput(){
      if(stopwatchStates == StopwatchStates::STOPPED){
        stopwatchStates = StopwatchStates::RUNNING;
        return;
      }
      if(stopwatchStates == StopwatchStates::RUNNING){
        stopwatchStates = StopwatchStates::STOPPED;
      }
    }

    void resetInput(){
      if(stopwatchStates == StopwatchStates::STOPPED){
        alreadyMeasured = 0;
        display.reset();
      }
    }

    void lapInput(){
      if(stopwatchStates == StopwatchStates::RUNNING){
        stopwatchStates = StopwatchStates::LAPPED;
        lappedValue = alreadyMeasured;
        return;
      }
      if(stopwatchStates == StopwatchStates::LAPPED){
        stopwatchStates = StopwatchStates::RUNNING;
      }
    }

    
};

Stopwatch stopwatch;

class StartStopButton: public Button{
  public:
    StartStopButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      stopwatch.startStopInput();
    }
};


class ResetButton: public Button{
  public:
    ResetButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      stopwatch.resetInput();
    }
};

class LapButton: public Button{
  public:
    LapButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      stopwatch.lapInput();
    }
};

StartStopButton startStopButton(button1_pin);
LapButton lapButton(button2_pin);
ResetButton resetButton(button3_pin);

static void buttonSetup(){
  pinMode(button1_pin, INPUT);
  pinMode(button2_pin, INPUT);
  pinMode(button3_pin, INPUT);
}

void setup() {
  buttonSetup();
  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);  
}

void loop() {
  int time = millis();
  startStopButton.loop();
  lapButton.loop();
  resetButton.loop();

  stopwatch.evaluateTime(time);
  stopwatch.displayTime();  

  stopwatch.display.setForNextNumber();
}
