#include "funshield.h"

constexpr int ledPins[] { led1_pin, led2_pin, led3_pin, led4_pin };
constexpr int ledPinsCount = sizeof(ledPins) / sizeof(ledPins[0]);

constexpr int activationDelay = 1000; // how long before button starts to perform periodic updates [ms]
constexpr int periodicDelay = 300; // delay between consecutive periodic updates [ms]
constexpr int activationPeriodicDelay = activationDelay + periodicDelay;

int number = 0;

static void setLeds(){
  int number_ = number;
  for (int i = 3; i >= 0; --i){
    int state = number_ & 1;
    digitalWrite(ledPins[i], !state);
    number_ = number_ >> 1;
  }
}

static void ledSetup(){
  for (int i = 0; i < ledPinsCount; ++i) {
    pinMode(ledPins[i], OUTPUT);
    digitalWrite(ledPins[i], OFF);
  }
}

static void normalizeNumber(){
  number = number % 16;
}

static void buttonSetup(){
  pinMode(button1_pin, INPUT);
  pinMode(button2_pin, INPUT);
}

class Button {
  public:
    Button(int pin, int valueShift) {
      pin_ = pin;
      valueShift_ = valueShift;
    }
    void changeNumber(){
      number += valueShift_;
    }

    int timeElapsed(int currentTime){
      return currentTime - lastPressed_;
    }

    void setup() {
      state_ = digitalRead(pin_);
    }

    void resetAfterButtonRelease(){
      state_ = false;
    }

    bool repeatActivationCheck(int elapsedTime){
      return elapsedTime >= activationDelay;
    }

    void loop(int time) {
      auto currentState = digitalRead(pin_);
      if(currentState == ON)
      {     
        if (state_ == false){
          changeNumber();
          lastPressed_ = time;
        }
        
        int elapsedTime = timeElapsed(time);
        if (repeatActivationCheck(elapsedTime)){
          changeNumber();
          lastPressed_ += periodicDelay;
        }
                 
        state_ = true; 
        return;
      }     
      resetAfterButtonRelease();
    }
  private:
    int pin_;
    int valueShift_;
    int lastPressed_;
    int state_;
};


Button incrementButton(button1_pin, 1);
Button decrementButton(button2_pin, -1);


void setup() {
  ledSetup();
  buttonSetup();
}

void loop() {
  int time = millis();
  incrementButton.loop(time);
  decrementButton.loop(time);
  normalizeNumber();
  setLeds();
  
}







