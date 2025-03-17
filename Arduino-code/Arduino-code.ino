const int potPinLeft = A0;   // Potensio kiri di pin A0
const int potPinRight = A1;  // Potensio kanan di pin A1
const int buttonPin = 2;     // Button di pin D2

// Variable untuk menyimpan status button
int buttonState = 0;
int lastButtonState = 0;

void setup() {
  Serial.begin(9600);
  pinMode(buttonPin, INPUT_PULLUP);  // Set pin button sebagai input dengan pull-up internal
}

void loop() {
  // Baca nilai kedua potensio
  int leftValue = analogRead(potPinLeft);
  int rightValue = analogRead(potPinRight);
  
  // Baca status button
  buttonState = digitalRead(buttonPin);

  // Deteksi perubahan status button (falling edge)
  if (buttonState == LOW && lastButtonState == HIGH) {
    // Kirim sinyal start game
    Serial.println("START");
    delay(50); // Debounce delay
  }
  
  // Simpan status button terakhir
  lastButtonState = buttonState;
  
  // Kirim data potensio dengan format "kiri,kanan"
  Serial.print(leftValue);
  Serial.print(",");
  Serial.println(rightValue);
  
  delay(50); // Delay kecil untuk stabilitas
}
