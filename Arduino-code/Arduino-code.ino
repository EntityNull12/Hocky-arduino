const int potPinLeft = A0;   // Potensio kiri di pin A0
const int potPinRight = A1;  // Potensio kanan di pin A1
const int buttonPin = 2;     // Button di pin D2

int buttonState = 0;
int lastButtonState = 0;

void setup() {
  Serial.begin(9600);
  pinMode(buttonPin, INPUT_PULLUP);
}

void loop() {
  int leftValue = analogRead(potPinLeft);
  int rightValue = analogRead(potPinRight);
  
  buttonState = digitalRead(buttonPin);

  if (buttonState == LOW && lastButtonState == HIGH) {
    Serial.println("RESTART");
    delay(50);
  }
  
  lastButtonState = buttonState;
  
  Serial.print(leftValue);
  Serial.print(",");
  Serial.println(rightValue);
  
  delay(10);
}
