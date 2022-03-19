#include "funshield.h"

unsigned long startTime;
int currentLed = 0;
const unsigned long ledChangeSpeed = 300; //unsigned long is here instead of int, because recodex doesnt want accept int

const int ledPins[] = {led1_pin, led2_pin, led3_pin, led4_pin, led3_pin, led2_pin, led1_pin};

static void changeLed(){
    if (currentLed == 6){
      currentLed = 0;
    }
    digitalWrite(ledPins[currentLed], OFF);
    digitalWrite(ledPins[currentLed + 1], ON);
    currentLed += 1;
}

void setup() {
  // put your setup code here, to run once:
  for (int i = 0; i < 4; ++i){
    pinMode(ledPins[i], OUTPUT);
    digitalWrite(ledPins[i], OFF);
  }
  startTime = millis();
  digitalWrite(led1_pin, ON);
}

void loop() {
  auto currentTime = millis();

  if (currentTime - startTime >= ledChangeSpeed){
    startTime = currentTime;
    changeLed();
  } 
}
