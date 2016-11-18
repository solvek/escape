#include <Servo.h>
#include <SPI.h>
#include <Wire.h>
#include <BrainPad.h>

void setup() {
  Serial.begin(9600);
  Serial.print("Hello world!");

//  BrainPad.Display.DrawText(0,0,new String("Hello Sergi"));
}

void loop() {
 while (BrainPad.Looping) { BrainPad.TrafficLight.TurnGreenLightOn(); BrainPad.Wait.Seconds(0.5); BrainPad.TrafficLight.TurnGreenLightOff(); BrainPad.Wait.Seconds(0.5); }
}
