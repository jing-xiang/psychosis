#include <WiFiS3.h>
#include <WiFiUdp.h>

// WiFi network credentials
const char* ssid = "IMAN";     //  WiFi network name
const char* password = "12345678";  // WiFi password

// UDP settings
WiFiUDP udp;
unsigned int localPort = 8888;  // Port to listen on
IPAddress targetIP(192,168,174,244);  // with Unity computer's IP address
unsigned int targetPort = 8889;  // Port on your computer to send to same as Unity configuration

// Accelerometer pins
const int xPin = A0;
const int yPin = A1;
const int zPin = A2;

// Calibration values - adjust these after testing
float xOffset = 2.5; // Typical midpoint for 0g when using 5V reference
float yOffset = 2.5;
float zOffset = 2.5;

void setup() {
  Serial.begin(9600);
  
  // Connect to WiFi network
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  WiFi.begin(ssid, password);
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  
  Serial.println("");
  Serial.println("WiFi connected");
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
  
  // Begin UDP
  udp.begin(localPort);
  Serial.print("UDP server started on port ");
  Serial.println(localPort);
}

void loop() {
  // Read accelerometer values
  int xValue = analogRead(xPin);
  int yValue = analogRead(yPin);
  int zValue = analogRead(zPin);

  // Convert to acceleration in g
  float xAccel = (xValue / 1023.0) * 5.0 - xOffset;
  float yAccel = (yValue / 1023.0) * 5.0 - yOffset;
  float zAccel = (zValue / 1023.0) * 5.0 - zOffset;

  // Create data string directly with String class
  String dataString = "X:" + String(xAccel, 2) + ",Y:" + String(yAccel, 2) + ",Z:" + String(zAccel, 2);
  
  // Send UDP packet using String directly
  udp.beginPacket(targetIP, targetPort);
  udp.print(dataString);  // Use print() instead of write()
  udp.endPacket();
  
  // Print to Serial for debugging
  Serial.println(dataString);
  
  delay(100); // Adjust delay as needed
}