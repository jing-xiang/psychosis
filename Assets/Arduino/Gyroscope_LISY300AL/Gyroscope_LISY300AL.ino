
/*Connections:
Connect the VCC pin of the LISY300AL to the 5V pin on the Arduino.
Connect the GND pin of the LISY300AL to the GND pin on the Arduino.
Connect the XOUT pin of the LISY300AL to an analog input pin (e.g., A0) on the Arduino.
Connect the YOUT pin of the LISY300AL to another analog input pin (e.g., A1) on the Arduino.*/

const int xOutPin = A0; // XOUT pin

const float sensitivity = 0.3; // Sensitivity of the LISY300AL in mV/°/s (adjust based on your sensor's datasheet)
const int zeroRate = 1.65; // Zero-rate level (calibrate based on your sensor's output at rest)

void setup() {
  Serial.begin(9600); // Start the Serial communication
}

void loop() {
  int rawValue = analogRead(xOutPin)-370; // Read the raw XOUT value
  Serial.println(rawValue);
  float voltage = (rawValue / 1024.0) * 3.3; // Convert the raw value to voltage (use 3.3V instead of 5V)
  float angularVelocity = (voltage - (zeroRate / 1024.0) * 3.3) / sensitivity; // Calculate the angular velocity in °/s

  Serial.print("Yaw (left/right) Angular Velocity: ");
  Serial.print(angularVelocity);
  Serial.println(" °/s");
  //Serial.println(zeroRate);
  delay(1000); // Wait for 100 milliseconds
}
