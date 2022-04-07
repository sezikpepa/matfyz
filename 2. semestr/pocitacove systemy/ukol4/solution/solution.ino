#include "funshield.h"

constexpr int ledPins[] { led1_pin, led2_pin, led3_pin, led4_pin };
constexpr int ledPinsCount = sizeof(ledPins) / sizeof(ledPins[0]);

constexpr int activationDelay = 1000;
constexpr int periodicDelay = 300;
constexpr int activationPeriodicDelay = activationDelay + periodicDelay;

int number = 0;
int position = 0;

static void normalizeNumber(){
  number = number % 10000;
}

static void normalizePosition(){
  position = position % 4;
}

class Button{
  public:
    Button(int pin){
      pin_ = pin;
    }

    void resetAfterButtonRelease(){
      state_ = false;
    }

    bool repeatActivationFirstCheck(int elapsedTime){
      return elapsedTime >= activationDelay;
    }

    virtual void actionAfterClick(){
    }

    int timeElapsed(int currentTime){
      return currentTime - lastPressed_;
    }

    void loop(int time) {
      auto currentState = digitalRead(pin_);
      if(currentState == ON)
      {     
        if (state_ == false){
          actionAfterClick();
          lastPressed_ = time;
        }
        
        int elapsedTime = timeElapsed(time);
        if (repeatActivationFirstCheck(elapsedTime)){
          actionAfterClick();
          lastPressed_ += periodicDelay;
        }
                 
        state_ = true; 
        return;
      }     
      resetAfterButtonRelease();
    }

  protected:
    int pin_;
    int state_;
    int lastPressed_;
};

class ChangeValueButton: public Button{
  public:
    ChangeValueButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      number += increments_[position];
      normalizeNumber();
    }

    int increments_[4]; 
};

class PositionChangeButton: public Button{
  public:
    PositionChangeButton(int pin):Button(pin){
    }

    void actionAfterClick(){
      position += 1;
    }
};


ChangeValueButton incrementButton(button1_pin);
ChangeValueButton decrementButton(button2_pin);
PositionChangeButton positionChangeButton(button3_pin);

static void setButtonsIncrements(){
  incrementButton.increments_[0] = 1;
  incrementButton.increments_[1] = 10;
  incrementButton.increments_[2] = 100;
  incrementButton.increments_[3] = 1000;

  decrementButton.increments_[0] = 9999;
  decrementButton.increments_[1] = 9990;
  decrementButton.increments_[2] = 9900;
  decrementButton.increments_[3] = 9000;
}

static void buttonSetup(){
  pinMode(button1_pin, INPUT);
  pinMode(button2_pin, INPUT);
  pinMode(button3_pin, INPUT);
}

static int getDigitValue(int number, int position_){
  int number_ = number;
  for(int i = 0; i < position_; ++i){
    number_ = number_ / 10;
  }
  return number_ % 10;
}

static void displayDigit(byte digit, byte position){
  shiftOut(data_pin, clock_pin, MSBFIRST, digit);
  shiftOut(data_pin, clock_pin, MSBFIRST, position);
  digitalWrite(latch_pin, ON);
  digitalWrite(latch_pin, OFF);
}

static void displayResult(){
  int digit = getDigitValue(number, position);
  displayDigit(digits[digit], digit_muxpos[3 - position]);
}

void setup() {
  buttonSetup();
  setButtonsIncrements();

  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);  
}

void loop() {
  int time = millis();
  incrementButton.loop(time);
  decrementButton.loop(time);
  positionChangeButton.loop(time);
  normalizeNumber();
  normalizePosition();
  displayResult();
}







